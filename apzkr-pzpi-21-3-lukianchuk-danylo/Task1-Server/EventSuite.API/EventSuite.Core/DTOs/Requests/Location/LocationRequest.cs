﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSuite.Core.DTOs.Requests.Location
{
    public class LocationRequest
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? StreetType { get; set; }
        public string? BuildingNumber { get; set; }
    }
}
