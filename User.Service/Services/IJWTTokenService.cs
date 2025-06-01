using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace User.Application.Services;

public interface IJWTTokenService
{
    string GenerateToken(int userId, List<string> roles);
}
