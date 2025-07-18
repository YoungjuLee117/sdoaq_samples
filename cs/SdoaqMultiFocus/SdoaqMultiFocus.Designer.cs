namespace SdoaqMultiFocus
{
    partial class SdoaqMultiFocus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdoaqMultiFocus));
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.gr_Control = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gr_Acq = new System.Windows.Forms.GroupBox();
            this.pnl_ListView = new System.Windows.Forms.Panel();
            this.cmb_Param_func = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Script_Reset = new System.Windows.Forms.Button();
            this.btn_Script_Remove = new System.Windows.Forms.Button();
            this.btn_Script_Modify = new System.Windows.Forms.Button();
            this.btn_Script_Add = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_Param_rect_get = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Param_id = new System.Windows.Forms.TextBox();
            this.txt_Param_rect = new System.Windows.Forms.TextBox();
            this.txt_Param_focus = new System.Windows.Forms.TextBox();
            this.btn_StopMF = new System.Windows.Forms.Button();
            this.btn_PlayMF = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Param_focusStepOutside = new System.Windows.Forms.TextBox();
            this.btn_Param_focusStepOutside = new System.Windows.Forms.Button();
            this.tmr_LogUpdate = new System.Windows.Forms.Timer(this.components);
            this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_View = new System.Windows.Forms.Panel();
            this.pnl_Controls = new System.Windows.Forms.Panel();
            this.btn_Param_rect_set = new System.Windows.Forms.Button();
            this.pnl_Viewer = new SDOAQNet.Component.SdoPanel();
            this.gr_Control.SuspendLayout();
            this.gr_Acq.SuspendLayout();
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
            this.txt_Log.Location = new System.Drawing.Point(10, 591);
            this.txt_Log.Margin = new System.Windows.Forms.Padding(2);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.Size = new System.Drawing.Size(611, 167);
            this.txt_Log.TabIndex = 1;
            this.txt_Log.Text = "";
            this.txt_Log.WordWrap = false;
            // 
            // gr_Control
            // 
            this.gr_Control.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr_Control.Controls.Add(this.textBox1);
            this.gr_Control.Controls.Add(this.gr_Acq);
            this.gr_Control.Controls.Add(this.btn_StopMF);
            this.gr_Control.Controls.Add(this.btn_PlayMF);
            this.gr_Control.Controls.Add(this.label3);
            this.gr_Control.Controls.Add(this.txt_Param_focusStepOutside);
            this.gr_Control.Controls.Add(this.btn_Param_focusStepOutside);
            this.gr_Control.Location = new System.Drawing.Point(10, 9);
            this.gr_Control.Name = "gr_Control";
            this.gr_Control.Size = new System.Drawing.Size(611, 568);
            this.gr_Control.TabIndex = 0;
            this.gr_Control.TabStop = false;
            this.gr_Control.Text = "Control";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 22);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(593, 68);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // gr_Acq
            // 
            this.gr_Acq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr_Acq.Controls.Add(this.pnl_ListView);
            this.gr_Acq.Controls.Add(this.cmb_Param_func);
            this.gr_Acq.Controls.Add(this.label7);
            this.gr_Acq.Controls.Add(this.btn_Script_Reset);
            this.gr_Acq.Controls.Add(this.btn_Script_Remove);
            this.gr_Acq.Controls.Add(this.btn_Script_Modify);
            this.gr_Acq.Controls.Add(this.btn_Script_Add);
            this.gr_Acq.Controls.Add(this.label6);
            this.gr_Acq.Controls.Add(this.btn_Param_rect_set);
            this.gr_Acq.Controls.Add(this.btn_Param_rect_get);
            this.gr_Acq.Controls.Add(this.label5);
            this.gr_Acq.Controls.Add(this.label1);
            this.gr_Acq.Controls.Add(this.txt_Param_id);
            this.gr_Acq.Controls.Add(this.txt_Param_rect);
            this.gr_Acq.Controls.Add(this.txt_Param_focus);
            this.gr_Acq.Location = new System.Drawing.Point(6, 96);
            this.gr_Acq.Name = "gr_Acq";
            this.gr_Acq.Size = new System.Drawing.Size(599, 360);
            this.gr_Acq.TabIndex = 0;
            this.gr_Acq.TabStop = false;
            this.gr_Acq.Text = "Paramter";
            // 
            // pnl_ListView
            // 
            this.pnl_ListView.Location = new System.Drawing.Point(6, 22);
            this.pnl_ListView.Name = "pnl_ListView";
            this.pnl_ListView.Size = new System.Drawing.Size(587, 136);
            this.pnl_ListView.TabIndex = 0;
            // 
            // cmb_Param_func
            // 
            this.cmb_Param_func.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Param_func.FormattingEnabled = true;
            this.cmb_Param_func.Location = new System.Drawing.Point(310, 199);
            this.cmb_Param_func.Name = "cmb_Param_func";
            this.cmb_Param_func.Size = new System.Drawing.Size(123, 23);
            this.cmb_Param_func.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 269);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "Rect (left,top,width,height)";
            // 
            // btn_Script_Reset
            // 
            this.btn_Script_Reset.Location = new System.Drawing.Point(428, 308);
            this.btn_Script_Reset.Name = "btn_Script_Reset";
            this.btn_Script_Reset.Size = new System.Drawing.Size(96, 36);
            this.btn_Script_Reset.TabIndex = 16;
            this.btn_Script_Reset.Text = "Reset";
            this.btn_Script_Reset.UseVisualStyleBackColor = true;
            this.btn_Script_Reset.Click += new System.EventHandler(this.btn_Script_Reset_Click);
            // 
            // btn_Script_Remove
            // 
            this.btn_Script_Remove.Location = new System.Drawing.Point(313, 308);
            this.btn_Script_Remove.Name = "btn_Script_Remove";
            this.btn_Script_Remove.Size = new System.Drawing.Size(96, 36);
            this.btn_Script_Remove.TabIndex = 16;
            this.btn_Script_Remove.Text = "Remove";
            this.btn_Script_Remove.UseVisualStyleBackColor = true;
            this.btn_Script_Remove.Click += new System.EventHandler(this.btn_Script_Remove_Click);
            // 
            // btn_Script_Modify
            // 
            this.btn_Script_Modify.Location = new System.Drawing.Point(198, 308);
            this.btn_Script_Modify.Name = "btn_Script_Modify";
            this.btn_Script_Modify.Size = new System.Drawing.Size(96, 36);
            this.btn_Script_Modify.TabIndex = 16;
            this.btn_Script_Modify.Text = "Modify";
            this.btn_Script_Modify.UseVisualStyleBackColor = true;
            this.btn_Script_Modify.Click += new System.EventHandler(this.btn_Script_Modify_Click);
            // 
            // btn_Script_Add
            // 
            this.btn_Script_Add.Location = new System.Drawing.Point(83, 308);
            this.btn_Script_Add.Name = "btn_Script_Add";
            this.btn_Script_Add.Size = new System.Drawing.Size(96, 36);
            this.btn_Script_Add.TabIndex = 16;
            this.btn_Script_Add.Text = "Add";
            this.btn_Script_Add.UseVisualStyleBackColor = true;
            this.btn_Script_Add.Click += new System.EventHandler(this.btn_Script_Add_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 234);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(273, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Focus (focus step for fixed-focus roi)";
            // 
            // btn_Param_rect_get
            // 
            this.btn_Param_rect_get.Location = new System.Drawing.Point(518, 269);
            this.btn_Param_rect_get.Name = "btn_Param_rect_get";
            this.btn_Param_rect_get.Size = new System.Drawing.Size(63, 23);
            this.btn_Param_rect_get.TabIndex = 16;
            this.btn_Param_rect_get.Text = "Get";
            this.btn_Param_rect_get.UseVisualStyleBackColor = true;
            this.btn_Param_rect_get.Click += new System.EventHandler(this.btn_Param_rect_get_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(259, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Func (1: auto-focus, 2 :fixed focus)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID (unique number)";
            // 
            // txt_Param_id
            // 
            this.txt_Param_id.Location = new System.Drawing.Point(310, 164);
            this.txt_Param_id.Name = "txt_Param_id";
            this.txt_Param_id.Size = new System.Drawing.Size(123, 23);
            this.txt_Param_id.TabIndex = 7;
            this.txt_Param_id.Text = "0";
            // 
            // txt_Param_rect
            // 
            this.txt_Param_rect.Location = new System.Drawing.Point(310, 269);
            this.txt_Param_rect.Name = "txt_Param_rect";
            this.txt_Param_rect.Size = new System.Drawing.Size(123, 23);
            this.txt_Param_rect.TabIndex = 7;
            this.txt_Param_rect.Text = "0";
            // 
            // txt_Param_focus
            // 
            this.txt_Param_focus.Location = new System.Drawing.Point(310, 234);
            this.txt_Param_focus.Name = "txt_Param_focus";
            this.txt_Param_focus.Size = new System.Drawing.Size(123, 23);
            this.txt_Param_focus.TabIndex = 7;
            this.txt_Param_focus.Text = "0";
            // 
            // btn_StopMF
            // 
            this.btn_StopMF.Location = new System.Drawing.Point(316, 513);
            this.btn_StopMF.Name = "btn_StopMF";
            this.btn_StopMF.Size = new System.Drawing.Size(166, 36);
            this.btn_StopMF.TabIndex = 16;
            this.btn_StopMF.Text = "Stop MF";
            this.btn_StopMF.UseVisualStyleBackColor = true;
            this.btn_StopMF.Click += new System.EventHandler(this.btn_StopMF_Click);
            // 
            // btn_PlayMF
            // 
            this.btn_PlayMF.Location = new System.Drawing.Point(122, 513);
            this.btn_PlayMF.Name = "btn_PlayMF";
            this.btn_PlayMF.Size = new System.Drawing.Size(166, 36);
            this.btn_PlayMF.TabIndex = 16;
            this.btn_PlayMF.Text = "Play MF";
            this.btn_PlayMF.UseVisualStyleBackColor = true;
            this.btn_PlayMF.Click += new System.EventHandler(this.btn_PlayMF_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 475);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Focus Step outside of MF ROI";
            // 
            // txt_Param_focusStepOutside
            // 
            this.txt_Param_focusStepOutside.Location = new System.Drawing.Point(316, 475);
            this.txt_Param_focusStepOutside.Name = "txt_Param_focusStepOutside";
            this.txt_Param_focusStepOutside.Size = new System.Drawing.Size(123, 23);
            this.txt_Param_focusStepOutside.TabIndex = 8;
            this.txt_Param_focusStepOutside.Text = "160";
            // 
            // btn_Param_focusStepOutside
            // 
            this.btn_Param_focusStepOutside.Location = new System.Drawing.Point(445, 475);
            this.btn_Param_focusStepOutside.Name = "btn_Param_focusStepOutside";
            this.btn_Param_focusStepOutside.Size = new System.Drawing.Size(85, 23);
            this.btn_Param_focusStepOutside.TabIndex = 16;
            this.btn_Param_focusStepOutside.Text = "Set";
            this.btn_Param_focusStepOutside.UseVisualStyleBackColor = true;
            this.btn_Param_focusStepOutside.Click += new System.EventHandler(this.btn_Param_focusStepOutside_Click);
            // 
            // tmr_LogUpdate
            // 
            this.tmr_LogUpdate.Tick += new System.EventHandler(this.tmr_LogUpdate_Tick);
            // 
            // tlp_Main
            // 
            this.tlp_Main.ColumnCount = 2;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 630F));
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
            this.pnl_View.Location = new System.Drawing.Point(633, 3);
            this.pnl_View.Name = "pnl_View";
            this.pnl_View.Size = new System.Drawing.Size(593, 766);
            this.pnl_View.TabIndex = 1;
            // 
            // pnl_Controls
            // 
            this.pnl_Controls.Controls.Add(this.gr_Control);
            this.pnl_Controls.Controls.Add(this.txt_Log);
            this.pnl_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Controls.Location = new System.Drawing.Point(3, 3);
            this.pnl_Controls.Name = "pnl_Controls";
            this.pnl_Controls.Size = new System.Drawing.Size(624, 766);
            this.pnl_Controls.TabIndex = 0;
            // 
            // btn_Param_rect_set
            // 
            this.btn_Param_rect_set.Location = new System.Drawing.Point(444, 269);
            this.btn_Param_rect_set.Name = "btn_Param_rect_set";
            this.btn_Param_rect_set.Size = new System.Drawing.Size(63, 23);
            this.btn_Param_rect_set.TabIndex = 16;
            this.btn_Param_rect_set.Text = "Set";
            this.btn_Param_rect_set.UseVisualStyleBackColor = true;
            this.btn_Param_rect_set.Click += new System.EventHandler(this.btn_Param_rect_set_Click);
            // 
            // pnl_Viewer
            // 
            this.pnl_Viewer.BorderColor = System.Drawing.Color.Black;
            this.pnl_Viewer.BorderWidth = 1;
            this.pnl_Viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Viewer.Location = new System.Drawing.Point(0, 0);
            this.pnl_Viewer.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_Viewer.Name = "pnl_Viewer";
            this.pnl_Viewer.Size = new System.Drawing.Size(593, 766);
            this.pnl_Viewer.TabIndex = 2;
            // 
            // SdoaqMultiFocus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 772);
            this.Controls.Add(this.tlp_Main);
            this.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SdoaqMultiFocus";
            this.Text = "SDOAQ Multiple Lighting Sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SdoaqAutoFocus_FormClosed);
            this.Load += new System.EventHandler(this.SdoaqAutoFocus_Load);
            this.gr_Control.ResumeLayout(false);
            this.gr_Control.PerformLayout();
            this.gr_Acq.ResumeLayout(false);
            this.gr_Acq.PerformLayout();
            this.tlp_Main.ResumeLayout(false);
            this.pnl_View.ResumeLayout(false);
            this.pnl_Controls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gr_Control;
        private System.Windows.Forms.RichTextBox txt_Log;
        private System.Windows.Forms.TextBox txt_Param_focusStepOutside;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmr_LogUpdate;
        private SDOAQNet.Component.SdoPanel pnl_Viewer;
        private System.Windows.Forms.TableLayoutPanel tlp_Main;
        private System.Windows.Forms.Panel pnl_View;
        private System.Windows.Forms.Panel pnl_Controls;
        private System.Windows.Forms.GroupBox gr_Acq;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Param_rect_get;
        private System.Windows.Forms.TextBox txt_Param_id;
        private System.Windows.Forms.TextBox txt_Param_rect;
        private System.Windows.Forms.TextBox txt_Param_focus;
        private System.Windows.Forms.Button btn_Script_Modify;
        private System.Windows.Forms.Button btn_Script_Add;
        private System.Windows.Forms.Button btn_Script_Reset;
        private System.Windows.Forms.Button btn_Script_Remove;
        private System.Windows.Forms.Button btn_Param_focusStepOutside;
        private System.Windows.Forms.Button btn_PlayMF;
        private System.Windows.Forms.Button btn_StopMF;
        private System.Windows.Forms.ComboBox cmb_Param_func;
        private System.Windows.Forms.Panel pnl_ListView;
        private System.Windows.Forms.Button btn_Param_rect_set;
    }
}

