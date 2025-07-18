using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SDOAQ;
using SDOAQ_EDOF;
using SDOAQCSharp.Tool;
using SDOAQCSharp;
using SDOAQCSharp.Component;
using System.Diagnostics;

namespace SdoaqEdof
{
	public partial class SdoaqRunCudaEDoF : Form
	{
		private StringBuilder _logBuffer = new StringBuilder();
		private object _lockLog = new object();
		private Dictionary<int, MySdoaq> _sdoaqObjList = null;

		private SdoaqImageViewr _imgViewer;

		private int _cudaAvailability;
		private string _calibFileName;
	

		public SdoaqRunCudaEDoF()
		{
			InitializeComponent();
			cmb_EdofResizeRatio.SelectedItem = "0.5";

			_imgViewer = new SdoaqImageViewr(false);
			_imgViewer.Dock = DockStyle.Fill;

			pnl_Viewer.Controls.Add(_imgViewer);
			_sdoaqObjList = MySdoaq.LoadScript();
			_imgViewer.Set_SdoaqObj(GetSdoaqObj());

			MySdoaq.LogReceived += Sdoaq_LogDataReceived;
			MySdoaq.Initialized += Sdoaq_Initialized;
		}

		private void SdoaqEDoF_Load(object sender, EventArgs e)
		{
			OpenFileDialogSet();
			tmr_LogUpdate.Start();
			Frm_Load();
		}

		private void SdoaqEDoF_FormClosed(object sender, FormClosedEventArgs e)
		{
			MySdoaq.LogReceived -= Sdoaq_LogDataReceived;
			MySdoaq.Initialized -= Sdoaq_Initialized;

			GetSdoaqObj()?.AcquisitionStop();

			SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_FinalizeLibrary();

			MySdoaq.DisposeStaticResouce();

			Task.Run(() => { MySdoaq.SDOAQ_Finalize(); });
		}

		private void Frm_Load()
		{
			//================================================================================================================
			//
			// If you capture images directly without using SDOAQ library, you do not need to perform library initialization.
			//
			//================================================================================================================
			MySdoaq.SDOAQ_Initialize();

			var version = SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_GetVersion();
			Write_Log($"SD EDoF Algorithm version = {version}");
		}

		private MySdoaq GetSdoaqObj()
		{
			return _sdoaqObjList[0];
		}

		private void tmr_LogUpdate_Tick(object sender, EventArgs e)
		{
			if (_logBuffer.Length == 0)
			{
				return;
			}

			lock (_lockLog)
			{
				txt_Log.AppendText(_logBuffer.ToString());
				txt_Log.ScrollToCaret();
				_logBuffer.Clear();
			}
		}

		private void Sdoaq_LogDataReceived(object sender, LoggerEventArgs e)
		{
			lock (_lockLog)
			{
				_logBuffer.Append(e.Data);
			}
		}

		private void Sdoaq_Initialized(object sender, SdoaqEventArgs e)
		{
			this.Invoke(() =>
			{
				if (e.ErrorCode == SDOAQ.SDOAQ_API.eErrorCode.ecNoError)
				{
					_cudaAvailability = SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_CheckAvailability();
					Write_Log($"SD EDoF Algorithm version = {_cudaAvailability}");

					if (_cudaAvailability < 0)
						Write_Log("Your NVIDIA graphics card does not support CUDA-based EDoF processing. Please upgrade your graphics card to enable this feature.");
				}				
			});
		}

		private void Write_Log(string str)
		{
			Sdoaq_LogDataReceived(null, new LoggerEventArgs(str + Environment.NewLine));
		}

		private void btn_OpenCalibration_Click(object sender, EventArgs e)
		{
			if (openFile.ShowDialog() == DialogResult.OK)
			{
				_calibFileName = openFile.FileName;
			}
		}

		private void btn_RunEDoF_Click(object sender, EventArgs e)
		{
			var focusList = GetSdoaqObj().FocusList.GetStepList();
			var acqParam = GetSdoaqObj().CamInfo.AcqParam;
			var camInfo = GetSdoaqObj().CamInfo;
			var focusImagePointerList = new IntPtr[focusList.Length];

			//----------------------------------------------------------------------------
			// If you capture images directly without using SDOAQ library,
			// there's no need to execute the image capture code below.
			//----------------------------------------------------------------------------
			if (true)
			{
				byte[][] imageBuffer = null;
				imageBuffer = new byte[focusList.Length][];

				var focusImageBufferSizeList = new ulong[focusList.Length];
				int sizeOfImage = acqParam.cameraRoiHeight * acqParam.cameraRoiWidth * camInfo.ColorByte;

				for (int focus = 0; focus < focusList.Length; focus++)
				{
					imageBuffer[focus] = new byte[sizeOfImage];
					focusImageBufferSizeList[focus] = (ulong)sizeOfImage;
					unsafe
					{
						fixed (byte* pointerToFirst = imageBuffer[focus])
						{
							focusImagePointerList[focus] = new IntPtr(pointerToFirst);
						}
					}
				}

				var rvSdoaq = SDOAQ_API.SDOAQ_SingleShotFocusStackEx(
						ref acqParam,
						focusList, focusList.Length,
						focusImagePointerList, focusImageBufferSizeList);
				if (rvSdoaq != SDOAQ_API.eErrorCode.ecNoError)
				{
					// Error occurred while capturing image. cannot proceed with EDOF algorithm.
					return;
				}
			}

			//================================================================================================================
			// 
			//		If you are operating both the vision system and a motion controller together,
			//		this is the appropriate timing to move the motion controller, as the image capture has been completed.
			//
			//		Generate an EDoF image based on the captured images at this point.
			//		You can either use the API provided by the SDOAQ library or run your own custom algorithm.
			//
			//================================================================================================================

			//----------------------------------------------------------------------------
			//
			//		Starting point of the EDOF algorithm execution.
			//
			//		Make sure to set each parameter to a suitable value.
			//
			//		Don't forget to specify the calibration file before proceeding.
			//
			//----------------------------------------------------------------------------

			if (_cudaAvailability < 0)
			{
				Write_Log("Your NVIDIA graphics card does not support CUDA-based EDoF processing. Please upgrade your graphics card to enable this feature.");
				return;
			}

			SDOAQ_CUDA_EDOF_API.SDOAQ_CUDA_EDOF_Params edofParams = new SDOAQ_CUDA_EDOF_API.SDOAQ_CUDA_EDOF_Params();
			edofParams.numMalsStep = focusList.Length;
			edofParams.imageWidth = acqParam.cameraRoiWidth;
			edofParams.imageHeight = acqParam.cameraRoiHeight;
			edofParams.imageOffsetX = acqParam.cameraRoiLeft;
			edofParams.imageOffsetY = acqParam.cameraRoiTop;
			edofParams.pixelColorType = camInfo.ColorByte;

			double.TryParse(cmb_EdofResizeRatio.SelectedItem.ToString(), out double resize_ratio);
			edofParams.samplingMode = resize_ratio;

			Int32.TryParse(txt_Iteration.Text, out int pixelwise_iteration);
			edofParams.pixelwiseKernelIteration = pixelwise_iteration;			

			Double.TryParse(txt_Threshold.Text, out double depth_quality_th);
			edofParams.depthThreshold = depth_quality_th;

			edofParams.isScaleCorrectionEnabled = true;
			Int32.TryParse(txt_ScaleStep.Text, out int dst_step);
			edofParams.scaleCorrectionDstStep = dst_step;

			// 1. Initialize EDoF algorithm library with EDoF parameter and calibration file
			// Call SDOAQ_CUDAEDOF_InitializeLibrary() when calibration data or core parameters are changed.
			// For updating certain tunable parameters at runtime, use SDOAQ_CUDAEDOF_UpdateParams() without reinitializing.
			var rv_edof = SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_InitializeLibrary(ref edofParams, _calibFileName);
			if (0 > rv_edof)
			{
				Write_Log($"Check SDOAQ_EDOF_InitializeFromCalibFile Error Code[{rv_edof}]");
				return;
			}


			// 2. Register memory with cudaHostRegister
			for (int i = 0; i < focusList.Length; i++)
			{
				SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_RegisterMemory(focusImagePointerList[i], sizeof(byte) * camInfo.ImgSize);
			}

			Stopwatch edofRun = new Stopwatch();
			edofRun.Start();

			// 3. Add focus stack image to the algorithm
			for (int i = 0; i < focusList.Length; i++)
			{
				SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_AddImage(focusImagePointerList[i], i);
			}


			// 4. Run EDoF algorithm and generate output image
			var bufferEdofImage = new byte[camInfo.ImgSize];
			unsafe
			{
				fixed (byte* p = bufferEdofImage)
				{
					IntPtr ptr = (IntPtr)p;

					SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_RegisterMemory(ptr, sizeof(byte) * camInfo.ImgSize);
					rv_edof = SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_Run(ptr);

					edofRun.Stop();
					Write_Log($"SDOAQ_CUDAEDOF_Run() takes {edofRun.Elapsed.TotalMilliseconds.ToString()} ms.");

					SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_UnregisterMemory(ptr);
				}
			}


			// 5. Unregister memory with cudaHostUnregister
			for (int i = 0; i < focusList.Length; i++)
			{
				SDOAQ_CUDA_EDOF_API.SDOAQ_CUDAEDOF_UnregisterMemory(focusImagePointerList[i]);
			}

			var imgInfoList = new List<SdoaqImageInfo>();
			if (rv_edof >= 0)
			{
				//Write_Log("SDOAQ_EDOF_Run() completed.");
				imgInfoList.Add(new SdoaqImageInfo("Edof",
					acqParam.cameraRoiWidth, acqParam.cameraRoiHeight, camInfo.ColorByte,
					bufferEdofImage));

				GetSdoaqObj().CallBackMsgLoop.Invoke((MySdoaq.emCallBackMessage.Edof, new object[] { imgInfoList }));
			}
			else
			{
				Write_Log($"Check SDOAQ_EDOF_Run Error Code[{rv_edof}]");
			}	
		}

		private void btn_SetROI_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.SetRoi(txt_ROI.Text);
		}

		private void btn_SetMALSFocus_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.SetFocus(txt_MALSFocus.Text);
		}

		private void btn_SetResizeRatio_Click(object sender, EventArgs e)
		{
			//GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_resize_ratio, cmb_EdofResizeRatio.SelectedItem.ToString());
		}
		
		private void btn_SetIteration_Click(object sender, EventArgs e)
		{
			//GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_pixelwise_iteration, txt_Iteration.Text);
		}

		private void btn_SetThreshold_Click(object sender, EventArgs e)
		{
			//GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_depth_quality_th, txt_Threshold.Text);
		}

		private void btn_SetScaleStep_Click(object sender, EventArgs e)
		{
			//GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_scale_correction_dst_step, txt_ScaleStep.Text);
		}

		private void OpenFileDialogSet()
		{
			openFile.Title = "Select calibration file for objective";
			openFile.FileName = "";
			openFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
		}
	}
}
