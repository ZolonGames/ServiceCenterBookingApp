using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceCenterBookingApp.Models;

namespace ServiceCenterBookingApp.Utilities
{
    /// <summary>
    /// A helper class made to handle intake of the Center JSON file.
    /// </summary>
    public class CenterJSONFile
    {
        public List<CenterJSONHelper> centers { get; set; }
        public List<CenterType> centerTypes { get; set; }
    }
}
