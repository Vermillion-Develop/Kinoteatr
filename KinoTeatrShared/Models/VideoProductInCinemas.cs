using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class VideoProductInCinemas
    {
        public int Id { get; set; }
        public int? VideoProductId { get; set; }
        public int? CinemaId { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Rating { get; set; }
    }
}
