using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class SeriaList
    {
        public int Id { get; set; }
        public int? SeriaId { get; set; }
        public int? SezonId { get; set; }   
        public int? VideoProductId { get; set; }

    }
}
