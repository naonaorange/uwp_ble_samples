using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uwp_ble_gatt_read
{
    class SensorTagUuid
    {
        // https://sensortag.codeplex.com/SourceControl/latest#SensorTagLibrary/SensorTagLibrary/Source/SensorTagUuid.cs
        public const string UuidIrtService = "f000aa00-0451-4000-b000-000000000000";
        public const string UuidIrtData = "f000aa01-0451-4000-b000-000000000000";
        public const string UuidIrtConf = "f000aa02-0451-4000-b000-000000000000";
    }
}
