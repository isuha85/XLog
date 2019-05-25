namespace XLog
{
    partial class SQLToolControl
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLToolControl));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.lbPos = new System.Windows.Forms.ToolStripStatusLabel();
			this.lbTime = new System.Windows.Forms.ToolStripStatusLabel();
			this.lbRow = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tb = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpResult = new System.Windows.Forms.TabPage();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.tpOutput = new System.Windows.Forms.TabPage();
			this.rtbOutput = new System.Windows.Forms.RichTextBox();
			this.tpDbmsOutput = new System.Windows.Forms.TabPage();
			this.rtbServerOutput = new System.Windows.Forms.RichTextBox();
			this.tpStat = new System.Windows.Forms.TabPage();
			this.dgvStat = new System.Windows.Forms.DataGridView();
			this.tpPlan = new System.Windows.Forms.TabPage();
			this.dgvTmp = new System.Windows.Forms.DataGridView();
			this.btnOpt = new System.Windows.Forms.Button();
			this.btnPrev = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.btnLast = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnOpen = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.statusStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tb)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tpResult.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.tpOutput.SuspendLayout();
			this.tpDbmsOutput.SuspendLayout();
			this.tpStat.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvStat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvTmp)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel4,
            this.toolStripProgressBar1,
            this.lbPos,
            this.lbTime,
            this.lbRow});
			this.statusStrip1.Location = new System.Drawing.Point(0, 770);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(800, 30);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel4
			// 
			this.toolStripStatusLabel4.AutoSize = false;
			this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
			this.toolStripStatusLabel4.Size = new System.Drawing.Size(100, 25);
			this.toolStripStatusLabel4.Text = "Running ..";
			this.toolStripStatusLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(40, 24);
			// 
			// lbPos
			// 
			this.lbPos.AutoSize = false;
			this.lbPos.Name = "lbPos";
			this.lbPos.Size = new System.Drawing.Size(150, 25);
			this.lbPos.Text = "Ln X, Col Y";
			this.lbPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbTime
			// 
			this.lbTime.AutoSize = false;
			this.lbTime.Name = "lbTime";
			this.lbTime.Size = new System.Drawing.Size(150, 25);
			this.lbTime.Text = "0.00 sec";
			this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbRow
			// 
			this.lbRow.AutoSize = false;
			this.lbRow.Name = "lbRow";
			this.lbRow.Size = new System.Drawing.Size(100, 25);
			this.lbRow.Text = "0 rows";
			this.lbRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(714, 770);
			this.panel1.TabIndex = 6;
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tb);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer1.Panel2.Controls.Add(this.dgvTmp);
			this.splitContainer1.Size = new System.Drawing.Size(714, 770);
			this.splitContainer1.SplitterDistance = 200;
			this.splitContainer1.TabIndex = 6;
			// 
			// tb
			// 
			this.tb.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
			this.tb.AutoIndentCharsPatterns = "";
			this.tb.AutoScrollMinSize = new System.Drawing.Size(263, 22);
			this.tb.BackBrush = null;
			this.tb.CharHeight = 22;
			this.tb.CharWidth = 12;
			this.tb.CommentPrefix = "--";
			this.tb.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tb.IsReplaceMode = false;
			this.tb.Language = FastColoredTextBoxNS.Language.SQL;
			this.tb.LeftBracket = '(';
			this.tb.Location = new System.Drawing.Point(0, 0);
			this.tb.Name = "tb";
			this.tb.Paddings = new System.Windows.Forms.Padding(0);
			this.tb.RightBracket = ')';
			this.tb.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tb.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tb.ServiceColors")));
			this.tb.Size = new System.Drawing.Size(150, 150);
			this.tb.TabIndex = 3;
			this.tb.Text = "fastColoredTextBox2";
			this.tb.Zoom = 100;
			this.tb.SelectionChangedDelayed += new System.EventHandler(this.tb_SelectionChangedDelayed);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpResult);
			this.tabControl1.Controls.Add(this.tpOutput);
			this.tabControl1.Controls.Add(this.tpDbmsOutput);
			this.tabControl1.Controls.Add(this.tpStat);
			this.tabControl1.Controls.Add(this.tpPlan);
			this.tabControl1.Location = new System.Drawing.Point(3, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(553, 197);
			this.tabControl1.TabIndex = 5;
			// 
			// tpResult
			// 
			this.tpResult.Controls.Add(this.dataGridView);
			this.tpResult.Location = new System.Drawing.Point(4, 28);
			this.tpResult.Name = "tpResult";
			this.tpResult.Padding = new System.Windows.Forms.Padding(3);
			this.tpResult.Size = new System.Drawing.Size(545, 165);
			this.tpResult.TabIndex = 0;
			this.tpResult.Text = "Result";
			this.tpResult.UseVisualStyleBackColor = true;
			// 
			// dataGridView
			// 
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView.Location = new System.Drawing.Point(3, 3);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowTemplate.Height = 30;
			this.dataGridView.Size = new System.Drawing.Size(539, 159);
			this.dataGridView.TabIndex = 5;
			// 
			// tpOutput
			// 
			this.tpOutput.Controls.Add(this.rtbOutput);
			this.tpOutput.Location = new System.Drawing.Point(4, 28);
			this.tpOutput.Name = "tpOutput";
			this.tpOutput.Padding = new System.Windows.Forms.Padding(3);
			this.tpOutput.Size = new System.Drawing.Size(545, 165);
			this.tpOutput.TabIndex = 1;
			this.tpOutput.Text = "Output";
			this.tpOutput.UseVisualStyleBackColor = true;
			// 
			// rtbOutput
			// 
			this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbOutput.Location = new System.Drawing.Point(3, 3);
			this.rtbOutput.Name = "rtbOutput";
			this.rtbOutput.Size = new System.Drawing.Size(539, 159);
			this.rtbOutput.TabIndex = 2;
			this.rtbOutput.Text = "";
			// 
			// tpDbmsOutput
			// 
			this.tpDbmsOutput.Controls.Add(this.rtbServerOutput);
			this.tpDbmsOutput.Location = new System.Drawing.Point(4, 28);
			this.tpDbmsOutput.Name = "tpDbmsOutput";
			this.tpDbmsOutput.Size = new System.Drawing.Size(545, 165);
			this.tpDbmsOutput.TabIndex = 2;
			this.tpDbmsOutput.Text = "DBMS Output";
			this.tpDbmsOutput.UseVisualStyleBackColor = true;
			// 
			// rtbServerOutput
			// 
			this.rtbServerOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbServerOutput.Location = new System.Drawing.Point(0, 0);
			this.rtbServerOutput.Name = "rtbServerOutput";
			this.rtbServerOutput.Size = new System.Drawing.Size(545, 165);
			this.rtbServerOutput.TabIndex = 3;
			this.rtbServerOutput.Text = "";
			// 
			// tpStat
			// 
			this.tpStat.Controls.Add(this.dgvStat);
			this.tpStat.Location = new System.Drawing.Point(4, 28);
			this.tpStat.Name = "tpStat";
			this.tpStat.Size = new System.Drawing.Size(545, 165);
			this.tpStat.TabIndex = 3;
			this.tpStat.Text = "Statistics";
			this.tpStat.UseVisualStyleBackColor = true;
			// 
			// dgvStat
			// 
			this.dgvStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvStat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvStat.Location = new System.Drawing.Point(0, 0);
			this.dgvStat.Name = "dgvStat";
			this.dgvStat.RowTemplate.Height = 30;
			this.dgvStat.Size = new System.Drawing.Size(545, 165);
			this.dgvStat.TabIndex = 6;
			// 
			// tpPlan
			// 
			this.tpPlan.Location = new System.Drawing.Point(4, 28);
			this.tpPlan.Name = "tpPlan";
			this.tpPlan.Size = new System.Drawing.Size(545, 165);
			this.tpPlan.TabIndex = 4;
			this.tpPlan.Text = "Explain Plan";
			this.tpPlan.UseVisualStyleBackColor = true;
			// 
			// dgvTmp
			// 
			this.dgvTmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvTmp.Location = new System.Drawing.Point(10, 206);
			this.dgvTmp.Name = "dgvTmp";
			this.dgvTmp.RowTemplate.Height = 30;
			this.dgvTmp.Size = new System.Drawing.Size(40, 40);
			this.dgvTmp.TabIndex = 3;
			this.dgvTmp.Visible = false;
			// 
			// btnOpt
			// 
			this.btnOpt.Location = new System.Drawing.Point(3, 291);
			this.btnOpt.Name = "btnOpt";
			this.btnOpt.Size = new System.Drawing.Size(80, 30);
			this.btnOpt.TabIndex = 7;
			this.btnOpt.Text = "OPT";
			this.btnOpt.UseVisualStyleBackColor = true;
			this.btnOpt.Click += new System.EventHandler(this.btnOpt_Click);
			// 
			// btnPrev
			// 
			this.btnPrev.Location = new System.Drawing.Point(3, 255);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.Size = new System.Drawing.Size(80, 30);
			this.btnPrev.TabIndex = 6;
			this.btnPrev.Text = "Prev";
			this.btnPrev.UseVisualStyleBackColor = true;
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(3, 219);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(80, 30);
			this.btnNext.TabIndex = 4;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.Location = new System.Drawing.Point(3, 183);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(80, 30);
			this.btnFirst.TabIndex = 5;
			this.btnFirst.Text = "First";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// btnLast
			// 
			this.btnLast.Location = new System.Drawing.Point(3, 147);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(80, 30);
			this.btnLast.TabIndex = 0;
			this.btnLast.Text = "Last";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(3, 111);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(80, 30);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(3, 75);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(80, 30);
			this.btnOpen.TabIndex = 2;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(3, 39);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(80, 30);
			this.btnStop.TabIndex = 8;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(3, 3);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(80, 30);
			this.btnGo.TabIndex = 1;
			this.btnGo.Text = "GO";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.btnGo);
			this.flowLayoutPanel1.Controls.Add(this.btnStop);
			this.flowLayoutPanel1.Controls.Add(this.btnOpen);
			this.flowLayoutPanel1.Controls.Add(this.btnClose);
			this.flowLayoutPanel1.Controls.Add(this.btnLast);
			this.flowLayoutPanel1.Controls.Add(this.btnFirst);
			this.flowLayoutPanel1.Controls.Add(this.btnNext);
			this.flowLayoutPanel1.Controls.Add(this.btnPrev);
			this.flowLayoutPanel1.Controls.Add(this.btnOpt);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(714, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(86, 770);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// SQLToolControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.statusStrip1);
			this.Name = "SQLToolControl";
			this.Size = new System.Drawing.Size(800, 800);
			this.Load += new System.EventHandler(this.SQLToolUserControl_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tb)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tpResult.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.tpOutput.ResumeLayout(false);
			this.tpDbmsOutput.ResumeLayout(false);
			this.tpStat.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvStat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvTmp)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel lbPos;
        private System.Windows.Forms.ToolStripStatusLabel lbTime;
        private System.Windows.Forms.ToolStripStatusLabel lbRow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpResult;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TabPage tpOutput;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.TabPage tpDbmsOutput;
        private System.Windows.Forms.RichTextBox rtbServerOutput;
        private System.Windows.Forms.TabPage tpStat;
        private System.Windows.Forms.DataGridView dgvStat;
        private System.Windows.Forms.TabPage tpPlan;
        private System.Windows.Forms.DataGridView dgvTmp;
        private System.Windows.Forms.Button btnOpt;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private FastColoredTextBoxNS.FastColoredTextBox tb;
	}
}
