using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class Zatrat
    {
        public int Id { get; set; }
        public string? TypeZatratId { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Date { get; set; }
    }
}
