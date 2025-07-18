using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public sealed class LoggerEventArgs : EventArgs
    {
        public string Data;

        public LoggerEventArgs(string data)
        {
            Data = data;
        }
    }
}
