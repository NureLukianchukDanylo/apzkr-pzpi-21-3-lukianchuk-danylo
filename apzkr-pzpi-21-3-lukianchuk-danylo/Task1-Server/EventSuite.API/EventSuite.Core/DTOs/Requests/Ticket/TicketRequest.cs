﻿using EventSuite.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSuite.Core.DTOs.Requests.Ticket
{
    public class TicketRequest
    {
        public decimal Price { get; set; }
        public TicketType? Type { get; set; }
        public int? RegistrationId { get; set; }
        public int? SmartBraceletId { get; set; }
    }
}
