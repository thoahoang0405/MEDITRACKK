using MEDITRACK.COMMON.Entities;
using MEDITRACK.DL.RecordDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.RecordBL
{
    public class RecordBL: BaseBL<RecordsEntity>, IRecordBL
    {

        #region contructor
        private IRecordDL _recordDL;
        public RecordBL(IRecordDL recordDL) : base(recordDL)
        {
            _recordDL = recordDL;
        }

        #endregion
        public PagingData FilterChoose(
       string? keyword,
       int? pageSize,
       int? pageNumber

       )
        {
            return _recordDL.FilterChoose( keyword, pageSize, pageNumber );
        }
        public RecordsEntity InsertRecord(RecordsEntity record)
        {
            return _recordDL.InsertRecord(record);
        }
        public RecordsEntity UpdateRecord(RecordsEntity record)
        {
            return _recordDL.UpdateRecord(record);  
        }

    }
}
