using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Exceptions.Services
{
    public class CustomExceptionService : ICustomExceptionService
    {
        public void ThrowItemNotFoundException(string message)
        {
            throw new ItemNotFoundException(message);
        }

        public void ThrowArgumentNullException(string message)
        {
            throw new ArgumentNullException(message);
        }

        public void ThrowInvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}
