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

        public virtual int UpdateRecord(T entity, Guid id)
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

                return numberOfAffectedRows;



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

        public T GetRecordByID(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                var idName = typeof(T).GetProperties().First().Name;
                string className = EntityUtilities.GetTableName<T>();
                string sqlCommand = $"SELECT * FROM {className} Where {idName}='{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.QueryFirstOrDefault<T>(sqlCommand, parameters);


            }
        }

        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <param name=""></param>
        /// <returns>mã code mới</returns>
        /// CreatedBy: HTTHOA(16/0333/2023)

        public string GetNewCode()
        {
            string tableName = EntityUtilities.GetTableName<T>();
          

            string newCode = "";
            // câu lệnh lấy code dài nhất
         

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                string storedProcedureName = $"Proc_{tableName}_GetMaxCode";
                string maxCode = sqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // nếu phần mã thay đổi bằng null thì tạo ra mã mới
                string prefix = EntityUtilities.GetPreFix<T>();
                if (maxCode == null) return $"{prefix}00001";

                // lấy vị trí xuất hiện của số đuôi
                int indexOfNumber = 0;
                for (int i = maxCode.Length - 1; i >= 0; i--)
                {
                    if (maxCode[i] < '0' || maxCode[i] > '9')
                    {
                        indexOfNumber = i + 1;
                        break;
                    }
                }

                // nếu tồn tài số đuôi thì thực hiện cộng 1 không có thì thực hiện thêm đuôi là 0
                if (indexOfNumber < maxCode.Length)
                {
                    // phần số thay đổi và độ dài của số đuôi
                    var autoNumber = maxCode.Substring(indexOfNumber);
                    int lengthOfAutoNumber = autoNumber.Length;

                    string newNumber = (Int64.Parse(autoNumber) + 1).ToString();

                    // nếu độ dài số đuôi nhỏ hơn số đuôi trước đó thì thực hiện nối '0' vào trước 
                    if (newNumber.Length < lengthOfAutoNumber)
                    {
                        for (int i = newNumber.Length; i < lengthOfAutoNumber; i++)
                        {
                            newNumber = '0' + newNumber;
                        }
                    }

                    newCode = maxCode.Substring(0, indexOfNumber) + newNumber;
                }
                else
                {
                    newCode = maxCode + '0';
                }
            }
            return newCode;
         
        }
        /// <summary>
        /// lấy mã code và check trùng mã
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(25/03/2023)
        public virtual bool CheckDuplicateCode(T record)
        {
            var duplicateID = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            var id = duplicateID?.GetValue(record);

            // lấy mã record
            var duplicateCode = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(DuplicateAttribute), true).Count() > 0);
            var code = duplicateCode?.GetValue(record);

            //lấy tên procedure
            string tableName = EntityUtilities.GetTableName<T>();
            string procedureName = $"Proc_{tableName}_DuplicateCode";

            //tạo các param
            string className = typeof(T).Name;
            var parameters = new DynamicParameters();
            parameters.Add($"v_Code", code);
            parameters.Add($"v_Id", id);


            using (var mySqlConnection = new MySqlConnection(_connectionDB))
            {
                // thực hiện chạy câu lệnh 
                var result = mySqlConnection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

           
        }

        #endregion
    }
}
