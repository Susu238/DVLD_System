using DVLD_Business;
using DVLD_System.Controls;
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

using static System.Net.Mime.MediaTypeNames;
namespace DVLD_System.Applications.Local_Driving_License
{
    public partial class frmNewLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseID = -1;
        private int _SelectedPersonID = -1;
        public frmNewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmNewLocalDrivingLicenseApplication(int applicationID)
        {
            InitializeComponent();
            _Mode = enMode.AddNew;

            _LocalDrivingLicenseID = applicationID;
        }
        private void _FillClassesInComoboBox()
        {
            DataTable dtClasses= clsLicenseClass.GetAllClasses();

            foreach (DataRow row in dtClasses.Rows)
            {

                cmxLicenseClasses.Items.Add(row["ClassName"]);

            }
        }
        private void _RetDefaultValues()
        {
            _FillClassesInComoboBox();
            if (_Mode == enMode.AddNew)
            {
                label1.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                applicationInfo.Enabled = false;
                btnSave.Enabled = false;
                ctrPersonDetailsWithFilter2.FilterFocus();
                cmxLicenseClasses.SelectedIndex = 2;
                lblDate.Text = DateTime.Now.ToShortDateString();
                lblFees.Text = clsApplicationType.FindByApplicationTypeID((int)clsApplications.enApplicationType.NewDrivingLicense).ApplicationFees.ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.userName;
            }
            else
            {
                label1.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                applicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
         
        }
        private void _LoadData()
        {
          _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(_LocalDrivingLicenseID);
            ctrPersonDetailsWithFilter2.FilterEnabled = false;
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show(" No application with ID = " + _LocalDrivingLicenseApplication, "Application not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            lblDate.Text = _LocalDrivingLicenseApplication.ApplicationID.ToString();
            lblDate.Text =clsFormat.DateToShort (_LocalDrivingLicenseApplication.ApplicationDate);
            cmxLicenseClasses.SelectedIndex = cmxLicenseClasses.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedBy.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).userName;
            ctrPersonDetailsWithFilter2.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);

        }
        private void ctrPersonDetailsWithFilter1_Load(object sender, EventArgs e)
        {

        }
        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrPersonDetailsWithFilter2.LoadPersonInfo(PersonID);


        }

        private void frmNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _RetDefaultValues();
          if(_Mode== enMode.Update)
            {
                _LoadData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update )
            {
                btnSave.Enabled = true;
                applicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["applicationInfo"];
                return;

            }

            if(ctrPersonDetailsWithFilter2.PersonID != -1)
            {
                btnSave.Enabled = true;
                applicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["applicationInfo"];
                return;

            }
            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrPersonDetailsWithFilter2.FilterFocus();
            }

        }
        private void cmxLicenseClasses_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClass.FindLicenseClass(cmxLicenseClasses.Text).LicenseClassID;
            int ActiveApplicationID = clsApplications.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplications.enApplicationType.NewDrivingLicense,
LicenseClassID);


            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               cmxLicenseClasses. Focus();
                return;
            }
            if (clsLicenses.IsLicenseExistByPersonID(ctrPersonDetailsWithFilter2.PersonID, LicenseClassID)){
                MessageBox.Show("Person have already a license with the same applied  driving license! ");
                return;
            }
            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrPersonDetailsWithFilter2.PersonID;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
           _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplications.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.userID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            if (_LocalDrivingLicenseApplication.Save())
            {
                lblID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void applicationInfo_Click(object sender, EventArgs e)
        {
            //if (_Mode == enMode.Update)
               
        }

        private void ctrPersonDetailsWithFilter2_Load(object sender, EventArgs e)
        {

        }

        private void ctrPersonDetailsWithFilter2_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmNewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrPersonDetailsWithFilter2.FilterFocus();
        }
    }
}
