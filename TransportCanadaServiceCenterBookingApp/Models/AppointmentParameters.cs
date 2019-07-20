using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// Helper Class for use with gathering certain information for POST and PUT requests on Appointments.
    /// </summary>
    public class AppointmentParameters
    {
        public string ClientFullName { get; set; }
        public string Date { get; set; }
        public int CenterID { get; set; }
    }
}
