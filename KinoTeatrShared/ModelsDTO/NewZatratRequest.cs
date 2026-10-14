using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class NewZatratRequest
    {
        public int Id { get; set; }
        public int? TypeZatratId { get; set; }
        public decimal? Cost { get; set; }
        public DateTimeOffset? Date { get; set; }
        public int? VideoProductId { get; set; }
    }
}
