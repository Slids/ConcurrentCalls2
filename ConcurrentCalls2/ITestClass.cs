using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentCalls2
{
    interface ITestClass
    {
        void makeCalls(int numCalls, int numUsers);

    }
}
