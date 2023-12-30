using MEDITRACK.BL;
using MEDITRACK.COMMON.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.DL.PrescriptionDL
{
    public interface IPrescriptionDL : IBaseDL<PrescriptionEntity>
    {
        public PagingData FilterChoose(
      string? keyword,
      int? pageSize,
      int? pageNumber, Guid id, int? status

      );
        public IEnumerable<dynamic> GetDetailMedication(Guid id);
        public PrescriptionEntity InsertPrescription(PrescriptionEntity record);
        public PrescriptionEntity UpdatePrescription(PrescriptionEntity record);
    }
}
