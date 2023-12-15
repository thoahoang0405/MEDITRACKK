using MEDITRACK.COMMON.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON
{
    public static class MISAResource
    {
        public static string GetResource(string key)
        {
            return MEDITRACK.COMMON.Resource.Resource.ResourceManager.GetString(key);
        }
    }
}
