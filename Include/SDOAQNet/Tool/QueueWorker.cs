using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SDOAQNet.Tool
{
    public sealed class QueueWorker<T> : IDisposable
    {
        public event EventHandler<QueueWorkerMessageEventArgs<T>> MessageProcessed;

        public string Name { get; }

        private int _sizeofQueue = 0;
        public int SizeofQueue => _sizeofQueue;

        private ConcurrentQueue<QueueMsg> _queue = new ConcurrentQueue<QueueMsg>();
        private Thread _thrQueue;
        private ManualResetEventSlim _evtWaitQueue = new ManualResetEventSlim(false);
        private bool _bThreadStop = false;
        private bool _bDisposedValue = false;

        private class QueueMsg : IDisposable
        {
            public T Item;
            public ManualResetEventSlim WaitMsgLoop;
            public QueueMsg(T item, bool bWaitMsgLoop = false)
            {
                Item = item;
                if (bWaitMsgLoop)
                {
                    WaitMsgLoop = new ManualResetEventSlim(false);
                }
            }

            public void Dispose()
            {
                Item = default(T);
                WaitMsgLoop?.Dispose();
            }
        }

        private const int WAIT_STOP_QUEUE_EVENT = 1000;

        public QueueWorker(string name, bool bQueueStart = true)
        {
            Name = name;

            if (bQueueStart)
            {
                _thrQueue = new Thread(ThreadQueueMain);
                _thrQueue.IsBackground = true;
                _thrQueue.Start();
            }
        }

        public void Start()
        {
            if (_thrQueue == null)
            {
                _thrQueue = new Thread(ThreadQueueMain);
                _thrQueue.IsBackground = true;
            }

            if (_thrQueue.IsAlive == false)
            {
                _thrQueue.Start();
            }
        }

        public void Stop()
        {
            _bThreadStop = true;

            if (_thrQueue != null && _thrQueue.IsAlive)
            {
                _evtWaitQueue.Set();

                if (_thrQueue.Join(WAIT_STOP_QUEUE_EVENT) == false)
                {
                    _thrQueue.Abort();
                }
                _thrQueue = null;
            }
        }


        public void Enq_Msg(T item)
        {
            Push_Msg(new QueueMsg(item));
        }

        public void Run_Msg(T item)
        {
            var msg = new QueueMsg(item, true);
            Push_Msg(msg);

            msg.WaitMsgLoop.Wait();
        }

        private void Push_Msg(QueueMsg msg)
        {
            _queue.Enqueue(msg);
            Interlocked.Increment(ref _sizeofQueue);
            _evtWaitQueue.Set();
        }

        private void ThreadQueueMain()
        {
            var msgList = new List<QueueMsg>();

            while (true)
            {
                if (_queue.TryDequeue(out var msg))
                {
                    msgList.Add(msg);
                    Interlocked.Decrement(ref _sizeofQueue);
                }
                else
                {
                    _evtWaitQueue.Reset();
                    _evtWaitQueue.Wait();
                }

                while (_queue.TryDequeue(out msg))
                {
                    msgList.Add(msg);
                    Interlocked.Decrement(ref _sizeofQueue);
                }

                foreach (var message in msgList)
                {
                    if (_bThreadStop == false)
                    {
                        MessageProcessed?.Invoke(this, new QueueWorkerMessageEventArgs<T>(message.Item));
                    }
                    message.WaitMsgLoop?.Set();
                    message.Dispose();
                }

                msgList.Clear();

                if (_bThreadStop)
                {
                    return;
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (_bDisposedValue)
            {
                return;
            }

            if (disposing)
            {
                Stop();

                while (_queue.TryDequeue(out var msg))
                {
                    msg.Dispose();
                }

                _evtWaitQueue.Dispose();
            }

            _bDisposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
