using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SDOWSIO;
using SDOAQNet;
using SDOAQNet.Tool;

namespace SdoaqMultiCameraFrameCallback
{
    public partial class CamViewer : UserControl
    {
        private int _tickFrameUpdate = 0;
        private int _skipFrame = 0;
        private SdoaqController _camObj;

        private const int FRAME_UPDATE_TIME_INTERVAL = 100;
        public CamViewer()
        {
            InitializeComponent();

            CreateViewPanel(pb_CamImage, "Main viewer");
        }

        public void Set_CamObject(SdoaqController camObj)
        {
            if (_camObj != null)
            {
                _camObj.CallBackMessageProcessed -= CamObj_CallBackMessageProcessed;
            }

            _camObj = camObj;
            _camObj.CallBackMessageProcessed += CamObj_CallBackMessageProcessed;
        }

        

        public void Update_CamParam()
        {
            if (_camObj == null)
            {
                return;
            }

            this.Invoke(new MethodInvoker(() =>
            {
                var camObj = _camObj as ICamera;

                _skipFrame = 0;

                camObj.GetFOV(out var width, out var height, out var offset_x, out var offset_y, out int binnging);

                txt_FOV_Width.Text = width.ToString();
                txt_FOV_Height.Text = height.ToString();
                txt_FOV_Offset_X.Text = offset_x.ToString();
                txt_FOV_Offset_Y.Text = offset_y.ToString();

                camObj.GetExposureTime(out var exposureTime);

                txt_ExposureTime.Text = exposureTime.ToString();

                camObj.GetGain(out var gain);
                txt_Gain.Text = gain.ToString();

                camObj.SetTriggerMode(SDOAQ.SDOAQ_API.eCameraTriggerMode.ctmSoftware);
                camObj.SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing);
            }));
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

        private void LayoutUpdate()
        {
            if (pb_CamImage.Tag != null)
            {
                WSIO.UTIL.WSUT_IV_ShowWindow((IntPtr)pb_CamImage.Tag, 1, pb_CamImage.Left, pb_CamImage.Top - 35, pb_CamImage.Right, pb_CamImage.Bottom - 35);
            }
        }

        private void CamObj_CallBackMessageProcessed(object sender, SdoaqController.CallBackMessageEventArgs e)
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
                                var frameCount = (_camObj as ICamera).RecivedFrameCount;
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

        private void btn_FOV_Set_Click(object sender, EventArgs e)
        {
            var cam = _camObj as ICamera;

            cam.SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOffGrabbing);

            var width = int.Parse(txt_FOV_Width.Text);
            var height = int.Parse(txt_FOV_Height.Text);
            var offset_x = int.Parse(txt_FOV_Offset_X.Text);
            var offset_y = int.Parse(txt_FOV_Offset_Y.Text);

            _camObj.AppendLog(Logger.emLogLevel.User, $"SetFOV, width = {width}, height = {height}, offset x = {offset_x}, offset y = {offset_y}");
            
            if (cam.SetFOV(width, height, offset_x, offset_y) == false)
            {
                _camObj.AppendLog(Logger.emLogLevel.Warning, $"SetFOV, Fail");
                return;
            }
            
            if (cam.GetFOV(out width, out height, out offset_x, out offset_y, out int binnging) == false)
            {
                _camObj.AppendLog(Logger.emLogLevel.Warning, $"GetFOV, Fail");
                return;
            }

            _camObj.AppendLog(Logger.emLogLevel.User, $"GetFOV, width = {width}, height = {height}, offset x = {offset_x}, offset y = {offset_y}");

            txt_FOV_Width.Text = width.ToString();
            txt_FOV_Height.Text = height.ToString();
            txt_FOV_Offset_X.Text = offset_x.ToString();
            txt_FOV_Offset_Y.Text = offset_y.ToString();

            cam.SetGrabState(SDOAQ.SDOAQ_API.eCameraGrabbingStatus.cgsOnGrabbing);
        }

        private void btn_ExposureTime_Set_Click(object sender, EventArgs e)
        {
            var cam = _camObj as ICamera;

            var exposureTime = int.Parse(txt_ExposureTime.Text);

            _camObj.AppendLog(Logger.emLogLevel.User, $"Set ExposureTime : {exposureTime}");
            
            if (cam.SetExposureTime(exposureTime) == false)
            {
                _camObj.AppendLog(Logger.emLogLevel.Warning, $"SetExposureTime, Fail");
                return;
            }
            
            if (cam.GetExposureTime(out exposureTime) == false)
            {
                _camObj.AppendLog(Logger.emLogLevel.Warning, $"GetExposureTime, Fail");
                return;
            }

            _camObj.AppendLog(Logger.emLogLevel.User, $"Get ExposureTime : {exposureTime}");

            txt_ExposureTime.Text = exposureTime.ToString();
        }

        private void btn_Gain_Set_Click(object sender, EventArgs e)
        {
            var cam = _camObj as ICamera;

            var gain = double.Parse(txt_Gain.Text);

            _camObj.AppendLog(Logger.emLogLevel.User, $"Set Gain : {gain}");

            var rv = cam.SetGain(gain);

            if (cam.SetGain(gain) == false)
            {
                _camObj.AppendLog(Logger.emLogLevel.Warning, $"SetGain, Fail ({rv})");
                return;
            }

            
            if (cam.GetGain(out gain) == false)
            {
                _camObj.AppendLog(Logger.emLogLevel.Warning, $"GetGain, Fail ({rv})");
                return;
            }

            _camObj.AppendLog(Logger.emLogLevel.User, $"Get Gain : {gain}");

            txt_Gain.Text = gain.ToString();
        }

        private void pnl_View_Resize(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            pb_CamImage.SetBounds(0, lbl_ImageStatus.Bottom, panel.Width, panel.Height - lbl_ImageStatus.Height);
            LayoutUpdate();
        }

        private void btn_SwTrigger_Click(object sender, EventArgs e)
        {
            var cam = _camObj as ICamera;
            
            cam.SetExeSoftwareTrigger();

            _camObj.AppendLog(Logger.emLogLevel.User, $"Set Exe Software Trigger");
        }
    }
}
