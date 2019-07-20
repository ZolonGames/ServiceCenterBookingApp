using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCenterBookingApp.Models
{
    /// <summary>
    /// Database Context for interaction with Entity Framework's In-Memory Database.
    /// Provides access to persistent records for Appointments, Centers, and CenterTypes.
    /// </summary>
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
            :base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Center> Centers { get; set; }
        public DbSet<CenterType> CenterTypes { get; set; }
    }
}
