using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class ZatratDTO
    {
        public int Id { get; set; }
        public int? TypeZatratId { get; set; }
        public decimal? Cost { get; set; }
        public DateTimeOffset? Date { get; set; }
        public int? VideoProductId { get; set; }

        public string? VideoName { get; set; }
        public string? TypeName { get; set; }


    }
}
