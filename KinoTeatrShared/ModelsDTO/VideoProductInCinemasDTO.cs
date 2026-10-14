using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class VideoProductInCinemasDTO
    {
        public int Id { get; set; }
        public int? VideoProductId { get; set; }
        public int? CinemaId { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Rating { get; set; }

        public string? VideoName { get; set; }
        public string? CinemaName { get; set; }
        public DateTimeOffset? DateProduct { get; set; }
    }
}
