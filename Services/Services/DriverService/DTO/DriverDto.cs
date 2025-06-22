
using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.DriverService.DTO
{
    public class DriverDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public DriverStatus Status { get; set; }
    }
}
