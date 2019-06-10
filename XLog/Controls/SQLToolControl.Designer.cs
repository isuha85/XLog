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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.dgvBind = new System.Windows.Forms.DataGridView();
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.tb = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tpPlanText = new System.Windows.Forms.TabPage();
			this.tbPlan = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tpPlan = new System.Windows.Forms.TabPage();
			this.tpResult = new System.Windows.Forms.TabPage();
			this.dataGridView = new System.Windows.Forms.DataGridView();
			this.tpOutput = new System.Windows.Forms.TabPage();
			this.rtbOutput = new System.Windows.Forms.RichTextBox();
			this.tpDbmsOutput = new System.Windows.Forms.TabPage();
			this.rtbServerOutput = new System.Windows.Forms.RichTextBox();
			this.tpStat = new System.Windows.Forms.TabPage();
			this.dgvStat = new System.Windows.Forms.DataGridView();
			this.btnOpt = new System.Windows.Forms.Button();
			this.btnPrev = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.btnLast = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnOpen = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.statusStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvBind)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tb)).BeginInit();
			this.tabControl.SuspendLayout();
			this.tpPlanText.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbPlan)).BeginInit();
			this.tpResult.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
			this.tpOutput.SuspendLayout();
			this.tpDbmsOutput.SuspendLayout();
			this.tpStat.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvStat)).BeginInit();
			this.flowLayoutPanel.SuspendLayout();
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
			this.statusStrip1.Location = new System.Drawing.Point(0, 720);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
			this.statusStrip1.Size = new System.Drawing.Size(1000, 30);
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
			this.toolStripProgressBar1.Size = new System.Drawing.Size(32, 24);
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
			this.panel1.Controls.Add(this.splitContainer);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(932, 720);
			this.panel1.TabIndex = 6;
			// 
			// splitContainer
			// 
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.dgvBind);
			this.splitContainer.Panel1.Controls.Add(this.dockPanel);
			this.splitContainer.Panel1.Controls.Add(this.tb);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.tabControl);
			this.splitContainer.Size = new System.Drawing.Size(932, 720);
			this.splitContainer.SplitterDistance = 347;
			this.splitContainer.SplitterWidth = 3;
			this.splitContainer.TabIndex = 6;
			this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer_SplitterMoved);
			this.splitContainer.Resize += new System.EventHandler(this.splitContainer_Resize);
			// 
			// dgvBind
			// 
			this.dgvBind.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgvBind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvBind.Location = new System.Drawing.Point(275, 22);
			this.dgvBind.Margin = new System.Windows.Forms.Padding(2);
			this.dgvBind.Name = "dgvBind";
			this.dgvBind.RowTemplate.Height = 30;
			this.dgvBind.Size = new System.Drawing.Size(465, 125);
			this.dgvBind.TabIndex = 6;
			this.dgvBind.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBind_CellClick);
			// 
			// dockPanel
			// 
			this.dockPanel.BackColor = System.Drawing.SystemColors.ControlLight;
			this.dockPanel.Location = new System.Drawing.Point(6, 92);
			this.dockPanel.Margin = new System.Windows.Forms.Padding(2);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new System.Drawing.Size(250, 183);
			this.dockPanel.TabIndex = 4;
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
			this.tb.AutoScrollMinSize = new System.Drawing.Size(51, 18);
			this.tb.BackBrush = null;
			this.tb.CharHeight = 18;
			this.tb.CharWidth = 10;
			this.tb.CommentPrefix = "--";
			this.tb.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tb.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tb.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.tb.IsReplaceMode = false;
			this.tb.Language = FastColoredTextBoxNS.Language.SQL;
			this.tb.LeftBracket = '(';
			this.tb.Location = new System.Drawing.Point(6, 22);
			this.tb.Margin = new System.Windows.Forms.Padding(2);
			this.tb.Name = "tb";
			this.tb.Paddings = new System.Windows.Forms.Padding(0);
			this.tb.RightBracket = ')';
			this.tb.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tb.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tb.ServiceColors")));
			this.tb.Size = new System.Drawing.Size(250, 65);
			this.tb.TabIndex = 3;
			this.tb.Text = "tb";
			this.tb.Zoom = 100;
			this.tb.SelectionChangedDelayed += new System.EventHandler(this.tb_SelectionChangedDelayed);
			this.tb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tb_MouseDoubleClick);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tpPlanText);
			this.tabControl.Controls.Add(this.tpPlan);
			this.tabControl.Controls.Add(this.tpResult);
			this.tabControl.Controls.Add(this.tpOutput);
			this.tabControl.Controls.Add(this.tpDbmsOutput);
			this.tabControl.Controls.Add(this.tpStat);
			this.tabControl.Location = new System.Drawing.Point(2, 2);
			this.tabControl.Margin = new System.Windows.Forms.Padding(2);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(442, 164);
			this.tabControl.TabIndex = 5;
			// 
			// tpPlanText
			// 
			this.tpPlanText.Controls.Add(this.tbPlan);
			this.tpPlanText.Location = new System.Drawing.Point(4, 25);
			this.tpPlanText.Margin = new System.Windows.Forms.Padding(2);
			this.tpPlanText.Name = "tpPlanText";
			this.tpPlanText.Size = new System.Drawing.Size(434, 135);
			this.tpPlanText.TabIndex = 4;
			this.tpPlanText.Text = "Plan (Text)";
			this.tpPlanText.UseVisualStyleBackColor = true;
			// 
			// tbPlan
			// 
			this.tbPlan.AutoCompleteBracketsList = new char[] {
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
			this.tbPlan.AutoIndentCharsPatterns = "";
			this.tbPlan.AutoScrollMinSize = new System.Drawing.Size(91, 18);
			this.tbPlan.BackBrush = null;
			this.tbPlan.CharHeight = 18;
			this.tbPlan.CharWidth = 10;
			this.tbPlan.CommentPrefix = "--";
			this.tbPlan.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbPlan.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tbPlan.IsReplaceMode = false;
			this.tbPlan.Language = FastColoredTextBoxNS.Language.SQL;
			this.tbPlan.LeftBracket = '(';
			this.tbPlan.Location = new System.Drawing.Point(93, 36);
			this.tbPlan.Margin = new System.Windows.Forms.Padding(2);
			this.tbPlan.Name = "tbPlan";
			this.tbPlan.Paddings = new System.Windows.Forms.Padding(0);
			this.tbPlan.RightBracket = ')';
			this.tbPlan.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tbPlan.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tbPlan.ServiceColors")));
			this.tbPlan.Size = new System.Drawing.Size(250, 65);
			this.tbPlan.TabIndex = 4;
			this.tbPlan.Text = "tbPlan";
			this.tbPlan.Zoom = 100;
			// 
			// tpPlan
			// 
			this.tpPlan.Location = new System.Drawing.Point(4, 25);
			this.tpPlan.Margin = new System.Windows.Forms.Padding(2);
			this.tpPlan.Name = "tpPlan";
			this.tpPlan.Size = new System.Drawing.Size(434, 135);
			this.tpPlan.TabIndex = 5;
			this.tpPlan.Text = "Plan";
			this.tpPlan.UseVisualStyleBackColor = true;
			// 
			// tpResult
			// 
			this.tpResult.Controls.Add(this.dataGridView);
			this.tpResult.Location = new System.Drawing.Point(4, 25);
			this.tpResult.Margin = new System.Windows.Forms.Padding(2);
			this.tpResult.Name = "tpResult";
			this.tpResult.Padding = new System.Windows.Forms.Padding(2);
			this.tpResult.Size = new System.Drawing.Size(434, 135);
			this.tpResult.TabIndex = 0;
			this.tpResult.Text = "Result";
			this.tpResult.UseVisualStyleBackColor = true;
			// 
			// dataGridView
			// 
			this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView.Location = new System.Drawing.Point(2, 2);
			this.dataGridView.Margin = new System.Windows.Forms.Padding(2);
			this.dataGridView.Name = "dataGridView";
			this.dataGridView.RowTemplate.Height = 30;
			this.dataGridView.Size = new System.Drawing.Size(430, 131);
			this.dataGridView.TabIndex = 5;
			this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
			// 
			// tpOutput
			// 
			this.tpOutput.Controls.Add(this.rtbOutput);
			this.tpOutput.Location = new System.Drawing.Point(4, 25);
			this.tpOutput.Margin = new System.Windows.Forms.Padding(2);
			this.tpOutput.Name = "tpOutput";
			this.tpOutput.Padding = new System.Windows.Forms.Padding(2);
			this.tpOutput.Size = new System.Drawing.Size(434, 135);
			this.tpOutput.TabIndex = 1;
			this.tpOutput.Text = "Output";
			this.tpOutput.UseVisualStyleBackColor = true;
			// 
			// rtbOutput
			// 
			this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbOutput.Location = new System.Drawing.Point(2, 2);
			this.rtbOutput.Margin = new System.Windows.Forms.Padding(2);
			this.rtbOutput.Name = "rtbOutput";
			this.rtbOutput.Size = new System.Drawing.Size(430, 131);
			this.rtbOutput.TabIndex = 2;
			this.rtbOutput.Text = "";
			// 
			// tpDbmsOutput
			// 
			this.tpDbmsOutput.Controls.Add(this.rtbServerOutput);
			this.tpDbmsOutput.Location = new System.Drawing.Point(4, 25);
			this.tpDbmsOutput.Margin = new System.Windows.Forms.Padding(2);
			this.tpDbmsOutput.Name = "tpDbmsOutput";
			this.tpDbmsOutput.Size = new System.Drawing.Size(434, 135);
			this.tpDbmsOutput.TabIndex = 2;
			this.tpDbmsOutput.Text = "DBMS Output";
			this.tpDbmsOutput.UseVisualStyleBackColor = true;
			// 
			// rtbServerOutput
			// 
			this.rtbServerOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbServerOutput.Location = new System.Drawing.Point(0, 0);
			this.rtbServerOutput.Margin = new System.Windows.Forms.Padding(2);
			this.rtbServerOutput.Name = "rtbServerOutput";
			this.rtbServerOutput.Size = new System.Drawing.Size(434, 135);
			this.rtbServerOutput.TabIndex = 3;
			this.rtbServerOutput.Text = "";
			// 
			// tpStat
			// 
			this.tpStat.Controls.Add(this.dgvStat);
			this.tpStat.Location = new System.Drawing.Point(4, 25);
			this.tpStat.Margin = new System.Windows.Forms.Padding(2);
			this.tpStat.Name = "tpStat";
			this.tpStat.Size = new System.Drawing.Size(434, 135);
			this.tpStat.TabIndex = 3;
			this.tpStat.Text = "Statistics";
			this.tpStat.UseVisualStyleBackColor = true;
			// 
			// dgvStat
			// 
			this.dgvStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvStat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvStat.Location = new System.Drawing.Point(0, 0);
			this.dgvStat.Margin = new System.Windows.Forms.Padding(2);
			this.dgvStat.Name = "dgvStat";
			this.dgvStat.RowTemplate.Height = 30;
			this.dgvStat.Size = new System.Drawing.Size(434, 135);
			this.dgvStat.TabIndex = 6;
			// 
			// btnOpt
			// 
			this.btnOpt.Location = new System.Drawing.Point(2, 234);
			this.btnOpt.Margin = new System.Windows.Forms.Padding(2);
			this.btnOpt.Name = "btnOpt";
			this.btnOpt.Size = new System.Drawing.Size(64, 25);
			this.btnOpt.TabIndex = 7;
			this.btnOpt.Text = "OPT";
			this.btnOpt.UseVisualStyleBackColor = true;
			this.btnOpt.Click += new System.EventHandler(this.btnOpt_Click);
			// 
			// btnPrev
			// 
			this.btnPrev.Location = new System.Drawing.Point(2, 205);
			this.btnPrev.Margin = new System.Windows.Forms.Padding(2);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.Size = new System.Drawing.Size(64, 25);
			this.btnPrev.TabIndex = 6;
			this.btnPrev.Text = "Prev";
			this.btnPrev.UseVisualStyleBackColor = true;
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(2, 176);
			this.btnNext.Margin = new System.Windows.Forms.Padding(2);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(64, 25);
			this.btnNext.TabIndex = 4;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.Location = new System.Drawing.Point(2, 147);
			this.btnFirst.Margin = new System.Windows.Forms.Padding(2);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(64, 25);
			this.btnFirst.TabIndex = 5;
			this.btnFirst.Text = "First";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// btnLast
			// 
			this.btnLast.Location = new System.Drawing.Point(2, 118);
			this.btnLast.Margin = new System.Windows.Forms.Padding(2);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(64, 25);
			this.btnLast.TabIndex = 0;
			this.btnLast.Text = "Last";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(2, 89);
			this.btnClose.Margin = new System.Windows.Forms.Padding(2);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(64, 25);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(2, 60);
			this.btnOpen.Margin = new System.Windows.Forms.Padding(2);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(64, 25);
			this.btnOpen.TabIndex = 2;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(2, 31);
			this.btnStop.Margin = new System.Windows.Forms.Padding(2);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(64, 25);
			this.btnStop.TabIndex = 8;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(2, 2);
			this.btnGo.Margin = new System.Windows.Forms.Padding(2);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(64, 25);
			this.btnGo.TabIndex = 1;
			this.btnGo.Text = "GO";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.AutoSize = true;
			this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel.Controls.Add(this.btnGo);
			this.flowLayoutPanel.Controls.Add(this.btnStop);
			this.flowLayoutPanel.Controls.Add(this.btnOpen);
			this.flowLayoutPanel.Controls.Add(this.btnClose);
			this.flowLayoutPanel.Controls.Add(this.btnLast);
			this.flowLayoutPanel.Controls.Add(this.btnFirst);
			this.flowLayoutPanel.Controls.Add(this.btnNext);
			this.flowLayoutPanel.Controls.Add(this.btnPrev);
			this.flowLayoutPanel.Controls.Add(this.btnOpt);
			this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel.Location = new System.Drawing.Point(932, 0);
			this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(68, 720);
			this.flowLayoutPanel.TabIndex = 4;
			// 
			// SQLToolControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.flowLayoutPanel);
			this.Controls.Add(this.statusStrip1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "SQLToolControl";
			this.Size = new System.Drawing.Size(1000, 750);
			this.Load += new System.EventHandler(this.SQLToolUserControl_Load);
			this.Resize += new System.EventHandler(this.SQLToolControl_Resize);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvBind)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tb)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tpPlanText.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbPlan)).EndInit();
			this.tpResult.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
			this.tpOutput.ResumeLayout(false);
			this.tpDbmsOutput.ResumeLayout(false);
			this.tpStat.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvStat)).EndInit();
			this.flowLayoutPanel.ResumeLayout(false);
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
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpResult;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TabPage tpOutput;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.TabPage tpDbmsOutput;
        private System.Windows.Forms.RichTextBox rtbServerOutput;
        private System.Windows.Forms.TabPage tpStat;
        private System.Windows.Forms.DataGridView dgvStat;
        private System.Windows.Forms.TabPage tpPlanText;
        private System.Windows.Forms.Button btnOpt;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private FastColoredTextBoxNS.FastColoredTextBox tb;
		private System.Windows.Forms.TabPage tpPlan;
		private FastColoredTextBoxNS.FastColoredTextBox tbPlan;
		private System.Windows.Forms.DataGridView dgvBind;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
	}
}
