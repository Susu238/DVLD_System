using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Licenses.Cnotrol
{
    public partial class ctrrDrivereLicenses : UserControl
    {
        private int _DriverID;
        private clsDrivers _Driver;
        private DataTable dtAllLocalLicenses;
        private DataTable dtAllInternationalLicenses;

        public ctrrDrivereLicenses()
        {
            InitializeComponent();
        }
        private void LoadLocalLicensesInfo()
        {
            dtAllLocalLicenses = clsDrivers.GetDriverLicenses(_DriverID);
            dgvAllLocals.DataSource = dtAllLocalLicenses;
            lblRecord.Text = dgvAllLocals.Rows.Count.ToString();
            if(dgvAllLocals.Rows.Count > 0)
            {
                dgvAllLocals.Columns[0].HeaderText = "License ID";
                dgvAllLocals.Columns[0].Width = 110;
                dgvAllLocals.Columns[1].HeaderText = "App.ID";
                dgvAllLocals.Columns[1].Width = 110;
                dgvAllLocals.Columns[2].HeaderText = "Class Name";
                dgvAllLocals.Columns[2].Width = 210;
                dgvAllLocals.Columns[3].HeaderText = " Issue Date";
                dgvAllLocals.Columns[3].Width = 160;
                dgvAllLocals.Columns[4].HeaderText = "Expiration Date";
                dgvAllLocals.Columns[4].Width = 150;
                dgvAllLocals.Columns[5].HeaderText = "Is Active";
                dgvAllLocals.Columns[5].Width = 110;


            }
        }
        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDrivers.FindByDriverID(_DriverID);
            if (_Driver == null)
            {
                MessageBox.Show("No Person with ID =" + DriverID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadLocalLicensesInfo();
            LoadInternationalLicense();
        }
        private void LoadInternationalLicense()
        {
            dtAllInternationalLicenses = clsDrivers.GetInternationalLicenses(_DriverID);
            dgvAllInternationl.DataSource = dtAllInternationalLicenses;
            lblRecord.Text = dgvAllInternationl.Rows.Count.ToString();
            if (dgvAllInternationl.Rows.Count > 0)
            {
                dgvAllInternationl.Columns[0].HeaderText = "License ID";
                dgvAllInternationl.Columns[0].Width = 110;
                dgvAllInternationl.Columns[1].HeaderText = "App.ID";
                dgvAllInternationl.Columns[1].Width = 110;
                dgvAllInternationl.Columns[2].HeaderText = "Class Name";
                dgvAllInternationl.Columns[2].Width = 210;
                dgvAllInternationl.Columns[3].HeaderText = " Issue Date";
                dgvAllInternationl.Columns[3].Width = 160;
                dgvAllInternationl.Columns[4].HeaderText = "Expiration Date";
                dgvAllInternationl.Columns[4].Width = 150;
                dgvAllInternationl.Columns[5].HeaderText = "Is Active";
                dgvAllInternationl.Columns[5].Width = 110;
            }
        }
        public void LoadInfoByPersonID(int PersonID)
        {
            _Driver = clsDrivers.FindByPersonID(PersonID);
            if(_Driver == null)
            {
                MessageBox.Show("No Person with ID =" + PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = _Driver.DriverID;
            LoadLocalLicensesInfo();
            LoadInternationalLicense();
        }
        public void Clear()
        {
            if (dtAllInternationalLicenses != null)
            {
                dtAllInternationalLicenses.Clear();
            }
            dtAllLocalLicenses.Clear();
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
