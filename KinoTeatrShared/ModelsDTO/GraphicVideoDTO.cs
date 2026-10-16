using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class GraphicVideoDTO
    {
        public int Id { get; set; }
        public int? StaffGroupId { get; set; }
        public int? VideoProductId { get; set; }
        public int? AreaId { get; set; }
        public int? SceneId { get; set; }
        public DateTimeOffset? Date { get; set; }
        public TimeSpan? Time { get; set; }

        public string? StaffName { get; set; }
        public string? VideoName { get; set; }
        public string? AreaName { get; set; }
        public string? SceneName { get; set; }
        public string? StreetName { get; set; }
        public string? TownName { get; set; }
    }
}
