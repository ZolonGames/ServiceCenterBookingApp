using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// A service center record. Denoted by an ID, contains a name, a street address, an stores the center type ID and value.
    /// </summary>
    public class Center
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        [JsonIgnore]
        public int CenterTypeId { get; set; }
        public string CenterTypeValue { get; set; }
    }
}
