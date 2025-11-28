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
    public partial class frmListTestAppointment : Form
    {
        public DataTable dtTestAppointments;
        private int _LocalDrivingLicenseApplicstionID =-1;
        private clsTestTypes.enTestTypes _TestType = clsTestTypes.enTestTypes.VisionTest;
        public frmListTestAppointment(int LocalDrivingLicenseID,clsTestTypes.enTestTypes TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicstionID = LocalDrivingLicenseID;
            _TestType = TestType;
        }
        private void _LoadTestTypeTitleAndImage()
        {
            switch (_TestType)
            {
                case clsTestTypes.enTestTypes.VisionTest:
                    lblTitle.Text = " Vision Test";
                    this.Text = lblTitle.Text;
                    pbTestType.BackgroundImage = Properties.Resources.vision;
                    break;
                case clsTestTypes.enTestTypes.WrittenTest:
                    lblTitle.Text = " Written Test";
                    this.Text = lblTitle.Text;
                    pbTestType.BackgroundImage = Properties.Resources.written;
                    break;
                case clsTestTypes.enTestTypes.StreetTest:
                    lblTitle.Text = " Street Test";
                    this.Text = lblTitle.Text;
                    pbTestType.BackgroundImage = Properties.Resources.street;
                    break;
            }
        }
        private void frmListTestAppointment_Load(object sender, EventArgs e)
        {
            _LoadTestTypeTitleAndImage();
            ctrDrivingLicenseApplicationInfo1.LoadApplicationIfoByLocalDrivingLicenseID(_LocalDrivingLicenseApplicstionID);
            dtTestAppointments = clsTestAppointments.GetApplicationTestAppointmentPerTestType(_LocalDrivingLicenseApplicstionID,_TestType);
            dgvAllAppointments.DataSource = dtTestAppointments;

            lblRecords.Text = dgvAllAppointments.Rows.Count.ToString();
            if (dgvAllAppointments.Rows.Count > 0)
            {
                dgvAllAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvAllAppointments.Columns[0].Width = 120;
                dgvAllAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvAllAppointments.Columns[1].Width = 200;
                dgvAllAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvAllAppointments.Columns[2].Width = 120;
                dgvAllAppointments.Columns[3].HeaderText = "Is Locked";
                dgvAllAppointments.Columns[3].Width = 100;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrDrivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {

        }

        private void lblRecords_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicstionID);
            if (LocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
                    }
            clsTests LastTest = LocalDrivingLicenseApplication.GetLastTestPerTestType(_TestType);
            if(LastTest == null)
            {
                frmScheduleTest FRM = new frmScheduleTest(_LocalDrivingLicenseApplicstionID, _TestType);
                FRM.ShowDialog();
                frmListTestAppointment_Load(null, null);
                return;
            }
            if(LastTest.TestResult== true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmScheduleTest frm = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestType);
            frm.ShowDialog();
            frmListTestAppointment_Load(null, null);

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointment = (int)dgvAllAppointments.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicstionID, _TestType, TestAppointment);
            frm.ShowDialog();
            frmListTestAppointment_Load(null, null);

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointment = (int)dgvAllAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(TestAppointment, _TestType);
            frm.ShowDialog();
        }
    }
}
