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

namespace DVLD_System.Tests
{
    public partial class frmScheduleTest : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestTypes.enTestTypes _TestTypeID = clsTestTypes.enTestTypes.VisionTest;
        private int _TestAppointmentID = -1;
        public frmScheduleTest(int LocalDrivingLicenseID, clsTestTypes.enTestTypes TestTypes, int testAppointmentID = -1)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseID;
            _TestTypeID = TestTypes;
            _TestAppointmentID = testAppointmentID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrSecheduleTest1.TestType = _TestTypeID;
            ctrSecheduleTest1.LoadInfo(_LocalDrivingLicenseApplicationID, _TestAppointmentID);
        }

        private void ctrSecheduleTest1_Load(object sender, EventArgs e)
        {

        }
    }
}
