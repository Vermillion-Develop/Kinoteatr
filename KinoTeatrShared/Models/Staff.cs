using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string? Family { get; set; }
        public string? Name { get; set; }
        public string? Father { get; set; }
        public int? SpecializationId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public decimal? Stavka { get; set; }
        public string? DataLogId { get; set; }
    }
}
