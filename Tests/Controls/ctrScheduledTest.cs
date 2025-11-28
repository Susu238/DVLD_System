using DVLD_Business;
using DVLD_System.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Tests.Controls
{
    public partial class ctrScheduledTest : UserControl
    {
        public clsTestTypes.enTestTypes _TestTypeID;
        public int TestId = -1;
        private int _TestAppointmentID = -1;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public int TestAppointmentID
        {
            get
            {
                return _TestAppointmentID;
            }
        }
        private int _LocalDrivingApplicationID;
        private clsTestAppointments _TestAppointment;
        public clsTestTypes.enTestTypes TestType
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;
                switch (_TestTypeID)
                {
                    case clsTestTypes.enTestTypes.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestImage.Image = Properties.Resources.vision;
                        break;
                    case clsTestTypes.enTestTypes.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestImage.Image = Properties.Resources.written;
                        break;
                    case clsTestTypes.enTestTypes.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestImage.Image = Properties.Resources.street;
                        break;

                }
            }
        }
        public  void LoadInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            _TestAppointment = clsTestAppointments.Find(_TestAppointmentID);
            if(_TestAppointment == null)
            {
                MessageBox.Show("No Appointment ID = " + _TestAppointmentID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }
            TestId = _TestAppointment.TestID;
            _LocalDrivingApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Local Driving License with  ID = " + _LocalDrivingApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblClass.Text = _LocalDrivingLicenseApplication.ClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;
            lbTrial.Text = _LocalDrivingLicenseApplication.TotalTrialPerTest(_TestTypeID).ToString();
            lblDate.Text = clsFormat.DateToShort(_TestAppointment.AppointmentDate);
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblTestID.Text = (_TestAppointment.TestID == -1) ? "Not Taken Yet" : _TestAppointment.TestID.ToString();
        }
        public ctrScheduledTest()
        {
            InitializeComponent();
        }

        private void gbTestType_Enter(object sender, EventArgs e)
        {

        }

        private void ctrScheduledTest_Load(object sender, EventArgs e)
        {

        }
    }
}
