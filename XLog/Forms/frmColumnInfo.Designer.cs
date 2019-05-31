namespace XLog
{
	partial class frmColumnInfo
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel1T = new System.Windows.Forms.FlowLayoutPanel();
			this.lbAlias = new System.Windows.Forms.Label();
			this.tbAlias = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1B = new System.Windows.Forms.FlowLayoutPanel();
			this.cbInsertComma = new System.Windows.Forms.CheckBox();
			this.cbLowerCase = new System.Windows.Forms.CheckBox();
			this.cbShowPK = new System.Windows.Forms.CheckBox();
			this.cbShowComment = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dataGrdiView = new System.Windows.Forms.DataGridView();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1T.SuspendLayout();
			this.flowLayoutPanel1B.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrdiView)).BeginInit();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel1T);
			this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel1B);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 76);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// flowLayoutPanel1T
			// 
			this.flowLayoutPanel1T.Controls.Add(this.lbAlias);
			this.flowLayoutPanel1T.Controls.Add(this.tbAlias);
			this.flowLayoutPanel1T.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel1T.Name = "flowLayoutPanel1T";
			this.flowLayoutPanel1T.Size = new System.Drawing.Size(640, 33);
			this.flowLayoutPanel1T.TabIndex = 7;
			// 
			// lbAlias
			// 
			this.lbAlias.AutoSize = true;
			this.lbAlias.Location = new System.Drawing.Point(3, 10);
			this.lbAlias.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.lbAlias.Name = "lbAlias";
			this.lbAlias.Size = new System.Drawing.Size(95, 18);
			this.lbAlias.TabIndex = 1;
			this.lbAlias.Text = "Table Alias";
			// 
			// tbAlias
			// 
			this.tbAlias.Location = new System.Drawing.Point(104, 3);
			this.tbAlias.Name = "tbAlias";
			this.tbAlias.Size = new System.Drawing.Size(250, 28);
			this.tbAlias.TabIndex = 2;
			// 
			// flowLayoutPanel1B
			// 
			this.flowLayoutPanel1B.Controls.Add(this.cbInsertComma);
			this.flowLayoutPanel1B.Controls.Add(this.cbLowerCase);
			this.flowLayoutPanel1B.Controls.Add(this.cbShowPK);
			this.flowLayoutPanel1B.Controls.Add(this.cbShowComment);
			this.flowLayoutPanel1B.Location = new System.Drawing.Point(3, 42);
			this.flowLayoutPanel1B.Name = "flowLayoutPanel1B";
			this.flowLayoutPanel1B.Size = new System.Drawing.Size(640, 30);
			this.flowLayoutPanel1B.TabIndex = 8;
			// 
			// cbInsertComma
			// 
			this.cbInsertComma.AutoSize = true;
			this.cbInsertComma.Location = new System.Drawing.Point(6, 3);
			this.cbInsertComma.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
			this.cbInsertComma.Name = "cbInsertComma";
			this.cbInsertComma.Size = new System.Drawing.Size(147, 22);
			this.cbInsertComma.TabIndex = 4;
			this.cbInsertComma.Text = "Insert Comma";
			this.cbInsertComma.UseVisualStyleBackColor = true;
			this.cbInsertComma.CheckedChanged += new System.EventHandler(this.cbInsertComma_CheckedChanged);
			this.cbInsertComma.CheckStateChanged += new System.EventHandler(this.cbInsertComma_CheckStateChanged);
			// 
			// cbLowerCase
			// 
			this.cbLowerCase.AutoSize = true;
			this.cbLowerCase.Location = new System.Drawing.Point(159, 3);
			this.cbLowerCase.Name = "cbLowerCase";
			this.cbLowerCase.Size = new System.Drawing.Size(132, 22);
			this.cbLowerCase.TabIndex = 5;
			this.cbLowerCase.Text = "Lower Case";
			this.cbLowerCase.UseVisualStyleBackColor = true;
			this.cbLowerCase.CheckedChanged += new System.EventHandler(this.cbLowerCase_CheckedChanged);
			this.cbLowerCase.CheckStateChanged += new System.EventHandler(this.cbLowerCase_CheckStateChanged);
			// 
			// cbShowPK
			// 
			this.cbShowPK.AutoSize = true;
			this.cbShowPK.Location = new System.Drawing.Point(297, 3);
			this.cbShowPK.Name = "cbShowPK";
			this.cbShowPK.Size = new System.Drawing.Size(108, 22);
			this.cbShowPK.TabIndex = 6;
			this.cbShowPK.Text = "Show PK";
			this.cbShowPK.UseVisualStyleBackColor = true;
			this.cbShowPK.CheckedChanged += new System.EventHandler(this.cbShowPK_CheckedChanged);
			this.cbShowPK.CheckStateChanged += new System.EventHandler(this.cbShowPK_CheckStateChanged);
			// 
			// cbShowComment
			// 
			this.cbShowComment.AutoSize = true;
			this.cbShowComment.Location = new System.Drawing.Point(411, 3);
			this.cbShowComment.Name = "cbShowComment";
			this.cbShowComment.Size = new System.Drawing.Size(164, 22);
			this.cbShowComment.TabIndex = 7;
			this.cbShowComment.Text = "Show Comment";
			this.cbShowComment.UseVisualStyleBackColor = true;
			this.cbShowComment.CheckedChanged += new System.EventHandler(this.cbShowComment_CheckedChanged);
			this.cbShowComment.CheckStateChanged += new System.EventHandler(this.cbShowComment_CheckStateChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.dataGrdiView);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 76);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(800, 374);
			this.panel1.TabIndex = 6;
			// 
			// dataGrdiView
			// 
			this.dataGrdiView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGrdiView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrdiView.Location = new System.Drawing.Point(0, 0);
			this.dataGrdiView.Name = "dataGrdiView";
			this.dataGrdiView.RowTemplate.Height = 30;
			this.dataGrdiView.Size = new System.Drawing.Size(800, 374);
			this.dataGrdiView.TabIndex = 6;
			// 
			// frmColumnInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Name = "frmColumnInfo";
			this.Text = "frmColumnInfo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmColumnInfo_FormClosing);
			this.Load += new System.EventHandler(this.frmColumnInfo_Load);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1T.ResumeLayout(false);
			this.flowLayoutPanel1T.PerformLayout();
			this.flowLayoutPanel1B.ResumeLayout(false);
			this.flowLayoutPanel1B.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrdiView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1T;
		private System.Windows.Forms.Label lbAlias;
		private System.Windows.Forms.TextBox tbAlias;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1B;
		private System.Windows.Forms.CheckBox cbInsertComma;
		private System.Windows.Forms.CheckBox cbLowerCase;
		private System.Windows.Forms.CheckBox cbShowPK;
		private System.Windows.Forms.CheckBox cbShowComment;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dataGrdiView;
	}
}