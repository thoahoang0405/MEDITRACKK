﻿using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.PrescriptionBL
{
    public interface IPrescriptionBL : IBaseBL<PrescriptionEntity>
    {
        public PagingData FilterChoose(
    string? keyword,
    int? pageSize,
    int? pageNumber

    );
        public PrescriptionEntity InsertPrescription(PrescriptionEntity record);
        public PrescriptionEntity UpdatePrescription(PrescriptionEntity record);
    }
}
