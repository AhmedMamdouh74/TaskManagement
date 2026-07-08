using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Common.Exceptions
{
    public class ServerInternalException:Exception
    {
        public ServerInternalException(string message):base(message) {}
    }
}
