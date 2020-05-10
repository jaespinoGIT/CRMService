using System;

namespace CRMService.Core
{
    public class ApplicationException : Exception
    {
        internal ApplicationException(string businessMessage) : base(businessMessage) { }
    }
}
