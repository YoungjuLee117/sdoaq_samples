namespace SDOAQNet.Component
{
    partial class SdoaqCameraPredefinedParams
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_ROI = new System.Windows.Forms.Label();
            this.txt_Roi_Left = new System.Windows.Forms.TextBox();
            this.btn_Roi = new System.Windows.Forms.Button();
            this.btn_ExposureTime = new System.Windows.Forms.Button();
            this.txt_ExposureTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Gain = new System.Windows.Forms.Button();
            this.txt_Gain = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_WhiteBalance = new System.Windows.Forms.Button();
            this.txt_WhiteBalance_R = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Roi_Top = new System.Windows.Forms.TextBox();
            this.txt_Roi_Width = new System.Windows.Forms.TextBox();
            this.txt_Roi_Height = new System.Windows.Forms.TextBox();
            this.txt_WhiteBalance_G = new System.Windows.Forms.TextBox();
            this.txt_WhiteBalance_B = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_WhiteBalance_B);
            this.groupBox1.Controls.Add(this.txt_WhiteBalance_G);
            this.groupBox1.Controls.Add(this.txt_WhiteBalance_R);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txt_Gain);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_WhiteBalance);
            this.groupBox1.Controls.Add(this.txt_ExposureTime);
            this.groupBox1.Controls.Add(this.btn_Gain);
            this.groupBox1.Controls.Add(this.lbl_ROI);
            this.groupBox1.Controls.Add(this.btn_ExposureTime);
            this.groupBox1.Controls.Add(this.txt_Roi_Height);
            this.groupBox1.Controls.Add(this.txt_Roi_Width);
            this.groupBox1.Controls.Add(this.txt_Roi_Top);
            this.groupBox1.Controls.Add(this.txt_Roi_Left);
            this.groupBox1.Controls.Add(this.btn_Roi);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pre-defined parameters setting";
            // 
            // lbl_ROI
            // 
            this.lbl_ROI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_ROI.Location = new System.Drawing.Point(16, 28);
            this.lbl_ROI.Name = "lbl_ROI";
            this.lbl_ROI.Size = new System.Drawing.Size(209, 23);
            this.lbl_ROI.TabIndex = 8;
            this.lbl_ROI.Text = "ROI (left,top,width,height)";
            this.lbl_ROI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Roi_Left
            // 
            this.txt_Roi_Left.Location = new System.Drawing.Point(231, 28);
            this.txt_Roi_Left.Name = "txt_Roi_Left";
            this.txt_Roi_Left.Size = new System.Drawing.Size(50, 23);
            this.txt_Roi_Left.TabIndex = 7;
            this.txt_Roi_Left.Text = "1000";
            // 
            // btn_Roi
            // 
            this.btn_Roi.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Roi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Roi.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_Roi.Location = new System.Drawing.Point(464, 28);
            this.btn_Roi.Name = "btn_Roi";
            this.btn_Roi.Size = new System.Drawing.Size(56, 23);
            this.btn_Roi.TabIndex = 6;
            this.btn_Roi.Text = "Set";
            this.btn_Roi.UseVisualStyleBackColor = true;
            // 
            // btn_ExposureTime
            // 
            this.btn_ExposureTime.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_ExposureTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExposureTime.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_ExposureTime.Location = new System.Drawing.Point(464, 59);
            this.btn_ExposureTime.Name = "btn_ExposureTime";
            this.btn_ExposureTime.Size = new System.Drawing.Size(56, 23);
            this.btn_ExposureTime.TabIndex = 6;
            this.btn_ExposureTime.Text = "Set";
            this.btn_ExposureTime.UseVisualStyleBackColor = true;
            // 
            // txt_ExposureTime
            // 
            this.txt_ExposureTime.Location = new System.Drawing.Point(231, 59);
            this.txt_ExposureTime.Name = "txt_ExposureTime";
            this.txt_ExposureTime.Size = new System.Drawing.Size(227, 23);
            this.txt_ExposureTime.TabIndex = 7;
            this.txt_ExposureTime.Text = "0,0,2040,1086";
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(16, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Exposure tme (us)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_Gain
            // 
            this.btn_Gain.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_Gain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Gain.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_Gain.Location = new System.Drawing.Point(464, 90);
            this.btn_Gain.Name = "btn_Gain";
            this.btn_Gain.Size = new System.Drawing.Size(56, 23);
            this.btn_Gain.TabIndex = 6;
            this.btn_Gain.Text = "Set";
            this.btn_Gain.UseVisualStyleBackColor = true;
            // 
            // txt_Gain
            // 
            this.txt_Gain.Location = new System.Drawing.Point(231, 90);
            this.txt_Gain.Name = "txt_Gain";
            this.txt_Gain.Size = new System.Drawing.Size(227, 23);
            this.txt_Gain.TabIndex = 7;
            this.txt_Gain.Text = "0,0,2040,1086";
            // 
            // label2
            // 
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(16, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Gain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_WhiteBalance
            // 
            this.btn_WhiteBalance.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_WhiteBalance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_WhiteBalance.Font = new System.Drawing.Font("Verdana", 8F);
            this.btn_WhiteBalance.Location = new System.Drawing.Point(464, 121);
            this.btn_WhiteBalance.Name = "btn_WhiteBalance";
            this.btn_WhiteBalance.Size = new System.Drawing.Size(56, 23);
            this.btn_WhiteBalance.TabIndex = 6;
            this.btn_WhiteBalance.Text = "Set";
            this.btn_WhiteBalance.UseVisualStyleBackColor = true;
            // 
            // txt_WhiteBalance_R
            // 
            this.txt_WhiteBalance_R.Location = new System.Drawing.Point(231, 121);
            this.txt_WhiteBalance_R.Name = "txt_WhiteBalance_R";
            this.txt_WhiteBalance_R.Size = new System.Drawing.Size(66, 23);
            this.txt_WhiteBalance_R.TabIndex = 7;
            this.txt_WhiteBalance_R.Text = "000";
            // 
            // label3
            // 
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(16, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "White balance (r,g,b)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt_Roi_Top
            // 
            this.txt_Roi_Top.Location = new System.Drawing.Point(290, 28);
            this.txt_Roi_Top.Name = "txt_Roi_Top";
            this.txt_Roi_Top.Size = new System.Drawing.Size(50, 23);
            this.txt_Roi_Top.TabIndex = 7;
            this.txt_Roi_Top.Text = "1000";
            // 
            // txt_Roi_Width
            // 
            this.txt_Roi_Width.Location = new System.Drawing.Point(349, 28);
            this.txt_Roi_Width.Name = "txt_Roi_Width";
            this.txt_Roi_Width.Size = new System.Drawing.Size(50, 23);
            this.txt_Roi_Width.TabIndex = 7;
            this.txt_Roi_Width.Text = "1000";
            // 
            // txt_Roi_Height
            // 
            this.txt_Roi_Height.Location = new System.Drawing.Point(408, 28);
            this.txt_Roi_Height.Name = "txt_Roi_Height";
            this.txt_Roi_Height.Size = new System.Drawing.Size(50, 23);
            this.txt_Roi_Height.TabIndex = 7;
            this.txt_Roi_Height.Text = "1000";
            // 
            // txt_WhiteBalance_G
            // 
            this.txt_WhiteBalance_G.Location = new System.Drawing.Point(311, 121);
            this.txt_WhiteBalance_G.Name = "txt_WhiteBalance_G";
            this.txt_WhiteBalance_G.Size = new System.Drawing.Size(66, 23);
            this.txt_WhiteBalance_G.TabIndex = 7;
            this.txt_WhiteBalance_G.Text = "000";
            // 
            // txt_WhiteBalance_B
            // 
            this.txt_WhiteBalance_B.Location = new System.Drawing.Point(391, 121);
            this.txt_WhiteBalance_B.Name = "txt_WhiteBalance_B";
            this.txt_WhiteBalance_B.Size = new System.Drawing.Size(66, 23);
            this.txt_WhiteBalance_B.TabIndex = 7;
            this.txt_WhiteBalance_B.Text = "000";
            // 
            // SdoaqCameraPredefinedParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SdoaqCameraPredefinedParams";
            this.Size = new System.Drawing.Size(549, 171);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_WhiteBalance_B;
        private System.Windows.Forms.TextBox txt_WhiteBalance_G;
        private System.Windows.Forms.TextBox txt_WhiteBalance_R;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Gain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_WhiteBalance;
        private System.Windows.Forms.TextBox txt_ExposureTime;
        private System.Windows.Forms.Button btn_Gain;
        private System.Windows.Forms.Label lbl_ROI;
        private System.Windows.Forms.Button btn_ExposureTime;
        private System.Windows.Forms.TextBox txt_Roi_Height;
        private System.Windows.Forms.TextBox txt_Roi_Width;
        private System.Windows.Forms.TextBox txt_Roi_Top;
        private System.Windows.Forms.TextBox txt_Roi_Left;
        private System.Windows.Forms.Button btn_Roi;
    }
}
