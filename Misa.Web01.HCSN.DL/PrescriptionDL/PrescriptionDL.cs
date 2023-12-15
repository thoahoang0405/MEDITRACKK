using Dapper;
using Google.Protobuf.WellKnownTypes;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Utilities;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MEDITRACK.DL.PrescriptionDL
{
    public class PrescriptionDL : BaseDL<PrescriptionEntity>, IPrescriptionDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion

        public PagingData FilterChoose(
        string? keyword,
        int? pageSize,
        int? pageNumber

        )
        {

            string storedProcedureName = "Proc_prescriptions_getPaging";

            var orConditions = new List<string>();
            var andConditions = new List<string>();
            var requiredConditions = new List<string>();
            string whereClause = "";


            if (keyword != null)
            {
                orConditions.Add($"PrescriptionName LIKE '%{keyword}%'");
                orConditions.Add($"PatientName LIKE '%{keyword}%'");

            }

            string condition = "";

            if (orConditions.Count() > 0)
            {
                condition = string.Join(" OR ", orConditions);
                if (andConditions.Count() > 0)
                {
                    condition = "(" + condition + ") AND " + String.Join(" AND ", andConditions);
                }
            }
            else if (andConditions.Count() > 0)
            {
                condition = string.Join(" AND ", andConditions);
                if (orConditions.Count() > 0)
                {
                    condition = "(" + condition + ") AND " + String.Join(" AND ", andConditions);

                }
            }
            else
            {
                condition = "";
            }
            if (condition != "")
            {
                whereClause = "(" + whereClause + ") AND (" + condition + ")";
            }


            var parameters = new DynamicParameters();
            parameters.Add("v_Sort", "ModifiedDate DESC");

            parameters.Add("v_Limit", pageSize);
            parameters.Add("v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("v_Where", whereClause);
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipleResults = sqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (multipleResults != null)
                {
                    var prescriptions = multipleResults.Read<object>().ToList();
                    var TotalRecords = multipleResults.Read<long>().Single();



                    int TotalPagesAll = 1;

                    if (TotalRecords >= 0 && pageSize > 0)
                    {
                        TotalPagesAll = (int)(decimal)(TotalRecords / pageSize);
                        if (TotalRecords % pageSize != 0)
                        {
                            TotalPagesAll = TotalPagesAll + 1;
                        }
                        if (TotalRecords < pageSize)
                        {
                            pageNumber = 1;
                            TotalPagesAll = 1;
                        }
                    }


                    return new PagingData()
                    {
                        Data = prescriptions,
                        TotalRecords = TotalRecords,



                        TotalPages = TotalPagesAll

                    };
                }
            }
            return null;

        }

        public PrescriptionEntity InsertPrescription(PrescriptionEntity record)
        {
            var newId = Guid.NewGuid();
            var primary = typeof(PrescriptionEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            if (primary != null)
            {
                primary.SetValue(record, newId);
            }

            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<PrescriptionEntity>();
            string insertRecordProcedureName = $"Proc_{tableName}_Insert";
            // chuẩn bị tham số đầu vào 
            var properties = typeof(PrescriptionEntity).GetProperties();
            var parameters = new DynamicParameters();
            List<MedicationsEntity> medicationsEntity = new List<MedicationsEntity>();
            foreach (var property in properties)
            {
                var value = property.GetValue(record); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property
                if (propertyName == "Medications")
                {
                    medicationsEntity = record.Medications;
                }
                else
                {

                    parameters.Add($"p_{propertyName}", value);
                }
            }

            // thực hiện gọi vào DB
            int numberOfAffetedRows = 0;
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {
                    numberOfAffetedRows = sqlConnection.Execute(insertRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    if (numberOfAffetedRows > 0)
                    {
                        int resultDetail = this.InsertMedication(newId, record.Medications, sqlConnection, trans: trans);
                        if (resultDetail > 0)
                        {
                            trans.Commit();
                            return record;
                        }
                        else
                        {
                            trans.Rollback();
                            return record;
                        }
                    }
                    else
                    {
                        trans.Rollback();
                        return record;
                    }

                }
            }

        }

        public PrescriptionEntity UpdatePrescription(PrescriptionEntity record)
        {

            string storeProcedureName = $"Proc_prescriptions_Update";
            var properties = typeof(PrescriptionEntity).GetProperties();
            var parameters = new DynamicParameters();


            List<MedicationsEntity> medicationsEntity = new List<MedicationsEntity>();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);
                if (propertyName == "Medications")
                {
                    medicationsEntity = record.Medications;
                }
                else
                {

                    parameters.Add($"p_{propertyName}", propertyValue);
                }

            }
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {
                    int numberOfAffectedRows = 0;
                    numberOfAffectedRows = sqlConnection.Execute(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    if (numberOfAffectedRows > 0)
                    {
                        int deleteRecord = DeleteMedicationByPres(record.PrescriptionID);
                        if (deleteRecord > 0)
                        {
                            int insertMedication = this.InsertMedication(record.PrescriptionID, medicationsEntity, sqlConnection, trans: trans);
                            if (insertMedication > 0)
                            {
                                trans.Commit();
                                return record;
                            }
                            else
                            {
                                trans.Rollback();
                                return record;
                            }
                        }
                        else
                        {
                            trans.Rollback();
                            return record;
                        }

                    }
                    trans.Rollback();
                    return record;
                }
            }
        }

        public int InsertMedication(Guid idMaster, List<MedicationsEntity> details, MySqlConnection sqlConnection, MySqlTransaction trans)
        {

            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<MedicationsEntity>();
            string insertMedicationStore = $"Proc_medications_Insert";
            int result = 0;
            foreach (var detail in details)
            {
                var newId = Guid.NewGuid();
                var primary = typeof(MedicationsEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                if (primary != null)
                {
                    primary.SetValue(detail, newId);
                }

                // chuẩn bị tham số đầu vào 
                var properties = typeof(MedicationsEntity).GetProperties();
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var value = property.GetValue(detail); // lấy giá trị của property

                    var propertyName = property.Name; // lấy tên của property
                    if (propertyName == "PrescriptionID")
                    {
                        value = idMaster.ToString();
                    }
                    parameters.Add($"p_{propertyName}", value);
                }
                int row = sqlConnection.Execute(insertMedicationStore, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                if (row > 0)
                {
                    result++;
                }

            }

            if (result > 0)
            {
                return 1;
            }
            return 0;
        }

        public int DeleteMedicationByPres(Guid id)
        {
            string sqlCommand = $"DELETE FROM medications Where PrescriptionID='{id}'";
            var parameters = new DynamicParameters();


            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();

                // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
                int numberOfAffectedRows = sqlConnection.Execute(sqlCommand, parameters);
                if (numberOfAffectedRows > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
