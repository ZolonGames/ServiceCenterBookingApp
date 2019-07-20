using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ServiceCenterBookingApp.Models;
using ServiceCenterBookingApp.Utilities;

namespace ServiceCenterBookingApp.Services
{
    /// <summary>
    /// Helper class designed to populate the list of service centers. In an application that used a persistent relational database
    /// this class may not be necessary.
    /// </summary>
    public class CenterDBPrep
    {
        public static void PrepCentersDB(APIContext context)
        {
            CenterJSONFile centerFile = JsonConvert.DeserializeObject<CenterJSONFile>(File.ReadAllText("Utilities/centers.json"));

            foreach (CenterType t in centerFile.centerTypes)
            {
                context.CenterTypes.Add(t);
            }

            // This iterates on the CenterJSONHelper, as the JSONIgnore portion of the Center object causes this to crash with
            // A null pointed exception when used directly.
            foreach (CenterJSONHelper c in centerFile.centers)
            {
                context.Centers.Add(new Center
                {
                    Id = c.Id,
                    Name = c.Name,
                    CenterTypeValue = context.CenterTypes.Find(c.CenterTypeId).Value,
                    StreetAddress = c.StreetAddress
                });
            }
            context.SaveChanges();
        }
    }
}
