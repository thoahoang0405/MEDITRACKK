using Dapper;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.DL.NoticeDL
{
    public class NotificationDL: BaseDL<NotificationsEntity>, INotificationDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        public IEnumerable<dynamic> GetAppointmentDay(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                string className = EntityUtilities.GetTableName<NotificationsEntity>();
                string sqlCommand = $"SELECT * FROM {className} Where NoticeDate <=  CURDATE() AND ToDate >= CURDATE()  AND UserID = '{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.Query(sqlCommand);


            }
        
    }

        public IEnumerable<dynamic> GetPrescriptionDay(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                string className = EntityUtilities.GetTableName<PrescriptionEntity>();
                string sqlCommand = $"SELECT * FROM {className} Where FromDate <= CURRENT_TIMESTAMP() AND ToDate >= CURRENT_TIMESTAMP() AND UserID = '{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.Query(sqlCommand);


            }

        }
    }
}
