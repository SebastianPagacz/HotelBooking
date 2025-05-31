using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() : base("User already exists") { }
    public UserAlreadyExistsException(Exception innerException) : base("User already exists", innerException) { }
}
