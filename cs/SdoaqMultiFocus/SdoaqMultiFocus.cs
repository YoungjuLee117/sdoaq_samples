using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

using SDOAQNet;
using SDOAQNet.Component;
using SDOAQNet.Tool;
using SDOAQ;

using BrightIdeasSoftware;


namespace SdoaqMultiFocus
{
    public partial class SdoaqMultiFocus : Form
    {
        private StringBuilder _logBuffer = new StringBuilder();
        private object _lockLog = new object();
        private Dictionary<int, SdoaqController> _sdoaqObjList = null;

        private SdoaqImageViewr _imgViewer;
        private ObjectListView _olvMuiltiFocusItemList;
        private MultiFocusItem _selectedItem;

        private List<MultiFocusItem> _multiFocusItemList = new List<MultiFocusItem>();
        
        public SdoaqMultiFocus()
        {
            InitializeComponent();

            _imgViewer = new SdoaqImageViewr(false);
            _imgViewer.VisiBleImageListBox = false;
            _imgViewer.Dock = DockStyle.Fill;

            pnl_Viewer.Controls.Add(_imgViewer);
            _sdoaqObjList = SdoaqController.LoadScript();
            _imgViewer.Set_SdoaqObj(GetSdoaqObj());

            _olvMuiltiFocusItemList = new ObjectListView();
            
            pnl_ListView.Controls.Add(_olvMuiltiFocusItemList);

            BuildListView(_olvMuiltiFocusItemList, _multiFocusItemList);

            foreach (var val in Enum.GetValues(typeof(emMultiFocusFunc)))
            {
                cmb_Param_func.Items.Add(val.ToString());
            }

            SdoaqController.LogReceived += Sdoaq_LogDataReceived;
            SdoaqController.Initialized += Sdoaq_Initialized;
            
            tmr_LogUpdate.Start();
        }

        private SdoaqController GetSdoaqObj()
        {
            return _sdoaqObjList[0];
        }

        private void BuildListView(ObjectListView olv, List<MultiFocusItem> data)
        {
            olv.VirtualMode = false;
            olv.FullRowSelect = true;
            olv.GridLines = true;
            olv.HideSelection = false;            
            olv.ShowGroups = false;
            olv.MultiSelect = false;
            olv.IsHeaderSorting = false;
            olv.PrimarySortOrder = SortOrder.None;
            olv.PrimarySortColumn = null;
            olv.UseSubItemCheckBoxes = true;
            olv.Dock = DockStyle.Fill;

            olv.SelectionChanged += olv_SelectionChanged;

            olv.Clear();

            Type itemType = typeof(MultiFocusItem);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var column = new OLVColumn
                {
                    Text = prop.Name,
                    AspectName = prop.Name,
                    TextAlign = HorizontalAlignment.Center,
                };
                
                if (prop.PropertyType == typeof(Rectangle))
                {
                    column.AspectGetter = row =>
                    {
                        var val = (Rectangle)prop.GetValue(row);
                        return $"({val.Left},{val.Top},{val.Width},{val.Height})";
                    };
                }

                olv.Columns.Add(column);
            }

            const double col_1_ratio = 0.15;
            const double col_2_ratio = 0.2;

            int totalWidth = olv.ClientSize.Width;
            int columnCount = olv.Columns.Count;
            
            int col_1_Width = (int)(totalWidth * col_1_ratio);
            int col_2_Width = (int)(totalWidth * col_2_ratio);

            int remainingWidth = totalWidth - col_1_Width - col_2_Width;
            int otherWidth = remainingWidth / (columnCount - 2);
            int remainder = remainingWidth % (columnCount - 2);

            for (int i = 0; i < columnCount; i++)
            {
                var col = olv.GetColumn(i);

                if (i == 0)
                {
                    col.Width = col_1_Width;
                }
                else if (i == 1)
                {
                    col.Width = col_2_Width;
                }
                else
                {
                    col.Width = otherWidth + (i == columnCount - 1 ? remainder : 0);
                }
            }

            olv.SetObjects(data, true);
        }

        private void Write_Log(string str)
        {
            Sdoaq_LogDataReceived(null, new LoggerEventArgs(str + Environment.NewLine));
        }
        
        private string GetFunctionScript()
        {
            /*	MF script format example
	        // Number of MR MULTI = 4
		    // MR MULTI 0 = {1,2,23,1159,400,1445,675}
		    // MR MULTI 1 = {2,1,23,879,707,1010,945}
		    // MR MULTI 2 = {3,1,23,697,1041,927,1385}
		    // MR MULTI 3 = {4,2,23,183,408,578,971}	
            */

            var sbScript = new StringBuilder();
            int count = _multiFocusItemList.Count;

            sbScript.AppendLine($"Number of MR MULTI = {count }");

            for(int i = 0; i < count; i++)
            {
                sbScript.AppendLine(_multiFocusItemList[i].GetParam_ScriptFormat(i));
            }

            return sbScript.ToString();
        }

        private void UpdateParameter(MultiFocusItem item)
        {
            _selectedItem = item;

            txt_Param_id.Text = item.Id.ToString();
            cmb_Param_func.SelectedItem = item.Func.ToString();
            txt_Param_focus.Text = item.Focus.ToString();
            txt_Param_rect.Text = item.GetParam_Rect();
        }
        
        private bool GetMultiFocusItemByUI(out MultiFocusItem item)
        {
            item = null;
            int id;
            emMultiFocusFunc func;
            int focus;
            Rectangle? rect;
            try
            {
                id = int.Parse(txt_Param_id.Text);
                func = (emMultiFocusFunc)Enum.Parse(typeof(emMultiFocusFunc), cmb_Param_func.SelectedItem.ToString());
                focus = int.Parse(txt_Param_focus.Text);

                string[] spltRectValue = txt_Param_rect.Text.Split(',');

                rect = new Rectangle(int.Parse(spltRectValue[0]), int.Parse(spltRectValue[1]),
                    int.Parse(spltRectValue[2]), int.Parse(spltRectValue[3]));

                item = new MultiFocusItem()
                {
                    Id = id,
                    Func = func,
                    Focus = focus,
                    Rect = rect.Value,
                };

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"GetMultiFocusItemByUI() Exception! {ex}");
                return false;
            }
        }

        public bool ContainsMultiFocusItemId(MultiFocusItem checkItem)
        {
            foreach (var item in _multiFocusItemList)
            {
                if (item == checkItem)
                {
                    continue;
                }

                if (item.Id == checkItem.Id)
                {
                    Write_Log($"id({checkItem.Id}) duplicate");
                    return false;
                }
            }

            return true;
        }

        private void ResetItemList()
        {
            _multiFocusItemList.Clear();
            _selectedItem = null;

            _multiFocusItemList.Add(MultiFocusItem.GetDummy());

            _olvMuiltiFocusItemList.SetObjects(_multiFocusItemList, true);

            GetSdoaqObj().UpdateScriptPlayMf(GetFunctionScript());
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
                const int unit = 32;
                GetSdoaqObj().GetIntParamRange(SDOAQ_API.eParameterId.piFocusPosition, out int lowFocus, out int highFocus);
                GetSdoaqObj().SetFocus($"{lowFocus}-{highFocus}-{unit}");

                const int focusStepOutside = 160;
                GetSdoaqObj().SetParam(SDOAQ_API.eParameterId.piSingleFocus, focusStepOutside.ToString());
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
            SdoaqController.SDOAQ_Initialize(false);

            ResetItemList();
        }
        
        private void SdoaqAutoFocus_FormClosed(object sender, FormClosedEventArgs e)
        {
            SdoaqController.LogReceived -= Sdoaq_LogDataReceived;

            GetSdoaqObj()?.AcquisitionStop();

            SdoaqController.DisposeStaticResouce();
            
            Task.Run(() => { SdoaqController.SDOAQ_Finalize(); });
        }

        private void olv_SelectionChanged(object sender, EventArgs e)
        {
            var olv = sender as ObjectListView;

            var obj = olv.SelectedObject;

            if (obj == null)
            {
                return;
            }

            UpdateParameter(obj as MultiFocusItem);
        }

        private void btn_Acq_Click(object sender, EventArgs e)
        {
            var task = GetSdoaqObj().Acquisition_FocusStackAsync();
        }

        private void btn_Script_Modify_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            if (GetMultiFocusItemByUI(out var item) == false)
            {
                return;
            }
            
            _selectedItem.SetParam(item);
            _olvMuiltiFocusItemList.UpdateObject(_selectedItem);

            GetSdoaqObj().UpdateScriptPlayMf(GetFunctionScript());
        }

        private void btn_Script_Add_Click(object sender, EventArgs e)
        {
            if (GetMultiFocusItemByUI(out var item) == false)
            {
                return;
            }

            if (ContainsMultiFocusItemId(item) == false)
            {
                return;
            }

            _multiFocusItemList.Add(item);
            _olvMuiltiFocusItemList.SetObjects(_multiFocusItemList, true);
            _olvMuiltiFocusItemList.SelectedObject = item;

            GetSdoaqObj().UpdateScriptPlayMf(GetFunctionScript());
        }

        private void btn_Script_Remove_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            _multiFocusItemList.Remove(_selectedItem);
            _selectedItem = null;
            _olvMuiltiFocusItemList.SetObjects(_multiFocusItemList, true);

            GetSdoaqObj().UpdateScriptPlayMf(GetFunctionScript());
        }

        private void btn_Script_Reset_Click(object sender, EventArgs e)
        {
            ResetItemList();
        }

        private void btn_Param_focusStepOutside_Click(object sender, EventArgs e)
        {
            GetSdoaqObj().SetParam(SDOAQ_API.eParameterId.piSingleFocus, txt_Param_focusStepOutside.Text);
        }

        private void btn_PlayMF_Click(object sender, EventArgs e)
        {
            GetSdoaqObj().AcquisitionContinuous_Mf(GetFunctionScript());
        }

        private void btn_StopMF_Click(object sender, EventArgs e)
        {
            GetSdoaqObj().AcquisitionStop_Mf();
        }

        private void btn_Param_rect_get_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }
            
            var rect = _imgViewer.GetImageViewrDragArea();

            txt_Param_rect.Text = $"{rect.Left},{rect.Top},{rect.Width},{rect.Height}";
        }

        private void btn_Param_rect_set_Click(object sender, EventArgs e)
        {
            if (GetMultiFocusItemByUI(out var item) == false)
            {
                return;
            }

            _imgViewer.SetImageViewrDragArea(item.Rect);
        }
    }
}
