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
    public partial class ctrSecheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        public clsTestTypes.enTestTypes _TestTypeID = clsTestTypes.enTestTypes.VisionTest;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointments _TestAppointment;
        private int _TestAppointmentID = -1;
public enum enCreationMode { FirstTimeSechedule = 0,RetakeTestSchedule =1};
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSechedule;
        public clsTestTypes.enTestTypes TestType
        {
            get { return _TestTypeID; }
            set
            {
                _TestTypeID = value;
                switch (_TestTypeID)
                {
                    case clsTestTypes.enTestTypes.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestType.Image = Properties.Resources.vision;
                        break;
                    case clsTestTypes.enTestTypes.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestType.Image = Properties.Resources.written;
                        break;
                    case clsTestTypes.enTestTypes.StreetTest:
                        gbTestType.Text= "Street Test";
                        pbTestType.Image = Properties.Resources.street;
                        break;
                }
            }
        }
        public void LoadInfo(int LocalDrivingLicenseApplicationID , int AppointmentID = -1)
        {
            if(AppointmentID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
             if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;

            }
            //decide if the createion mode is retake test or not based if the person attended this test before
            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
            {
                _CreationMode = enCreationMode.RetakeTestSchedule;
            }
            else
            {
                _CreationMode = enCreationMode.FirstTimeSechedule;

            }
            if(_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblTitle.Text = "Shedule Retake Test";
                lblRetakeFees.Text = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.RetakeTest).ApplicationFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblRetakID.Text = "0";

            }
            else
            {
                lblTitle.Text = "Shedule Test";

                lblRetakeFees.Text = "0";
                gbRetakeTestInfo.Enabled = false;
                lblRetakID.Text = "N/A";

            }
            lblAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblClass.Text = _LocalDrivingLicenseApplication.ClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lbTrial.Text = _LocalDrivingLicenseApplication.TotalTrialPerTest(_TestTypeID).ToString();
            if(_Mode== enMode.AddNew)
            {
                lblFees.Text = clsTestTypes.FindByTestTypeID(_TestTypeID).TestTypeFees.ToString();
                dateTimePicker1.MinDate = DateTime.Now;
                lblRetakID.Text = "N/A";
                _TestAppointment = new clsTestAppointments();
            }
            else
            {
                if (!LoadTestAppointmentData())
                    return;
            }
            lblTotalFees.Text = (Convert.ToSingle(lblRetakeFees.Text) + Convert.ToSingle(lblFees.Text)).ToString();
            if (!_HandelActiveTestAppointmentConstraint())
                return;
            if (!_HandelAppointmentLockedConstraint())
                return;
            if (_HandelPreviousTestConstraint())
                return;
        }
        public ctrSecheduleTest()
        {
            InitializeComponent();
        }
        private bool _HandelAppointmentLockedConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID)) 
            {
                lblUserMessage.Text = " Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                return false;
            }
            return true;
        }
        private bool _HandelActiveTestAppointmentConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = " Person Already have sat appointment for this test it is locked";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                return false;
            }
            else
            {

                lblUserMessage.Visible = false;

                return true;
            }
        }
        private bool _HandelPreviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannno apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.
            switch (TestType)
            {
                case clsTestTypes.enTestTypes.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;
                case clsTestTypes.enTestTypes.WrittenTest:
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestTypes.VisionTest))
                    {

                        lblUserMessage.Visible = true;
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        btnSave.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        return false;

                    }
                    else
                    {

                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;

                    }
                    return true;
                case clsTestTypes.enTestTypes.StreetTest:
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestTypes.enTestTypes.WrittenTest))
                    {

                        lblUserMessage.Visible = true;
                        lblUserMessage.Text = "Cannot Sechule, Street Test should be passed first";
                        btnSave.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        return false;

                    }
                    else
                    {

                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;

                    }
                    return true;
            }
            return true;
        }
        private bool _HandleRetakeApplication()
        {
            if(_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplications Application = new clsApplications();
                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplications.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.RetakeTest).ApplicationFees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.userID;
                if (!Application.Save())
                {
                    _TestAppointment.ReTakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment.ReTakeTestApplicationID = Application.ApplicationID;
            }
            return true;
        }
        private bool LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointments.Find(_TestAppointmentID);
            if(_TestAppointment == null) {
                MessageBox.Show("No Appointment with ID " + _TestAppointmentID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            //we compare the current date with the appointment date to set the min date.
            if(DateTime.Compare(DateTime.Now,_TestAppointment.AppointmentDate) < 0)
            {
                dateTimePicker1.MinDate = DateTime.Now;
            }
            else
            {
                dateTimePicker1.MinDate = _TestAppointment.AppointmentDate;
            }
            dateTimePicker1.Value = _TestAppointment.AppointmentDate;
            if(_TestAppointment.ReTakeTestApplicationID== -1)
            {
                lblRetakeFees.Text = "0";
                lblRetakID.Text = "N/A";
                gbRetakeTestInfo.Enabled = false;

            }
            else
            {
                lblRetakeFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFees.ToString();
                lblRetakID.Text = _TestAppointment.ReTakeTestApplicationID.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Sechudule Retake Test";

            }
            return true;
        }

        private void gbTestType_Enter(object sender, EventArgs e)
        {

        }

        private void pbTestType_Click(object sender, EventArgs e)
        {

        }

        private void lblApplicant_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void lblApplicantt_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void ctrSecheduleTest_Load(object sender, EventArgs e)
        {

        }

        private void lbTrial_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;
            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dateTimePicker1.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.userID;
            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }
    }
}
