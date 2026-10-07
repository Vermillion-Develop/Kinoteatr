using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatrShared.Models
{
    public class GraphicVideo
    {
        public int Id { get; set; }
        public int? StaffGroupId { get; set; }
        public int? VideoProductId { get; set; }
        public int? AreaId { get; set; }
        public int? SceneId { get; set; }
        public DateOnly? Date { get; set; }
        public TimeSpan? Time { get; set; }
    }
}
