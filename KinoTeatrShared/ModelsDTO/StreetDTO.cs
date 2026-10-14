using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class StreetDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TownId { get; set; }
        public string? TownName { get; set; }
        public string? FullName => $"{Name} {TownName}";
    }
}
