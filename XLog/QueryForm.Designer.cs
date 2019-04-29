namespace XLog
{
    partial class QueryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.lbPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnOpt = new System.Windows.Forms.Button();
            this.testDataSet = new XLog.testDataSet();
            this.testDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.testDataSet1 = new XLog.testDataSet();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvTmp = new System.Windows.Forms.DataGridView();
            this.rtbSqlEdit = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpResult = new System.Windows.Forms.TabPage();
            this.tpOutput = new System.Windows.Forms.TabPage();
            this.tpDbmsOutput = new System.Windows.Forms.TabPage();
            this.tpStat = new System.Windows.Forms.TabPage();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.rtbServerOutput = new System.Windows.Forms.RichTextBox();
            this.dgvStat = new System.Windows.Forms.DataGridView();
            this.tpPlan = new System.Windows.Forms.TabPage();
            this.statusStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTmp)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpResult.SuspendLayout();
            this.tpOutput.SuspendLayout();
            this.tpDbmsOutput.SuspendLayout();
            this.tpStat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).BeginInit();
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 714);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(778, 30);
            this.statusStrip1.TabIndex = 0;
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
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 24);
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(692, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(86, 714);
            this.flowLayoutPanel1.TabIndex = 3;
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
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(3, 147);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(80, 30);
            this.btnLast.TabIndex = 0;
            this.btnLast.Text = "To Last";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(3, 183);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(80, 30);
            this.btnFirst.TabIndex = 5;
            this.btnFirst.Text = "To First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(3, 219);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 30);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "To Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(3, 255);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(80, 30);
            this.btnPrev.TabIndex = 6;
            this.btnPrev.Text = "To Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
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
            // testDataSet
            // 
            this.testDataSet.DataSetName = "testDataSet";
            this.testDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // testDataSetBindingSource
            // 
            this.testDataSetBindingSource.DataSource = this.testDataSet;
            this.testDataSetBindingSource.Position = 0;
            // 
            // testDataSet1
            // 
            this.testDataSet1.DataSetName = "testDataSet";
            this.testDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbSqlEdit);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Controls.Add(this.dgvTmp);
            this.splitContainer1.Size = new System.Drawing.Size(637, 711);
            this.splitContainer1.SplitterDistance = 355;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgvTmp
            // 
            this.dgvTmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTmp.Location = new System.Drawing.Point(7, 267);
            this.dgvTmp.Name = "dgvTmp";
            this.dgvTmp.RowTemplate.Height = 30;
            this.dgvTmp.Size = new System.Drawing.Size(40, 40);
            this.dgvTmp.TabIndex = 3;
            this.dgvTmp.Visible = false;
            // 
            // rtbSqlEdit
            // 
            this.rtbSqlEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSqlEdit.Location = new System.Drawing.Point(0, 0);
            this.rtbSqlEdit.Name = "rtbSqlEdit";
            this.rtbSqlEdit.Size = new System.Drawing.Size(635, 353);
            this.rtbSqlEdit.TabIndex = 1;
            this.rtbSqlEdit.Text = "11";
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
            this.tpResult.Controls.Add(this.dgvResult);
            this.tpResult.Location = new System.Drawing.Point(4, 31);
            this.tpResult.Name = "tpResult";
            this.tpResult.Padding = new System.Windows.Forms.Padding(3);
            this.tpResult.Size = new System.Drawing.Size(545, 162);
            this.tpResult.TabIndex = 0;
            this.tpResult.Text = "Result";
            this.tpResult.UseVisualStyleBackColor = true;
            // 
            // tpOutput
            // 
            this.tpOutput.Controls.Add(this.rtbOutput);
            this.tpOutput.Location = new System.Drawing.Point(4, 31);
            this.tpOutput.Name = "tpOutput";
            this.tpOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutput.Size = new System.Drawing.Size(545, 162);
            this.tpOutput.TabIndex = 1;
            this.tpOutput.Text = "Output";
            this.tpOutput.UseVisualStyleBackColor = true;
            // 
            // tpDbmsOutput
            // 
            this.tpDbmsOutput.Controls.Add(this.rtbServerOutput);
            this.tpDbmsOutput.Location = new System.Drawing.Point(4, 31);
            this.tpDbmsOutput.Name = "tpDbmsOutput";
            this.tpDbmsOutput.Size = new System.Drawing.Size(545, 162);
            this.tpDbmsOutput.TabIndex = 2;
            this.tpDbmsOutput.Text = "DBMS Output";
            this.tpDbmsOutput.UseVisualStyleBackColor = true;
            // 
            // tpStat
            // 
            this.tpStat.Controls.Add(this.dgvStat);
            this.tpStat.Location = new System.Drawing.Point(4, 31);
            this.tpStat.Name = "tpStat";
            this.tpStat.Size = new System.Drawing.Size(545, 162);
            this.tpStat.TabIndex = 3;
            this.tpStat.Text = "Statistics";
            this.tpStat.UseVisualStyleBackColor = true;
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(3, 3);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.RowTemplate.Height = 30;
            this.dgvResult.Size = new System.Drawing.Size(539, 156);
            this.dgvResult.TabIndex = 5;
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutput.Location = new System.Drawing.Point(3, 3);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(539, 156);
            this.rtbOutput.TabIndex = 2;
            this.rtbOutput.Text = "";
            // 
            // rtbServerOutput
            // 
            this.rtbServerOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbServerOutput.Location = new System.Drawing.Point(0, 0);
            this.rtbServerOutput.Name = "rtbServerOutput";
            this.rtbServerOutput.Size = new System.Drawing.Size(545, 162);
            this.rtbServerOutput.TabIndex = 3;
            this.rtbServerOutput.Text = "";
            // 
            // dgvStat
            // 
            this.dgvStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStat.Location = new System.Drawing.Point(0, 0);
            this.dgvStat.Name = "dgvStat";
            this.dgvStat.RowTemplate.Height = 30;
            this.dgvStat.Size = new System.Drawing.Size(545, 162);
            this.dgvStat.TabIndex = 6;
            // 
            // tpPlan
            // 
            this.tpPlan.Location = new System.Drawing.Point(4, 31);
            this.tpPlan.Name = "tpPlan";
            this.tpPlan.Size = new System.Drawing.Size(545, 162);
            this.tpPlan.TabIndex = 4;
            this.tpPlan.Text = "Explain Plan";
            this.tpPlan.UseVisualStyleBackColor = true;
            // 
            // QueryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(778, 744);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "QueryForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.QueryForm_Load);
            this.Resize += new System.EventHandler(this.QueryForm_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testDataSet1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTmp)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpResult.ResumeLayout(false);
            this.tpOutput.ResumeLayout(false);
            this.tpDbmsOutput.ResumeLayout(false);
            this.tpStat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.BindingSource testDataSetBindingSource;
        private testDataSet testDataSet;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnClose;
        private testDataSet testDataSet1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnOpt;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel lbPos;
        private System.Windows.Forms.ToolStripStatusLabel lbTime;
        private System.Windows.Forms.ToolStripStatusLabel lbRow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvTmp;
        private System.Windows.Forms.RichTextBox rtbSqlEdit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpResult;
        private System.Windows.Forms.TabPage tpOutput;
        private System.Windows.Forms.TabPage tpDbmsOutput;
        private System.Windows.Forms.TabPage tpStat;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.RichTextBox rtbServerOutput;
        private System.Windows.Forms.DataGridView dgvStat;
        private System.Windows.Forms.TabPage tpPlan;
    }
}