using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core.Exceptions.Services
{
    public interface ICustomExceptionService
    {
        void ThrowItemNotFoundException(string message);
        void ThrowArgumentNullException(string message);
        void ThrowInvalidOperationException(string message);
        
    }
}
