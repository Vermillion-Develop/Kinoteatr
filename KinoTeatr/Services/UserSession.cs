using KinoTeatrShared.Models;
using KinoTeatrShared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoTeatr.Services
{
    public static class UserSession
    {
        public static StaffDTO? CurrentStaff { get; set; }
    }
}
