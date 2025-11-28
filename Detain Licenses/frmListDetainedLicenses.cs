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

namespace DVLD_System.Detain_Licenses
{
    public partial class frmListDetainedLicenses : Form
    {
        private int _DetainedLicenseID;
        private DataTable dtDetainedLicenses = new DataTable();
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvAllDetainedLicenses.DataSource = dtDetainedLicenses;
            lblRecords.Text = dgvAllDetainedLicenses.Rows.Count.ToString();
            if(dgvAllDetainedLicenses.Rows.Count > 0)
            {
                dgvAllDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvAllDetainedLicenses.Columns[0].Width = 90;
                dgvAllDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvAllDetainedLicenses.Columns[1].Width = 90;
                dgvAllDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvAllDetainedLicenses.Columns[2].Width = 160;
                dgvAllDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvAllDetainedLicenses.Columns[3].Width = 110;
                dgvAllDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvAllDetainedLicenses.Columns[4].Width = 110;
                dgvAllDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvAllDetainedLicenses.Columns[5].Width = 160;
                dgvAllDetainedLicenses.Columns[6].HeaderText = "National NO";
                dgvAllDetainedLicenses.Columns[6].Width = 90;
                dgvAllDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvAllDetainedLicenses.Columns[7].Width = 300;
                dgvAllDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
                dgvAllDetainedLicenses.Columns[8].Width = 150;






            }
            cmbFilter.SelectedIndex = 0;

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cmbFilter.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;
                case "Is Released":
                    FilterColumn = "IsReleased";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            // reset filter in case nothing selected 
            if(txtFilter.Text == "" || FilterColumn == "None")
            {
                dtDetainedLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = dgvAllDetainedLicenses.Rows.Count.ToString();
                return;
            }
            if(FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
            {
                dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] ={1}", FilterColumn, txtFilter.Text.Trim());

            }
            else
            {
                dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());

            }
            lblRecords.Text = dgvAllDetainedLicenses.Rows.Count.ToString();

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbFilter.Text == "Is Released")
            {
                cmbIsReleased.Visible = true;
                txtFilter.Visible = false;
                cmbIsReleased.Focus();
                cmbIsReleased.SelectedIndex = 0;
            }
            else
            {
                txtFilter.Visible = (cmbFilter.Text != "None");
                cmbIsReleased.Visible = false;
                if(cmbFilter.Text == "None")
                {
                    txtFilter.Enabled = false;
                }
                else 
                    txtFilter.Enabled = true;
                txtFilter.Text = "";
                txtFilter.Focus();

            }
        }

        private void cmbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = cmbIsReleased.Text.Trim();
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
                dtDetainedLicenses.DefaultView.RowFilter = "";
            }
            else
            {
                dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);
            }
            lblRecords.Text = dgvAllDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "Detain ID" || cmbFilter.Text == "Release Application ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvAllDetainedLicenses.CurrentRow.Cells[1].Value;
            int PersonID = clsLicenses.FindLicense(LicenseID).DriverInfo.PersonID;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvAllDetainedLicenses.CurrentRow.Cells[1].Value;
            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvAllDetainedLicenses.CurrentRow.Cells[1].Value;
            frmShowPersonLicenseInfoWithHistory frm = new frmShowPersonLicenseInfoWithHistory(LicenseID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvAllDetainedLicenses.CurrentRow.Cells[1].Value;
            frmReleaseLicense frm = new frmReleaseLicense(LicenseID);
            frm.ShowDialog();


        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseLicense frm = new frmReleaseLicense();
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvAllDetainedLicenses.CurrentRow.Cells[3].Value;
        }
    }
}
