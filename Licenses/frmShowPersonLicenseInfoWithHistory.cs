using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Licenses
{
    public partial class frmShowPersonLicenseInfoWithHistory : Form
    {
        private int _PersonID = -1;
        public frmShowPersonLicenseInfoWithHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;

        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowPersonLicenseInfoWithHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                ctrPersonDetailsWithFilter1.LoadPersonInfo(_PersonID);
                ctrPersonDetailsWithFilter1.Enabled = false;

                ctrrDrivereLicenses1.LoadInfoByPersonID(_PersonID);
            }
            else
            {
                ctrPersonDetailsWithFilter1.Enabled = true;
                ctrPersonDetailsWithFilter1.FilterFocus();
            }
        }

        private void ctrPersonDetailsWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;
            if(_PersonID == -1)
            {
                ctrrDrivereLicenses1.Clear();
            }
            else
            {
                ctrrDrivereLicenses1.LoadInfoByPersonID(_PersonID);
            }
        }
    }
}
