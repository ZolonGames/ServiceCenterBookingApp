using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceCenterBookingApp.Models;
using ServiceCenterBookingApp.Services;

namespace ServiceCenterBookingApp.Controllers
{
    /// <summary>
    /// Controller for Center related functions.
    /// Provides Get (All), Get (Individual) functionality.
    /// </summary>
    [Route("centers")]
    [ApiController]
    public class CenterController : ControllerBase
    {
        private readonly APIContext _context;

        /// <summary>
        /// Constructor for Center Controller. Takes in an injected APIContext, then checks to see if the database
        /// has been populated with service centers from the provided JSON file. If it hasn't, it calls the CenterDBPrep helper class
        /// function PrepCentersDB, sending in the injected context for manipulation.
        /// </summary>
        /// <param name="context">An injected APIContext provided through registered services.</param>
        public CenterController(APIContext context)
        {
            _context = context;

            if (_context.Centers.Count() == 0)
            {
                CenterDBPrep.PrepCentersDB(_context);
            }
        }

        /// <summary>
        /// Gets a list of all centers.
        /// </summary>
        /// <returns>A list of all service centers.</returns>
        [HttpGet]
        public ActionResult<List<Center>> GetAll()
        {
            return _context.Centers.ToList();
        }

        /// <summary>
        /// Gets a specific service center by ID.
        /// </summary>
        /// <param name="id">The ID of the service center to look up.</param>
        /// <returns>404 Not Found if the record does not exist, or the JSON formatted data on a success.</returns>
        [HttpGet("{Id}", Name = "GetCenter")]
        public ActionResult<Center> GetCenter(int id)
        {
            var ctr = _context.Centers.Find(id);
            if (ctr == null)
            {
                return NotFound();
            }

            return ctr;
        }
    }
}