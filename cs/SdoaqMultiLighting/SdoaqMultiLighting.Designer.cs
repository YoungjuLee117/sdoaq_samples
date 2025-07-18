namespace SdoaqMultiLighting
{
    partial class SdoaqMultiLighting
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdoaqMultiLighting));
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.gr_Paramter = new System.Windows.Forms.GroupBox();
            this.cmb_Param_SelectLighting = new System.Windows.Forms.ComboBox();
            this.btn_Param_SetData = new System.Windows.Forms.Button();
            this.txt_Param_Gain = new System.Windows.Forms.TextBox();
            this.txt_Param_ExposureTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tmr_LogUpdate = new System.Windows.Forms.Timer(this.components);
            this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_View = new System.Windows.Forms.Panel();
            this.pnl_Viewer = new SDOAQNet.Component.SdoPanel();
            this.pnl_Controls = new System.Windows.Forms.Panel();
            this.pnl_Init = new SDOAQNet.Component.SdoPanel();
            this.btn_Init = new System.Windows.Forms.Button();
            this.btn_Final = new System.Windows.Forms.Button();
            this.gr_Acq = new System.Windows.Forms.GroupBox();
            this.cmb_Acq_SelectLighting = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_Acq = new System.Windows.Forms.Button();
            this.gr_Paramter.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            this.pnl_View.SuspendLayout();
            this.pnl_Controls.SuspendLayout();
            this.pnl_Init.SuspendLayout();
            this.gr_Acq.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Log
            // 
            this.txt_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Log.Location = new System.Drawing.Point(10, 357);
            this.txt_Log.Margin = new System.Windows.Forms.Padding(2);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.Size = new System.Drawing.Size(577, 401);
            this.txt_Log.TabIndex = 1;
            this.txt_Log.Text = "";
            this.txt_Log.WordWrap = false;
            // 
            // gr_Paramter
            // 
            this.gr_Paramter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr_Paramter.Controls.Add(this.cmb_Param_SelectLighting);
            this.gr_Paramter.Controls.Add(this.btn_Param_SetData);
            this.gr_Paramter.Controls.Add(this.txt_Param_Gain);
            this.gr_Paramter.Controls.Add(this.txt_Param_ExposureTime);
            this.gr_Paramter.Controls.Add(this.label3);
            this.gr_Paramter.Controls.Add(this.label2);
            this.gr_Paramter.Controls.Add(this.label1);
            this.gr_Paramter.Location = new System.Drawing.Point(10, 56);
            this.gr_Paramter.Name = "gr_Paramter";
            this.gr_Paramter.Size = new System.Drawing.Size(385, 185);
            this.gr_Paramter.TabIndex = 0;
            this.gr_Paramter.TabStop = false;
            this.gr_Paramter.Text = "Parameter";
            // 
            // cmb_Param_SelectLighting
            // 
            this.cmb_Param_SelectLighting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Param_SelectLighting.FormattingEnabled = true;
            this.cmb_Param_SelectLighting.Location = new System.Drawing.Point(250, 25);
            this.cmb_Param_SelectLighting.Name = "cmb_Param_SelectLighting";
            this.cmb_Param_SelectLighting.Size = new System.Drawing.Size(123, 23);
            this.cmb_Param_SelectLighting.TabIndex = 4;
            this.cmb_Param_SelectLighting.SelectedIndexChanged += new System.EventHandler(this.cmb_Param_SelectLighting_SelectedIndexChanged);
            // 
            // btn_Param_SetData
            // 
            this.btn_Param_SetData.Location = new System.Drawing.Point(10, 131);
            this.btn_Param_SetData.Name = "btn_Param_SetData";
            this.btn_Param_SetData.Size = new System.Drawing.Size(363, 36);
            this.btn_Param_SetData.TabIndex = 16;
            this.btn_Param_SetData.Text = "Set data";
            this.btn_Param_SetData.UseVisualStyleBackColor = true;
            this.btn_Param_SetData.Click += new System.EventHandler(this.btn_Param_SetData_Click);
            // 
            // txt_Param_Gain
            // 
            this.txt_Param_Gain.Location = new System.Drawing.Point(250, 98);
            this.txt_Param_Gain.Name = "txt_Param_Gain";
            this.txt_Param_Gain.Size = new System.Drawing.Size(123, 23);
            this.txt_Param_Gain.TabIndex = 8;
            this.txt_Param_Gain.Text = "1";
            // 
            // txt_Param_ExposureTime
            // 
            this.txt_Param_ExposureTime.Location = new System.Drawing.Point(250, 61);
            this.txt_Param_ExposureTime.Name = "txt_Param_ExposureTime";
            this.txt_Param_ExposureTime.Size = new System.Drawing.Size(123, 23);
            this.txt_Param_ExposureTime.TabIndex = 7;
            this.txt_Param_ExposureTime.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Gain";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Exposure time (us)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select lighting to set data";
            // 
            // tmr_LogUpdate
            // 
            this.tmr_LogUpdate.Tick += new System.EventHandler(this.tmr_LogUpdate_Tick);
            // 
            // tlp_Main
            // 
            this.tlp_Main.ColumnCount = 2;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 599F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Main.Controls.Add(this.pnl_View, 1, 0);
            this.tlp_Main.Controls.Add(this.pnl_Controls, 0, 0);
            this.tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Main.Location = new System.Drawing.Point(0, 0);
            this.tlp_Main.Name = "tlp_Main";
            this.tlp_Main.RowCount = 1;
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Main.Size = new System.Drawing.Size(1229, 772);
            this.tlp_Main.TabIndex = 1;
            // 
            // pnl_View
            // 
            this.pnl_View.Controls.Add(this.pnl_Viewer);
            this.pnl_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_View.Location = new System.Drawing.Point(602, 3);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(624, 766);
            this.pnl_View.TabIndex = 1;
            // 
            // pnl_Viewer
            // 
            this.pnl_Viewer.BorderColor = System.Drawing.Color.Black;
            this.pnl_Viewer.BorderWidth = 1;
            this.pnl_Viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Viewer.Location = new System.Drawing.Point(0, 0);
            this.pnl_Viewer.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_Viewer.Name = "pnl_Viewer";
            this.pnl_Viewer.Size = new System.Drawing.Size(624, 766);
            this.pnl_Viewer.TabIndex = 2;
            // 
            // pnl_Controls
            // 
            this.pnl_Controls.Controls.Add(this.pnl_Init);
            this.pnl_Controls.Controls.Add(this.gr_Acq);
            this.pnl_Controls.Controls.Add(this.gr_Paramter);
            this.pnl_Controls.Controls.Add(this.txt_Log);
            this.pnl_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Controls.Location = new System.Drawing.Point(3, 3);
            this.pnl_Controls.Name = "pnl_Controls";
            this.pnl_Controls.Size = new System.Drawing.Size(593, 766);
            this.pnl_Controls.TabIndex = 0;
            // 
            // pnl_Init
            // 
            this.pnl_Init.BorderColor = System.Drawing.Color.Black;
            this.pnl_Init.BorderWidth = 1;
            this.pnl_Init.Controls.Add(this.btn_Init);
            this.pnl_Init.Controls.Add(this.btn_Final);
            this.pnl_Init.Location = new System.Drawing.Point(10, 10);
            this.pnl_Init.Name = "pnl_Init";
            this.pnl_Init.Size = new System.Drawing.Size(385, 40);
            this.pnl_Init.TabIndex = 2;
            // 
            // btn_Init
            // 
            this.btn_Init.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Init.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Init.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.btn_Init.Location = new System.Drawing.Point(10, 5);
            this.btn_Init.Name = "btn_Init";
            this.btn_Init.Size = new System.Drawing.Size(174, 29);
            this.btn_Init.TabIndex = 1;
            this.btn_Init.Text = "Initialize";
            this.btn_Init.UseVisualStyleBackColor = true;
            this.btn_Init.Click += new System.EventHandler(this.btn_Init_Click);
            // 
            // btn_Final
            // 
            this.btn_Final.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Final.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Final.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.btn_Final.Location = new System.Drawing.Point(199, 5);
            this.btn_Final.Name = "btn_Final";
            this.btn_Final.Size = new System.Drawing.Size(174, 29);
            this.btn_Final.TabIndex = 1;
            this.btn_Final.Text = "Finalize";
            this.btn_Final.UseVisualStyleBackColor = true;
            this.btn_Final.Click += new System.EventHandler(this.btn_Final_Click);
            // 
            // gr_Acq
            // 
            this.gr_Acq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr_Acq.Controls.Add(this.cmb_Acq_SelectLighting);
            this.gr_Acq.Controls.Add(this.label6);
            this.gr_Acq.Controls.Add(this.btn_Acq);
            this.gr_Acq.Location = new System.Drawing.Point(10, 247);
            this.gr_Acq.Name = "gr_Acq";
            this.gr_Acq.Size = new System.Drawing.Size(385, 105);
            this.gr_Acq.TabIndex = 0;
            this.gr_Acq.TabStop = false;
            this.gr_Acq.Text = "Acquisition";
            // 
            // cmb_Acq_SelectLighting
            // 
            this.cmb_Acq_SelectLighting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Acq_SelectLighting.FormattingEnabled = true;
            this.cmb_Acq_SelectLighting.Location = new System.Drawing.Point(250, 31);
            this.cmb_Acq_SelectLighting.Name = "cmb_Acq_SelectLighting";
            this.cmb_Acq_SelectLighting.Size = new System.Drawing.Size(121, 23);
            this.cmb_Acq_SelectLighting.TabIndex = 4;
            this.cmb_Acq_SelectLighting.SelectedIndexChanged += new System.EventHandler(this.cmb_Acq_SelectLighting_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "Select lighting to activate";
            // 
            // btn_Acq
            // 
            this.btn_Acq.Location = new System.Drawing.Point(10, 63);
            this.btn_Acq.Name = "btn_Acq";
            this.btn_Acq.Size = new System.Drawing.Size(363, 36);
            this.btn_Acq.TabIndex = 16;
            this.btn_Acq.Text = "Acquire a single-focus image";
            this.btn_Acq.UseVisualStyleBackColor = true;
            this.btn_Acq.Click += new System.EventHandler(this.btn_Acq_Click);
            // 
            // SdoaqMultiLighting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 772);
            this.Controls.Add(this.tlp_Main);
            this.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SdoaqMultiLighting";
            this.Text = "SDOAQ Multiple Lighting Sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SdoaqAutoFocus_FormClosed);
            this.Load += new System.EventHandler(this.SdoaqAutoFocus_Load);
            this.gr_Paramter.ResumeLayout(false);
            this.gr_Paramter.PerformLayout();
            this.tlp_Main.ResumeLayout(false);
            this.pnl_View.ResumeLayout(false);
            this.pnl_Controls.ResumeLayout(false);
            this.pnl_Init.ResumeLayout(false);
            this.gr_Acq.ResumeLayout(false);
            this.gr_Acq.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gr_Paramter;
        private System.Windows.Forms.RichTextBox txt_Log;
        private System.Windows.Forms.Button btn_Param_SetData;
        private System.Windows.Forms.TextBox txt_Param_Gain;
        private System.Windows.Forms.TextBox txt_Param_ExposureTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmr_LogUpdate;
        private SDOAQNet.Component.SdoPanel pnl_Viewer;
        private System.Windows.Forms.TableLayoutPanel tlp_Main;
        private System.Windows.Forms.Panel pnl_View;
        private System.Windows.Forms.Panel pnl_Controls;
        private System.Windows.Forms.GroupBox gr_Acq;
        private System.Windows.Forms.ComboBox cmb_Acq_SelectLighting;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_Acq;
        private System.Windows.Forms.ComboBox cmb_Param_SelectLighting;
        private SDOAQNet.Component.SdoPanel pnl_Init;
        private System.Windows.Forms.Button btn_Init;
        private System.Windows.Forms.Button btn_Final;
    }
}

