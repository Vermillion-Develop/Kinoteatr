using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class Area
    {
        public int Id { get; set; }
        public int? TypeAreaId { get; set; }
        public string? Adress { get; set; }
        public int? StreetId { get; set; }
        public decimal? DayCost { get; set; }
        public int? Capacity { get; set; }
    }
}
