using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using SDOWSIO;
using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;


namespace SdoaqCameraFrameCallback
{
    public partial class MainForm : Form
    {
        private Dictionary<int, SdoaqController> _sdoaqCamList = null;

        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLog = new object();

        private List<Control> _controlList = new List<Control>();

        private int _tickFrameUpdate = 0;
        private int _skipFrame = 0;

        private SDOAQ.SDOAQ_API.eCameraTriggerMode _seletedTriggerMode = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmSoftware;
        private bool _checkedCallBackFrame = false;

        private const int FRAME_UPDATE_TIME_INTERVAL = 100;
        private const string STR_CAM_GRABBING = "Grabbing...";
        private const string STR_CAM_IDLE = "IDLE...";

        public MainForm()
        {
            InitializeComponent();

            _controlList.AddRange(new Control[]
            {
                btnInit,
                btnFinal,

                gr_TriggerMode,

                gr_FOV,
                gr_Image,
                gr_GrabState,

                gr_AcquisitionControl,
            });

            CreateViewPanel(pb_CamImage, "Main viewer");

            _sdoaqCamList = SdoaqController.LoadScript();
            
            SdoaqController.LogReceived += Logger_DataReceived;
            SdoaqController.Initialized += MyCamera_Initialized;
            foreach (var cam in _sdoaqCamList.Values)
            {
                cam.CallBackMessageProcessed += Cam_CallBackMessageProcessed;
            }

            rdo_TrigeerMode_Cam_FreeRun.Tag = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmCameraFreerun;
            rdo_TrigeerMode_Cam_Software.Tag = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmCameraSoftware;
            rdo_TrigeerMode_Cam_External.Tag = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmCameraExternal;
            rdo_TrigeerMode_Grabber_FreeRun.Tag = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmGrabberFreerun;
            rdo_TrigeerMode_Grabber_Software.Tag = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmGrabberSoftware;
            rdo_TrigeerMode_Grabber_External.Tag = SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmGrabberExternal;

            rdo_Grab_ON.Tag = SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing;
            rdo_Grab_OFF.Tag = SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOffGrabbing;

            EnableCameraControl(false, btnInit);

            rdo_TrigeerMode_Cam_Software.Checked = true;

            tmr_LogUpdate.Start();
            tmr_GrabStatus.Start();
        }

       

        private void CreateViewPanel(Control ctrl, string viewrName)
        {
            WSIO.UTIL.WSUT_IV_CreateImageViewer(viewrName
                    , ctrl.Handle, out var hwnd, 0
                    , WSIO.UTIL.WSUTIVOPMODE.WSUTIVOPMODE_VISION
                    | WSIO.UTIL.WSUTIVOPMODE.WSUTIVOPMODE_INFOOSD);

            WSIO.UTIL.WSUT_IV_SetColor(hwnd, WSIO.UTIL.WSUTIVRESOURCE.WSUTIVRESOURCE_OUTERFRAME, ImageHelper.RGB(70, 130, 180));

            ctrl.Tag = hwnd;
        }

        private ICamera GetCamObj()
        {
            return _sdoaqCamList[0];
        }

        private void LayoutUpdate()
        {
            if (pb_CamImage.Tag != null)
            {
                WSIO.UTIL.WSUT_IV_ShowWindow((IntPtr)pb_CamImage.Tag, 1, pb_CamImage.Left, pb_CamImage.Top - 35, pb_CamImage.Right, pb_CamImage.Bottom - 35);
            }
        }

        private void Logger_DataReceived(object sender, LoggerEventArgs e)
        {
            lock (_lockLog)
            {
                _logBuffer.Append(e.Data);
            }
        }

        private void MyCamera_Initialized(object sender, SdoaqEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                _skipFrame = 0;

                GetCamObj().GetFOV(out var width, out var height, out var offset_x, out var offset_y, out int binnging);

                txt_FOV_Width.Text = width.ToString();
                txt_FOV_Height.Text = height.ToString();
                txt_FOV_Offset_X.Text = offset_x.ToString();
                txt_FOV_Offset_Y.Text = offset_y.ToString();

                GetCamObj().GetExposureTime(out var exposureTime);

                txt_ExposureTime.Text = exposureTime.ToString();

                GetCamObj().GetGain(out var gain);
                txt_Gain.Text = gain.ToString();

                SdoaqController.EanbleCameraFrameCallBack(_checkedCallBackFrame);

                GetCamObj().SetTriggerMode(_seletedTriggerMode);

                GetCamObj().GetGrabState(out var grabState);
                if (grabState == SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing)
                {
                    rdo_Grab_ON.Checked = true;
                }
                else
                {
                    rdo_Grab_OFF.Checked = true;
                }
                EnableCameraControl(true);
            }));
        }

        private void Cam_CallBackMessageProcessed(object sender, SdoaqController.CallBackMessageEventArgs e)
        {
            switch (e.Message)
            {
                case SdoaqController.emCallBackMessage.Frame:
                    {
                        //Frame is too fast to prevent UI lock
                        if (_tickFrameUpdate < Environment.TickCount)
                        {
                            var imgInfo = e.ImgInfoList[0];
                            
                            WSIO.UTIL.WSUT_IV_AttachRawImgData((IntPtr)pb_CamImage.Tag, (uint)imgInfo.Width, (uint)imgInfo.Height,
                                (uint)imgInfo.Lines,
                                (uint)imgInfo.ColorByte,
                                imgInfo.Data, (uint)imgInfo.Data.Length);

                            this.BeginInvoke(new MethodInvoker(() =>
                            {
                                var frameCount = GetCamObj().RecivedFrameCount;
                                lbl_ImageStatus.Text = $"Received Frame = { frameCount}  (Displayed Frame = { frameCount - _skipFrame})";
                            }));

                            _tickFrameUpdate = Environment.TickCount + FRAME_UPDATE_TIME_INTERVAL;
                        }
                        else
                        {
                            ++_skipFrame;
                        }
                    }
                    break;
            }
        }
        

        private void EnableCameraControl(bool bEnable, params Control[] alwaysEnableControls)
        {
            foreach (var ctrl in _controlList)
            {
                if (bEnable)
                {
                    ctrl.Enabled = true;
                }
                else if (alwaysEnableControls != null && alwaysEnableControls.Contains(ctrl))
                {
                    ctrl.Enabled = true;
                }
                else
                {
                    ctrl.Enabled = false;
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            LayoutUpdate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SdoaqController.WriteLog(Logger.emLogLevel.Info, $"SDOAQ Version : {SdoaqController.GetVersion()}");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SdoaqController.LogReceived -= Logger_DataReceived;
            SdoaqController.Initialized -= MyCamera_Initialized;

            SdoaqController.EanbleCameraFrameCallBack(false);

            foreach (ICamera cam in _sdoaqCamList.Values)
            {
                cam.SetTriggerMode(SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmSoftware);
                cam.SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOffGrabbing);
                
                (cam as SdoaqController).CallBackMessageProcessed -= Cam_CallBackMessageProcessed;
            }

            SdoaqController.DisposeStaticResouce();

            SdoaqController.SDOAQ_Finalize();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Initialize Start");
            EnableCameraControl(false);
            SdoaqController.SDOAQ_Initialize(false, true);
        }

        private void btnFinal_Click(object sender, EventArgs e)
        {
            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Finalize");
            SdoaqController.SDOAQ_Finalize();
            EnableCameraControl(false, btnInit);
        }

        private void cb_EnableCameraFrameCallBack_CheckedChanged(object sender, EventArgs e)
        {
            _checkedCallBackFrame = (sender as CheckBox).Checked;
            SdoaqController.EanbleCameraFrameCallBack(_checkedCallBackFrame);
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            LayoutUpdate();
        }

        private void rdo_TrigeerMode_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;

            if (rdo.Checked == false)
            {
                return;
            }
            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Selected TriggerMode {rdo.Text}");

            _seletedTriggerMode = (SDOAQ.SDOAQ_API.eCameraTriggerMode)rdo.Tag;
            GetCamObj().SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOffGrabbing);
            GetCamObj().SetTriggerMode(_seletedTriggerMode);
            GetCamObj().SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing);
        }

        private void btn_SwTrigger_Click(object sender, EventArgs e)
        {
            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Set Exe Software Trigger");
            GetCamObj().SetExeSoftwareTrigger();
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

        private void btn_FOV_Set_Click(object sender, EventArgs e)
        {
            GetCamObj().SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOffGrabbing);

            var width = int.Parse(txt_FOV_Width.Text);
            var height = int.Parse(txt_FOV_Height.Text);
            var offset_x = int.Parse(txt_FOV_Offset_X.Text);
            var offset_y = int.Parse(txt_FOV_Offset_Y.Text);

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"SetFOV, width = {width}, height = {height}, offset x = {offset_x}, offset y = {offset_y}");
            
            if (GetCamObj().SetFOV(width, height, offset_x, offset_y) == false)
            {
                SdoaqController.WriteLog(Logger.emLogLevel.Warning, $"SetFOV, Fail");
                return;
            }

            
            if (GetCamObj().GetFOV(out width, out height, out offset_x, out offset_y, out int binnging) == false)
            {
                SdoaqController.WriteLog(Logger.emLogLevel.Warning, $"GetFOV, Fail");
                return;
            }

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"GetFOV, width = {width}, height = {height}, offset x = {offset_x}, offset y = {offset_y}");

            txt_FOV_Width.Text = width.ToString();
            txt_FOV_Height.Text = height.ToString();
            txt_FOV_Offset_X.Text = offset_x.ToString();
            txt_FOV_Offset_Y.Text = offset_y.ToString();

            GetCamObj().SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing);
        }

        private void btn_ExposureTime_Set_Click(object sender, EventArgs e)
        {
            var exposureTime = int.Parse(txt_ExposureTime.Text);

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Set ExposureTime : {exposureTime}");
            
            if (GetCamObj().SetExposureTime(exposureTime) == false)
            {
                SdoaqController.WriteLog(Logger.emLogLevel.Warning, $"SetExposureTime, Fail");
                return;
            }
            
            if (GetCamObj().GetExposureTime(out exposureTime) == false)
            {
                SdoaqController.WriteLog(Logger.emLogLevel.Warning, $"GetExposureTime, Fail");
                return;
            }

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Get ExposureTime : {exposureTime}");

            txt_ExposureTime.Text = exposureTime.ToString();
        }

        private void btn_Gain_Set_Click(object sender, EventArgs e)
        {
            var gain = double.Parse(txt_Gain.Text);

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Set Gain : {gain}");
            
            if (GetCamObj().SetGain(gain) == false)
            {
                SdoaqController.WriteLog(Logger.emLogLevel.Warning, $"SetGain, Fail");
                return;
            }
            
            if (GetCamObj().GetGain(out gain) == false)
            {
                SdoaqController.WriteLog(Logger.emLogLevel.Warning, $"GetGain, Fail");
                return;
            }

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Get Gain : {gain}");

            txt_Gain.Text = gain.ToString();
        }
        
        private void rdo_Grab_Click(object sender, EventArgs e)
        {
            GetCamObj().SetGrabState((SDOAQ.SDOAQ_API.eCameraGrabbingStatus)(sender as Control).Tag);
        }

        private void tmr_GrabStatus_Tick(object sender, EventArgs e)
        {
            if (SdoaqController.IsInitialize == false)
            {
                btn_GrabStatus.BackColor = System.Drawing.Color.Transparent;
                btn_GrabStatus.Text = string.Empty;
                return;
            }

            GetCamObj().GetGrabState(out var grabState);

            if (grabState == SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing)
            {
                btn_GrabStatus.BackColor = System.Drawing.Color.LawnGreen;
                btn_GrabStatus.Text = STR_CAM_GRABBING;
            }
            else
            {
                btn_GrabStatus.BackColor = System.Drawing.Color.Yellow;
                btn_GrabStatus.Text = STR_CAM_IDLE;
            }
        }
    }
}
