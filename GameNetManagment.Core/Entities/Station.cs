using GameNetManagment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameNetManagment.Core.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? IpAddress { get; set; } // برای کلاینت PC ها
        public StationStatus Status { get; set; }
    }
}
