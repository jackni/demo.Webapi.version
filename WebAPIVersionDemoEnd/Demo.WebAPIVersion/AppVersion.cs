using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WebAPIVersion
{
    public class AppVersion
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public string PreReleaseTag { get; set; }
        public string ContractVersion => $"{Major}.{Minor}";
        public string FullVersion => $"{Major}.{Minor}.{Patch}-{PreReleaseTag}";
    }
}
