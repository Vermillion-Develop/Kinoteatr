using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class CinemaGroupDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? VideoId { get; set; }
        public string? VideoName { get; set; }
    }
}
