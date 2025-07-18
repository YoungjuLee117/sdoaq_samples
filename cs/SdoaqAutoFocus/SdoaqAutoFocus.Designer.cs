namespace SdoaqAutoFocus
{
    partial class SdoaqAutoFocus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdoaqAutoFocus));
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.gpb_Controls = new System.Windows.Forms.GroupBox();
            this.btn_StopAF = new System.Windows.Forms.Button();
            this.btn_PlayAF = new System.Windows.Forms.Button();
            this.btn_SingleShotAF = new System.Windows.Forms.Button();
            this.btn_DebounceCount = new System.Windows.Forms.Button();
            this.btn_StabilityMethod = new System.Windows.Forms.Button();
            this.btn_ResamplingMethod = new System.Windows.Forms.Button();
            this.btn_SharpnessMethod = new System.Windows.Forms.Button();
            this.btn_SetROI = new System.Windows.Forms.Button();
            this.txt_DebounceCount = new System.Windows.Forms.TextBox();
            this.txt_StabilityMethod = new System.Windows.Forms.TextBox();
            this.txt_ResamplingMethod = new System.Windows.Forms.TextBox();
            this.txt_SharpnessMethod = new System.Windows.Forms.TextBox();
            this.txt_ROI = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tmr_LogUpdate = new System.Windows.Forms.Timer(this.components);
            this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_View = new System.Windows.Forms.Panel();
            this.pnl_Viewer = new SDOAQNet.Component.SdoPanel();
            this.pnl_Controls = new System.Windows.Forms.Panel();
            this.gpb_Controls.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            this.pnl_View.SuspendLayout();
            this.pnl_Controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Log
            // 
            this.txt_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Log.Location = new System.Drawing.Point(13, 278);
            this.txt_Log.Margin = new System.Windows.Forms.Padding(2);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.Size = new System.Drawing.Size(545, 226);
            this.txt_Log.TabIndex = 1;
            this.txt_Log.Text = "";
            // 
            // gpb_Controls
            // 
            this.gpb_Controls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpb_Controls.Controls.Add(this.btn_StopAF);
            this.gpb_Controls.Controls.Add(this.btn_PlayAF);
            this.gpb_Controls.Controls.Add(this.btn_SingleShotAF);
            this.gpb_Controls.Controls.Add(this.btn_DebounceCount);
            this.gpb_Controls.Controls.Add(this.btn_StabilityMethod);
            this.gpb_Controls.Controls.Add(this.btn_ResamplingMethod);
            this.gpb_Controls.Controls.Add(this.btn_SharpnessMethod);
            this.gpb_Controls.Controls.Add(this.btn_SetROI);
            this.gpb_Controls.Controls.Add(this.txt_DebounceCount);
            this.gpb_Controls.Controls.Add(this.txt_StabilityMethod);
            this.gpb_Controls.Controls.Add(this.txt_ResamplingMethod);
            this.gpb_Controls.Controls.Add(this.txt_SharpnessMethod);
            this.gpb_Controls.Controls.Add(this.txt_ROI);
            this.gpb_Controls.Controls.Add(this.label5);
            this.gpb_Controls.Controls.Add(this.label4);
            this.gpb_Controls.Controls.Add(this.label3);
            this.gpb_Controls.Controls.Add(this.label2);
            this.gpb_Controls.Controls.Add(this.label1);
            this.gpb_Controls.Location = new System.Drawing.Point(13, 8);
            this.gpb_Controls.Name = "gpb_Controls";
            this.gpb_Controls.Size = new System.Drawing.Size(545, 255);
            this.gpb_Controls.TabIndex = 0;
            this.gpb_Controls.TabStop = false;
            this.gpb_Controls.Text = "Controls";
            // 
            // btn_StopAF
            // 
            this.btn_StopAF.Location = new System.Drawing.Point(392, 209);
            this.btn_StopAF.Name = "btn_StopAF";
            this.btn_StopAF.Size = new System.Drawing.Size(145, 36);
            this.btn_StopAF.TabIndex = 18;
            this.btn_StopAF.Text = "Stop AF";
            this.btn_StopAF.UseVisualStyleBackColor = true;
            this.btn_StopAF.Click += new System.EventHandler(this.btn_StopAF_Click);
            // 
            // btn_PlayAF
            // 
            this.btn_PlayAF.Location = new System.Drawing.Point(201, 209);
            this.btn_PlayAF.Name = "btn_PlayAF";
            this.btn_PlayAF.Size = new System.Drawing.Size(145, 36);
            this.btn_PlayAF.TabIndex = 17;
            this.btn_PlayAF.Text = "Play AF";
            this.btn_PlayAF.UseVisualStyleBackColor = true;
            this.btn_PlayAF.Click += new System.EventHandler(this.btn_PlayAF_Click);
            // 
            // btn_SingleShotAF
            // 
            this.btn_SingleShotAF.Location = new System.Drawing.Point(10, 209);
            this.btn_SingleShotAF.Name = "btn_SingleShotAF";
            this.btn_SingleShotAF.Size = new System.Drawing.Size(145, 36);
            this.btn_SingleShotAF.TabIndex = 16;
            this.btn_SingleShotAF.Text = "SingleShot AF";
            this.btn_SingleShotAF.UseVisualStyleBackColor = true;
            this.btn_SingleShotAF.Click += new System.EventHandler(this.btn_SingleShotAF_Click);
            // 
            // btn_DebounceCount
            // 
            this.btn_DebounceCount.Location = new System.Drawing.Point(486, 172);
            this.btn_DebounceCount.Name = "btn_DebounceCount";
            this.btn_DebounceCount.Size = new System.Drawing.Size(52, 25);
            this.btn_DebounceCount.TabIndex = 15;
            this.btn_DebounceCount.Text = "Set";
            this.btn_DebounceCount.UseVisualStyleBackColor = true;
            this.btn_DebounceCount.Click += new System.EventHandler(this.btn_DebounceCount_Click);
            // 
            // btn_StabilityMethod
            // 
            this.btn_StabilityMethod.Location = new System.Drawing.Point(486, 135);
            this.btn_StabilityMethod.Name = "btn_StabilityMethod";
            this.btn_StabilityMethod.Size = new System.Drawing.Size(52, 25);
            this.btn_StabilityMethod.TabIndex = 14;
            this.btn_StabilityMethod.Text = "Set";
            this.btn_StabilityMethod.UseVisualStyleBackColor = true;
            this.btn_StabilityMethod.Click += new System.EventHandler(this.btn_StabilityMethod_Click);
            // 
            // btn_ResamplingMethod
            // 
            this.btn_ResamplingMethod.Location = new System.Drawing.Point(486, 98);
            this.btn_ResamplingMethod.Name = "btn_ResamplingMethod";
            this.btn_ResamplingMethod.Size = new System.Drawing.Size(52, 25);
            this.btn_ResamplingMethod.TabIndex = 13;
            this.btn_ResamplingMethod.Text = "Set";
            this.btn_ResamplingMethod.UseVisualStyleBackColor = true;
            this.btn_ResamplingMethod.Click += new System.EventHandler(this.btn_ResamplingMethod_Click);
            // 
            // btn_SharpnessMethod
            // 
            this.btn_SharpnessMethod.Location = new System.Drawing.Point(486, 61);
            this.btn_SharpnessMethod.Name = "btn_SharpnessMethod";
            this.btn_SharpnessMethod.Size = new System.Drawing.Size(52, 25);
            this.btn_SharpnessMethod.TabIndex = 12;
            this.btn_SharpnessMethod.Text = "Set";
            this.btn_SharpnessMethod.UseVisualStyleBackColor = true;
            this.btn_SharpnessMethod.Click += new System.EventHandler(this.btn_SharpnessMethod_Click);
            // 
            // btn_SetROI
            // 
            this.btn_SetROI.Location = new System.Drawing.Point(486, 24);
            this.btn_SetROI.Name = "btn_SetROI";
            this.btn_SetROI.Size = new System.Drawing.Size(52, 25);
            this.btn_SetROI.TabIndex = 11;
            this.btn_SetROI.Text = "Set";
            this.btn_SetROI.UseVisualStyleBackColor = true;
            this.btn_SetROI.Click += new System.EventHandler(this.btn_SetROI_Click);
            // 
            // txt_DebounceCount
            // 
            this.txt_DebounceCount.Location = new System.Drawing.Point(250, 172);
            this.txt_DebounceCount.Name = "txt_DebounceCount";
            this.txt_DebounceCount.Size = new System.Drawing.Size(230, 23);
            this.txt_DebounceCount.TabIndex = 10;
            this.txt_DebounceCount.Text = "4";
            // 
            // txt_StabilityMethod
            // 
            this.txt_StabilityMethod.Location = new System.Drawing.Point(250, 135);
            this.txt_StabilityMethod.Name = "txt_StabilityMethod";
            this.txt_StabilityMethod.Size = new System.Drawing.Size(230, 23);
            this.txt_StabilityMethod.TabIndex = 9;
            this.txt_StabilityMethod.Text = "1";
            // 
            // txt_ResamplingMethod
            // 
            this.txt_ResamplingMethod.Location = new System.Drawing.Point(250, 98);
            this.txt_ResamplingMethod.Name = "txt_ResamplingMethod";
            this.txt_ResamplingMethod.Size = new System.Drawing.Size(230, 23);
            this.txt_ResamplingMethod.TabIndex = 8;
            this.txt_ResamplingMethod.Text = "1";
            // 
            // txt_SharpnessMethod
            // 
            this.txt_SharpnessMethod.Location = new System.Drawing.Point(250, 61);
            this.txt_SharpnessMethod.Name = "txt_SharpnessMethod";
            this.txt_SharpnessMethod.Size = new System.Drawing.Size(230, 23);
            this.txt_SharpnessMethod.TabIndex = 7;
            this.txt_SharpnessMethod.Text = "0";
            // 
            // txt_ROI
            // 
            this.txt_ROI.Location = new System.Drawing.Point(250, 24);
            this.txt_ROI.Name = "txt_ROI";
            this.txt_ROI.Size = new System.Drawing.Size(230, 23);
            this.txt_ROI.TabIndex = 6;
            this.txt_ROI.Text = "1020,543,128,128";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Stability Debounce Count(0~10)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Stability Method(1~4)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Resampling Method (0~2)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sharpness Measure Method(0~2)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "AF ROI (left,top,width,height)";
            // 
            // tmr_LogUpdate
            // 
            this.tmr_LogUpdate.Tick += new System.EventHandler(this.tmr_LogUpdate_Tick);
            // 
            // tlp_Main
            // 
            this.tlp_Main.ColumnCount = 2;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 580F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Main.Controls.Add(this.pnl_View, 1, 0);
            this.tlp_Main.Controls.Add(this.pnl_Controls, 0, 0);
            this.tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Main.Location = new System.Drawing.Point(0, 0);
            this.tlp_Main.Name = "tlp_Main";
            this.tlp_Main.RowCount = 1;
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Main.Size = new System.Drawing.Size(1234, 521);
            this.tlp_Main.TabIndex = 1;
            // 
            // pnl_View
            // 
            this.pnl_View.Controls.Add(this.pnl_Viewer);
            this.pnl_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_View.Location = new System.Drawing.Point(583, 3);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(648, 515);
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
            this.pnl_Viewer.Size = new System.Drawing.Size(648, 515);
            this.pnl_Viewer.TabIndex = 2;
            // 
            // pnl_Controls
            // 
            this.pnl_Controls.Controls.Add(this.gpb_Controls);
            this.pnl_Controls.Controls.Add(this.txt_Log);
            this.pnl_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Controls.Location = new System.Drawing.Point(3, 3);
            this.pnl_Controls.Name = "pnl_Controls";
            this.pnl_Controls.Size = new System.Drawing.Size(574, 515);
            this.pnl_Controls.TabIndex = 0;
            // 
            // SdoaqAutoFocus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 521);
            this.Controls.Add(this.tlp_Main);
            this.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SdoaqAutoFocus";
            this.Text = "SDOAQ Auto-focus Sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SdoaqAutoFocus_FormClosed);
            this.Load += new System.EventHandler(this.SdoaqAutoFocus_Load);
            this.gpb_Controls.ResumeLayout(false);
            this.gpb_Controls.PerformLayout();
            this.tlp_Main.ResumeLayout(false);
            this.pnl_View.ResumeLayout(false);
            this.pnl_Controls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gpb_Controls;
        private System.Windows.Forms.RichTextBox txt_Log;
        private System.Windows.Forms.Button btn_StopAF;
        private System.Windows.Forms.Button btn_PlayAF;
        private System.Windows.Forms.Button btn_SingleShotAF;
        private System.Windows.Forms.Button btn_DebounceCount;
        private System.Windows.Forms.Button btn_StabilityMethod;
        private System.Windows.Forms.Button btn_ResamplingMethod;
        private System.Windows.Forms.Button btn_SharpnessMethod;
        private System.Windows.Forms.Button btn_SetROI;
        private System.Windows.Forms.TextBox txt_DebounceCount;
        private System.Windows.Forms.TextBox txt_StabilityMethod;
        private System.Windows.Forms.TextBox txt_ResamplingMethod;
        private System.Windows.Forms.TextBox txt_SharpnessMethod;
        private System.Windows.Forms.TextBox txt_ROI;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmr_LogUpdate;
        private SDOAQNet.Component.SdoPanel pnl_Viewer;
        private System.Windows.Forms.TableLayoutPanel tlp_Main;
        private System.Windows.Forms.Panel pnl_View;
        private System.Windows.Forms.Panel pnl_Controls;
    }
}

