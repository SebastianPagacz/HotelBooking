﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.Seeders;

public interface IHotelBookingSeeder
{
    Task SeedAsync();
}
