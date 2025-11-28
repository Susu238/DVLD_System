using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
namespace DVLD_System
{
    public partial class frmPeopleList : Form
    {
        private static DataTable _dtAllPeople = clsPeopleBusiness.GetAllPeople();
        //only show the columns that you want to show in the grid 


        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName",
               "SecondName", "ThirdName", "LastName", "GenderCaption", "DateOfBirth", "CountryName", "Phone", "Email");
        //clsPeopleBusiness _Person;
        //private int _selectedPersonID;
        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPeopleBusiness.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName",
                    "SecondName", "ThirdName", "LastName", "GenderCaption", "DateOfBirth", "CountryName", "Phone", "Email");

            dgvAllPeople.DataSource = _dtPeople;

            lblRecords.Text = dgvAllPeople.Rows.Count.ToString();

        }

        public frmPeopleList()
        {
            InitializeComponent();
        }
        
        private void frmPeopleList_Load(object sender, EventArgs e)
            
        {


            //dgvAllPeople.AutoGenerateColumns = true;

            dgvAllPeople.DataSource = _dtPeople;
            //dgvAllPeople.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //dgvAllPeople.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (cmbFilter.SelectedIndex > 0)
                cmbFilter.SelectedIndex = 0;
            lblRecords.Text = dgvAllPeople.Rows.Count.ToString();
            if (dgvAllPeople.Rows.Count > 0)
            {
                dgvAllPeople.Columns[0].HeaderText = "Person ID";
                dgvAllPeople.Columns[0].Width = 110;

                dgvAllPeople.Columns[1].HeaderText = "National No";
                dgvAllPeople.Columns[1].Width = 100;

                dgvAllPeople.Columns[2].HeaderText = "First Name";
                dgvAllPeople.Columns[2].Width = 100;
                dgvAllPeople.Columns[3].HeaderText = "Second Name";
                dgvAllPeople.Columns[3].Width = 100;

                dgvAllPeople.Columns[4].HeaderText = "Third Name";
                dgvAllPeople.Columns[4].Width = 100;
                dgvAllPeople.Columns[5].HeaderText = "Last Name";
                dgvAllPeople.Columns[5].Width = 100;
                dgvAllPeople.Columns[6].HeaderText = "Gender";
                dgvAllPeople.Columns[6].Width = 100;
                dgvAllPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvAllPeople.Columns[7].Width = 100;
             
                dgvAllPeople.Columns[8].HeaderText = "Country";
                dgvAllPeople.Columns[8].Width = 100;
                dgvAllPeople.Columns[9].HeaderText = "Phone";
                dgvAllPeople.Columns[9].Width = 100;
                dgvAllPeople.Columns[10].HeaderText = "Email";
                dgvAllPeople.Columns[10].Width = 160;





                cmbFilter.SelectedIndex = 0;



            }
            //_RefreshPeopleList();
        }
 
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //ApplyFilter();
            string FilterColumn = "";
            switch (cmbFilter.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "First Name":
                    FilterColumn = "FirstName";
                    break;
                case "Second Name":
                    FilterColumn = "SecondName";
                    break;
                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Country":
                    FilterColumn = "CountryName";
                    break;
                case "Gender":
                    FilterColumn = "GenderCaption";
                    break;
                case "Date Of Birth":
                    FilterColumn = "DateOfBirth";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            //Reseting the filter in case  nothing selected or filter value contains nothing

            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text = dgvAllPeople.Rows.Count.ToString();
                return;

            }
            if (FilterColumn == "PersonID")
            {
                //in thiscase we deal with integer not string
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());

            }
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvAllPeople.Rows.Count.ToString();







        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int personID = Convert.ToInt32(dgvAllPeople.CurrentRow.Cells["PersonID"].Value);
            frmShowPersonInfo frm = new frmShowPersonInfo(personID);
            frm.ShowDialog();
            //_RefreshPeopleList();


        }

        private void dgvAllPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            //{
            //    // Select the row under the mouse
            //    dgvAllPeople.ClearSelection();
            //    dgvAllPeople.Rows[e.RowIndex].Selected = true;

            //    // Save the PersonID from that row
            //    _selectedPersonID = Convert.ToInt32(

            //        dgvAllPeople.Rows[e.RowIndex].Cells[0].Value)
            //    ;
            //}
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //if (_selectedPersonID > 0)
            //{
           


            frmAddEditPerson frm = new frmAddEditPerson((int)dgvAllPeople.CurrentRow.Cells[0].Value);

                frm.ShowDialog();
                _RefreshPeopleList();
            //}
    }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

           

            if (MessageBox.Show("Are you sure you want to delete this person ["
               + (int)dgvAllPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsPeopleBusiness.DeletePerson((int)dgvAllPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person deleted successfully.");
                    _RefreshPeopleList();
                }
                else
                {
                    MessageBox.Show("Person is not deleted .");

                }
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Can not send Email","Warnning" ,MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Can not call ", "Warnning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cmbFilter.Text != "None");
            if(txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cmbFilter.Text=="Person ID" )
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

                
        }

        private void dgvAllPeople_DoubleClick(object sender, EventArgs e)
        {
            //int personID = (int)dgvAllPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo((int)dgvAllPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
