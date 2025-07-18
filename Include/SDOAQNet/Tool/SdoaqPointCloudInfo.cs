using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQNet.Tool
{
    public class SdoaqPointCloudInfo : IDisposable
    {
        public string Name { get; }
        public uint VertexDataSize { get; }
        public float[] VertexDataBuffer { get; private set; }
        public uint ImgDataSize { get; }
        public byte[] ImgDataBuffer { get; private set; }
        public int Width { get; }
        public int Height { get; }
        public int SliceCount { get; }
        public SdoaqPointCloudInfo(string name,
            int width, int height, int sliceCount,
            float[] vertexDataBuffer, uint vertexDataSize,
            byte[] imgDataBuffer, uint imgDataSize)
        {
            Name = name;
            Width = width;
            Height = height;
            SliceCount = sliceCount;
            VertexDataSize = vertexDataSize;
            VertexDataBuffer = vertexDataBuffer;
            ImgDataSize = imgDataSize;
            ImgDataBuffer = imgDataBuffer;
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (ImgDataBuffer != null)
                    {
                        ImgDataBuffer = null;
                    }

                    if (VertexDataBuffer != null)
                    {
                        VertexDataBuffer = null;
                    }
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
            ImgDataBuffer = null;
            VertexDataBuffer = null;
        }
        #endregion
    }
}
