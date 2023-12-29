using Dapper;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace MEDITRACK.DL.AccountDL
{
    public class UserDL: BaseDL<UsersEntity>, IUserDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        public UsersEntity GetUsers(string userName)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                string className = EntityUtilities.GetTableName<UsersEntity>();
                string sqlCommand = $"SELECT * FROM {className} Where UserName='{userName}'";
                var parameters = new DynamicParameters();
                return sqlConnection.QueryFirstOrDefault<UsersEntity>(sqlCommand, parameters);


            }
        }

        public UsersEntity InsertUser(UsersEntity record)
        {
            var newId = Guid.NewGuid();
            var primary = typeof(UsersEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            if (primary != null)
            {
                primary.SetValue(record, newId);
            }

            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<UsersEntity>();
            string insertRecordProcedureName = $"Proc_{tableName}_Insert";
            // chuẩn bị tham số đầu vào 
            var properties = typeof(UsersEntity).GetProperties();
            var parameters = new DynamicParameters();
            List<UserDetailsEntity> details = new List<UserDetailsEntity>();
            foreach (var property in properties)
            {
                var value = property.GetValue(record); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                if (propertyName == "Detail")
                {
                    details = record.Detail;
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
                        int insert = this.InsertUserDetail(record.UserID, details, sqlConnection, trans: trans);
                        if (insert > 0)
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

        //public AppointmentEntity UpdateAppointment(AppointmentEntity record)
        //{

        //    string storeProcedureName = $"Proc_appointments_Update";
        //    var properties = typeof(AppointmentEntity).GetProperties();
        //    var parameters = new DynamicParameters();


        //    List<NotificationsEntity> notificationEntities = new List<NotificationsEntity>();
        //    foreach (var property in properties)
        //    {
        //        var propertyName = property.Name;
        //        var propertyValue = property.GetValue(record);

        //        if (propertyName == "Notice")
        //        {
        //            notificationEntities = record.Notice;
        //        }
        //        else
        //        {

        //            parameters.Add($"p_{propertyName}", propertyValue);
        //        }


        //    }
        //    using (var sqlConnection = new MySqlConnection(_connectionDB))
        //    {
        //        sqlConnection.Open();
        //        using (MySqlTransaction trans = sqlConnection.BeginTransaction())
        //        {
        //            int numberOfAffectedRows = 0;
        //            numberOfAffectedRows = sqlConnection.Execute(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

        //            if (numberOfAffectedRows > 0)
        //            {
        //                int deleteNotice = DeleteNoticeByPres(record.AppointmentID);
        //                if (deleteNotice > 0)
        //                {
        //                    int insertNotice = this.InsertNotice(record.AppointmentID, notificationEntities, sqlConnection, trans: trans);
        //                    if (insertNotice > 0)
        //                    {
        //                        trans.Commit();
        //                        return record;
        //                    }
        //                    else
        //                    {
        //                        trans.Rollback();
        //                        return record;
        //                    }
        //                }
        //                else
        //                {
        //                    trans.Rollback();
        //                    return record;
        //                }

        //            }
        //            trans.Rollback();
        //            return record;
        //        }
        //    }
        //}


        public int InsertUserDetail(Guid idMaster, List<UserDetailsEntity> details, MySqlConnection sqlConnection, MySqlTransaction trans)
        {

            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<UserDetailsEntity>();
            string insertMedicationStore = $"Proc_user_details_Insert";
            int result = 0;
            foreach (var detail in details)
            {
                var newId = Guid.NewGuid();
                var primary = typeof(UserDetailsEntity).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                if (primary != null)
                {
                    primary.SetValue(detail, newId);
                }

                // chuẩn bị tham số đầu vào 
                var properties = typeof(UserDetailsEntity).GetProperties();
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var value = property.GetValue(detail); // lấy giá trị của property

                    var propertyName = property.Name; // lấy tên của property
                    if (propertyName == "UserID")
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


        //public int DeleteNoticeByPres(Guid id)
        //{
        //    string sqlCommand = $"DELETE FROM notifications Where TypeID='{id}'";
        //    var parameters = new DynamicParameters();


        //    using (var sqlConnection = new MySqlConnection(_connectionDB))
        //    {
        //        sqlConnection.Open();

        //        // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
        //        int numberOfAffectedRows = sqlConnection.Execute(sqlCommand, parameters);
        //        if (numberOfAffectedRows > 0)
        //        {
        //            return 1;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}


        public UserDetailsEntity GetUserDetail(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                string className = EntityUtilities.GetTableName<UserDetailsEntity>();
                string sqlCommand = $"SELECT * FROM {className} Where UserID='{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.QueryFirstOrDefault<UserDetailsEntity>(sqlCommand, parameters);


            }
        }

        public Guid UpdateUserDetail(UserDetailsEntity entity, Guid id)
        {
            string tableName = EntityUtilities.GetTableName<UserDetailsEntity>();
            string storeProcedureName = $"Proc_{tableName}_Update";
            var properties = typeof(UserDetailsEntity).GetProperties();
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
    }
}
