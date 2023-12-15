using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON.Entities.DTO
{
    public class PagingAsset<T>
    {
        /// <summary>
        /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        public long TotalCount { get; set; }
        public long TotalPage { get; set; }

        public long TotalCost { get; set; }

        public PagingAsset()
        {
            Data = new List<T>();
            TotalCount = 0;
        }
    }
}
