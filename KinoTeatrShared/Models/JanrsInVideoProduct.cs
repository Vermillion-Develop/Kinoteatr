using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class JanrsInVideoProduct
    {
        public int Id { get; set; }
        public int? VideoProductId { get; set; }
        public int? JanrId { get; set; }
    }
}
