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
namespace DVLD_System.Users
{
    public partial class frmUsersList : Form
    {

        private static DataTable _dtAllUsers;

        private void _RefreshUsersList()
        {
            _dtAllUsers = clsUser.GetAllUser();

            dgvAllUsers.DataSource = _dtAllUsers;

            lblRecords.Text = _dtAllUsers.Rows.Count.ToString();

        }
        public frmUsersList()
        {
            InitializeComponent();
            
        }

        private void frmUsersList_Load(object sender, EventArgs e)
        {
            _dtAllUsers = clsUser.GetAllUser();
            dgvAllUsers.DataSource = _dtAllUsers;
          
                cmbFilter.SelectedIndex = 0;
                
                lblRecords.Text = _dtAllUsers.Rows.Count.ToString();
                if (dgvAllUsers.Rows.Count > 0){
                    dgvAllUsers.Columns[0].HeaderText = "User ID";
                dgvAllUsers.Columns[0].Width = 110;
                dgvAllUsers.Columns[1].HeaderText = "Person ID";
                dgvAllUsers.Columns[1].Width = 100;

                dgvAllUsers.Columns[2].HeaderText = "Full Name";
                dgvAllUsers.Columns[2].Width = 100;
                dgvAllUsers.Columns[3].HeaderText = "User Name";
                dgvAllUsers.Columns[3].Width = 100;

                dgvAllUsers.Columns[4].HeaderText = "Is Active";
                dgvAllUsers.Columns[4].Width = 100;
                }


        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

           
            string FilterColumn = "";
            switch (cmbFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "User Name":
                    FilterColumn = "UserName";
                    break;
                //case "Is Active":
                   
                //    return; // stop
                    

                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecords.Text = _dtAllUsers.Rows.Count.ToString();
                return;

            }
          
            if (FilterColumn != "FullName" && FilterColumn != "UserName")
            {
                //in thiscase we deal with integer not string
                //_dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());


            }

            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = _dtAllUsers.Rows.Count.ToString();


        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();
        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cmbIsActive.Text.Trim();
            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;




            }
            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecords.Text = _dtAllUsers.DefaultView.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cmbFilter.Text == "Person ID" || cmbFilter.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbFilter.Text == "IsActive")
            {
                txtFilter.Visible = false;
                cmbIsActive.Visible = true;
                cmbIsActive.SelectedIndex = 0; // Reset to "All"
                cmbIsActive.Focus();
            }
            else
            {
                txtFilter.Visible = (cmbFilter.Text != "None");
                cmbIsActive.Visible = false;
                 txtFilter.Text = "";
                    txtFilter.Focus();
                
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Can not send Email", "Warnning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Can not call", "Warnning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser((int)dgvAllUsers.CurrentRow.Cells[0].Value);
                frm.ShowDialog();
            _RefreshUsersList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to delete this user ["
               + (int)dgvAllUsers.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsUser.DeleteUser((int)dgvAllUsers.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User deleted successfully.");
                    frmUsersList_Load(null, null);
                }
                else
                {
                    MessageBox.Show("User is not deleted due to data connected to it  .","Faild",MessageBoxButtons.OK,MessageBoxIcon.Warning);

                }
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e){



            int UserID = (int)dgvAllUsers.CurrentRow.Cells[0].Value;
            frmChangePassword frm = new frmChangePassword(UserID);
            frm.ShowDialog();
        }
    }
}
