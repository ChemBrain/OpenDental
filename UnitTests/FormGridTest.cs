using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace UnitTests{
	public partial class FormGridTest : Form{
		private List<Patient> _listPatients;

		public FormGridTest(){
			InitializeComponent();
		}

		private void FormGridTest_Load(object sender, EventArgs e){
			FillPatients();
			FillGrid();
		}

		private void FillPatients(){
			_listPatients=new List<Patient>();
			Patient patient;
			for(int i=0;i<200;i++){
				patient=new Patient();
				patient.LName="LName"+i.ToString();
				patient.FName="FName"+i.ToString();
				_listPatients.Add(patient);
			}

		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn("LName",80);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn("FName",80);
			gridMain.ListGridColumns.Add(col);
			gridMain.NoteSpanStop=1;
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_listPatients.Count;i++){
				row=new GridRow();
				if(i==2){
					row.Cells.Add(new GridCell("click"){ColorBackG=Color.LightGray,ClickEvent=DeleteClick });
				}
				else{
					row.Cells.Add(_listPatients[i].LName+" "+_listPatients[i].FName+" "+_listPatients[i].FName);
				}
				row.Cells.Add(_listPatients[i].FName);
				if(i==5){
					row.DropDownParent=gridMain.ListGridRows[4];
				}
				if(i==6){
					row.DropDownParent=gridMain.ListGridRows[4];
				}
				if(i==7){
					row.DropDownParent=gridMain.ListGridRows[4];
				}
				if(i==8){
					row.DropDownParent=gridMain.ListGridRows[7];
				}
				if(i==10){
					row.Note="Some note";
				}
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void DeleteClick(object sender,EventArgs e) {
			MessageBox.Show("Clicked");
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			Font _fontCell=new Font(FontFamily.GenericSansSerif,8.5f);
			Font _fontCellBold=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold);
			int h1=_fontCell.Height;
			int h2=_fontCellBold.Height;
		}

		/*private void ButObjects_Click(object sender, EventArgs e){
			//2M basic objects per second, 500k fonts per second
			DateTime dt=DateTime.Now;
			ODGridRow row;
			List<ODGridRow> listRows=new List<ODGridRow>();
			Font[] fonts=new Font[1000000];
			for(int i=0;i<1000000;i++){
				Font font=new Font("Arial",8);
				fonts[i]=font;
				row=new ODGridRow();
				row.RowNum=i;
				row.Tag="tag";
				row.Note="note";
				row.RowHeight=20;
				listRows.Add(row);
			}
			TimeSpan time=DateTime.Now-dt;
			MessageBox.Show(fonts[2].ToString()+"   "+time.ToString());
		}*/
	}
}
