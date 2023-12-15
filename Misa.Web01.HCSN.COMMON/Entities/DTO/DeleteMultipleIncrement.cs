using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON.Entities.DTO
{
    public class DeleteMultipleIncrement
    {
        public List<Guid> listIncrementDeleted { get; set; }
        public List<Guid> listFixedAssetUpdate { get; set; }
    }
    
}
