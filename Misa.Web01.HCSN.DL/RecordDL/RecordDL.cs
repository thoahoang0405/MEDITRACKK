using Dapper;
using Google.Protobuf.WellKnownTypes;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.DL.RecordDL
{
    public class RecordDL : BaseDL<RecordsEntity>, IRecordDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        public PagingData FilterChoose(
       string? keyword,
       int? pageSize,
       int? pageNumber , Guid id
       )
        {

            string storedProcedureName = "Proc_records_getPaging";

            var orConditions = new List<string>();
            string whereClause = "";


            if (keyword != null)
            {
                orConditions.Add($"RecordTitle LIKE '%{keyword}%'");
                orConditions.Add($"DoctorName LIKE '%{keyword}%'");
                orConditions.Add($"FullName LIKE '%{keyword}%'");

            }

            string condition = "";

            if (orConditions.Count() > 0)
            {
                condition = string.Join(" OR ", orConditions);

            }

            else
            {
                condition = "";
            }
            if (condition != "")
            {
                whereClause = "(" + condition + ")";
            }

            if (whereClause != "" )
            {

                whereClause = whereClause + $" AND UserID= '{id}'";
            }
            else
            {
                whereClause = $"UserID= '{id}'";
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



        public RecordsEntity InsertRecord(RecordsEntity record)
        {
            var newId = Guid.NewGuid();
            var primary = typeof(RecordsEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            if (primary != null)
            {
                primary.SetValue(record, newId);
            }

            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<RecordsEntity>();
            string insertRecordProcedureName = $"Proc_{tableName}_Insert";
            // chuẩn bị tham số đầu vào 
            var properties = typeof(RecordsEntity).GetProperties();
            var parameters = new DynamicParameters();
            List<MedicalTestsEntity> medicalTests = new List<MedicalTestsEntity>();
            List<TreatmentsEntity> treatments = new List<TreatmentsEntity>();
            foreach (var property in properties)
            {
                var value = property.GetValue(record); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property
                if (propertyName == "MedicalTests")
                {
                    medicalTests = record.MedicalTests;
                }
                else if (propertyName == "Treatments")
                {
                    treatments = record.Treatments;
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
                        int resultMedical = this.InsertMedicalTests(newId, medicalTests, sqlConnection, trans: trans);
                        int resultTreatments = this.InsertTreatments(newId, treatments, sqlConnection, trans: trans);
                        if (resultMedical > 0 && resultTreatments > 0)
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

        public RecordsEntity UpdateRecord(RecordsEntity record)
        {
            string storeProcedureName = $"Proc_records_Update";
            var properties = typeof(RecordsEntity).GetProperties();
            var parameters = new DynamicParameters();


            List<MedicalTestsEntity> medicalTests = new List<MedicalTestsEntity>();
            List<TreatmentsEntity> treatments = new List<TreatmentsEntity>();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);
                if (propertyName == "MedicalTests")
                {
                    medicalTests = record.MedicalTests;
                }
                else if (propertyName == "Treatments")
                {
                    treatments = record.Treatments;
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
                        int deleteTest = DeleteMedicalTest(record.RecordID);
                        int deleteTreat = DeleteTreat(record.RecordID);
                        if (deleteTest > 0 && deleteTreat > 0)
                        {
                            int resultMedical = this.InsertMedicalTests(record.RecordID, medicalTests, sqlConnection, trans: trans);
                            int resultTreatments = this.InsertTreatments(record.RecordID, treatments, sqlConnection, trans: trans);
                            if (resultMedical > 0 && resultTreatments > 0)
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

        public int InsertMedicalTests(Guid idMaster, List<MedicalTestsEntity> details, MySqlConnection sqlConnection, MySqlTransaction trans)
        {

            // tên proc dùng để truy vấn

            string insertMedicationStore = $"Proc_medicaltests_Insert";
            int result = 0;
            foreach (var detail in details)
            {
                var newId = Guid.NewGuid();
                var primary = typeof(MedicalTestsEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                if (primary != null)
                {
                    primary.SetValue(detail, newId);
                }

                // chuẩn bị tham số đầu vào 
                var properties = typeof(MedicalTestsEntity).GetProperties();
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var value = property.GetValue(detail); // lấy giá trị của property

                    var propertyName = property.Name; // lấy tên của property
                    if (propertyName == "RecordID")
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

        public int InsertTreatments(Guid idMaster, List<TreatmentsEntity> details, MySqlConnection sqlConnection, MySqlTransaction trans)
        {

            // tên proc dùng để truy vấn

            string insertMedicationStore = $"Proc_treatments_Insert";
            int result = 0;
            foreach (var detail in details)
            {
                var newId = Guid.NewGuid();
                var primary = typeof(TreatmentsEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                if (primary != null)
                {
                    primary.SetValue(detail, newId);
                }

                // chuẩn bị tham số đầu vào 
                var properties = typeof(TreatmentsEntity).GetProperties();
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var value = property.GetValue(detail); // lấy giá trị của property

                    var propertyName = property.Name; // lấy tên của property
                    if (propertyName == "RecordID")
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
        public Guid DeleteRecords(Guid id)
        {

            string sqlCommand = $"DELETE FROM records Where RecordID='{id}'";
            var parameters = new DynamicParameters();
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {

                    int deleteTest = DeleteMedicalTest(id);
                    int deleteTreat = DeleteTreat(id);
                    if (deleteTest > 0 && deleteTreat > 0)
                    {
                        int numberOfAffectedRows = sqlConnection.Execute(sqlCommand, parameters);

                        if (numberOfAffectedRows > 0)
                        {
                            trans.Commit();
                            return id;

                        }
                        else
                        {
                            trans.Rollback();
                            return Guid.Empty;
                        }

                    }
                    trans.Rollback();
                    return Guid.Empty;
                }
            }
        }

        public int DeleteMedicalTest(Guid id)
        {
            string sqlCommand = $"DELETE FROM medicaltests Where RecordID='{id}'";
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
        public int DeleteTreat(Guid id)
        {
            string sqlCommand = $"DELETE FROM treatments Where RecordID='{id}'";
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

        public IEnumerable<dynamic> GetDetailMedicalTest(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                string className = EntityUtilities.GetTableName<MedicalTestsEntity>();
                string sqlCommand = $"SELECT * FROM {className} Where RecordID='{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.Query(sqlCommand);


            }
        }
        public IEnumerable<dynamic> GetDetailTreatment(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                string className = EntityUtilities.GetTableName<TreatmentsEntity>();
                string sqlCommand = $"SELECT * FROM {className} Where RecordID='{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.Query(sqlCommand);


            }
        }
    }
}