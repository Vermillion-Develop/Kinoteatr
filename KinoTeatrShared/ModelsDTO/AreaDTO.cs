using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class AreaDTO
    {
        public int Id { get; set; }
        public int? TypeAreaId { get; set; }
        public string? Adress { get; set; }
        public int? StreetId { get; set; }
        public decimal? DayCost { get; set; }
        public int? Capacity { get; set; }

        public string? TypeAreaName { get; set; }
        public string? StreetName { get; set; }
    }
}
