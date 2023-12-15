using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.RecordBL
{
    public interface IRecordBL : IBaseBL<RecordsEntity>
    {
        public PagingData FilterChoose(
       string? keyword,
       int? pageSize,
       int? pageNumber

       );
        public RecordsEntity InsertRecord(RecordsEntity record);
        public RecordsEntity UpdateRecord(RecordsEntity record);
    }
}
