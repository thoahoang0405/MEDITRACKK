using MEDITRACK.BL;
using MEDITRACK.COMMON;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Entities.DTO;
using MEDITRACK.COMMON.Resource;
using MEDITRACK.COMMON;
using MEDITRACK.COMMON.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region contructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
        #endregion
        #region method
        /// <summary>
        /// API thêm mới bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public Guid InsertRecord(T record)
        {
           
                return  _baseDL.InsertRecord(record);
              
            

        }


        /// sửa 1  bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(17/03/2023)
        public Guid UpdateRecord(T entity, Guid id)
        {

            return (_baseDL.UpdateRecord(entity, id));  
           
        }
        /// <summary>
        /// lấy tất cả bản  ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(15/03/2023)
        public IEnumerable<dynamic> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }
        /// <summary>
        /// xóa bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public int DeleteRecordID(Guid id)
        {
            return _baseDL.DeleteRecordID(id);
        }
        /// <summary>
        /// lấy bản ghi theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public IEnumerable<dynamic> GetRecordByID(Guid id)
        {
            return _baseDL.GetRecordByID(id);
        }


       
      

        #endregion
    }
}
