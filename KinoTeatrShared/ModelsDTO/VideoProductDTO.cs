using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class VideoProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Budget { get; set; }
        public int? VidVideoProductId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? StatusVideoProductId { get; set; }
        public int? AdultVideoProductId { get; set; }
        public string? Idea { get; set; }

        public string? StatusVideoName { get; set; }
        public string? AdultVideoName { get; set; }
        public string? VidVideoName { get; set; }

    }
}
