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

namespace DVLD_System.Applications.internationalLicense
{
    public partial class frmListInternationalLicenses : Form
    {
        private DataTable dtInternationalLicenses = new DataTable();
        public frmListInternationalLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmInternationalLicense frm = new frmInternationalLicense();
            frm.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvAllInternationalDrivingLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindByDriverID(DriverID).PersonID;

            frmShowPersonInfo FRM = new frmShowPersonInfo(PersonID);
            FRM.ShowDialog();
        }

        private void frmListInternationalLicenses_Load(object sender, EventArgs e)
        {
            dtInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();
            dgvAllInternationalDrivingLicenses.DataSource = dtInternationalLicenses;
            lblRecords.Text = dgvAllInternationalDrivingLicenses.Rows.Count.ToString();
            if (dgvAllInternationalDrivingLicenses.Rows.Count > 0) {
                dgvAllInternationalDrivingLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvAllInternationalDrivingLicenses.Columns[0].Width = 160;
                dgvAllInternationalDrivingLicenses.Columns[1].HeaderText = "Application ID";
                dgvAllInternationalDrivingLicenses.Columns[1].Width = 150;
                dgvAllInternationalDrivingLicenses.Columns[2].HeaderText = "Driver ID";
                dgvAllInternationalDrivingLicenses.Columns[2].Width = 130;
                dgvAllInternationalDrivingLicenses.Columns[3].HeaderText = "L.License ID";
                dgvAllInternationalDrivingLicenses.Columns[3].Width = 130;
                dgvAllInternationalDrivingLicenses.Columns[4].HeaderText = "Issue Date";
                dgvAllInternationalDrivingLicenses.Columns[4].Width = 160;
                dgvAllInternationalDrivingLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvAllInternationalDrivingLicenses.Columns[5].Width = 160;
                dgvAllInternationalDrivingLicenses.Columns[6].HeaderText = "Is Active";
                dgvAllInternationalDrivingLicenses.Columns[6].Width = 160;





            }
            cmbFilter.SelectedIndex = 0;

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvAllInternationalDrivingLicenses.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvAllInternationalDrivingLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindByDriverID(DriverID).PersonID;
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(PersonID);
            frm.ShowDialog();

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbFilter.Text == "Is Active")
            {
                cmIsActive.Enabled = true;
                cmIsActive.Visible = true;
                txtFilter.Visible = false;
                cmIsActive.Focus();
                cmIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilter.Visible = (cmbFilter.Text == "None");
                cmIsActive.Visible = false;
                if (cmbFilter.Text == "None")
                {
                    txtFilter.Enabled = false;
                }
                else
                    txtFilter.Enabled = true;
                txtFilter.Visible = true;
                txtFilter.Text = "";
                txtFilter.Focus();

            }
        }

        private void cmIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cmIsActive.Text;
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
            if(FilterValue == "All")
            {
                dtInternationalLicenses.DefaultView.RowFilter = "";
            }
            else
            {
                dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn,FilterValue);
                lblRecords.Text = dgvAllInternationalDrivingLicenses.Rows.Count.ToString();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cmbFilter.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicense D";
                    break;
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;
                case "Is Active":
                    FilterColumn = "IsActive";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if(txtFilter.Text.Trim() == "" && FilterColumn == "None")
            {
                dtInternationalLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = dgvAllInternationalDrivingLicenses.Rows.Count.ToString();
                return;
            }
            dtInternationalLicenses.DefaultView.RowFilter = string.Format("{[0]} = {1}", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvAllInternationalDrivingLicenses.Rows.Count.ToString();

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow numbers only becasue all fiters are numbers.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }
    }
}
