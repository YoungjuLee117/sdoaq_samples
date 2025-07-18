using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SDOAQ;
using SDOAQNet.Tool;

namespace SDOAQNet.Component
{
    public partial class SdoaqParams : UserControl
    {
        private SdoaqController _sdoaqObj;

        private static readonly Size INIT_SIZE = new Size(500, 355);
        public SdoaqParams()
        {
            InitializeComponent();

            txt_ParamValue.Enabled = false;
            btn_SetParam.Enabled = false;
            foreach (SDOAQ_API.eParameterId paramID in Enum.GetValues(typeof(SDOAQ_API.eParameterId)))
            {
                cmb_Param.Items.Add(paramID.ToString());
            }

            cmb_EdofResizeRatio.Items.Add(SdoaqController.RESIZE_RATIO_ORIGINAL.ToString());
            cmb_EdofResizeRatio.Items.Add(SdoaqController.RESIZE_RATIO_HALF.ToString());
            cmb_EdofResizeRatio.Items.Add(SdoaqController.RESIZE_RATIO_QUARTER.ToString());
            cmb_EdofResizeRatio.SelectedItem = SdoaqController.RESIZE_RATIO_ORIGINAL.ToString();
        }

        public void Set_SdoaqObj(SdoaqController sdoaqObj)
        {
            _sdoaqObj = sdoaqObj;
        }

        public void Update_Param()
        {
            if (_sdoaqObj == null)
            {
                return;
            }

            this.Invoke(() =>
            {
                txt_ROI.Text = _sdoaqObj.CamInfo.GetCamRoi();
                _sdoaqObj.GetRoi_AF(out int[] roi_AF);

                txt_AFROI.Text = string.Join(",", roi_AF);
                txt_FocusSet.Text = _sdoaqObj.FocusList.ToString();
                txt_SnapFocusSet.Text = _sdoaqObj.SnapFocusList.ToString();
                txt_RingBufferSize.Text = _sdoaqObj.PlayerRingBufferSize.ToString();

                if (_sdoaqObj.GetParam(SDOAQ_API.eParameterId.pi_edof_calc_resize_ratio, out bool isWritalbe, out string paramValue))
                {
                    cmb_EdofResizeRatio.SelectedItem = paramValue;
                }
            });
        }

        private void UpdateParamValue(ComboBox comboBox, TextBox textBox, Button btnSet)
        {
            textBox.Enabled = false;
            btnSet.Enabled = false;
            if (_sdoaqObj == null)
            {
                return;
            }

            if (Enum.TryParse((string)comboBox.SelectedItem, out SDOAQ_API.eParameterId paramID) == false)
            {
                return;
            }

            if (_sdoaqObj.GetParam(paramID, out bool isWriteable, out string paramValue))
            {
                textBox.Text = paramValue;
                textBox.Enabled = isWriteable;
                btnSet.Enabled = isWriteable;
            }
        }

        private void SetParam(ComboBox comboBox, TextBox textBox)
        {
            if (_sdoaqObj == null)
            {
                return;
            }

            if (comboBox.SelectedItem == null)
            {
                return;
            }

            if (Enum.TryParse((string)comboBox.SelectedItem, out SDOAQ_API.eParameterId paramID) == false)
            {
                return;
            }

            _sdoaqObj.SetParam(paramID, textBox.Text);
        }

        private void SdoaqParams_Resize(object sender, EventArgs e)
        {
            this.Size = INIT_SIZE;
        }

        private void cmb_Param_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateParamValue(cmb_Param, txt_ParamValue, btn_SetParam);
        }

        private void btn_SetParam_Click(object sender, EventArgs e)
        {
            SetParam(cmb_Param, txt_ParamValue);
            Update_Param();
        }

        private void btn_SetROI_Click(object sender, EventArgs e)
        {
            _sdoaqObj?.SetRoi(txt_ROI.Text);
            Update_Param();
        }

        private void btn_SetAFROI_Click(object sender, EventArgs e)
        {
            _sdoaqObj?.SetRoi_AF(txt_AFROI.Text);
            Update_Param();
        }

        private void btn_SetRingBufferSize_Click(object sender, EventArgs e)
        {
            string text = txt_RingBufferSize.Text;
            if (int.TryParse(text, out int ringBufferSize) == false)
            {
                MessageBox.Show($"Ring Buffer Size Param Int Parse False ({text})");
                return;
            }

            if (_sdoaqObj != null)
            {
                _sdoaqObj.PlayerRingBufferSize = ringBufferSize;
            }
        }

        private void btn_SetFocus_Click(object sender, EventArgs e)
        {
            _sdoaqObj?.SetFocus(txt_FocusSet.Text);
            Update_Param();
        }

        private void btn_SetSnapFocus_Click(object sender, EventArgs e)
        {
            _sdoaqObj?.SetSnapFocus(txt_SnapFocusSet.Text);
            Update_Param();
        }

        private void btn_EdofRatio_Click(object sender, EventArgs e)
        {
            _sdoaqObj?.SetParam(SDOAQ_API.eParameterId.pi_edof_calc_resize_ratio, cmb_EdofResizeRatio.SelectedItem.ToString());
            Update_Param();
        }

        private void btn_SetCalFile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();

            dlg.Filter = "calibration file (*.csv)|*.csv|.AllFiles(*.*)|*.*";
            dlg.InitialDirectory = Directory.GetCurrentDirectory();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _sdoaqObj?.SetCalibrationFile(dlg.FileName);
            }
        }
    }
}
