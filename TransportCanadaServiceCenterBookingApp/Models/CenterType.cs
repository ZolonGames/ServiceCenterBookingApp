using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// A center type record. Denoted by an ID, and contains a value denoting the type of service center it is.
    /// </summary>
    public class CenterType
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
