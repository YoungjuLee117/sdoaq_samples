namespace SdoaqEdof
{
    partial class SdoaqEDoF
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdoaqEDoF));
			this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
			this.pnl_Controls = new System.Windows.Forms.Panel();
			this.txt_Log = new System.Windows.Forms.RichTextBox();
			this.btn_StopEDoF = new System.Windows.Forms.Button();
			this.btn_PlayEDoF = new System.Windows.Forms.Button();
			this.btn_SingleShotEDoF = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cmb_EdofResizeRatio = new System.Windows.Forms.ComboBox();
			this.btn_SetScaleStep = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.txt_ScaleStep = new System.Windows.Forms.TextBox();
			this.btn_SetThreshold = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.txt_Threshold = new System.Windows.Forms.TextBox();
			this.btn_SetIteration = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txt_Iteration = new System.Windows.Forms.TextBox();
			this.btn_SetKernelSize = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.txt_KernelSize = new System.Windows.Forms.TextBox();
			this.btn_SetResizeRatio = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btn_SetMALSFocus = new System.Windows.Forms.Button();
			this.txt_MALSFocus = new System.Windows.Forms.TextBox();
			this.btn_SetROI = new System.Windows.Forms.Button();
			this.txt_ROI = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btn_OpenCalibration = new System.Windows.Forms.Button();
			this.pnl_Viewer = new SDOAQNet.Component.SdoPanel();
			this.tmr_LogUpdate = new System.Windows.Forms.Timer(this.components);
			this.openFile = new System.Windows.Forms.OpenFileDialog();
			this.tlp_Main.SuspendLayout();
			this.pnl_Controls.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlp_Main
			// 
			this.tlp_Main.ColumnCount = 2;
			this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 610F));
			this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlp_Main.Controls.Add(this.pnl_Controls, 0, 0);
			this.tlp_Main.Controls.Add(this.pnl_Viewer, 1, 0);
			this.tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlp_Main.Location = new System.Drawing.Point(0, 0);
			this.tlp_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tlp_Main.Name = "tlp_Main";
			this.tlp_Main.RowCount = 1;
			this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlp_Main.Size = new System.Drawing.Size(1234, 591);
			this.tlp_Main.TabIndex = 0;
			// 
			// pnl_Controls
			// 
			this.pnl_Controls.Controls.Add(this.txt_Log);
			this.pnl_Controls.Controls.Add(this.btn_StopEDoF);
			this.pnl_Controls.Controls.Add(this.btn_PlayEDoF);
			this.pnl_Controls.Controls.Add(this.btn_SingleShotEDoF);
			this.pnl_Controls.Controls.Add(this.groupBox2);
			this.pnl_Controls.Controls.Add(this.groupBox1);
			this.pnl_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnl_Controls.Location = new System.Drawing.Point(3, 3);
			this.pnl_Controls.Name = "pnl_Controls";
			this.pnl_Controls.Size = new System.Drawing.Size(604, 585);
			this.pnl_Controls.TabIndex = 0;
			// 
			// txt_Log
			// 
			this.txt_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txt_Log.Location = new System.Drawing.Point(11, 401);
			this.txt_Log.Margin = new System.Windows.Forms.Padding(2);
			this.txt_Log.Name = "txt_Log";
			this.txt_Log.Size = new System.Drawing.Size(575, 173);
			this.txt_Log.TabIndex = 14;
			this.txt_Log.Text = "";
			// 
			// btn_StopEDoF
			// 
			this.btn_StopEDoF.Location = new System.Drawing.Point(417, 353);
			this.btn_StopEDoF.Name = "btn_StopEDoF";
			this.btn_StopEDoF.Size = new System.Drawing.Size(160, 35);
			this.btn_StopEDoF.TabIndex = 13;
			this.btn_StopEDoF.Text = "stop EDoF";
			this.btn_StopEDoF.UseVisualStyleBackColor = true;
			this.btn_StopEDoF.Click += new System.EventHandler(this.btn_StopEDoF_Click);
			// 
			// btn_PlayEDoF
			// 
			this.btn_PlayEDoF.Location = new System.Drawing.Point(214, 353);
			this.btn_PlayEDoF.Name = "btn_PlayEDoF";
			this.btn_PlayEDoF.Size = new System.Drawing.Size(160, 35);
			this.btn_PlayEDoF.TabIndex = 12;
			this.btn_PlayEDoF.Text = "play EDoF";
			this.btn_PlayEDoF.UseVisualStyleBackColor = true;
			this.btn_PlayEDoF.Click += new System.EventHandler(this.btn_PlayEDoF_Click);
			// 
			// btn_SingleShotEDoF
			// 
			this.btn_SingleShotEDoF.Location = new System.Drawing.Point(11, 353);
			this.btn_SingleShotEDoF.Name = "btn_SingleShotEDoF";
			this.btn_SingleShotEDoF.Size = new System.Drawing.Size(160, 35);
			this.btn_SingleShotEDoF.TabIndex = 11;
			this.btn_SingleShotEDoF.Text = "SingleShotEDoF";
			this.btn_SingleShotEDoF.UseVisualStyleBackColor = true;
			this.btn_SingleShotEDoF.Click += new System.EventHandler(this.btn_SingleShotEDoF_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cmb_EdofResizeRatio);
			this.groupBox2.Controls.Add(this.btn_SetScaleStep);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.txt_ScaleStep);
			this.groupBox2.Controls.Add(this.btn_SetThreshold);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.txt_Threshold);
			this.groupBox2.Controls.Add(this.btn_SetIteration);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.txt_Iteration);
			this.groupBox2.Controls.Add(this.btn_SetKernelSize);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.txt_KernelSize);
			this.groupBox2.Controls.Add(this.btn_SetResizeRatio);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Location = new System.Drawing.Point(11, 144);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(575, 201);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Live-compatible EDoF Parameters";
			// 
			// cmb_EdofResizeRatio
			// 
			this.cmb_EdofResizeRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmb_EdofResizeRatio.FormattingEnabled = true;
			this.cmb_EdofResizeRatio.Items.AddRange(new object[] {
            "1",
            "0.5",
            "0.25"});			
			this.cmb_EdofResizeRatio.Location = new System.Drawing.Point(350, 31);
			this.cmb_EdofResizeRatio.Name = "cmb_EdofResizeRatio";
			this.cmb_EdofResizeRatio.Size = new System.Drawing.Size(160, 23);
			this.cmb_EdofResizeRatio.TabIndex = 26;
			// 
			// btn_SetScaleStep
			// 
			this.btn_SetScaleStep.Location = new System.Drawing.Point(516, 166);
			this.btn_SetScaleStep.Name = "btn_SetScaleStep";
			this.btn_SetScaleStep.Size = new System.Drawing.Size(50, 25);
			this.btn_SetScaleStep.TabIndex = 25;
			this.btn_SetScaleStep.Text = "Set";
			this.btn_SetScaleStep.UseVisualStyleBackColor = true;
			this.btn_SetScaleStep.Click += new System.EventHandler(this.btn_SetScaleStep_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 171);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(329, 15);
			this.label7.TabIndex = 23;
			this.label7.Text = "scale correction refer step (range MALS steps)";
			// 
			// txt_ScaleStep
			// 
			this.txt_ScaleStep.Location = new System.Drawing.Point(350, 167);
			this.txt_ScaleStep.Name = "txt_ScaleStep";
			this.txt_ScaleStep.Size = new System.Drawing.Size(160, 23);
			this.txt_ScaleStep.TabIndex = 24;
			this.txt_ScaleStep.Text = "160";
			// 
			// btn_SetThreshold
			// 
			this.btn_SetThreshold.Location = new System.Drawing.Point(516, 132);
			this.btn_SetThreshold.Name = "btn_SetThreshold";
			this.btn_SetThreshold.Size = new System.Drawing.Size(50, 25);
			this.btn_SetThreshold.TabIndex = 22;
			this.btn_SetThreshold.Text = "Set";
			this.btn_SetThreshold.UseVisualStyleBackColor = true;
			this.btn_SetThreshold.Click += new System.EventHandler(this.btn_SetThreshold_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 137);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(182, 15);
			this.label6.TabIndex = 20;
			this.label6.Text = "threshold (range 0.0~9.9)";
			// 
			// txt_Threshold
			// 
			this.txt_Threshold.Location = new System.Drawing.Point(350, 133);
			this.txt_Threshold.Name = "txt_Threshold";
			this.txt_Threshold.Size = new System.Drawing.Size(160, 23);
			this.txt_Threshold.TabIndex = 21;
			this.txt_Threshold.Text = "1.0";
			// 
			// btn_SetIteration
			// 
			this.btn_SetIteration.Location = new System.Drawing.Point(516, 98);
			this.btn_SetIteration.Name = "btn_SetIteration";
			this.btn_SetIteration.Size = new System.Drawing.Size(50, 25);
			this.btn_SetIteration.TabIndex = 19;
			this.btn_SetIteration.Text = "Set";
			this.btn_SetIteration.UseVisualStyleBackColor = true;
			this.btn_SetIteration.Click += new System.EventHandler(this.btn_SetIteration_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 103);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(238, 15);
			this.label5.TabIndex = 17;
			this.label5.Text = "pixel-wise iteration (range 0~16)";
			// 
			// txt_Iteration
			// 
			this.txt_Iteration.Location = new System.Drawing.Point(350, 99);
			this.txt_Iteration.Name = "txt_Iteration";
			this.txt_Iteration.Size = new System.Drawing.Size(160, 23);
			this.txt_Iteration.TabIndex = 18;
			this.txt_Iteration.Tag = "8";
			this.txt_Iteration.Text = "8";
			// 
			// btn_SetKernelSize
			// 
			this.btn_SetKernelSize.Location = new System.Drawing.Point(516, 64);
			this.btn_SetKernelSize.Name = "btn_SetKernelSize";
			this.btn_SetKernelSize.Size = new System.Drawing.Size(50, 25);
			this.btn_SetKernelSize.TabIndex = 16;
			this.btn_SetKernelSize.Text = "Set";
			this.btn_SetKernelSize.UseVisualStyleBackColor = true;
			this.btn_SetKernelSize.Click += new System.EventHandler(this.btn_SetKernelSize_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(245, 15);
			this.label4.TabIndex = 14;
			this.label4.Text = "pixel-wise kernel size (range 3~5)";
			// 
			// txt_KernelSize
			// 
			this.txt_KernelSize.Location = new System.Drawing.Point(350, 65);
			this.txt_KernelSize.Name = "txt_KernelSize";
			this.txt_KernelSize.Size = new System.Drawing.Size(160, 23);
			this.txt_KernelSize.TabIndex = 15;
			this.txt_KernelSize.Text = "5";
			// 
			// btn_SetResizeRatio
			// 
			this.btn_SetResizeRatio.Location = new System.Drawing.Point(516, 30);
			this.btn_SetResizeRatio.Name = "btn_SetResizeRatio";
			this.btn_SetResizeRatio.Size = new System.Drawing.Size(50, 25);
			this.btn_SetResizeRatio.TabIndex = 13;
			this.btn_SetResizeRatio.Text = "Set";
			this.btn_SetResizeRatio.UseVisualStyleBackColor = true;
			this.btn_SetResizeRatio.Click += new System.EventHandler(this.btn_SetResizeRatio_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(245, 15);
			this.label3.TabIndex = 11;
			this.label3.Text = "EDoF resize ratio (0.25, 0.5, 1.0)";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btn_SetMALSFocus);
			this.groupBox1.Controls.Add(this.txt_MALSFocus);
			this.groupBox1.Controls.Add(this.btn_SetROI);
			this.groupBox1.Controls.Add(this.txt_ROI);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btn_OpenCalibration);
			this.groupBox1.Location = new System.Drawing.Point(11, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(575, 129);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Non-live-compatible Parameters";
			// 
			// btn_SetMALSFocus
			// 
			this.btn_SetMALSFocus.Location = new System.Drawing.Point(516, 94);
			this.btn_SetMALSFocus.Name = "btn_SetMALSFocus";
			this.btn_SetMALSFocus.Size = new System.Drawing.Size(50, 25);
			this.btn_SetMALSFocus.TabIndex = 10;
			this.btn_SetMALSFocus.Text = "Set";
			this.btn_SetMALSFocus.UseVisualStyleBackColor = true;
			this.btn_SetMALSFocus.Click += new System.EventHandler(this.btn_SetMALSFocus_Click);
			// 
			// txt_MALSFocus
			// 
			this.txt_MALSFocus.Location = new System.Drawing.Point(350, 95);
			this.txt_MALSFocus.Name = "txt_MALSFocus";
			this.txt_MALSFocus.Size = new System.Drawing.Size(160, 23);
			this.txt_MALSFocus.TabIndex = 9;
			this.txt_MALSFocus.Text = "0-319-35";
			// 
			// btn_SetROI
			// 
			this.btn_SetROI.Location = new System.Drawing.Point(516, 61);
			this.btn_SetROI.Name = "btn_SetROI";
			this.btn_SetROI.Size = new System.Drawing.Size(50, 25);
			this.btn_SetROI.TabIndex = 8;
			this.btn_SetROI.Text = "Set";
			this.btn_SetROI.UseVisualStyleBackColor = true;
			this.btn_SetROI.Click += new System.EventHandler(this.btn_SetROI_Click);
			// 
			// txt_ROI
			// 
			this.txt_ROI.Location = new System.Drawing.Point(350, 62);
			this.txt_ROI.Name = "txt_ROI";
			this.txt_ROI.Size = new System.Drawing.Size(160, 23);
			this.txt_ROI.TabIndex = 7;
			this.txt_ROI.Text = "0,0,2040,1086";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(210, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "MALS focus set(low-high-unit)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 66);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(189, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = "ROI(left,top,width,height)";
			// 
			// btn_OpenCalibration
			// 
			this.btn_OpenCalibration.Location = new System.Drawing.Point(15, 24);
			this.btn_OpenCalibration.Name = "btn_OpenCalibration";
			this.btn_OpenCalibration.Size = new System.Drawing.Size(280, 28);
			this.btn_OpenCalibration.TabIndex = 0;
			this.btn_OpenCalibration.Text = "Select calibration file for objective";
			this.btn_OpenCalibration.UseVisualStyleBackColor = true;
			this.btn_OpenCalibration.Click += new System.EventHandler(this.btn_OpenCalibration_Click);
			// 
			// pnl_Viewer
			// 
			this.pnl_Viewer.BorderColor = System.Drawing.Color.Black;
			this.pnl_Viewer.BorderWidth = 1;
			this.pnl_Viewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnl_Viewer.Location = new System.Drawing.Point(610, 0);
			this.pnl_Viewer.Margin = new System.Windows.Forms.Padding(0);
			this.pnl_Viewer.Name = "pnl_Viewer";
			this.pnl_Viewer.Size = new System.Drawing.Size(624, 591);
			this.pnl_Viewer.TabIndex = 3;
			// 
			// tmr_LogUpdate
			// 
			this.tmr_LogUpdate.Tick += new System.EventHandler(this.tmr_LogUpdate_Tick);
			// 
			// openFile
			// 
			this.openFile.FileName = "openFileDialog1";
			// 
			// SdoaqEDoF
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1234, 591);
			this.Controls.Add(this.tlp_Main);
			this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "SdoaqEDoF";
			this.Text = "SDOAQ EDoF Sample";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SdoaqEDoF_FormClosed);
			this.Load += new System.EventHandler(this.SdoaqEDoF_Load);
			this.tlp_Main.ResumeLayout(false);
			this.pnl_Controls.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_Main;
        private System.Windows.Forms.Panel pnl_Controls;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_OpenCalibration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SetResizeRatio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_SetMALSFocus;
        private System.Windows.Forms.TextBox txt_MALSFocus;
        private System.Windows.Forms.Button btn_SetROI;
        private System.Windows.Forms.TextBox txt_ROI;
        private System.Windows.Forms.Button btn_SetScaleStep;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_ScaleStep;
        private System.Windows.Forms.Button btn_SetThreshold;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_Threshold;
        private System.Windows.Forms.Button btn_SetIteration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Iteration;
        private System.Windows.Forms.Button btn_SetKernelSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_KernelSize;
        private System.Windows.Forms.Button btn_StopEDoF;
        private System.Windows.Forms.Button btn_PlayEDoF;
        private System.Windows.Forms.Button btn_SingleShotEDoF;
        private System.Windows.Forms.RichTextBox txt_Log;
        private SDOAQNet.Component.SdoPanel pnl_Viewer;
        private System.Windows.Forms.Timer tmr_LogUpdate;
        private System.Windows.Forms.ComboBox cmb_EdofResizeRatio;
        private System.Windows.Forms.OpenFileDialog openFile;
    }
}

