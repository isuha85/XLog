namespace XLog
{
    partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.richTextBox2 = new System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.richTextBox3 = new System.Windows.Forms.RichTextBox();
			this.textBox = new System.Windows.Forms.TextBox();
			this.fctb = new FastColoredTextBoxNS.FastColoredTextBox();
			this.fastColoredTextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
			((System.ComponentModel.ISupportInitialize)(this.fctb)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(701, 117);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "select 1 from dual;";
			this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(707, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(99, 93);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// richTextBox2
			// 
			this.richTextBox2.Location = new System.Drawing.Point(0, 123);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.Size = new System.Drawing.Size(701, 185);
			this.richTextBox2.TabIndex = 2;
			this.richTextBox2.Text = "";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.statusStrip1.Location = new System.Drawing.Point(0, 690);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1002, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// richTextBox3
			// 
			this.richTextBox3.Location = new System.Drawing.Point(707, 99);
			this.richTextBox3.Name = "richTextBox3";
			this.richTextBox3.Size = new System.Drawing.Size(295, 571);
			this.richTextBox3.TabIndex = 4;
			this.richTextBox3.Text = "";
			// 
			// textBox
			// 
			this.textBox.Location = new System.Drawing.Point(902, 65);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(100, 28);
			this.textBox.TabIndex = 5;
			// 
			// fctb
			// 
			this.fctb.AutoCompleteBracketsList = new char[] {
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
			this.fctb.AutoScrollMinSize = new System.Drawing.Size(263, 22);
			this.fctb.BackBrush = null;
			this.fctb.CharHeight = 22;
			this.fctb.CharWidth = 12;
			this.fctb.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fctb.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fctb.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.fctb.IsReplaceMode = false;
			this.fctb.Location = new System.Drawing.Point(0, 330);
			this.fctb.Name = "fctb";
			this.fctb.Paddings = new System.Windows.Forms.Padding(0);
			this.fctb.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fctb.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb.ServiceColors")));
			this.fctb.Size = new System.Drawing.Size(701, 253);
			this.fctb.TabIndex = 6;
			this.fctb.Text = "select 1 from dual;";
			this.fctb.Zoom = 100;
			// 
			// fastColoredTextBox1
			// 
			this.fastColoredTextBox1.AutoCompleteBracketsList = new char[] {
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
			this.fastColoredTextBox1.AutoScrollMinSize = new System.Drawing.Size(263, 22);
			this.fastColoredTextBox1.BackBrush = null;
			this.fastColoredTextBox1.CharHeight = 22;
			this.fastColoredTextBox1.CharWidth = 12;
			this.fastColoredTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.fastColoredTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.fastColoredTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.fastColoredTextBox1.IsReplaceMode = false;
			this.fastColoredTextBox1.Location = new System.Drawing.Point(360, 613);
			this.fastColoredTextBox1.Name = "fastColoredTextBox1";
			this.fastColoredTextBox1.Paddings = new System.Windows.Forms.Padding(0);
			this.fastColoredTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.fastColoredTextBox1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBox1.ServiceColors")));
			this.fastColoredTextBox1.Size = new System.Drawing.Size(150, 150);
			this.fastColoredTextBox1.TabIndex = 7;
			this.fastColoredTextBox1.Text = "fastColoredTextBox1";
			this.fastColoredTextBox1.Zoom = 100;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1002, 712);
			this.Controls.Add(this.fastColoredTextBox1);
			this.Controls.Add(this.fctb);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.richTextBox3);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.richTextBox2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.richTextBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.fctb)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox richTextBox2;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.RichTextBox richTextBox3;
		private System.Windows.Forms.TextBox textBox;
		private FastColoredTextBoxNS.FastColoredTextBox fctb;
		private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBox1;
	}
}