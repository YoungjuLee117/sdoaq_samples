using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public sealed class QueueWorkerMessageEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public QueueWorkerMessageEventArgs(T item) => Item = item;
    }
}
