using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;


namespace SdoaqMultiLighting
{
    public partial class SdoaqMultiLighting : Form
    {
        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLog = new object();
        private Dictionary<int, SdoaqController> _sdoaqObjList = null;

        private SdoaqImageViewr _imgViewer;
        private const int STACK_IMAGE_FOCUS = 160;
        public SdoaqMultiLighting()
        {
            InitializeComponent();

            _imgViewer = new SdoaqImageViewr(false);
            _imgViewer.VisiBleImageListBox = false;
            _imgViewer.Dock = DockStyle.Fill;

            pnl_Viewer.Controls.Add(_imgViewer);
            _sdoaqObjList = SdoaqController.LoadScript();
            _imgViewer.Set_SdoaqObj(GetSdoaqObj());

            SdoaqController.LogReceived += Sdoaq_LogDataReceived;
            SdoaqController.Initialized += Sdoaq_Initialized;

            tmr_LogUpdate.Start();
        }

        private SdoaqController GetSdoaqObj()
        {
            return _sdoaqObjList[0];
        }

        private void EnableControl(bool bEnable)
        {
            gr_Paramter.Enabled = bEnable;
            gr_Acq.Enabled = bEnable;
        }

        private void UpdateParameter(string selectLightName)
        {
            GetSdoaqObj().SetParam(SDOAQ_API.eParameterId.piSelectSettingLighting, selectLightName);

            if (GetSdoaqObj().GetParam(SDOAQ_API.eParameterId.piDataExposureTime, out bool isWritable, out string exposureTime))
            {
                txt_Param_ExposureTime.Text = exposureTime;
            }

            if (GetSdoaqObj().GetParam(SDOAQ_API.eParameterId.piDataGain, out isWritable, out string gain))
            {
                txt_Param_Gain.Text = gain;
            }
        }
        
        private void Write_Log(string str)
        {
            Sdoaq_LogDataReceived(null, new LoggerEventArgs(str));
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
            this.Invoke(new MethodInvoker(() =>
            {
                GetSdoaqObj().SetFocus(STACK_IMAGE_FOCUS);

                cmb_Param_SelectLighting.Items.Clear();
                cmb_Acq_SelectLighting.Items.Clear();
                
                foreach (var lightingName in GetSdoaqObj().GetLightingList())
                {
                    cmb_Param_SelectLighting.Items.Add(lightingName);
                    cmb_Acq_SelectLighting.Items.Add(lightingName);
                }

                if (GetSdoaqObj().GetParam(SDOAQ_API.eParameterId.piActiveLightingList, out bool isWritable, out string activeLightingName))
                {
                    cmb_Param_SelectLighting.SelectedItem = activeLightingName;

                    cmb_Acq_SelectLighting.SelectedIndexChanged -= cmb_Acq_SelectLighting_SelectedIndexChanged;
                    cmb_Acq_SelectLighting.SelectedItem = activeLightingName;
                    cmb_Acq_SelectLighting.SelectedIndexChanged += cmb_Acq_SelectLighting_SelectedIndexChanged;
                }
                
                EnableControl(true);
            }));
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

        private void SdoaqAutoFocus_Load(object sender, EventArgs e)
        {
            EnableControl(false);
        }


        private void SdoaqAutoFocus_FormClosed(object sender, FormClosedEventArgs e)
        {
            SdoaqController.LogReceived -= Sdoaq_LogDataReceived;

            GetSdoaqObj()?.AcquisitionStop();

            SdoaqController.DisposeStaticResouce();
            
            Task.Run(() => { SdoaqController.SDOAQ_Finalize(); });
        }
        
        private void btn_Param_SetData_Click(object sender, EventArgs e)
        {
            if (GetSdoaqObj().SetParam(SDOAQ_API.eParameterId.piDataExposureTime, txt_Param_ExposureTime.Text) == false)
            {
                MessageBox.Show($"Set Exposrue Time Fail!");
                return;
            }

            if (GetSdoaqObj().SetParam(SDOAQ_API.eParameterId.piDataGain, txt_Param_Gain.Text) == false)
            {
                MessageBox.Show($"Set Gain Fail!");
                return;
            }
            
        }

        private void btn_Init_Click(object sender, EventArgs e)
        {
            EnableControl(false);
            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Initialize Click");
            SdoaqController.SDOAQ_Initialize(false);
        }

        private void btn_Final_Click(object sender, EventArgs e)
        {
            EnableControl(false);
            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Finalize Click");
            SdoaqController.SDOAQ_Finalize();
        }

        private void cmb_Param_SelectLighting_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combox = sender as ComboBox;

            if (combox.SelectedItem == null)
            {
                return;
            }

            string lightingName = combox.SelectedItem.ToString();

            UpdateParameter(lightingName);
        }

        private void cmb_Acq_SelectLighting_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combox = sender as ComboBox;

            if (combox.SelectedItem == null)
            {
                return;
            }

            string lightingName = combox.SelectedItem.ToString();

            GetSdoaqObj().SetParam(SDOAQ_API.eParameterId.piActiveLightingList, lightingName);
        }

        private void btn_Acq_Click(object sender, EventArgs e)
        {
            var task = GetSdoaqObj().Acquisition_FocusStackAsync();
        }
    }
}
