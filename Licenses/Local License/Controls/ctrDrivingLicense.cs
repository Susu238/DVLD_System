using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD_System.Global_Classes;

namespace DVLD_System.Licenses.Controls
{
    public partial class ctrDrivingLicense : UserControl
    {
        private int _LicenseID;
        private clsLicenses _License;
        public ctrDrivingLicense()
        {
            InitializeComponent();
            
        }
        public int LicenseID
        {
            get
            {
                return _LicenseID;
            }
        }
        public clsLicenses SelectedLicenseInfo
        {
            get
            {
                return _License;
            }
        }
        private void _LoadPersonImage()
        {
            if(_License.DriverInfo.PersonInfo.Gendor == 0)
            {
                pbDriverImage.Image = Properties.Resources.Male;
            }
            else
            {
                pbDriverImage.Image = Properties.Resources.femaleIcon;

            }
            string imagePath = _License.DriverInfo.PersonInfo.ImagePath;
            if(imagePath != "")
            {
                if (File.Exists(imagePath)){
                    pbDriverImage.Load(imagePath);
                }
                else
                {
                    MessageBox.Show("Could not find this image :" + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }    
            }
        }
        public void LoadInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicenses.FindLicense(LicenseID);
            if (_License == null)
            {
                MessageBox.Show("Could not find License with ID =" + LicenseID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }
            lblClass.Text = _License.ClassInfo.ClassName;
            lblName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNO.Text = _License.DriverInfo.PersonInfo.NationalNo.ToString();
            lblGender.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes == "" ? "No Notes" : _License.Notes;
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = _License.DriverInfo.PersonInfo.DateOfBirth.ToString();
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToString();
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            _LoadPersonImage();
        }


        private void gbDriverLicenseInfo_Enter(object sender, EventArgs e)
        {

        }

        private void ctrDrivingLicense_Load(object sender, EventArgs e)
        {

        }
    }
}
