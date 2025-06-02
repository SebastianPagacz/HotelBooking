using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Exceptions;

public class InvalidCredentialException : Exception
{
    public InvalidCredentialException() : base("User already exists") { }
    public InvalidCredentialException(Exception innerException) : base("User already exists", innerException) { }
}
