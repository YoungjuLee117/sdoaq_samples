using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
	public class Logger : IDisposable
	{
		public enum emLogLevel
		{
			Info,
            User,
			API,
			Warning,
			Error,
			Exception,
		};

		public event EventHandler<LoggerEventArgs> DataReceived;
		private static QueueWorker<string> _queue;

		public Logger()
		{
			_queue = new QueueWorker<string>("LoggerQueueWorkr");

			_queue.MessageProcessed += Queue_MessageProcessed;
		}

        public void WriteLog(string log)
		{
            _queue?.Enq_Msg($"[{DateTime.Now.ToString("HH:mm:ss.fff")}]{log}{Environment.NewLine}");	
		}

        private void Queue_MessageProcessed(object sender, QueueWorkerMessageEventArgs<string> e)
        {
            DataReceived?.Invoke(this, new LoggerEventArgs(e.Item));
        }

        public void Dispose()
		{
			if (_queue != null)
			{
                _queue.MessageProcessed -= Queue_MessageProcessed;
                _queue.Stop();
                _queue = null;
			}
		}
	}
}
