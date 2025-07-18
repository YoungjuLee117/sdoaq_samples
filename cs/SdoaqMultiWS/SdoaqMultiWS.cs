using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;


namespace SdoaqMultiWS
{
    /// <summary>
    /// Single WiseScope Control Example
    /// </summary>
	public partial class SdoaqMultiWS : Form
	{
        private enum AcquisitionMode
        {
            SingleShot,
            Continuous,
            Stop,
        }

        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLogBuffer = new object();

        private Dictionary<SdoaqController.emEofImgViewOption, CheckBox> _chkEdofImgViewOptionList;
        private Dictionary<int, SdoaqController> _sdoaqObjList = null;

        private List<SdoaqImageViewr> _imgViewerList = new List<SdoaqImageViewr>();
        private List<RadioButton> _rdoSelWsList = new List<RadioButton>();
        private int _currentIdxWs = 1;

        private static readonly Size SIZE_RDO_WS_SELECT = new Size(60, 30);
        private static readonly Point LOCATION_RDO_WS_SELECT_START = new Point(7, 20);
        //----------------------------------------------------------------------------
        public SdoaqMultiWS()
		{
			InitializeComponent();

            _chkEdofImgViewOptionList = new Dictionary<SdoaqController.emEofImgViewOption, CheckBox>()
            {
                { SdoaqController.emEofImgViewOption.StepMap, chk_StepMap },
                { SdoaqController.emEofImgViewOption.QuaalityMap, chk_QualityMap },
                { SdoaqController.emEofImgViewOption.HeightMap, chk_HeightMap },
                { SdoaqController.emEofImgViewOption.PointClound, chk_PointCloud },
                { SdoaqController.emEofImgViewOption.Edof, chk_Edof },
            };
            
            _sdoaqObjList = SdoaqController.LoadScript();

            for (int i = 0; i < _sdoaqObjList.Count; i++)
            {
                var imgViewer = new SdoaqImageViewr(i == 0 ? true : false);// Only one 3D viewer is supported

                imgViewer.Set_SdoaqObj(_sdoaqObjList[i]);
                
                _imgViewerList.Add(imgViewer);

                int idxWs = i + 1;
                
                var rdo = new RadioButton()
                {
                    AutoSize = false,
                    Tag = idxWs,
                    Name = $"rdo_WS_{idxWs}",
                    Text = $"WS {idxWs}",     
                    Size = SIZE_RDO_WS_SELECT,
                };
                rdo.CheckedChanged += rdo_SelWS_CheckedChanged;

                _rdoSelWsList.Add(rdo);
            }

            _imgViewerList.ForEach(imgViewr => pnl_Viewer.Controls.Add(imgViewr));
            _rdoSelWsList.ForEach(rdo => gr_SelectWS.Controls.Add(rdo));

            int rdoLeft = LOCATION_RDO_WS_SELECT_START.X;
            int rdoGap = 5;
            foreach (var rdo in _rdoSelWsList)
            {
                rdo.Location = new Point(rdoLeft, LOCATION_RDO_WS_SELECT_START.Y);
                rdoLeft = rdo.Right + rdoGap;
            }

            SdoaqController.LogReceived += Sdoaq_LogDataReceived;
            SdoaqController.Initialized += Sdoaq_Initialized;            
        }
        
        private SdoaqController GetSdoaqObj()
        {
            return _sdoaqObjList[Math.Max(0, _currentIdxWs - 1)];
        }

        private void LayoutUpdate()
        {   
            var rcArea = this.ClientRectangle;
            var rcControl = pnl_Control.ClientRectangle;

            var rcView = new Rectangle(rcControl.Right, 
                rcArea.Top,
                rcArea.Width - rcControl.Width,
                rcArea.Height);

            rcView = rcView.ShrinkRect(2);

            var rcView_Rows = rcView.DivideRect_Row(2, 4, new float[] { 0.3f, 0.7f });

            rtxb_Log.SetBounds(rcView_Rows[0]);
            pnl_Viewer.SetBounds(rcView_Rows[1]);

            var rcImgViewerList_Rows = pnl_Viewer.ClientRectangle.DivideRect_Row(_imgViewerList.Count);

            for (int i = 0; i < _imgViewerList.Count; i++)
            {
                if (_imgViewerList[i].VisiblePointCloud)
                {
                    _imgViewerList[i].SetBounds(rcImgViewerList_Rows[i]);
                }
                else
                {
                    var tmpCols = rcImgViewerList_Rows[i].DivideRect_Col(2);
                    _imgViewerList[i].SetBounds(tmpCols[0]);
                }
            }
        }

        private void WriteLog(string log, bool bNewLine = true)
        {
            lock (_lockLogBuffer)
            {
                _logBuffer.Append($"{log}{(bNewLine ? Environment.NewLine : string.Empty)}");
            }
        }

        private void EnableGroup(bool? bEnableInit = null, bool? bEnableParam = null, bool? bEnableEdofOption = null, bool? bEnableAcq = null)
        {
            if (bEnableInit != null)
            {
                pnl_Init.Enabled = (bool)bEnableInit;
            }
            
            if (bEnableParam != null) 
            {
                cmp_SdoaqParams.Enabled = (bool)bEnableParam;
            }
            
            if (bEnableEdofOption != null)
            {
                gr_EdofImgViewOption.Enabled = (bool)bEnableEdofOption;
            }
            
            if (bEnableAcq != null)
            {
                gr_Acquisition.Enabled = (bool)bEnableAcq;
            }
        }

        private void EnableAcqGroup_Continuous(Control ctrlEnableStopButton)
        {
            foreach (Control control in gr_Acquisition.Controls)
            {
                if (control == ctrlEnableStopButton)
                {
                    control.Enabled = true;
                }
                else
                {
                    control.Enabled = false;
                }
            }
            btn_Snap.Enabled = true;
        }

        private void EnableAcqGroup_Idle()
        {
            foreach (Control control in gr_Acquisition.Controls)
            {
                control.Enabled = !control.Name.Contains("Stop");
            }
            btn_Snap.Enabled = false;
        }

        private void UpdateSdoaqParam(int idxWs)
        {
            _currentIdxWs = idxWs;
           
            cmp_SdoaqParams.Set_SdoaqObj(GetSdoaqObj());
            if (SdoaqController.IsInitialize)
            {
                cmp_SdoaqParams.Update_Param();
            }
        }

        #region My SDOAQ Object
        private void Sdoaq_LogDataReceived(object sender, LoggerEventArgs e)
        {
            WriteLog(e.Data, false);
        }

        private void Sdoaq_Initialized(object sender, SdoaqEventArgs e)
        {
            this.Invoke(() =>
            {
                if (e.ErrorCode == SDOAQ.SDOAQ_API.eErrorCode.ecNoError)
                {
                    UpdateSdoaqParam(_currentIdxWs);

                    EnableGroup(bEnableInit: true, bEnableParam: true, bEnableEdofOption: true, bEnableAcq: true);
                    EnableAcqGroup_Idle();
                }
                else
                {
                    EnableGroup(bEnableInit: true, bEnableParam: false, bEnableEdofOption: false, bEnableAcq: false);
                }
            });            
        }
        #endregion

        #region Event
        private void tmr_LogUpdate_Tick(object sender, EventArgs e)
        {
            if (_logBuffer.Length == 0)
            {
                return;
            }

            lock (_lockLogBuffer)
            {
                rtxb_Log.AppendText(_logBuffer.ToString());
                rtxb_Log.ScrollToCaret();
                _logBuffer.Clear();
            }
        }

        private void SdoaqMultiWS_Load(object sender, EventArgs e)
        {
            LayoutUpdate();

            tmr_LogUpdate.Start();

            EnableGroup(bEnableInit: true, bEnableParam: true, bEnableEdofOption: true, bEnableAcq: true);
            EnableAcqGroup_Idle();

            WriteLog($">> SDOAQ DLL Version = {SdoaqController.GetVersion()}");
            //WriteLog($">> sdedof dll Version = {MySdoaq.GetVersion_SdEdofAlgorithm()}");

            _rdoSelWsList[0].Checked = true;
        }

		private void SdoaqMultiWS_FormClosed(object sender, FormClosedEventArgs e)
		{
            SdoaqController.LogReceived -= Sdoaq_LogDataReceived;
            SdoaqController.Initialized -= Sdoaq_Initialized;

            foreach (var sdoaqObj in _sdoaqObjList.Values)
            {
                sdoaqObj.AcquisitionStop();
            }

            SdoaqController.DisposeStaticResouce();

            _logBuffer = null;
            
            // Run SDOAQ_Finalize method asynchronously, preventing the main UI thread from being blocked.
            // When the form close button is clicked while image acquisition is in progress in C# application, the SDOAQ finalization is delayed.
            Task.Run(() => { SdoaqController.SDOAQ_Finalize(); });
        }

		private void SdoaqMultiWS_Resize(object sender, EventArgs e)
        {
            LayoutUpdate();
        }

        private void btn_Init_Click(object sender, EventArgs e)
        {
            SdoaqController.SDOAQ_Initialize(false);
        }

        private void btn_Final_Click(object sender, EventArgs e)
        {
            SdoaqController.SDOAQ_Finalize();
        }

        private void btn_AcqMode_Stack_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            if (btn == btn_AcqStack)
            {
                var task = GetSdoaqObj().Acquisition_FocusStackAsync();
            }
            else if (btn == btn_ContiStack)
            {
                if (GetSdoaqObj().AcquisitionContinuous_FocusStack())
                {
                    //EnableGroup(bEnableParam: false, bEnableAcq: true);
                    EnableAcqGroup_Continuous(btn_StopStack);
                }
            }
            else if (btn == btn_StopStack)
            {
                GetSdoaqObj().AcquisitionStop_FocusStack();
                //EnableGroup(bEnableParam: true, bEnableAcq: true);
                EnableAcqGroup_Idle();
			}
		}

        private void btn_AcqMode_Edof_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            var edofImageOption = new SdoaqController.EdofImageList()
            {
                EnableEdofImg = chk_Edof.Checked,
                EnableStepMapImg = chk_StepMap.Checked,
                EnableQualityMap = chk_QualityMap.Checked,
                EnableHeightMap = chk_HeightMap.Checked,
                EnablePointCloud = chk_PointCloud.Checked,
            };

            if (btn == btn_AcqEdof)
            {
                var task = GetSdoaqObj().Acquisition_EdofAsync(edofImageOption);
            }
            else if (btn == btn_ContiEdof)
            {
                if (GetSdoaqObj().AcquisitionContinuous_Edof(edofImageOption))
                {
                    //EnableGroup(bEnableParam: false, bEnableEdofOption: false, bEnableAcq: true);
                    EnableAcqGroup_Continuous(btn_StopEdof);
                }
            }
            else if (btn == btn_StopEdof)
            {
                GetSdoaqObj().AcquisitionStop_Edof();
                //EnableGroup(bEnableParam: true, bEnableEdofOption: true, bEnableAcq: true);
                EnableAcqGroup_Idle();
            }
        }

        private void btn_AcqMode_Af_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            if (btn == btn_AcqAF)
            {
                var task = GetSdoaqObj().Acquisition_AfAsync();
            }
            else if (btn == btn_ContiAF)
            {
                if (GetSdoaqObj().AcquisitionContinuous_Af())
                {
                    EnableGroup(bEnableParam: false, bEnableAcq: true);
                    EnableAcqGroup_Continuous(btn_StopAF);
                }
            }
            else if (btn == btn_StopAF)
            {
                GetSdoaqObj().AcquisitionStop_Af();
                EnableGroup(bEnableParam: true, bEnableAcq: true);
                EnableAcqGroup_Idle();
            }
        }

        private void btn_AcqMode_Snap_Click(object sender, EventArgs e)
        {
            string snapPath = System.IO.Path.Combine(@"C:\SDOAQ\Snap", $"{DateTime.Now:yyyy.MMM.dd.HHmmss}");

            GetSdoaqObj().Acquisition_Snap(snapPath);
        }

        private void rdo_SelWS_CheckedChanged(object sender, EventArgs e)
        {
            var rdo = sender as RadioButton;

            if (rdo.Checked)
            {
                UpdateSdoaqParam((int)rdo.Tag);

                if (GetSdoaqObj().IsRunPlayer)
                {
                    EnableGroup(bEnableParam: false, bEnableEdofOption: false, bEnableAcq: true);
                    Control control = null;

                    switch (GetSdoaqObj().PlayerMode)
                    {
                        case SdoaqController.emPlayerMode.FocusStack: control = btn_StopStack; break;
                        case SdoaqController.emPlayerMode.Edof: control = btn_StopEdof; break;
                        case SdoaqController.emPlayerMode.Af: control = btn_StopAF; break;
                    }
                    EnableAcqGroup_Continuous(control);
                }
                else
                {
                    EnableGroup(bEnableParam: true, bEnableEdofOption: true, bEnableAcq: true);
                    EnableAcqGroup_Idle();                  
                }
            }
        }
        #endregion

    }
}
