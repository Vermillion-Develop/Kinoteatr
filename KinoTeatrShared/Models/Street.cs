using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class Street
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TownId { get; set; }
    }
}
