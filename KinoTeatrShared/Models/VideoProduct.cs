using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class VideoProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Budget { get; set; }
        public int? VidVideoProductId { get; set; }
        public DateTimeOffset? DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public int? StatusVideoProductId { get; set; }
        public int? AdultVideoProductId { get; set; }
        public string? Idea { get; set; }
    }
}
