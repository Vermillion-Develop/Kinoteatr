using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.ModelsDTO
{
    public class NewGroupRequest
    {
        public string? Name { get; set; }
        public int? ProductId { get; set; }
        public int? GroupId { get; set; }
    }
}
