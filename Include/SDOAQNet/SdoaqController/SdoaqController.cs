using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDOAQNet.Tool;

namespace SDOAQNet
{
    public partial class SdoaqController : IDisposable, ICamera
    {
        public enum emEofImgViewOption
        {
            StepMap,
            QuaalityMap,
            HeightMap,
            PointClound,
            Edof,
        }

        public enum emCallBackMessage
        {
            FocusStack,
            Edof,
            Af,
            Mf,
            Snap,
            Frame,
        }

        public enum emPlayerMode
        {
            None,
            FocusStack,
            Af,
            Edof,
            Mf,
        }

        public class EdofImageList
        {
            public bool EnableEdofImg = false;
            public bool EnableStepMapImg = false;
            public bool EnableQualityMap = false;
            public bool EnableHeightMap = false;
            public bool EnablePointCloud = false;
        }

        public sealed class CallBackMessageEventArgs : System.EventArgs
        {
            public emCallBackMessage Message { get; }
            public List<SdoaqImageInfo> ImgInfoList { get; }
            public SdoaqPointCloudInfo PointCloudInfo { get; }

            public CallBackMessageEventArgs(emCallBackMessage message, List<SdoaqImageInfo> imgInfoList, SdoaqPointCloudInfo pointCloudInfo = null)
            {
                Message = message;
                ImgInfoList = imgInfoList;
                PointCloudInfo = pointCloudInfo;
            }
        }
        public event EventHandler<CallBackMessageEventArgs> CallBackMessageProcessed;

        public uint CamIndex { get; }
        public bool IsRunPlayer { get; private set; } = false;
        public emPlayerMode PlayerMode { get; private set; } = emPlayerMode.None;

        public SdoaqCamInfo CamInfo { get; private set; } = new SdoaqCamInfo();

        public FocusLHU FocusList { get; private set; } = new FocusLHU();
        public FocusLHU SnapFocusList { get; private set; } = new FocusLHU();

        public int PlayerRingBufferSize { get; set; } = DFLT_RING_BUFFER_SIZE;

        private RingBuffer _ringBuffer = new RingBuffer();
        private int _playerFoucsStepCount = 0;
        private EdofImageList _edofImageList = new EdofImageList();
        private QueueWorker<CallBackMessageEventArgs> _queueWorkerCallBackMsg;
        private bool _disposedValue = false;


        public const int DFLT_FOCUS_STEP = 160; //320 Step WiseScope Base (320/2)
        public const int MAX_FOCUS_STEP = 319; //320 Step WiseScope Base

        public const double RESIZE_RATIO_ORIGINAL = 1;
        public const double RESIZE_RATIO_HALF = 0.5;
        public const double RESIZE_RATIO_QUARTER = 0.25;

        public const int EDOF_RESULT_IMG_COUNT = 5;

        public const int DFLT_RING_BUFFER_SIZE = 3;

        public const string DFLT_FOCUS_LIST = "0-319-35";
        public const string DFLT_AF_ROI = "0,0,100,100";


        public SdoaqController(uint index)
        {
            CamIndex = index;

            _queueWorkerCallBackMsg = new QueueWorker<CallBackMessageEventArgs>($"SdoaqController_{index}");
            _queueWorkerCallBackMsg.MessageProcessed += queueWorkerCallBackMsg_MessageProcessed;
        }

        public void AppendLog(Logger.emLogLevel logLevel, string format, params object[] args)
        {
            WriteLog(logLevel, $"[Cam{CamIndex + 1}] {string.Format(format, args)}");
        }

        private void queueWorkerCallBackMsg_MessageProcessed(object sender, QueueWorkerMessageEventArgs<CallBackMessageEventArgs> e)
        {
            CallBackMessageProcessed?.Invoke(this, e.Item);
        }

        // ~SdoaqController()
        // {
        //     Dispose(disposing: false);
        // }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _queueWorkerCallBackMsg.Dispose();

                    _ringBuffer.Dispose();
                    _ringBuffer = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
