using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class StaffGroup
    {
        public int Id { get; set; }
        public int? StaffId { get; set; }
        public int? VideoProductId { get; set; }
        public int? GroupId { get; set; }
        public int? SpecializationId { get; set; }
    }
}
