
namespace SdoaqApiTester
{
    partial class SdoaqApiTester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdoaqApiTester));
            this.rtxb_Log = new System.Windows.Forms.RichTextBox();
            this.tmr_LogUpdate = new System.Windows.Forms.Timer(this.components);
            this.pnl_Viewer = new SDOAQNet.Component.SdoPanel();
            this.pnl_Control = new SDOAQNet.Component.SdoPanel();
            this.gr_Acquisition = new System.Windows.Forms.GroupBox();
            this.btn_Snap = new System.Windows.Forms.Button();
            this.btn_AcqAF = new System.Windows.Forms.Button();
            this.btn_StopAF = new System.Windows.Forms.Button();
            this.btn_AcqEdof = new System.Windows.Forms.Button();
            this.btn_StopEdof = new System.Windows.Forms.Button();
            this.btn_AcqStack = new System.Windows.Forms.Button();
            this.btn_ContiAF = new System.Windows.Forms.Button();
            this.btn_StopStack = new System.Windows.Forms.Button();
            this.btn_ContiEdof = new System.Windows.Forms.Button();
            this.btn_ContiStack = new System.Windows.Forms.Button();
            this.gr_EdofImgViewOption = new System.Windows.Forms.GroupBox();
            this.chk_PointCloud = new System.Windows.Forms.CheckBox();
            this.chk_HeightMap = new System.Windows.Forms.CheckBox();
            this.chk_QualityMap = new System.Windows.Forms.CheckBox();
            this.chk_Edof = new System.Windows.Forms.CheckBox();
            this.chk_StepMap = new System.Windows.Forms.CheckBox();
            this.cmp_SdoaqParams = new SDOAQNet.Component.SdoaqParams();
            this.pnl_Init = new SDOAQNet.Component.SdoPanel();
            this.btn_Init = new System.Windows.Forms.Button();
            this.btn_Final = new System.Windows.Forms.Button();
            this.pnl_Control.SuspendLayout();
            this.gr_Acquisition.SuspendLayout();
            this.gr_EdofImgViewOption.SuspendLayout();
            this.pnl_Init.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxb_Log
            // 
            this.rtxb_Log.BackColor = System.Drawing.SystemColors.Control;
            this.rtxb_Log.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxb_Log.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxb_Log.Location = new System.Drawing.Point(516, 10);
            this.rtxb_Log.MaxLength = 262144;
            this.rtxb_Log.Name = "rtxb_Log";
            this.rtxb_Log.Size = new System.Drawing.Size(490, 174);
            this.rtxb_Log.TabIndex = 2;
            this.rtxb_Log.Text = "";
            // 
            // tmr_LogUpdate
            // 
            this.tmr_LogUpdate.Tick += new System.EventHandler(this.tmr_LogUpdate_Tick);
            // 
            // pnl_Viewer
            // 
            this.pnl_Viewer.BorderColor = System.Drawing.Color.Black;
            this.pnl_Viewer.BorderWidth = 1;
            this.pnl_Viewer.Location = new System.Drawing.Point(516, 190);
            this.pnl_Viewer.Name = "pnl_Viewer";
            this.pnl_Viewer.Size = new System.Drawing.Size(218, 174);
            this.pnl_Viewer.TabIndex = 1;
            // 
            // pnl_Control
            // 
            this.pnl_Control.BorderColor = System.Drawing.Color.Black;
            this.pnl_Control.BorderWidth = 1;
            this.pnl_Control.Controls.Add(this.gr_Acquisition);
            this.pnl_Control.Controls.Add(this.gr_EdofImgViewOption);
            this.pnl_Control.Controls.Add(this.cmp_SdoaqParams);
            this.pnl_Control.Controls.Add(this.pnl_Init);
            this.pnl_Control.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnl_Control.Location = new System.Drawing.Point(0, 0);
            this.pnl_Control.Name = "pnl_Control";
            this.pnl_Control.Size = new System.Drawing.Size(510, 965);
            this.pnl_Control.TabIndex = 0;
            // 
            // gr_Acquisition
            // 
            this.gr_Acquisition.Controls.Add(this.btn_Snap);
            this.gr_Acquisition.Controls.Add(this.btn_AcqAF);
            this.gr_Acquisition.Controls.Add(this.btn_StopAF);
            this.gr_Acquisition.Controls.Add(this.btn_AcqEdof);
            this.gr_Acquisition.Controls.Add(this.btn_StopEdof);
            this.gr_Acquisition.Controls.Add(this.btn_AcqStack);
            this.gr_Acquisition.Controls.Add(this.btn_ContiAF);
            this.gr_Acquisition.Controls.Add(this.btn_StopStack);
            this.gr_Acquisition.Controls.Add(this.btn_ContiEdof);
            this.gr_Acquisition.Controls.Add(this.btn_ContiStack);
            this.gr_Acquisition.Location = new System.Drawing.Point(12, 607);
            this.gr_Acquisition.Name = "gr_Acquisition";
            this.gr_Acquisition.Size = new System.Drawing.Size(486, 202);
            this.gr_Acquisition.TabIndex = 3;
            this.gr_Acquisition.TabStop = false;
            this.gr_Acquisition.Text = "Acquisition";
            // 
            // btn_Snap
            // 
            this.btn_Snap.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Snap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Snap.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Snap.Location = new System.Drawing.Point(6, 154);
            this.btn_Snap.Name = "btn_Snap";
            this.btn_Snap.Size = new System.Drawing.Size(145, 35);
            this.btn_Snap.TabIndex = 26;
            this.btn_Snap.Text = "Conti Acq Snap";
            this.btn_Snap.UseVisualStyleBackColor = true;
            this.btn_Snap.Click += new System.EventHandler(this.btn_AcqMode_Snap_Click);
            // 
            // btn_AcqAF
            // 
            this.btn_AcqAF.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_AcqAF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AcqAF.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_AcqAF.Location = new System.Drawing.Point(6, 110);
            this.btn_AcqAF.Name = "btn_AcqAF";
            this.btn_AcqAF.Size = new System.Drawing.Size(145, 35);
            this.btn_AcqAF.TabIndex = 17;
            this.btn_AcqAF.Text = "Acq. AF";
            this.btn_AcqAF.UseVisualStyleBackColor = true;
            this.btn_AcqAF.Click += new System.EventHandler(this.btn_AcqMode_Af_Click);
            // 
            // btn_StopAF
            // 
            this.btn_StopAF.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_StopAF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopAF.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StopAF.Location = new System.Drawing.Point(324, 110);
            this.btn_StopAF.Name = "btn_StopAF";
            this.btn_StopAF.Size = new System.Drawing.Size(145, 35);
            this.btn_StopAF.TabIndex = 18;
            this.btn_StopAF.Text = "Stop AF";
            this.btn_StopAF.UseVisualStyleBackColor = true;
            this.btn_StopAF.Click += new System.EventHandler(this.btn_AcqMode_Af_Click);
            // 
            // btn_AcqEdof
            // 
            this.btn_AcqEdof.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_AcqEdof.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AcqEdof.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_AcqEdof.Location = new System.Drawing.Point(6, 66);
            this.btn_AcqEdof.Name = "btn_AcqEdof";
            this.btn_AcqEdof.Size = new System.Drawing.Size(145, 35);
            this.btn_AcqEdof.TabIndex = 19;
            this.btn_AcqEdof.Text = "Acq. EDOF";
            this.btn_AcqEdof.UseVisualStyleBackColor = true;
            this.btn_AcqEdof.Click += new System.EventHandler(this.btn_AcqMode_Edof_Click);
            // 
            // btn_StopEdof
            // 
            this.btn_StopEdof.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_StopEdof.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopEdof.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StopEdof.Location = new System.Drawing.Point(324, 66);
            this.btn_StopEdof.Name = "btn_StopEdof";
            this.btn_StopEdof.Size = new System.Drawing.Size(145, 35);
            this.btn_StopEdof.TabIndex = 20;
            this.btn_StopEdof.Text = "Stop EDOF";
            this.btn_StopEdof.UseVisualStyleBackColor = true;
            this.btn_StopEdof.Click += new System.EventHandler(this.btn_AcqMode_Edof_Click);
            // 
            // btn_AcqStack
            // 
            this.btn_AcqStack.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_AcqStack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AcqStack.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_AcqStack.Location = new System.Drawing.Point(6, 22);
            this.btn_AcqStack.Name = "btn_AcqStack";
            this.btn_AcqStack.Size = new System.Drawing.Size(145, 35);
            this.btn_AcqStack.TabIndex = 21;
            this.btn_AcqStack.Text = "Acq. STACK";
            this.btn_AcqStack.UseVisualStyleBackColor = true;
            this.btn_AcqStack.Click += new System.EventHandler(this.btn_AcqMode_Stack_Click);
            // 
            // btn_ContiAF
            // 
            this.btn_ContiAF.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_ContiAF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ContiAF.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ContiAF.Location = new System.Drawing.Point(165, 110);
            this.btn_ContiAF.Name = "btn_ContiAF";
            this.btn_ContiAF.Size = new System.Drawing.Size(145, 35);
            this.btn_ContiAF.TabIndex = 22;
            this.btn_ContiAF.Text = "Conti. AF";
            this.btn_ContiAF.UseVisualStyleBackColor = true;
            this.btn_ContiAF.Click += new System.EventHandler(this.btn_AcqMode_Af_Click);
            // 
            // btn_StopStack
            // 
            this.btn_StopStack.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_StopStack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StopStack.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StopStack.Location = new System.Drawing.Point(324, 22);
            this.btn_StopStack.Name = "btn_StopStack";
            this.btn_StopStack.Size = new System.Drawing.Size(145, 35);
            this.btn_StopStack.TabIndex = 23;
            this.btn_StopStack.Text = "Stop STACK";
            this.btn_StopStack.UseVisualStyleBackColor = true;
            this.btn_StopStack.Click += new System.EventHandler(this.btn_AcqMode_Stack_Click);
            // 
            // btn_ContiEdof
            // 
            this.btn_ContiEdof.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_ContiEdof.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ContiEdof.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ContiEdof.Location = new System.Drawing.Point(165, 66);
            this.btn_ContiEdof.Name = "btn_ContiEdof";
            this.btn_ContiEdof.Size = new System.Drawing.Size(145, 35);
            this.btn_ContiEdof.TabIndex = 24;
            this.btn_ContiEdof.Text = "Conti. EDOF";
            this.btn_ContiEdof.UseVisualStyleBackColor = true;
            this.btn_ContiEdof.Click += new System.EventHandler(this.btn_AcqMode_Edof_Click);
            // 
            // btn_ContiStack
            // 
            this.btn_ContiStack.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_ContiStack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ContiStack.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ContiStack.Location = new System.Drawing.Point(165, 22);
            this.btn_ContiStack.Name = "btn_ContiStack";
            this.btn_ContiStack.Size = new System.Drawing.Size(145, 35);
            this.btn_ContiStack.TabIndex = 25;
            this.btn_ContiStack.Text = "Conti. STACK";
            this.btn_ContiStack.UseVisualStyleBackColor = true;
            this.btn_ContiStack.Click += new System.EventHandler(this.btn_AcqMode_Stack_Click);
            // 
            // gr_EdofImgViewOption
            // 
            this.gr_EdofImgViewOption.Controls.Add(this.chk_PointCloud);
            this.gr_EdofImgViewOption.Controls.Add(this.chk_HeightMap);
            this.gr_EdofImgViewOption.Controls.Add(this.chk_QualityMap);
            this.gr_EdofImgViewOption.Controls.Add(this.chk_Edof);
            this.gr_EdofImgViewOption.Controls.Add(this.chk_StepMap);
            this.gr_EdofImgViewOption.Location = new System.Drawing.Point(12, 414);
            this.gr_EdofImgViewOption.Name = "gr_EdofImgViewOption";
            this.gr_EdofImgViewOption.Size = new System.Drawing.Size(486, 187);
            this.gr_EdofImgViewOption.TabIndex = 3;
            this.gr_EdofImgViewOption.TabStop = false;
            this.gr_EdofImgViewOption.Text = "EDOF Image View Option";
            // 
            // chk_PointCloud
            // 
            this.chk_PointCloud.AutoSize = true;
            this.chk_PointCloud.Checked = true;
            this.chk_PointCloud.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_PointCloud.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.chk_PointCloud.Location = new System.Drawing.Point(6, 158);
            this.chk_PointCloud.Name = "chk_PointCloud";
            this.chk_PointCloud.Size = new System.Drawing.Size(271, 19);
            this.chk_PointCloud.TabIndex = 20;
            this.chk_PointCloud.Text = "Point Cloud (3D vertex coordinates)";
            this.chk_PointCloud.UseVisualStyleBackColor = true;
            // 
            // chk_HeightMap
            // 
            this.chk_HeightMap.AutoSize = true;
            this.chk_HeightMap.Checked = true;
            this.chk_HeightMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_HeightMap.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.chk_HeightMap.Location = new System.Drawing.Point(6, 124);
            this.chk_HeightMap.Name = "chk_HeightMap";
            this.chk_HeightMap.Size = new System.Drawing.Size(257, 19);
            this.chk_HeightMap.TabIndex = 16;
            this.chk_HeightMap.Text = "HeightMap (height for each pixel)";
            this.chk_HeightMap.UseVisualStyleBackColor = true;
            // 
            // chk_QualityMap
            // 
            this.chk_QualityMap.AutoSize = true;
            this.chk_QualityMap.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.chk_QualityMap.Location = new System.Drawing.Point(6, 90);
            this.chk_QualityMap.Name = "chk_QualityMap";
            this.chk_QualityMap.Size = new System.Drawing.Size(460, 19);
            this.chk_QualityMap.TabIndex = 17;
            this.chk_QualityMap.Text = "QualityMap (the score for each pixel height in the height map)";
            this.chk_QualityMap.UseVisualStyleBackColor = true;
            // 
            // chk_Edof
            // 
            this.chk_Edof.AutoSize = true;
            this.chk_Edof.Checked = true;
            this.chk_Edof.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Edof.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.chk_Edof.Location = new System.Drawing.Point(6, 56);
            this.chk_Edof.Name = "chk_Edof";
            this.chk_Edof.Size = new System.Drawing.Size(243, 19);
            this.chk_Edof.TabIndex = 18;
            this.chk_Edof.Text = "Edof Image (all-in-focus image)";
            this.chk_Edof.UseVisualStyleBackColor = true;
            // 
            // chk_StepMap
            // 
            this.chk_StepMap.AutoSize = true;
            this.chk_StepMap.Checked = true;
            this.chk_StepMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_StepMap.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.chk_StepMap.Location = new System.Drawing.Point(6, 22);
            this.chk_StepMap.Name = "chk_StepMap";
            this.chk_StepMap.Size = new System.Drawing.Size(271, 19);
            this.chk_StepMap.TabIndex = 19;
            this.chk_StepMap.Text = "StepMap (focus step for each pixel)";
            this.chk_StepMap.UseVisualStyleBackColor = true;
            // 
            // cmp_SdoaqParams
            // 
            this.cmp_SdoaqParams.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmp_SdoaqParams.Location = new System.Drawing.Point(5, 52);
            this.cmp_SdoaqParams.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmp_SdoaqParams.Name = "cmp_SdoaqParams";
            this.cmp_SdoaqParams.Size = new System.Drawing.Size(500, 355);
            this.cmp_SdoaqParams.TabIndex = 2;
            // 
            // pnl_Init
            // 
            this.pnl_Init.BorderColor = System.Drawing.Color.Black;
            this.pnl_Init.BorderWidth = 1;
            this.pnl_Init.Controls.Add(this.btn_Init);
            this.pnl_Init.Controls.Add(this.btn_Final);
            this.pnl_Init.Location = new System.Drawing.Point(5, 5);
            this.pnl_Init.Name = "pnl_Init";
            this.pnl_Init.Size = new System.Drawing.Size(500, 40);
            this.pnl_Init.TabIndex = 1;
            // 
            // btn_Init
            // 
            this.btn_Init.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Init.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Init.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.btn_Init.Location = new System.Drawing.Point(7, 5);
            this.btn_Init.Name = "btn_Init";
            this.btn_Init.Size = new System.Drawing.Size(239, 29);
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
            this.btn_Final.Location = new System.Drawing.Point(254, 5);
            this.btn_Final.Name = "btn_Final";
            this.btn_Final.Size = new System.Drawing.Size(239, 29);
            this.btn_Final.TabIndex = 1;
            this.btn_Final.Text = "Finalize";
            this.btn_Final.UseVisualStyleBackColor = true;
            this.btn_Final.Click += new System.EventHandler(this.btn_Final_Click);
            // 
            // SdoaqApiTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1308, 965);
            this.Controls.Add(this.rtxb_Log);
            this.Controls.Add(this.pnl_Viewer);
            this.Controls.Add(this.pnl_Control);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "SdoaqApiTester";
            this.Text = "SdoaqApiTester";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SdoaqApiTester_FormClosed);
            this.Load += new System.EventHandler(this.SdoaqApiTester_Load);
            this.Resize += new System.EventHandler(this.SdoaqApiTester_Resize);
            this.pnl_Control.ResumeLayout(false);
            this.gr_Acquisition.ResumeLayout(false);
            this.gr_EdofImgViewOption.ResumeLayout(false);
            this.gr_EdofImgViewOption.PerformLayout();
            this.pnl_Init.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SDOAQNet.Component.SdoPanel pnl_Control;
        private SDOAQNet.Component.SdoPanel pnl_Viewer;
        private System.Windows.Forms.RichTextBox rtxb_Log;
        private SDOAQNet.Component.SdoPanel pnl_Init;
        private System.Windows.Forms.Button btn_Init;
        private System.Windows.Forms.Button btn_Final;
        private System.Windows.Forms.Timer tmr_LogUpdate;
        private SDOAQNet.Component.SdoaqParams cmp_SdoaqParams;
        private System.Windows.Forms.GroupBox gr_EdofImgViewOption;
        private System.Windows.Forms.CheckBox chk_PointCloud;
        private System.Windows.Forms.CheckBox chk_HeightMap;
        private System.Windows.Forms.CheckBox chk_QualityMap;
        private System.Windows.Forms.CheckBox chk_Edof;
        private System.Windows.Forms.CheckBox chk_StepMap;
        private System.Windows.Forms.GroupBox gr_Acquisition;
        private System.Windows.Forms.Button btn_Snap;
        private System.Windows.Forms.Button btn_AcqAF;
        private System.Windows.Forms.Button btn_StopAF;
        private System.Windows.Forms.Button btn_AcqEdof;
        private System.Windows.Forms.Button btn_StopEdof;
        private System.Windows.Forms.Button btn_AcqStack;
        private System.Windows.Forms.Button btn_ContiAF;
        private System.Windows.Forms.Button btn_StopStack;
        private System.Windows.Forms.Button btn_ContiEdof;
        private System.Windows.Forms.Button btn_ContiStack;
    }
}

