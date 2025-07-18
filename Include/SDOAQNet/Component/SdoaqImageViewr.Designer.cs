namespace SDOAQNet.Component
{
    partial class SdoaqImageViewr
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
            this.pnl_PointCloudViewerGroup = new SDOAQNet.Component.SdoPanel();
            this.pb_PointCloudViewer = new System.Windows.Forms.PictureBox();
            this.lbl_PointCloudViewer = new SDOAQNet.Component.SdoLabel();
            this.pnl_ImageViewerGroup = new SDOAQNet.Component.SdoPanel();
            this.pb_ImageViewer = new System.Windows.Forms.PictureBox();
            this.listbox_ImageList = new System.Windows.Forms.ListBox();
            this.lbl_ImageViewer = new SDOAQNet.Component.SdoLabel();
            this.pnl_PointCloudViewerGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_PointCloudViewer)).BeginInit();
            this.pnl_ImageViewerGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ImageViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_PointCloudViewerGroup
            // 
            this.pnl_PointCloudViewerGroup.BorderColor = System.Drawing.Color.Black;
            this.pnl_PointCloudViewerGroup.BorderWidth = 1;
            this.pnl_PointCloudViewerGroup.Controls.Add(this.pb_PointCloudViewer);
            this.pnl_PointCloudViewerGroup.Controls.Add(this.lbl_PointCloudViewer);
            this.pnl_PointCloudViewerGroup.Location = new System.Drawing.Point(148, 3);
            this.pnl_PointCloudViewerGroup.Name = "pnl_PointCloudViewerGroup";
            this.pnl_PointCloudViewerGroup.Size = new System.Drawing.Size(139, 179);
            this.pnl_PointCloudViewerGroup.TabIndex = 1;
            // 
            // pb_PointCloudViewer
            // 
            this.pb_PointCloudViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_PointCloudViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pb_PointCloudViewer.Location = new System.Drawing.Point(3, 35);
            this.pb_PointCloudViewer.Margin = new System.Windows.Forms.Padding(2);
            this.pb_PointCloudViewer.Name = "pb_PointCloudViewer";
            this.pb_PointCloudViewer.Size = new System.Drawing.Size(51, 35);
            this.pb_PointCloudViewer.TabIndex = 5;
            this.pb_PointCloudViewer.TabStop = false;
            // 
            // lbl_PointCloudViewer
            // 
            this.lbl_PointCloudViewer.BorderColor = System.Drawing.Color.Black;
            this.lbl_PointCloudViewer.BorderWidth = 1;
            this.lbl_PointCloudViewer.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_PointCloudViewer.Location = new System.Drawing.Point(0, 0);
            this.lbl_PointCloudViewer.Name = "lbl_PointCloudViewer";
            this.lbl_PointCloudViewer.Size = new System.Drawing.Size(139, 32);
            this.lbl_PointCloudViewer.TabIndex = 0;
            this.lbl_PointCloudViewer.Text = "3D Viewer";
            this.lbl_PointCloudViewer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_ImageViewerGroup
            // 
            this.pnl_ImageViewerGroup.BorderColor = System.Drawing.Color.Black;
            this.pnl_ImageViewerGroup.BorderWidth = 1;
            this.pnl_ImageViewerGroup.Controls.Add(this.pb_ImageViewer);
            this.pnl_ImageViewerGroup.Controls.Add(this.listbox_ImageList);
            this.pnl_ImageViewerGroup.Controls.Add(this.lbl_ImageViewer);
            this.pnl_ImageViewerGroup.Location = new System.Drawing.Point(3, 3);
            this.pnl_ImageViewerGroup.Name = "pnl_ImageViewerGroup";
            this.pnl_ImageViewerGroup.Size = new System.Drawing.Size(139, 179);
            this.pnl_ImageViewerGroup.TabIndex = 1;
            // 
            // pb_ImageViewer
            // 
            this.pb_ImageViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_ImageViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pb_ImageViewer.Location = new System.Drawing.Point(3, 35);
            this.pb_ImageViewer.Margin = new System.Windows.Forms.Padding(2);
            this.pb_ImageViewer.Name = "pb_ImageViewer";
            this.pb_ImageViewer.Size = new System.Drawing.Size(51, 35);
            this.pb_ImageViewer.TabIndex = 4;
            this.pb_ImageViewer.TabStop = false;
            // 
            // listbox_ImageList
            // 
            this.listbox_ImageList.FormattingEnabled = true;
            this.listbox_ImageList.ItemHeight = 15;
            this.listbox_ImageList.Location = new System.Drawing.Point(3, 75);
            this.listbox_ImageList.Name = "listbox_ImageList";
            this.listbox_ImageList.Size = new System.Drawing.Size(74, 49);
            this.listbox_ImageList.TabIndex = 3;
            // 
            // lbl_ImageViewer
            // 
            this.lbl_ImageViewer.BorderColor = System.Drawing.Color.Black;
            this.lbl_ImageViewer.BorderWidth = 1;
            this.lbl_ImageViewer.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_ImageViewer.Location = new System.Drawing.Point(0, 0);
            this.lbl_ImageViewer.Name = "lbl_ImageViewer";
            this.lbl_ImageViewer.Size = new System.Drawing.Size(139, 32);
            this.lbl_ImageViewer.TabIndex = 0;
            this.lbl_ImageViewer.Text = "Viewer";
            this.lbl_ImageViewer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SdoaqImageViewr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl_PointCloudViewerGroup);
            this.Controls.Add(this.pnl_ImageViewerGroup);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SdoaqImageViewr";
            this.Size = new System.Drawing.Size(294, 186);
            this.pnl_PointCloudViewerGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_PointCloudViewer)).EndInit();
            this.pnl_ImageViewerGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_ImageViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SDOAQNet.Component.SdoLabel lbl_ImageViewer;
        private SDOAQNet.Component.SdoPanel pnl_ImageViewerGroup;
        private SDOAQNet.Component.SdoPanel pnl_PointCloudViewerGroup;
        private SDOAQNet.Component.SdoLabel lbl_PointCloudViewer;
        private System.Windows.Forms.ListBox listbox_ImageList;
        private System.Windows.Forms.PictureBox pb_ImageViewer;
        private System.Windows.Forms.PictureBox pb_PointCloudViewer;
    }
}
