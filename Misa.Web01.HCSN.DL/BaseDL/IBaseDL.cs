using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL
{
    public interface IBaseDL<T>

    {
        /// <summary>
        /// API thêm mới bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(10/03/2023)
        public Guid InsertRecord(T record);

        /// <summary>
        /// API sửa bản ghi
        /// </summary>
        /// <param name="entity, id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(15/03/2023)
        public Guid UpdateRecord(T entity,Guid id);

        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public IEnumerable<dynamic> GetAllRecords();

        /// <summary>
        /// API lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public IEnumerable<dynamic> GetRecordByID(Guid id);

       

        /// <summary>
        /// API xóa bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public int DeleteRecordID(Guid id);

       
    }
}
