using System;
using System.Collections.Generic;

namespace SDOAQNet.Tool
{
    public class SdoaqImageInfo : ICloneable, IDisposable
    {
        public readonly string Name;
        public readonly int Width;
        public readonly int Height;
        public readonly int Lines;
        public readonly int ColorByte;
        //public readonly byte[] Data;
        public byte[] Data { get; private set; }

        public SdoaqImageInfo(string name, int width, int height, int lines, int colorByte, byte[] data)
        {
            Name = name;
            Width = width;
            Height = height;
            Lines = lines;
            ColorByte = colorByte;
            Data = data;
        }

        public object Clone()
        {
            var copy = (SdoaqImageInfo)this.MemberwiseClone();
            copy.Data = (byte[])this.Data?.Clone();
            return copy;
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Data = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void NullifyArray()
        {
            Data = null;
        }
        #endregion
    }
}
