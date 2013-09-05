using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSPTurkey13
{
    public class NotFoundOnIsolatedStorageException : Exception
    {
        public NotFoundOnIsolatedStorageException(string Message)
            : base(Message)
        {
        }
    }
}
