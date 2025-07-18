using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;
using SDOAQ_EDOF;

namespace SdoaqEdof
{
	public partial class SdoaqRunEDoF : Form
	{
		private StringBuilder _logBuffer = new StringBuilder();
		private object _lockLog = new object();
		private Dictionary<int, SdoaqController> _sdoaqObjList = null;

		private SdoaqImageViewr _imgViewer;

		public SdoaqRunEDoF()
		{
			InitializeComponent();
			cmb_EdofResizeRatio.SelectedItem = "0.5";

			_imgViewer = new SdoaqImageViewr(false);
            _imgViewer.VisiBleImageListBox = false;
			_imgViewer.Dock = DockStyle.Fill;

			pnl_Viewer.Controls.Add(_imgViewer);
			_sdoaqObjList = SdoaqController.LoadScript();
			_imgViewer.Set_SdoaqObj(GetSdoaqObj());

            SdoaqController.LogReceived += Sdoaq_LogDataReceived;
		}

		private void SdoaqEDoF_Load(object sender, EventArgs e)
		{
			OpenFileDialogSet();
			tmr_LogUpdate.Start();
			Frm_Load();
		}

		private void SdoaqEDoF_FormClosed(object sender, FormClosedEventArgs e)
		{
            SdoaqController.LogReceived -= Sdoaq_LogDataReceived;

			GetSdoaqObj()?.AcquisitionStop();

			SDOAQ_EDOF_API.SDOAQ_EDOF_Finalize();

            SdoaqController.DisposeStaticResouce();

			Task.Run(() => { SdoaqController.SDOAQ_Finalize(); });
		}

		private void Frm_Load()
		{
			//================================================================================================================
			//
			// If you capture images directly without using SDOAQ library, you do not need to perform library initialization.
			//
			//================================================================================================================
            SdoaqController.SDOAQ_Initialize(false);

			var version = SDOAQ_EDOF_API.SDOAQ_EDOF_GetVersion();
			Write_Log($"SD EDoF Algorithm version = {version}");
		}

		private SdoaqController GetSdoaqObj()
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

		private void Write_Log(string str)
		{
			Sdoaq_LogDataReceived(null, new LoggerEventArgs(str + Environment.NewLine));
		}

		private void btn_OpenCalibration_Click(object sender, EventArgs e)
		{
			string fullName = "";
			if (openFile.ShowDialog() == DialogResult.OK)
			{
				fullName = openFile.FileName;

				var rv_sdoaq = SDOAQ_API.SDOAQ_SetCalibrationFile(fullName);


				//----------------------------------------------------------------------------
				//		Specify the calibration file before proceeding.
				//----------------------------------------------------------------------------
				var rv_edof = SDOAQ_EDOF_API.SDOAQ_EDOF_InitializeFromCalibFile(fullName);
			}
		}

		private void btn_RunEDoF_Click(object sender, EventArgs e)
		{
			var focusList = GetSdoaqObj().FocusList.GetStepList();
			ref var acqParam = ref GetSdoaqObj().CamInfo.GetAcqParamRef();
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

			//----------------------------------------------------------------------------
			//
			//		Starting point of the EDOF algorithm execution.
			//
			//		Make sure to set each parameter to a suitable value.
			//
			//		Don't forget to specify the calibration file before proceeding.
			//
			//----------------------------------------------------------------------------

			SDOAQ_EDOF_API.SDOAQ_EDOF_FocalStackParams inParams = new SDOAQ_EDOF_API.SDOAQ_EDOF_FocalStackParams();
			inParams.focus_measure = SDOAQ_EDOF_API.SDOAQ_EDOF_FocusMeasure.MODIFIED_LAPLACIAN;
			inParams.image_num = focusList.Length;
			inParams.image_width = acqParam.cameraRoiWidth;
			inParams.image_height = acqParam.cameraRoiHeight;
			inParams.image_offset_x = acqParam.cameraRoiLeft;
			inParams.image_offset_y = acqParam.cameraRoiTop;
			inParams.binning_x = 1;
			inParams.binning_y = 1;
			inParams.num_channel = camInfo.ColorByte; // depending on the captured image color
			inParams.byte_per_channel = 1;
			inParams.num_padding_bit = 0;
			//inParams.roi_top = acqParam.cameraRoiTop;
			//inParams.roi_left = acqParam.cameraRoiLeft;
			//inParams.roi_width = inParams.image_width;
			//inParams.roi_height = inParams.image_height;

			double.TryParse(cmb_EdofResizeRatio.SelectedItem.ToString(), out double resize_ratio);
			Int32.TryParse(txt_KernelSize.Text, out int pixelwise_kernel_size);
			Int32.TryParse(txt_Iteration.Text, out int pixelwise_iteration);

			Double.TryParse(txt_Threshold.Text, out double depth_quality_th);

			Int32.TryParse(txt_ScaleStep.Text, out int dst_step);

			Stopwatch edofRun = new Stopwatch();
			edofRun.Start();

			int rv = GetSdoaqObj().RunEdof(focusImagePointerList, focusList, 
                camInfo.ImgSize , camInfo.ColorByte, 
                ref acqParam, 
                resize_ratio,
                pixelwise_kernel_size,
                pixelwise_iteration, 
                depth_quality_th, 
                dst_step);
			edofRun.Stop();
			Write_Log($"SDOAQ_EDOF_Run() takes {edofRun.Elapsed.TotalMilliseconds.ToString()} ms.");

            if (rv > 0)
            {
                //Write_Log("SDOAQ_EDOF_Run() completed.");
            }
            else
            {
                Write_Log($"Check SDOAQ_EDOF_Run Error Code[{rv}]");
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

		private void btn_SetKernelSize_Click(object sender, EventArgs e)
		{
			//GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_pixelwise_kernel_size, txt_KernelSize.Text);			
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
