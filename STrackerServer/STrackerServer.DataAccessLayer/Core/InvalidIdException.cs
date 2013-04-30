using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServer.DataAccessLayer.Core
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException()
            : base("the Id already exists in databae")
        {
        }
    }
}
