using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Licenses.Local_License.Controls
{
    public partial class ctrDrivingLicenseInfoWithFilter : UserControl
    {
        //define a cutom event handler with parameter.
        public event Action<int> OnSelectedLicense;
        // a protected method to raise the event.
        protected virtual void LicenseSelected (int LicenseID)
        {
            Action<int> handler = OnSelectedLicense;
            if(handler != null)
            {
                handler(LicenseID);
            }
        }
        public ctrDrivingLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _FilterEnabled = true;
        public bool filterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        private int  _LicenseID = -1;
        public int LicenseID
        {
            get
            {
                return ctrDrivingLicense1.LicenseID;
            }
        }
        public clsLicenses SelectedLicenseInfo
        {
            get
            {
                return ctrDrivingLicense1.SelectedLicenseInfo;
            }
        }
        public void LoadInfo(int LicenseID)
        {
            txtFilter.Text = LicenseID.ToString();
            ctrDrivingLicense1.LoadInfo(LicenseID);
            _LicenseID = ctrDrivingLicense1.LicenseID;
          if(OnSelectedLicense != null && _FilterEnabled)
            {
                OnSelectedLicense(_LicenseID);
            }

        }
        private void ctrDrivingLicenseInfoWithFilter_Load(object sender, EventArgs e)
        {

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // block if not digit
            e.Handled = !Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar);
            // if press Enter key
            if(e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid ! put the mouse over the red icon to see the error ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFilter.Focus();
                return;
            }
            _LicenseID = int.Parse(txtFilter.Text);
            LoadInfo(_LicenseID);
        }
        public void txtFilterFocus()
        {
            txtFilter.Focus();
        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilter, "Tis field is required can not be empty!");

            }
            else
            {
                errorProvider1.SetError(txtFilter, null);

            }
        }
    }
}
