using DVLD_Business;
using DVLD_System.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable dtDrivers = new DataTable();
      
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            dtDrivers = clsDrivers.GetAllDrivers();
            dgvAllDrivers.DataSource = dtDrivers;
            lblRecords.Text = dgvAllDrivers.Rows.Count.ToString();
            if(dgvAllDrivers.Rows.Count > 0)
            {
                cmbFilter.SelectedIndex = 0;

                lblRecords.Text = dtDrivers.Rows.Count.ToString();
                if (dgvAllDrivers.Rows.Count > 0)
                {
                    dgvAllDrivers.Columns[0].HeaderText = "Driver ID";
                    dgvAllDrivers.Columns[0].Width = 110;
                    dgvAllDrivers.Columns[1].HeaderText = "Person ID";
                    dgvAllDrivers.Columns[1].Width = 100;
                    dgvAllDrivers.Columns[2].HeaderText = "National NO";
                    dgvAllDrivers.Columns[2].Width = 100;

                    dgvAllDrivers.Columns[3].HeaderText = "Full Name";
                    dgvAllDrivers.Columns[3].Width = 100;
                    dgvAllDrivers.Columns[4].HeaderText = "Date";
                    dgvAllDrivers.Columns[4].Width = 100;

                    dgvAllDrivers.Columns[5].HeaderText = " Active Licenses";
                    dgvAllDrivers.Columns[5].Width = 100;
                }   }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cmbFilter.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Date":
                    FilterColumn = "Date";
                    break;
                case " Active Licenses":

                    FilterColumn = "IsActive";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                dtDrivers.DefaultView.RowFilter = "";
                lblRecords.Text = dtDrivers.Rows.Count.ToString();
                return;

            }

            if (FilterColumn != "FullName" && FilterColumn != "NationalNO")
            {
                //in thiscase we deal with integer not string
                //_dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
                dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());


            }

            else
                dtDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dtDrivers.Rows.Count.ToString();


        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cmbFilter.Text == "Driver ID" || cmbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvAllDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvAllDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(PersonID);
            frm.ShowDialog();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbFilter.Text == "None")
            {
                txtFilter.Enabled = false;
            }
            else
            {
                txtFilter.Enabled = true;
            }
        }
    }
}
