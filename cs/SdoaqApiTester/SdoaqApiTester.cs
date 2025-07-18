using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;

namespace SdoaqApiTester
{
	/// <summary>
	/// Single WiseScope Control Example
	/// </summary>
	public partial class SdoaqApiTester : Form
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

		private SdoaqImageViewr _imgViewer;
		//----------------------------------------------------------------------------
		public SdoaqApiTester()
		{
			InitializeComponent();

			_imgViewer = new SdoaqImageViewr(true);
			_imgViewer.Dock = DockStyle.Fill;

			pnl_Viewer.Controls.Add(_imgViewer);

			_chkEdofImgViewOptionList = new Dictionary<SdoaqController.emEofImgViewOption, CheckBox>()
			{
				{ SdoaqController.emEofImgViewOption.StepMap, chk_StepMap },
				{ SdoaqController.emEofImgViewOption.QuaalityMap, chk_QualityMap },
				{ SdoaqController.emEofImgViewOption.HeightMap, chk_HeightMap },
				{ SdoaqController.emEofImgViewOption.PointClound, chk_PointCloud },
				{ SdoaqController.emEofImgViewOption.Edof, chk_Edof },
			};

			//_sdoaqObjList = MySdoaq.LoadScript(MySdoaq.emPlayerMethod.Thread); // To implement continuous mode as a single shot thread.
			_sdoaqObjList = SdoaqController.LoadScript();

			_imgViewer.Set_SdoaqObj(GetSdoaqObj());
			cmp_SdoaqParams.Set_SdoaqObj(GetSdoaqObj());

            SdoaqController.LogReceived += Sdoaq_LogDataReceived;
            SdoaqController.Initialized += Sdoaq_Initialized;
		}

		private SdoaqController GetSdoaqObj()
		{
			return _sdoaqObjList[0];
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
					EnableGroup(bEnableInit: true, bEnableParam: true, bEnableEdofOption: true, bEnableAcq: true);
					EnableAcqGroup_Idle();
					cmp_SdoaqParams.Update_Param();
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

		private void SdoaqApiTester_Load(object sender, EventArgs e)
		{
			LayoutUpdate();

			tmr_LogUpdate.Start();

			EnableGroup(bEnableInit: true, bEnableParam: true, bEnableEdofOption: true, bEnableAcq: true);
			EnableAcqGroup_Idle();

			WriteLog($">> SDOAQ DLL Version = {SdoaqController.GetVersion()}");
            WriteLog($">> sdedof dll Version = {SdoaqController.GetVersion_SdEdofAlgorithm()}");
        }

        private void SdoaqApiTester_FormClosed(object sender, FormClosedEventArgs e)
		{
            SdoaqController.SDOAQ_Finalize();

			SdoaqController.LogReceived -= Sdoaq_LogDataReceived;
            SdoaqController.Initialized -= Sdoaq_Initialized;

			_logBuffer = null;

			foreach (var sdoaqObj in _sdoaqObjList.Values)
			{
				sdoaqObj.Dispose();
			}
            SdoaqController.DisposeStaticResouce();
        }

		private void SdoaqApiTester_Resize(object sender, EventArgs e)
		{
			LayoutUpdate();
		}

		private void btn_Init_Click(object sender, EventArgs e)
		{
            SdoaqController.SDOAQ_Initialize(false);
			
			MySdoaq.SDOAQ_RegisterMoveOk();
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
					//EnableGroup(bEnableParam: false, bEnableAcq: true);
					EnableAcqGroup_Continuous(btn_StopAF);
				}
			}
			else if (btn == btn_StopAF)
			{
				GetSdoaqObj().AcquisitionStop_Af();
				//EnableGroup(bEnableParam: true, bEnableAcq: true);
				EnableAcqGroup_Idle();
			}
		}

		private void btn_AcqMode_Snap_Click(object sender, EventArgs e)
		{
			string snapPath = System.IO.Path.Combine(@"C:\SDOAQ\Snap", $"{DateTime.Now:yyyy.MMM.dd.HHmmss}");

			GetSdoaqObj().Acquisition_Snap(snapPath);
		}
		#endregion

	}
}
