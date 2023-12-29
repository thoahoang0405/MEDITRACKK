using MEDITRACK.COMMON.Entities;
using MEDITRACK.DL.PrescriptionDL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.PrescriptionBL
{
    public class PrescriptionBL:BaseBL<PrescriptionEntity>, IPrescriptionBL
    {
        #region contructor
        private IPrescriptionDL _prescriptionDL;
        public PrescriptionBL(IPrescriptionDL prescriptionDL) : base(prescriptionDL)
        {
            _prescriptionDL = prescriptionDL;
        }

        #endregion

        public PagingData FilterChoose(
      string? keyword,
      int? pageSize,
      int? pageNumber, Guid id

      )

        {
            return _prescriptionDL.FilterChoose( keyword, pageSize, pageNumber ,id);
        }
        public IEnumerable<dynamic> GetDetailMedication(Guid id)
        {
            return _prescriptionDL.GetDetailMedication(id );
        }
        public  PrescriptionEntity InsertPrescription(PrescriptionEntity record)
        {
            return  _prescriptionDL.InsertPrescription(record);
        }
        public PrescriptionEntity UpdatePrescription(PrescriptionEntity record)
        {
            return _prescriptionDL.UpdatePrescription(record);
        }

    }
}
