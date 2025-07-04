﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TrackingService.DTO
{
    public class TrackingDataDto
    {
        public int BusId { get; set; }
        [Column(TypeName = "decimal(10,7)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(10,7)")]
        public decimal Longitude { get; set; }
    }
}
