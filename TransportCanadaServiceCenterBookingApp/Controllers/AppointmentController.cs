using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceCenterBookingApp.Models;
using ServiceCenterBookingApp.Services;

namespace ServiceCenterBookingApp.Controllers
{
    /// <summary>
    /// Controller for Appointment related functions.
    /// Provides Get (All), Get (Individual), Post (Create), Put (Update), and Delete functionality.
    /// </summary>
    [Route("appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        // Database Context
        private readonly APIContext _context;

        /// <summary>
        /// Constructor for Appointment Controller. Takes in an injected APIContext, then checks to see if the database
        /// has been populated with service centers from the provided JSON file. If it hasn't, it calls the CenterDBPrep helper class
        /// function PrepCentersDB, sending in the injected context for manipulation.
        /// </summary>
        /// <param name="context">An injected APIContext provided through registered services.</param>
        public AppointmentController(APIContext context)
        {
            _context = context;
            
            if(_context.Centers.Count() == 0)
            {
                CenterDBPrep.PrepCentersDB(_context);
            }
        }

        /// <summary>
        /// Provides a list of all appointments.
        /// </summary>
        /// <returns>A valid response containing a list of all available appointments.</returns>
        [HttpGet]
        public ActionResult<List<Appointment>> GetAll()
        {
            // Required code to assure relationships between appointments and centers are preserved.
            // Without this line, centers will be listed as null.
            var centers = _context.Centers.ToList();

            return _context.Appointments.ToList();
        }

        /// <summary>
        /// Accessed at /appointments/{ID}. Allows viewing of a single appointment record.
        /// </summary>
        /// <param name="id">The ID of the requested appointment, as noted in the URL.</param>
        /// <returns>Either a 404 not found if the requested appointment
        /// does not exist, or a valid appointment in JSON format.</returns>
        [HttpGet("{id}", Name = "GetAppointment")]
        public ActionResult<Appointment> GetById(int id)
        {
            // Required code to assure relationships between appointments and centers are preserved.
            // Without this line, centers will be listed as null.
            var centers = _context.Centers.ToList();

            var apt = _context.Appointments.Find(id);
            if (apt == null)
            {
                return NotFound();
            }

            return apt;
        }

        /// <summary>
        /// Creates a new Appointment record. Passes the record through validation to ensure data integrity.
        /// </summary>
        /// <param name="aptParams">Required Parameters for appointment creation, including client name, date, and centerID.</param>
        /// <returns>201 Created response if successful, with the created record, or a 400 bad request error if it fails.</returns>
        [HttpPost]
        public IActionResult Create(AppointmentParameters aptParams)
        {
            //Create a new appointment
            Appointment apt = new Appointment();
            apt.ClientFullName = aptParams.ClientFullName;
            apt.Date = aptParams.Date;
            apt.Center = _context.Centers.Find(aptParams.CenterID);

            // Check to ensure the appointment is valid
            List<ValidationError> errors = ValidateAppointment(apt);

            // If there are no errors, then the appointment is created and saved.
            if (errors.Count == 0)
            {
                _context.Appointments.Add(apt);
                _context.SaveChanges();

                return CreatedAtRoute("GetAppointment", new { id = apt.Id }, apt);
            }
            else
            {
                // Otherwise, return a 400 error with a list of errors.
                return BadRequest(errors);
            }
        }

        /// <summary>
        /// Updates a given record with the key {id}, using the provided parameters.
        /// </summary>
        /// <param name="id">The ID of the record to be changed.</param>
        /// <param name="aptParams">Required Parameters for appointment creation, including client name, date, and centerID.</param>
        /// <returns>204 No Content Response if successful, 400 Bad Request response if it fails, including error messages.</returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, AppointmentParameters aptParams)
        {
            // Required code to assure relationships between appointments and centers are preserved.
            // Without this line, centers will be listed as null.
            var centers = _context.Centers.ToList();

            Appointment modItem = _context.Appointments.Find(id);

            // Item does not exist if null
            if (modItem == null)
            {
                return NotFound();
            }

            // Set updated fields
            modItem.ClientFullName = aptParams.ClientFullName;
            modItem.Date = aptParams.Date;
            modItem.Center = _context.Centers.Find(aptParams.CenterID);

            // Validate record is valid
            List<ValidationError> errors = ValidateAppointment(modItem);

            // If Valid, update the record and save. Return 204.
            if (errors.Count == 0)
            {
                _context.Appointments.Update(modItem);
                _context.SaveChanges();

                return NoContent();
            }
            else
            {
                // Else, return 400 Bad Request and error list.
                return BadRequest(errors);
            }   
        }

        /// <summary>
        /// Deletes a given record with the provided ID from the database.
        /// </summary>
        /// <param name="id">The ID of the record to be deleted.</param>
        /// <returns>404 Not found if the record is not found, or 204 No content upon success.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Appointments.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(item);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Validates an appointment to ensure it does not violate any constraints, such as a non-existent service center,
        /// an invalid date format, or a date/center conflict.
        /// </summary>
        /// <param name="apt">An appointment to validate</param>
        /// <returns>A list full of errors. Potential Error Codes:
        /// 1: Center does not exist.
        /// 2: Date is not in valid format. (yyyy-dd-MM)
        /// 3: Appointment already exists at that center for the provided date.
        /// </returns>
        private List<ValidationError> ValidateAppointment(Appointment apt)
        {
            // Pull in centers to ensure linkage doesn't break between centers and appointments...
            var centers = _context.Centers.ToList();
            
            List<ValidationError> errors = new List<ValidationError>();

            // Error Code 1: Center Does Not Exist.
            if (apt.Center == null)
            {
                errors.Add(new ValidationError { Code = 1, Message = "Center does not exist" });
            }
            // Out variable for DateTime.TryParseExact
            DateTime testDate = new DateTime();

            //Error Code 2: Date is not in valid format.
            if (!DateTime.TryParseExact(apt.Date, "yyyy-dd-MM", new CultureInfo("en-US"), DateTimeStyles.None, out testDate))
            {
                errors.Add(new ValidationError { Code = 2, Message = "Date is not in valid format. Please format date as yyyy-dd-MM" });
            }

            //Error Code 3: Appointment already exists at center for that date.
            foreach(Appointment a in _context.Appointments.ToList())
            {
                if (a.Id != apt.Id)
                {
                    if ((a.Center.Id == apt.Center.Id) && (a.Date == apt.Date))
                    {
                        errors.Add(new ValidationError { Code = 3, Message = "An appointment already exists at that center for that date." });
                    }
                }
            }
            return errors;
        }
    }
}