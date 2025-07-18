namespace SDOAQNet.Component
{
    partial class SdoaqParams
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.gr_Params = new System.Windows.Forms.GroupBox();
            this.txt_ParamValue = new System.Windows.Forms.TextBox();
            this.cmb_Param = new System.Windows.Forms.ComboBox();
            this.btn_SetParam = new System.Windows.Forms.Button();
            this.gr_NoneLiveCompatibleParams = new System.Windows.Forms.GroupBox();
            this.cmb_EdofResizeRatio = new System.Windows.Forms.ComboBox();
            this.lbl_AFROI = new System.Windows.Forms.Label();
            this.txt_AFROI = new System.Windows.Forms.TextBox();
            this.btn_SetAFROI = new System.Windows.Forms.Button();
            this.lbl_SnapFocusSet = new System.Windows.Forms.Label();
            this.txt_SnapFocusSet = new System.Windows.Forms.TextBox();
            this.btn_SetSnapFocus = new System.Windows.Forms.Button();
            this.lbl_EdofResizeRatio = new System.Windows.Forms.Label();
            this.lbl_FocusSet = new System.Windows.Forms.Label();
            this.btn_EdofRatio = new System.Windows.Forms.Button();
            this.txt_FocusSet = new System.Windows.Forms.TextBox();
            this.btn_SetFocus = new System.Windows.Forms.Button();
            this.lbl_RingBufferSize = new System.Windows.Forms.Label();
            this.txt_RingBufferSize = new System.Windows.Forms.TextBox();
            this.btn_SetRingBufferSize = new System.Windows.Forms.Button();
            this.lbl_ROI = new System.Windows.Forms.Label();
            this.txt_ROI = new System.Windows.Forms.TextBox();
            this.btn_SetROI = new System.Windows.Forms.Button();
            this.btn_SetCalFile = new System.Windows.Forms.Button();
            this.pnl_CalFile = new SDOAQNet.Component.SdoPanel();
            this.gr_Params.SuspendLayout();
            this.gr_NoneLiveCompatibleParams.SuspendLayout();
            this.pnl_CalFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // gr_Params
            // 
            this.gr_Params.Controls.Add(this.txt_ParamValue);
            this.gr_Params.Controls.Add(this.cmb_Param);
            this.gr_Params.Controls.Add(this.btn_SetParam);
            this.gr_Params.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gr_Params.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gr_Params.Location = new System.Drawing.Point(5, 5);
            this.gr_Params.Name = "gr_Params";
            this.gr_Params.Size = new System.Drawing.Size(492, 65);
            this.gr_Params.TabIndex = 3;
            this.gr_Params.TabStop = false;
            this.gr_Params.Text = "Parameters";
            // 
            // txt_ParamValue
            // 
            this.txt_ParamValue.Location = new System.Drawing.Point(353, 22);
            this.txt_ParamValue.Name = "txt_ParamValue";
            this.txt_ParamValue.Size = new System.Drawing.Size(71, 23);
            this.txt_ParamValue.TabIndex = 4;
            // 
            // cmb_Param
            // 
            this.cmb_Param.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Param.FormattingEnabled = true;
            this.cmb_Param.Location = new System.Drawing.Point(7, 22);
            this.cmb_Param.Name = "cmb_Param";
            this.cmb_Param.Size = new System.Drawing.Size(340, 23);
            this.cmb_Param.TabIndex = 3;
            this.cmb_Param.SelectedIndexChanged += new System.EventHandler(this.cmb_Param_SelectedIndexChanged);
            // 
            // btn_SetParam
            // 
            this.btn_SetParam.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetParam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetParam.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_SetParam.Location = new System.Drawing.Point(430, 21);
            this.btn_SetParam.Name = "btn_SetParam";
            this.btn_SetParam.Size = new System.Drawing.Size(56, 23);
            this.btn_SetParam.TabIndex = 1;
            this.btn_SetParam.Text = "Set";
            this.btn_SetParam.UseVisualStyleBackColor = true;
            this.btn_SetParam.Click += new System.EventHandler(this.btn_SetParam_Click);
            // 
            // gr_NoneLiveCompatibleParams
            // 
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.cmb_EdofResizeRatio);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.lbl_AFROI);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.txt_AFROI);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.btn_SetAFROI);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.lbl_SnapFocusSet);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.txt_SnapFocusSet);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.btn_SetSnapFocus);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.lbl_EdofResizeRatio);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.lbl_FocusSet);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.btn_EdofRatio);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.txt_FocusSet);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.btn_SetFocus);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.lbl_RingBufferSize);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.txt_RingBufferSize);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.btn_SetRingBufferSize);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.lbl_ROI);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.txt_ROI);
            this.gr_NoneLiveCompatibleParams.Controls.Add(this.btn_SetROI);
            this.gr_NoneLiveCompatibleParams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gr_NoneLiveCompatibleParams.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gr_NoneLiveCompatibleParams.Location = new System.Drawing.Point(5, 76);
            this.gr_NoneLiveCompatibleParams.Name = "gr_NoneLiveCompatibleParams";
            this.gr_NoneLiveCompatibleParams.Size = new System.Drawing.Size(492, 200);
            this.gr_NoneLiveCompatibleParams.TabIndex = 4;
            this.gr_NoneLiveCompatibleParams.TabStop = false;
            this.gr_NoneLiveCompatibleParams.Text = "Non-live-compatible parameters";
            // 
            // cmb_EdofResizeRatio
            // 
            this.cmb_EdofResizeRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_EdofResizeRatio.FormattingEnabled = true;
            this.cmb_EdofResizeRatio.Location = new System.Drawing.Point(344, 171);
            this.cmb_EdofResizeRatio.Name = "cmb_EdofResizeRatio";
            this.cmb_EdofResizeRatio.Size = new System.Drawing.Size(80, 23);
            this.cmb_EdofResizeRatio.TabIndex = 20;
            // 
            // lbl_AFROI
            // 
            this.lbl_AFROI.Location = new System.Drawing.Point(11, 54);
            this.lbl_AFROI.Name = "lbl_AFROI";
            this.lbl_AFROI.Size = new System.Drawing.Size(242, 23);
            this.lbl_AFROI.TabIndex = 19;
            this.lbl_AFROI.Text = "AF ROI (Left, Top, Width, Height)";
            this.lbl_AFROI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_AFROI
            // 
            this.txt_AFROI.Location = new System.Drawing.Point(259, 54);
            this.txt_AFROI.Name = "txt_AFROI";
            this.txt_AFROI.Size = new System.Drawing.Size(165, 23);
            this.txt_AFROI.TabIndex = 18;
            this.txt_AFROI.Text = "956,479,128,128";
            // 
            // btn_SetAFROI
            // 
            this.btn_SetAFROI.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetAFROI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetAFROI.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_SetAFROI.Location = new System.Drawing.Point(430, 54);
            this.btn_SetAFROI.Name = "btn_SetAFROI";
            this.btn_SetAFROI.Size = new System.Drawing.Size(56, 23);
            this.btn_SetAFROI.TabIndex = 17;
            this.btn_SetAFROI.Text = "Set";
            this.btn_SetAFROI.UseVisualStyleBackColor = true;
            this.btn_SetAFROI.Click += new System.EventHandler(this.btn_SetAFROI_Click);
            // 
            // lbl_SnapFocusSet
            // 
            this.lbl_SnapFocusSet.Location = new System.Drawing.Point(11, 141);
            this.lbl_SnapFocusSet.Name = "lbl_SnapFocusSet";
            this.lbl_SnapFocusSet.Size = new System.Drawing.Size(327, 23);
            this.lbl_SnapFocusSet.TabIndex = 14;
            this.lbl_SnapFocusSet.Text = "Snap Focus set (Low-High-Unit)";
            this.lbl_SnapFocusSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_SnapFocusSet
            // 
            this.txt_SnapFocusSet.Location = new System.Drawing.Point(344, 142);
            this.txt_SnapFocusSet.Name = "txt_SnapFocusSet";
            this.txt_SnapFocusSet.Size = new System.Drawing.Size(80, 23);
            this.txt_SnapFocusSet.TabIndex = 13;
            this.txt_SnapFocusSet.Text = "0-319-18";
            // 
            // btn_SetSnapFocus
            // 
            this.btn_SetSnapFocus.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetSnapFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetSnapFocus.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_SetSnapFocus.Location = new System.Drawing.Point(430, 141);
            this.btn_SetSnapFocus.Name = "btn_SetSnapFocus";
            this.btn_SetSnapFocus.Size = new System.Drawing.Size(56, 23);
            this.btn_SetSnapFocus.TabIndex = 12;
            this.btn_SetSnapFocus.Text = "Set";
            this.btn_SetSnapFocus.UseVisualStyleBackColor = true;
            this.btn_SetSnapFocus.Click += new System.EventHandler(this.btn_SetSnapFocus_Click);
            // 
            // lbl_EdofResizeRatio
            // 
            this.lbl_EdofResizeRatio.Location = new System.Drawing.Point(11, 170);
            this.lbl_EdofResizeRatio.Name = "lbl_EdofResizeRatio";
            this.lbl_EdofResizeRatio.Size = new System.Drawing.Size(327, 23);
            this.lbl_EdofResizeRatio.TabIndex = 11;
            this.lbl_EdofResizeRatio.Text = "EDoF resize ratio (0.25, 0.5, 1.0)";
            this.lbl_EdofResizeRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_FocusSet
            // 
            this.lbl_FocusSet.Location = new System.Drawing.Point(11, 112);
            this.lbl_FocusSet.Name = "lbl_FocusSet";
            this.lbl_FocusSet.Size = new System.Drawing.Size(327, 23);
            this.lbl_FocusSet.TabIndex = 11;
            this.lbl_FocusSet.Text = "Live Focus set (Low-High-Unit)";
            this.lbl_FocusSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_EdofRatio
            // 
            this.btn_EdofRatio.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_EdofRatio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_EdofRatio.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_EdofRatio.Location = new System.Drawing.Point(430, 170);
            this.btn_EdofRatio.Name = "btn_EdofRatio";
            this.btn_EdofRatio.Size = new System.Drawing.Size(56, 23);
            this.btn_EdofRatio.TabIndex = 9;
            this.btn_EdofRatio.Text = "Set";
            this.btn_EdofRatio.UseVisualStyleBackColor = true;
            this.btn_EdofRatio.Click += new System.EventHandler(this.btn_EdofRatio_Click);
            // 
            // txt_FocusSet
            // 
            this.txt_FocusSet.Location = new System.Drawing.Point(344, 112);
            this.txt_FocusSet.Name = "txt_FocusSet";
            this.txt_FocusSet.Size = new System.Drawing.Size(80, 23);
            this.txt_FocusSet.TabIndex = 10;
            this.txt_FocusSet.Text = "0-319-35";
            // 
            // btn_SetFocus
            // 
            this.btn_SetFocus.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetFocus.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_SetFocus.Location = new System.Drawing.Point(430, 112);
            this.btn_SetFocus.Name = "btn_SetFocus";
            this.btn_SetFocus.Size = new System.Drawing.Size(56, 23);
            this.btn_SetFocus.TabIndex = 9;
            this.btn_SetFocus.Text = "Set";
            this.btn_SetFocus.UseVisualStyleBackColor = true;
            this.btn_SetFocus.Click += new System.EventHandler(this.btn_SetFocus_Click);
            // 
            // lbl_RingBufferSize
            // 
            this.lbl_RingBufferSize.Location = new System.Drawing.Point(11, 83);
            this.lbl_RingBufferSize.Name = "lbl_RingBufferSize";
            this.lbl_RingBufferSize.Size = new System.Drawing.Size(166, 23);
            this.lbl_RingBufferSize.TabIndex = 8;
            this.lbl_RingBufferSize.Text = "Ring buffer size";
            this.lbl_RingBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_RingBufferSize
            // 
            this.txt_RingBufferSize.Location = new System.Drawing.Point(344, 83);
            this.txt_RingBufferSize.Name = "txt_RingBufferSize";
            this.txt_RingBufferSize.Size = new System.Drawing.Size(80, 23);
            this.txt_RingBufferSize.TabIndex = 7;
            this.txt_RingBufferSize.Text = "3";
            // 
            // btn_SetRingBufferSize
            // 
            this.btn_SetRingBufferSize.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetRingBufferSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetRingBufferSize.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_SetRingBufferSize.Location = new System.Drawing.Point(430, 83);
            this.btn_SetRingBufferSize.Name = "btn_SetRingBufferSize";
            this.btn_SetRingBufferSize.Size = new System.Drawing.Size(56, 23);
            this.btn_SetRingBufferSize.TabIndex = 6;
            this.btn_SetRingBufferSize.Text = "Set";
            this.btn_SetRingBufferSize.UseVisualStyleBackColor = true;
            this.btn_SetRingBufferSize.Click += new System.EventHandler(this.btn_SetRingBufferSize_Click);
            // 
            // lbl_ROI
            // 
            this.lbl_ROI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_ROI.Location = new System.Drawing.Point(11, 25);
            this.lbl_ROI.Name = "lbl_ROI";
            this.lbl_ROI.Size = new System.Drawing.Size(242, 23);
            this.lbl_ROI.TabIndex = 5;
            this.lbl_ROI.Text = "ROI (Left, Top, Width, Height)";
            this.lbl_ROI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_ROI
            // 
            this.txt_ROI.Location = new System.Drawing.Point(259, 22);
            this.txt_ROI.Name = "txt_ROI";
            this.txt_ROI.Size = new System.Drawing.Size(165, 23);
            this.txt_ROI.TabIndex = 4;
            this.txt_ROI.Text = "0,0,2040,1086";
            // 
            // btn_SetROI
            // 
            this.btn_SetROI.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetROI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetROI.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_SetROI.Location = new System.Drawing.Point(430, 22);
            this.btn_SetROI.Name = "btn_SetROI";
            this.btn_SetROI.Size = new System.Drawing.Size(56, 23);
            this.btn_SetROI.TabIndex = 1;
            this.btn_SetROI.Text = "Set";
            this.btn_SetROI.UseVisualStyleBackColor = true;
            this.btn_SetROI.Click += new System.EventHandler(this.btn_SetROI_Click);
            // 
            // btn_SetCalFile
            // 
            this.btn_SetCalFile.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_SetCalFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SetCalFile.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SetCalFile.Location = new System.Drawing.Point(14, 10);
            this.btn_SetCalFile.Name = "btn_SetCalFile";
            this.btn_SetCalFile.Size = new System.Drawing.Size(472, 50);
            this.btn_SetCalFile.TabIndex = 12;
            this.btn_SetCalFile.Text = "Set calibration file";
            this.btn_SetCalFile.UseVisualStyleBackColor = true;
            this.btn_SetCalFile.Click += new System.EventHandler(this.btn_SetCalFile_Click);
            // 
            // pnl_CalFile
            // 
            this.pnl_CalFile.BorderColor = System.Drawing.Color.Black;
            this.pnl_CalFile.BorderWidth = 1;
            this.pnl_CalFile.Controls.Add(this.btn_SetCalFile);
            this.pnl_CalFile.Location = new System.Drawing.Point(5, 280);
            this.pnl_CalFile.Name = "pnl_CalFile";
            this.pnl_CalFile.Size = new System.Drawing.Size(492, 70);
            this.pnl_CalFile.TabIndex = 6;
            // 
            // SdoaqParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_CalFile);
            this.Controls.Add(this.gr_NoneLiveCompatibleParams);
            this.Controls.Add(this.gr_Params);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SdoaqParams";
            this.Size = new System.Drawing.Size(500, 355);
            this.Resize += new System.EventHandler(this.SdoaqParams_Resize);
            this.gr_Params.ResumeLayout(false);
            this.gr_Params.PerformLayout();
            this.gr_NoneLiveCompatibleParams.ResumeLayout(false);
            this.gr_NoneLiveCompatibleParams.PerformLayout();
            this.pnl_CalFile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gr_Params;
        private System.Windows.Forms.TextBox txt_ParamValue;
        private System.Windows.Forms.ComboBox cmb_Param;
        private System.Windows.Forms.Button btn_SetParam;
        private System.Windows.Forms.GroupBox gr_NoneLiveCompatibleParams;
        private System.Windows.Forms.Label lbl_AFROI;
        private System.Windows.Forms.TextBox txt_AFROI;
        private System.Windows.Forms.Button btn_SetAFROI;
        private System.Windows.Forms.Label lbl_SnapFocusSet;
        private System.Windows.Forms.TextBox txt_SnapFocusSet;
        private System.Windows.Forms.Button btn_SetSnapFocus;
        private System.Windows.Forms.Label lbl_EdofResizeRatio;
        private System.Windows.Forms.Label lbl_FocusSet;
        private System.Windows.Forms.Button btn_EdofRatio;
        private System.Windows.Forms.TextBox txt_FocusSet;
        private System.Windows.Forms.Button btn_SetFocus;
        private System.Windows.Forms.Label lbl_RingBufferSize;
        private System.Windows.Forms.TextBox txt_RingBufferSize;
        private System.Windows.Forms.Button btn_SetRingBufferSize;
        private System.Windows.Forms.Label lbl_ROI;
        private System.Windows.Forms.TextBox txt_ROI;
        private System.Windows.Forms.Button btn_SetROI;
        private System.Windows.Forms.ComboBox cmb_EdofResizeRatio;
        private System.Windows.Forms.Button btn_SetCalFile;
        private SDOAQNet.Component.SdoPanel pnl_CalFile;
    }
}
