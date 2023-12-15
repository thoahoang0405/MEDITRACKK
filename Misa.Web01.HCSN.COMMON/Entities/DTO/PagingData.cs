namespace MEDITRACK.COMMON.Entities
{
    public class PagingData
    {
        
       
        /// <summary>
        /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang 
        /// </summary>
        /// CreatedBy:HTTHOA(16/08/2022)
            public List<object> Data { get; set; } = new List<object>();


        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        /// CreatedBy:HTTHOA(16/08/2022)

        public long TotalRecords { get; set; }

       
      
        /// <summary>
        /// Tổng số trang
        /// </summary>
        /// CreatedBy:HTTHOA(16/08/2022)
        public int TotalPages { get; set; }
        
    }
}
