using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;       // https://hot-key.tistory.com/4?category=1010793

using System.Diagnostics;                   // Debug.Assert, Debug.WriteLine, ..
using System.Text.RegularExpressions;
using Sprache;

//using System.Threading;
//using System.Xml;

namespace XLog
{
    public partial class Form1 : Form
    {


		public Form1()
        {
            InitializeComponent();

			PUBLIC.RegisterHotKey(this.Handle, PUBLIC.HOTKEY_ID, PUBLIC.KeyModifiers.Control, Keys.L);
			PUBLIC.RegisterHotKey(this.Handle, PUBLIC.HOTKEY_ID, PUBLIC.KeyModifiers.Control, Keys.K);
			//PUBLIC.UnregisterHotKey(this.Handle, PUBLIC.HOTKEY_ID + 1);
		}

		protected override void WndProc(ref Message message)
		{
			switch (message.Msg)
			{
				case PUBLIC.WM_HOTKEY:
					Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
					PUBLIC.KeyModifiers modifier = (PUBLIC.KeyModifiers)((int)message.LParam & 0xFFFF);

					if ((PUBLIC.KeyModifiers.Control) == modifier)
					{
						if (Keys.L == key)
						{
						}

						if (Keys.K == key)
						{
							// Do Something..
							//public Point GetCaretPos(TextBox textBox)
							//{
							//	int tRowIdx = textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex);
							//	int tLineFir = textBox.GetCharacterIndexFromLineIndex(tRowIdx);
							//	int tColIdx = textBox.CaretIndex - tLineFir;
							//	FormattedText formattedText = new FormattedText(
							//		textBox.Text.Substring(tLineFir, textBox.CaretIndex - tLineFir),
							//		System.Globalization.CultureInfo.CurrentCulture, 
							//		FlowDirection.LeftToRight, 
							//		new Typeface(textBox.FontFamily.ToString()),
							//		textBox.FontSize, textBox.Foreground);
							//	//return new Point(formattedText.Width, (tRowIdx + 1) * formattedText.Height);
							//}


						}
					}
					break;
			}
			base.WndProc(ref message);
		}

		private void Form1_Load(object sender, EventArgs e)
        {
			string myStr = null;
			myStr = "" +
				" -- 1;\r\n" +
				//" // 1; \r\n" +
				"select /* ; */ 1 from dual" +
				";"
				;
			myStr += "\n\nselect 2 from dual;  ";
			myStr += "\n\nselect 3 from dual;  ";
			//myStr = "select 1 from dual;";
			string myStr2 = myStr;

			//myStr = myStr.Trim();
			myStr = PUBLIC.SimpleStripSQL(myStr);
			int sLen = myStr.Length;

			richTextBox1.Text = myStr;
			richTextBox2.Text = myStr2;

		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
		}


		private void button1_Click(object sender, EventArgs e)
		{
		}

	}
}
