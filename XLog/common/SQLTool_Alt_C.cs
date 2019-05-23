using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLog
{
	public partial class SQLTool_Alt_C : Form
	{
		// TODO: 휘발성으로, 재구동이후에는 유지되지 않는다. 설정파일 개념이 요구됨
		public static bool checked_insertComma { get; set; } = false;
		public static bool checked_lowerCase { get; set; } = false;
		public static bool checked_showPK { get; set; } = false;
		public static bool checked_showComment { get; set; } = false;

		public static Point preLocation { get; set; } = new Point(-1,-1);
		public static int preHeight { get; set; } = -1;
		public static int preWidth { get; set; } = -1;

		//private DataTable datatable;

		public SQLTool_Alt_C()
		{
			InitializeComponent();

			cbShowComment.Visible = false;
			cbShowPK.Visible = false;

			dgvResult.BackgroundColor = Color.White;

			cbInsertComma.Checked = checked_insertComma;
			cbLowerCase.Checked = checked_lowerCase;

			if (preLocation.X == -1)
			{
				//form.StartPosition = FormStartPosition.CenterParent;
				StartPosition = FormStartPosition.CenterScreen;
				//SQLTool_Alt_C.preLocation = form.Location;
			}
			else
			{
				StartPosition = FormStartPosition.Manual;
				Location = preLocation;
				Height = preHeight;
				Width = preWidth;
			}
		}

		public void SetDataTable(DataTable datatable_)
		{
			//dgvResult.DataSource = null;
			//datatable = datatable_;
			dgvResult.DataSource = datatable_;
			Application.DoEvents();
		}

		//private void Main(ref DataTable datatable_)
		//{
		//	dgvResult.DataSource = datatable_;
		//	Application.DoEvents();

		//	this.Show();
		//}

		private void cbInsertComma_CheckStateChanged(object sender, EventArgs e)
		{
			checked_insertComma = cbInsertComma.Checked;
		}

		private void cbLowerCase_CheckStateChanged(object sender, EventArgs e)
		{
			checked_lowerCase = cbLowerCase.Checked;
		}

		private void cbShowPK_CheckStateChanged(object sender, EventArgs e)
		{

		}

		private void cbShowComment_CheckStateChanged(object sender, EventArgs e)
		{

		}

		private void cbInsertComma_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void cbLowerCase_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void cbShowPK_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void cbShowComment_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void SQLTool_Alt_C_FormClosing(object sender, FormClosingEventArgs e)
		{
			SQLTool_Alt_C.preLocation = this.Location;
			SQLTool_Alt_C.preHeight = this.Height;
			SQLTool_Alt_C.preWidth = this.Width;
		}
	}
	
}
