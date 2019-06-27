using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leopard.Template.WebAPI.Utils
{
    public class LocalSystemClock : Microsoft.Extensions.Internal.ISystemClock
    {
        public DateTimeOffset UtcNow => DateTime.Now;
    }
}
