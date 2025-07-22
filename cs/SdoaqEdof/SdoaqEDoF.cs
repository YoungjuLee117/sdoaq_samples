using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;

namespace SdoaqEdof
{
	public partial class SdoaqEDoF : Form
    {
        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLog = new object();
        private Dictionary<int, SdoaqController> _sdoaqObjList = null;

        private SdoaqImageViewr _imgViewer;

        public SdoaqEDoF()
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

            SdoaqController.DisposeStaticResouce();

            Task.Run(() => { SdoaqController.SDOAQ_Finalize(); });
        }

        private void Frm_Load()
        {
            SdoaqController.SDOAQ_Initialize(false);
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
        

        private void btn_SingleShotEDoF_Click(object sender, EventArgs e)
        {
            var edofImageOption = new SdoaqController.EdofImageList()
            {
                EnableEdofImg = true,
                EnableStepMapImg = true,
                EnableQualityMap = true,
                EnableHeightMap = true,
                EnablePointCloud = true,
            };
            var task = GetSdoaqObj()?.Acquisition_EdofAsync(edofImageOption);
        }

        private void btn_PlayEDoF_Click(object sender, EventArgs e)
        {
            var edofImageOption = new SdoaqController.EdofImageList()
            {
                EnableEdofImg = true,
                EnableStepMapImg = true,
                EnableQualityMap = true,
                EnableHeightMap = true,
                EnablePointCloud = true,
            };

			GetSdoaqObj()?.AcquisitionContinuous_Edof(edofImageOption);
		}

		private void btn_StopEDoF_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.AcquisitionStop_Edof();
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
			GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_resize_ratio, cmb_EdofResizeRatio.SelectedItem.ToString());
		}

		private void btn_SetKernelSize_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_pixelwise_kernel_size, txt_KernelSize.Text);
		}

		private void btn_SetIteration_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_pixelwise_iteration, txt_Iteration.Text);
		}

		private void btn_SetThreshold_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_depth_quality_th, txt_Threshold.Text);
		}

		private void btn_SetScaleStep_Click(object sender, EventArgs e)
		{
			GetSdoaqObj()?.SetParam(SDOAQ_API.eParameterId.pi_edof_scale_correction_dst_step, txt_ScaleStep.Text);
		}

        private void btn_OpenCalibration_Click(object sender, EventArgs e)
        {
            string fullName = "";
			if (openFile.ShowDialog() == DialogResult.OK)
			{
                fullName = openFile.FileName;

                var rv_sdoaq = SDOAQ_API.SDOAQ_SetCalibrationFile(fullName);
            }
        }

        private void OpenFileDialogSet()
        {
            openFile.Title = "Select calibration file for objective";
            openFile.FileName = "";
            openFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
        }		
	}
}
