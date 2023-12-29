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
       int? pageNumber,
       Guid id

       )
        {
            return _recordDL.FilterChoose( keyword, pageSize, pageNumber,id );
        }
        public RecordsEntity InsertRecord(RecordsEntity record)
        {
            return _recordDL.InsertRecord(record);
        }
        public RecordsEntity UpdateRecord(RecordsEntity record)
        {
            return _recordDL.UpdateRecord(record);  
        }
        public IEnumerable<dynamic> GetDetailMedicalTest(Guid id)
        {
            return _recordDL.GetDetailMedicalTest(id);
        }
        public IEnumerable<dynamic> GetDetailTreatment(Guid id)
        {
            return _recordDL.GetDetailTreatment(id);
        }
        public Guid DeleteRecords(Guid id)
        {
            return _recordDL.DeleteRecords(id);
        }
}
}
