using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// Simple validation error container object, stores an error code and a message to be returned when validation errors
    /// are found.
    /// </summary>
    public class ValidationError
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
