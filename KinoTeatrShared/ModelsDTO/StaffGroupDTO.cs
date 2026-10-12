using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class StaffGroupDTO
    {
        public int Id { get; set; }
        public int? StaffId { get; set; }
        public int? VideoProductId { get; set; }
        public int? SpecializationId { get; set; }
        public int? GroupId { get; set; }

        public string? VideoProductName { get; set; }
        public string? StaffFamily { get; set; }
        public string? StaffName { get; set; }
        public string? StaffFather { get; set; }
        public string? StaffRoleName { get; set; }
        public decimal? StaffOklad { get; set; }
    }
}
