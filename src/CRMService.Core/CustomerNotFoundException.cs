using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core
{
    internal sealed class CustomerNotFoundException : ApplicationException
    {
        internal CustomerNotFoundException(string message) : base(message) { }
    }
}
