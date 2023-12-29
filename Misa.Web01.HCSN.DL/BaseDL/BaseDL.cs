using Dapper;
using Google.Protobuf.WellKnownTypes;
using MEDITRACK.DL;
using MEDITRACK.COMMON.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MEDITRACK.BL.BaseBL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;
        ///<T> reneric
        #endregion

        #region method
        /// <summary>
        /// thêm bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        ///  CreatedBy: HTTHOA(16/03/2023)
        public virtual Guid InsertRecord(T record)
        {
            var newId = Guid.NewGuid();
            var primary = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            if (primary != null)
            {
                primary.SetValue(record, newId);
            }
           
            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<T>();
            string insertRecordProcedureName = $"Proc_{tableName}_Insert";
            // chuẩn bị tham số đầu vào 
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                var value = property.GetValue(record); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                parameters.Add($"p_{propertyName}", value);
            }

            // thực hiện gọi vào DB
            int numberOfAffetedRows = 0;
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                numberOfAffetedRows = sqlConnection.Execute(insertRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfAffetedRows > 0)
                {
                    return newId;
                }
                return Guid.Empty;

            }

        }
       

        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)

        public Guid UpdateRecord(T entity, Guid id)
        {
            string tableName = EntityUtilities.GetTableName<T>();
            string storeProcedureName = $"Proc_{tableName}_Update";
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();


            foreach (var property in properties)
            {
                string propertyName = $"p_{property.Name}";
                var propertyValue = property.GetValue(entity);
                parameters.Add(propertyName, propertyValue);
            }
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                int numberOfAffectedRows = 0;
                numberOfAffectedRows = sqlConnection.Execute(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return id;



            }
        }

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public IEnumerable<dynamic> GetAllRecords()
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                string className = EntityUtilities.GetTableName<T>();
                var getAllRecords = $"SELECT * FROM {className}";
                var records = sqlConnection.Query(getAllRecords);

                return records;
            }


        }
        /// <summary>
        /// API xóa 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)

        public int DeleteRecordID(Guid id)
        {

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                var idName = typeof(T).GetProperties().First().Name;
                string className = EntityUtilities.GetTableName<T>();
                string sqlCommand = $"DELETE FROM {className} Where {idName}='{id}'";
                var parameters = new DynamicParameters();


                // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
                int numberOfAffectedRows = sqlConnection.Execute(sqlCommand, parameters);

                return numberOfAffectedRows;
            }


        }
        /// <summary>
        /// Lấy theo ID
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)

        public IEnumerable<dynamic> GetRecordByID(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                var idName = typeof(T).GetProperties().First().Name;
                string className = EntityUtilities.GetTableName<T>();
                string sqlCommand = $"SELECT * FROM {className} Where UserID='{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.Query(sqlCommand);


            }
        }

       
        #endregion
    }
}
