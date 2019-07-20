using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// A helper class to gather in centers from a JSON file.
    /// </summary>
    public class CenterJSONHelper
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public int CenterTypeId { get; set; }
    }
}
