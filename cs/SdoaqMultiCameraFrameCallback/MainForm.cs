using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using SDOWSIO;
using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;


namespace SdoaqMultiCameraFrameCallback
{
    public partial class MainForm : Form
    {
        private Dictionary<int, SdoaqController> _sdoaqCamList = null;
        private Dictionary<int, CamViewer> _camViewerList;

        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLog = new object();
        
        private readonly static SolidBrush BRUSH_SELECTED_TAB = new SolidBrush(Color.FromArgb(122, 159, 205));

        public MainForm()
        {
            InitializeComponent();
            
            _sdoaqCamList = SdoaqController.LoadScript();
            _camViewerList = new Dictionary<int, CamViewer>();

            foreach (var item in _sdoaqCamList)
            {
                int idxCam = (int)item.Value.CamIndex + 1;
                var camViewer = new CamViewer();
                camViewer.Name = $"CamViewr_Cam{idxCam}";

                camViewer.Dock = DockStyle.Fill;
                camViewer.Set_CamObject(item.Value);

                var tabPage = new TabPage();
                tabPage.Name = $"CamParamPage_{idxCam}";
                tabPage.Text = $"Cam {idxCam}";
                tabPage.Controls.Add(camViewer);

                tab_CamPage.Controls.Add(tabPage);


                _camViewerList.Add(item.Key, camViewer);
            }

            SdoaqController.LogReceived += Logger_DataReceived;
            SdoaqController.Initialized += MyCamera_Initialized;
            
            tmr_LogUpdate.Start();
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
            SdoaqController.EanbleCameraFrameCallBack(true);

            foreach (var item in _camViewerList)
            {
                item.Value.Update_CamParam();
            }
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            SdoaqController.WriteLog(Logger.emLogLevel.Info, $"SDOAQ Version : {SdoaqController.GetVersion()}");

            SdoaqController.WriteLog(Logger.emLogLevel.User, $"Initialize Start");

            SdoaqController.SDOAQ_Initialize(false, true);
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
            }

            SdoaqController.DisposeStaticResouce();

            SdoaqController.SDOAQ_Finalize();
        }
        
        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
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
        
        private void tab_CamPage_DrawItem(object sender, DrawItemEventArgs e)
        {

            var tab = sender as TabControl;
            Rectangle paddedBounds = e.Bounds;
            Size textLength = TextRenderer.MeasureText(tab.TabPages[e.Index].Text, e.Font);

            paddedBounds.Inflate(-((e.Bounds.Width - textLength.Width) / 2), -((e.Bounds.Height - textLength.Height) / 2));

            if (tab.SelectedIndex == e.Index)
            {
                e.Graphics.FillRectangle(BRUSH_SELECTED_TAB, e.Bounds);
                e.Graphics.DrawString(tab.TabPages[e.Index].Text, e.Font, SystemBrushes.WindowText, paddedBounds);
            }
            else
            {
                e.Graphics.DrawString(tab.TabPages[e.Index].Text, e.Font, SystemBrushes.GrayText, paddedBounds);
            }
        }
    }
}
