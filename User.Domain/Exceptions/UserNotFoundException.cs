using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User was not found") { }
    public UserNotFoundException(Exception innerException) : base("User was not found", innerException) { }
}
