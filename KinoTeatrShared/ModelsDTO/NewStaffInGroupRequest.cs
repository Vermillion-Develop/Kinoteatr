using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.ModelsDTO
{
    public class NewStaffInGroupRequest
    {
        public int? StaffId { get; set; }
        public int? GroupId { get; set; }
        public int? RoleId { get; set; }
        public int? ProductId { get; set; }

    }
}
