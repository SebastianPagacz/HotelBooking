using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.Services;

public interface IEmailService
{
   /// <summary>
   /// 
   /// </summary>
   /// <param name="to"></param>
   /// <param name="from"></param>
   /// <param name="subject"></param>
   /// <param name="message"></param>
   /// <returns></returns>
    public void Send(string to, string from, string subject, string message);
}
