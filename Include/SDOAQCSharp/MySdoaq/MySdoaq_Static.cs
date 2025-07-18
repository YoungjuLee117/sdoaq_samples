using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using SDOAQ;
using SDOAQCSharp.Tool;

namespace SDOAQCSharp
{
    partial class MySdoaq
    {
        private const string SCRIPT_FILE_NAME = "wisescope.script.txt";
        private const string SCRIPT_LINE_NUM_OF_WS = "Number of WSM";

        public static event EventHandler<SdoaqEventArgs> Initialized;
        public static event EventHandler<LoggerEventArgs> LogReceived
        {
            add
            {
				if (s_logger == null)
				{
					s_logger = new Logger();
				}
                s_logger.DataReceived += value;
            }

            remove
            {
                s_logger.DataReceived -= value;
            }
        }
        
        public static bool IsInitialize { get; private set; } = false;

        private static SDOAQ.SDOAQ_API.SDOAQ_LogCallback CallBack_SDOAQ_Log;
        private static SDOAQ.SDOAQ_API.SDOAQ_ErrorCallback CallBack_SDOAQ_Error;
		private static SDOAQ.SDOAQ_API.SDOAQ_MoveokCallback CallBack_SDOAQ_MoveOk;
		private static SDOAQ.SDOAQ_API.SDOAQ_InitDoneCallback CallBack_InitDone;
        private static SDOAQ.SDOAQ_API.SDOAQ_PlayCallbackEx CallBack_SDOAQ_PlayFocusStack;
        private static SDOAQ.SDOAQ_API.SDOAQ_PlayCallbackEx CallBack_SDOAQ_PlayEdof;
        private static SDOAQ.SDOAQ_API.SDOAQ_PlayAfCallbackEx2 CallBack_SDOAQ_PlayAf;
        private static SDOAQ.SDOAQ_API.SDOAQ_PlayMfCallbackEx CallBack_SDOAQ_PlayMf;
        private static SDOAQ.SDOAQ_API.SDOAQ_SnapCallbackEx CallBack_SDOAQ_Snap;

        private static bool s_isFirstInitialize = true;
        private static Dictionary<int, MySdoaq> s_sdoaqObjList = new Dictionary<int, MySdoaq>();
        private static Logger s_logger = new Logger();
        
        public static Dictionary<int, MySdoaq> LoadScript(emPlayerMethod playerMethod = emPlayerMethod.CallBackFunc, string scriptFilePath = "")
        {
			string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), SCRIPT_FILE_NAME);

			//string currentPath = System.IO.Directory.GetCurrentDirectory();
			//string upperPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(currentPath));
			//string path = System.IO.Path.Combine(upperPath, SCRIPT_FILE_NAME);

			SdoaqScriptReader.GetIntFromLineScript(path, SCRIPT_LINE_NUM_OF_WS, 1, out int numOfWiseScope);

            numOfWiseScope = Math.Max(1, numOfWiseScope);
            
			if (s_sdoaqObjList == null)
			{
				s_sdoaqObjList = new Dictionary<int, MySdoaq>();
			}
            s_sdoaqObjList.Clear();
            
            for (int i = 0; i< numOfWiseScope; i++)
            {
                s_sdoaqObjList.Add(i, new MySdoaq(playerMethod, i));
            }

            return s_sdoaqObjList;
        }

        public static bool SDOAQ_Initialize()
        {
            SDOAQ_Finalize();

            if (s_sdoaqObjList.Count > 1)
            {
                SDOAQ_API.SDOAQ_RegisterMultiWsApi();
            }

			// specify the script file name including the full file path.
			string scriptFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wisescope.script.txt");
			SDOAQ_API.SDOAQ_SetSystemScriptFilename(scriptFile);

			// set the path to the cam files folder
			string camFilesPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "CamFiles");
			SDOAQ_API.SDOAQ_SetCamfilePath(camFilesPath);

			var rv = SDOAQ_API.SDOAQ_Initialize(CallBack_SDOAQ_Log, CallBack_SDOAQ_Error, CallBack_InitDone);

            return rv == SDOAQ_API.eErrorCode.ecNoError;
        }

        public static bool SDOAQ_Finalize()
        {
            foreach (var sdoaqObj in s_sdoaqObjList.Values)
            {
                if (sdoaqObj.IsRunPlayer)
                {
                    sdoaqObj.AcquisitionStop();
                }
            }

            IsInitialize = false;

            return SDOAQ_API.SDOAQ_Finalize() == SDOAQ_API.eErrorCode.ecNoError;
        }

        public static void DisposeStaticResouce()
        {
            if (s_sdoaqObjList != null)
            {
                foreach (var key in s_sdoaqObjList.Keys.ToList())
                {
                    s_sdoaqObjList[key].Dispose();
                }
            }


            s_logger?.Dispose();
            s_logger = null;
        }

		public static bool SDOAQ_RegisterMoveOk()
		{
			var rv = SDOAQ_API.SDOAQ_RegisterMoveokCallback(CallBack_SDOAQ_MoveOk);

			return rv == SDOAQ_API.eErrorCode.ecNoError;
		}

		public static string GetVersion()
        {
            return $"{SDOAQ_API.SDOAQ_GetMajorVersion()}.{SDOAQ_API.SDOAQ_GetMinorVersion()}.{SDOAQ_API.SDOAQ_GetPatchVersion()}";
        }

        public static string GetVersion_SdEdofAlgorithm()
        {
            int algoVersion = SDOAQ_API.SDOAQ_GetAlgorithmVersion();
            return $"{algoVersion / 1000}.{algoVersion % 1000}";
        }

        public static void WriteLog(Logger.emLogLevel logLevel, string format, params object[] args)
        {
			if (s_logger == null)
				return;

            if (string.IsNullOrEmpty(format))
            {
                return;
            }

            if (args == null || args.Length == 0)
            {
                s_logger.WriteLog($"[{logLevel}] {format}");
            }
            else
            {
                s_logger.WriteLog($"[{logLevel}] {string.Format(format, args)}");
            }
        }
        
        public static bool SelectMultiWS(int idxCam)
        {
            if (s_sdoaqObjList.Count <= 1)
            {
                return true;
            }

            int idxWS = idxCam + 1;
            var rv = SDOAQ_API.SDOAQ_SelectMultiWs(idxWS);

            if (rv != SDOAQ_API.eErrorCode.ecNoError)
            {
                WriteLog(Logger.emLogLevel.API, $"SelectMultiWS(), WS Index = {idxWS}, rv = {rv}");
                return false;
            }

            return true;
        }

        private static void Add_CallbackFunction()
        {
            CallBack_SDOAQ_Log = OnSdoaq_Log;
            CallBack_SDOAQ_Error = OnSdoaq_Error;
			CallBack_SDOAQ_MoveOk = OnSdoaq_MoveOk;
			CallBack_InitDone = OnSdoaq_InitDone;
            CallBack_SDOAQ_PlayFocusStack = OnSdoaq_PlayFocusStack;
            CallBack_SDOAQ_PlayEdof = OnSdoaq_PlayEdof;
            CallBack_SDOAQ_PlayAf = OnSdoaq_PlayAf;
            CallBack_SDOAQ_PlayMf = OnSdoaq_PlayMf;
            CallBack_SDOAQ_Snap = OnSdoaq_Snap;
        }

        private static int GetCallBackMultWs()
        {
            return Math.Max(0, SDOAQ_API.SDOAQ_GetCallbackMultiWs() - 1);
        }

        private static MySdoaq GetSdoaqObj()
        {
            int idx = GetCallBackMultWs();

            if (s_sdoaqObjList.ContainsKey(idx) == false)
            {
                return null;
            }

            return s_sdoaqObjList[idx];
        }

		private static void OnSdoaq_Log(SDOAQ_API.eLogSeverity severity, StringBuilder pMessage)
        {
            WriteLog(Logger.emLogLevel.API, $"[{severity}]{pMessage.ToString()}");
        }

        private static void OnSdoaq_Error(SDOAQ_API.eErrorCode errorCode, StringBuilder pErrorMessage)
        {
            WriteLog(Logger.emLogLevel.API, $"[Error]Error Code = {errorCode}, {pErrorMessage.ToString()}");
        }

		private static void OnSdoaq_MoveOk(SDOAQ_API.eErrorCode errorCode, IntPtr callbackUserData)
		{			
			WriteLog(Logger.emLogLevel.API, "OnSdoaq_MoveOk");

			if (errorCode != SDOAQ_API.eErrorCode.ecNoError)
			{
				// Handle error:
				// - retry image capture
				// - skip this cycle
				// - log and continue
				return;
			}

			// Motion trigger logic here
		}

		private static void OnSdoaq_InitDone(SDOAQ_API.eErrorCode errorCode, StringBuilder pErrorMessage)
        {
            bool bInitDone = errorCode == SDOAQ_API.eErrorCode.ecNoError;

            IsInitialize = bInitDone;

            if (bInitDone)
            {
                WriteLog(Logger.emLogLevel.API, $"[INIT]Initialize Done");
				WriteLog(Logger.emLogLevel.API, $">> sdedof dll Version = {GetVersion_SdEdofAlgorithm()}");

				foreach (var sdoaqObj in s_sdoaqObjList.Values)
                {
                    SelectMultiWS(sdoaqObj.CamIndex);
                        
                    bool isWriteable = false;
                    string paramValue = string.Empty;

                    bool bCamColor = false;
                    int fullFrameSizeX = 0;
                    int fullFrameSizeY = 0;
                    int camBinning = 1;
                    if (sdoaqObj.GetParam(SDOAQ_API.eParameterId.piCameraColor, out isWriteable, out paramValue))
                    {
                        bCamColor = int.Parse(paramValue) == 0; 
                    }

                    if (sdoaqObj.GetParam(SDOAQ_API.eParameterId.piCameraFullFrameSizeX, out isWriteable, out paramValue))
                    {
                        fullFrameSizeX = int.Parse(paramValue);
                    }

                    if (sdoaqObj.GetParam(SDOAQ_API.eParameterId.piCameraFullFrameSizeY, out isWriteable, out paramValue))
                    {
                        fullFrameSizeY = int.Parse(paramValue);
                    }

                    if (sdoaqObj.GetParam(SDOAQ_API.eParameterId.piCameraBinning, out isWriteable, out paramValue))
                    {
                        camBinning = int.Parse(paramValue);
                    }

                    var camInfo = new SdoaqCamInfo()
                    {
                        ColorByte = bCamColor ? 3 : 1,
                        AcqParam = new SDOAQ_API.AcquisitionFixedParametersEx()
                        {
                            cameraRoiLeft = 0,
                            cameraRoiTop = 0,
                            cameraRoiWidth = fullFrameSizeX,
                            cameraRoiHeight = fullFrameSizeY,
                            cameraBinning = camBinning,
                        }
                    };

                    sdoaqObj.CamInfo = camInfo;

                    sdoaqObj.SetRoi_AF(DFLT_AF_ROI);
                    sdoaqObj.SetFocus(DFLT_FOCUS_LIST);
                    sdoaqObj.SetSnapFocus(DFLT_FOCUS_LIST);					
				}

                SelectMultiWS(0);
            }
            else
            {
                WriteLog(Logger.emLogLevel.API, $"[INIT]Initialize Error, Error Code = {errorCode}, {pErrorMessage.ToString()}");
            }

            Initialized?.Invoke(null, new SdoaqEventArgs(errorCode, pErrorMessage.ToString()));
        }
		
		private static void OnSdoaq_PlayFocusStack(SDOAQ_API.eErrorCode errorCode, int lastFilledRingBufferEntry, IntPtr callbackUserData)
        {
			var sdoaqObj = GetSdoaqObj();

			// If the SDOAQ object is null or the player is not running, exit the function
			if (sdoaqObj == null || sdoaqObj.IsRunPlayer == false)
			{
				//WriteLog(Logger.emLogLevel.Info, $"OnSdoaq_PlayFocusStack(), check status of the SDOAQ object.");
				return;
			}

			if (errorCode != SDOAQ_API.eErrorCode.ecNoError)
			{
				WriteLog(Logger.emLogLevel.API, $"OnSdoaq_PlayFocusStack(), Error = {errorCode}");
				return;
			}

			var foucsList = sdoaqObj.FocusList.GetStepList();

			int idxRingBuffer = lastFilledRingBufferEntry * foucsList.Length;
			var camInfo = sdoaqObj.CamInfo;
			var acqParam = camInfo.AcqParam;

			byte[][] resultImgList = new byte[sdoaqObj._playerFoucsStepCount][];
			unsafe
			{
				for (int i = 0; i < resultImgList.Length; i++)
				{
					int idx = idxRingBuffer + i;
					int size = (int)sdoaqObj._ringBuffer.Sizes[idx];
					byte[] buffer = new byte[size];

					if (size > 0)
					{
						var ptrSrc = (float*)sdoaqObj._ringBuffer.Buffer[idx];

						fixed (byte* ptrDst = buffer)
						{
							Buffer.MemoryCopy(ptrSrc, ptrDst, size, size);
						}
					}
					resultImgList[i] = buffer;
				}
			}

			var imgInfoList = new List<SdoaqImageInfo>();

			for (int i = 0; i < resultImgList.Length; i++)
			{
				imgInfoList.Add(new SdoaqImageInfo($"F-{foucsList[i]}", camInfo.PixelWidth, camInfo.PixelHeight, camInfo.ColorByte, resultImgList[i]));
			}

			sdoaqObj.CallBackMsgLoop.Invoke((emCallBackMessage.FocusStack, new object[] { imgInfoList }));
        }

        private static void OnSdoaq_PlayEdof(SDOAQ.SDOAQ_API.eErrorCode errorCode, int lastFilledRingBufferEntry, IntPtr callbackUserData)
        {
            var sdoaqObj = GetSdoaqObj();

            if (sdoaqObj == null || sdoaqObj.IsRunPlayer == false)
            {
                return;
            }

            if (errorCode != SDOAQ_API.eErrorCode.ecNoError)
            {
                WriteLog(Logger.emLogLevel.API, $"OnSdoaq_PlayEdof(), Error = {errorCode}");
                return;
            }

            int idxRingBuffer = lastFilledRingBufferEntry * EDOF_RESULT_IMG_COUNT;
            var camInfo = sdoaqObj.CamInfo;
            var acqParam = camInfo.AcqParam;
            int pixelSize = camInfo.PixelSize;

            byte[] resultImgEdof = new byte[sdoaqObj._ringBuffer.Sizes[idxRingBuffer]];

            if (sdoaqObj._ringBuffer.Buffer[idxRingBuffer] != IntPtr.Zero)
            {
                Marshal.Copy(sdoaqObj._ringBuffer.Buffer[idxRingBuffer], resultImgEdof, 0, resultImgEdof.Length);
            }
            
            float[][] resultImgList = new float[EDOF_RESULT_IMG_COUNT - 1][];
            unsafe
            {
                for (int i = 0; i < resultImgList.Length; i++)
                {
                    int idx = idxRingBuffer + i + 1;
                    int size = (int)sdoaqObj._ringBuffer.Sizes[idx];
					float[] buffer = new float[size];

					if (size > 0)
                    {
                        var ptrSrc = (float*)sdoaqObj._ringBuffer.Buffer[idx];

                        fixed (float* ptrDst = buffer)
                        {
                            Buffer.MemoryCopy(ptrSrc, ptrDst, size, size);
                        }
                    }
                    resultImgList[i] = buffer;
                }
            }

            var imgInfoList = new List<SdoaqImageInfo>();
            SdoaqPointCloudInfo pointCloudInfo = null;

            if (resultImgEdof.Length > 0)
            {

                imgInfoList.Add(new SdoaqImageInfo("Edof",
                    acqParam.cameraRoiWidth, acqParam.cameraRoiHeight, camInfo.ColorByte,
                    resultImgEdof));
            }

            if (resultImgList[0].Length > 0)
            {
                imgInfoList.Add(new SdoaqImageInfo("StepMap",
                    acqParam.cameraRoiWidth, acqParam.cameraRoiHeight, 1,
                    ConvertFloatBufferToByteBuffer(pixelSize, resultImgList[0])));
            }

            if (resultImgList[1].Length > 0)
            {
                imgInfoList.Add(new SdoaqImageInfo("QualityMap",
                    acqParam.cameraRoiWidth, acqParam.cameraRoiHeight, 1,
                    ConvertFloatBufferToByteBuffer(pixelSize, resultImgList[1])));
            }

            if (resultImgList[2].Length > 0)
            {
                imgInfoList.Add(new SdoaqImageInfo("HeightMap",
                    acqParam.cameraRoiWidth, acqParam.cameraRoiHeight, 1,
                    ConvertFloatBufferToByteBuffer(pixelSize, resultImgList[2])));
            }
            
            if (resultImgList[3].Length > 0 && resultImgEdof.Length > 0)
            {
                pointCloudInfo = new SdoaqPointCloudInfo("PointCloud",
                    acqParam.cameraRoiWidth, acqParam.cameraRoiHeight, sdoaqObj._playerFoucsStepCount,
                    resultImgList[3], (uint)resultImgList[3].Length,
                    resultImgEdof, (uint)resultImgEdof.Length);
            }

            sdoaqObj.CallBackMsgLoop.Invoke((emCallBackMessage.Edof, new object[] { imgInfoList, pointCloudInfo }));

        }

        private static void OnSdoaq_PlayAf(SDOAQ.SDOAQ_API.eErrorCode errorCode, int lastFilledRingBufferEntry, IntPtr callbackUserData, double dbBestFocusStep, double dbBestScore, double dbMatchedFocusStep)
        {
            var sdoaqObj = GetSdoaqObj();

            if (sdoaqObj == null || sdoaqObj.IsRunPlayer == false)
            {
                return;
            }

            if (errorCode != SDOAQ_API.eErrorCode.ecNoError)
            {
                WriteLog(Logger.emLogLevel.API, $"[Cam{sdoaqObj.CamIndex + 1}]OnSdoaq_PlayAf(), Error = {errorCode}");
                return;
            }

            WriteLog(Logger.emLogLevel.API, $"[Cam{sdoaqObj.CamIndex + 1}]OnSdoaq_PlayAf(), BestFocusStep = {dbBestFocusStep}, BestScore = {dbBestScore}, MatchedFocusStep = {dbMatchedFocusStep}");

            int idxRingBuffer = lastFilledRingBufferEntry;
            var camInfo = sdoaqObj.CamInfo;
            var acqParam = camInfo.AcqParam;

            byte[][] resultImgList = new byte[1][];
            unsafe
            {
                for (int i = 0; i < resultImgList.Length; i++)
                {
                    int idx = idxRingBuffer + i;
                    int size = (int)sdoaqObj._ringBuffer.Sizes[idx];
                    byte[] buffer = new byte[size];

                    if (size > 0)
                    {
                        var ptrSrc = (float*)sdoaqObj._ringBuffer.Buffer[idx];

                        fixed (byte* ptrDst = buffer)
                        {
                            Buffer.MemoryCopy(ptrSrc, ptrDst, size, size);
                        }
                    }
                    resultImgList[i] = buffer;
                }
            }

            var imgInfoList = new List<SdoaqImageInfo>();

            for (int i = 0; i < resultImgList.Length; i++)
            {
                imgInfoList.Add(new SdoaqImageInfo($"AF", camInfo.PixelWidth, camInfo.PixelHeight, camInfo.ColorByte, resultImgList[i]));
            }

            sdoaqObj.CallBackMsgLoop.Invoke((emCallBackMessage.Af, new object[] { imgInfoList }));
        }

        private static void OnSdoaq_PlayMf(SDOAQ.SDOAQ_API.eErrorCode errorCode, int lastFilledRingBufferEntry, IntPtr callbackUserData, int countRects, int[] pRectIdArray, int[] pRectStepArray)
        {
            var sdoaqObj = GetSdoaqObj();

            if (sdoaqObj == null || sdoaqObj.IsRunPlayer == false)
            {
                return;
            }

            if (errorCode != SDOAQ_API.eErrorCode.ecNoError)
            {
                WriteLog(Logger.emLogLevel.API, $"[Cam{sdoaqObj.CamIndex + 1}]OnSdoaq_PlayAf(), Error = {errorCode}");
                return;
            }
            
            int idxRingBuffer = lastFilledRingBufferEntry;
            var camInfo = sdoaqObj.CamInfo;
            var acqParam = camInfo.AcqParam;

            byte[][] resultImgList = new byte[1][];
            unsafe
            {
                for (int i = 0; i < resultImgList.Length; i++)
                {
                    int idx = idxRingBuffer + i;
                    int size = (int)sdoaqObj._ringBuffer.Sizes[idx];
                    byte[] buffer = new byte[size];

                    if (size > 0)
                    {
                        var ptrSrc = (float*)sdoaqObj._ringBuffer.Buffer[idx];

                        fixed (byte* ptrDst = buffer)
                        {
                            Buffer.MemoryCopy(ptrSrc, ptrDst, size, size);
                        }
                    }
                    resultImgList[i] = buffer;
                }
            }

            var imgInfoList = new List<SdoaqImageInfo>();

            for (int i = 0; i < resultImgList.Length; i++)
            {
                imgInfoList.Add(new SdoaqImageInfo($"AF", camInfo.PixelWidth, camInfo.PixelHeight, camInfo.ColorByte, resultImgList[i]));
            }

            sdoaqObj.CallBackMsgLoop.Invoke((emCallBackMessage.Af, new object[] { imgInfoList }));
        }

        private static void OnSdoaq_Snap(SDOAQ.SDOAQ_API.eErrorCode errorCode, int lastFilledRingBufferEntry, IntPtr callbackUserData)
        {
            var sdoaqObj = GetSdoaqObj();

            if (sdoaqObj == null)
            {
                return;
            }

            WriteLog(Logger.emLogLevel.API, $"[Cam{sdoaqObj.CamIndex + 1}]Snap Received, ErrorCode = {errorCode}");
        }

        private static byte[] ConvertFloatBufferToByteBuffer(int totalPixelSize, float[] data)
        {
            var query = data.AsParallel();
            var low = query.Min();
            var high = query.Max();

            float inc = ((float)256.0 / (high - low));

            var buffer = new byte[totalPixelSize];
            for (uint i = 0; i < totalPixelSize; i++)
            {
                buffer[i] = (byte)((data[i] - low) * inc);
            }

            return buffer;
        }
    }
}
