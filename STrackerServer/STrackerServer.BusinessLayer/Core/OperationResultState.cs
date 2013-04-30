using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServer.BusinessLayer.Core
{
    public enum OperationResultState
    {
        InProcess,

        Completed,

        Error,

        NotFound
    }
}
