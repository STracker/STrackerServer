using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServer.DataAccessLayer.Core
{
    public class DuplicatedIdException : Exception
    {
        public DuplicatedIdException() : base("the Id already exists in databae")
        {
        }
    }
}
