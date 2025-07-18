using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public sealed class ManualResetResult<T> : IDisposable
    {
        public T ReturnValue { get; private set; }
        public bool IsAbort { get; private set; }

        private bool _IsWaitSet;
        public bool IsWaitSet
        {
            get
            {
                try
                {
                    _lock.EnterReadLock();
                    return _IsWaitSet;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }

            private set
            {
                try
                {
                    _lock.EnterWriteLock();
                    _IsWaitSet = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private EventWaitHandle _eventWaitHandle;

        public ManualResetResult(bool initialState)
        {
            _eventWaitHandle = new EventWaitHandle(initialState, EventResetMode.ManualReset);
            IsWaitSet = !initialState;
            IsAbort = false;
        }

        public bool Abort(T returnValue)
        {
            ReturnValue = returnValue;
            IsWaitSet = false;
            IsAbort = true;
            return _eventWaitHandle.Set();
        }

        public bool Set(T returnValue)
        {
            ReturnValue = returnValue;
            IsWaitSet = false;
            IsAbort = false;
            return _eventWaitHandle.Set();
        }
        
        public bool Reset()
        {
            IsWaitSet = true;
            IsAbort = false;
            return _eventWaitHandle.Reset();
        }

        public bool WaitOne(int millisecondsTimeout, bool exitContext)
        {
            return _eventWaitHandle.WaitOne(millisecondsTimeout, exitContext);
        }

        public bool WaitOne(TimeSpan timeout)
        {
            return _eventWaitHandle.WaitOne(timeout);
        }

        public bool WaitOne(int millisecondsTimeout)
        {
            return _eventWaitHandle.WaitOne(millisecondsTimeout);
        }

        public bool WaitOne(TimeSpan timeout, bool exitContext)
        {
            return _eventWaitHandle.WaitOne(timeout, exitContext);
        }
        public bool WaitOne()
        {
            return _eventWaitHandle.WaitOne();
        }
        
        public void Dispose()
        {
            _eventWaitHandle.Dispose();
        }
    }
}
