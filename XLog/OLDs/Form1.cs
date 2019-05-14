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

//using Microsoft.Data.Schema.ScriptDom;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

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
							// Do Something..
							string xx = richTextBox1.SelectedText;
							string yy = richTextBox2.SelectedText;
							richTextBox3.Text = yy;
							fctb.Text = yy;
							
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
			//var formatter = new Formatter(fctb.Text);
			//richTextBox2.Text = formatter.Format();
			var parser = new TSql150Parser(false);

			IList<ParseError> Errors;
			TSqlFragment result = parser.Parse(
			//IScriptFragment result = parser.Parse(
				new StringReader("Select col from T1 where 1 = 1 group by 1;" +
					"select col2 from T2;" +
					"select col1 from tbl1 where id in (select id from tbl);"),
					out Errors);

			var Script = result as TSqlScript;

			foreach (var ts in Script.Batches)
			{
				Console.WriteLine("new batch");

				foreach (var st in ts.Statements)
				{
					IterateStatement(st);
				}
			}
		}

		static void IterateStatement(TSqlStatement statement)
		{
			Console.WriteLine("New Statement");

			if (statement is SelectStatement)
			{
				//PrintStatement(statement);
			}
		}
		

		public static TSqlScript ParseScript(string script, out IList<string> errorsList)
		{
			IList<ParseError> parseErrors;
			TSql100Parser tsqlParser = new TSql100Parser(true);
			TSqlFragment fragment;
			using (StringReader stringReader = new StringReader(script))
			{
				fragment = (TSqlFragment)tsqlParser.Parse(stringReader, out parseErrors);
			}
			errorsList = new List<string>();
			if (parseErrors.Count > 0)
			{
				var retMessage = string.Empty;
				foreach (var error in parseErrors)
				{
					//retMessage += error.Identifier + " - " + error.Message + " - position: " + error.Offset + "; ";
				}
			}
			return (TSqlScript)fragment;
		}
	}
}
