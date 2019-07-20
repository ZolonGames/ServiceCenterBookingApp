using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// An appointment record. Denoted by an ID, contains a client name, a date, and an associated service center.
    /// </summary>
    public class Appointment
    {
        public int Id { get; set; }
        public string ClientFullName { get; set; }
        public string Date { get; set; }
        public Center Center { get; set; }
    }
}
