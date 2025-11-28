using DVLD_Business;
using DVLD_System.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DVLD_System.Applications.internationalLicense.Controls
{
    public partial class ctrInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicense;
        public ctrInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }
        public clsInternationalLicense SelectedLicenseInfo
        {
            get
            {
                return _InternationalLicense;
            }
        }
        private void _LoadPersonImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0)
            {
                pbDriverImage.Image = Properties.Resources.Male;
            }
            else
            {
                pbDriverImage.Image = Properties.Resources.femaleIcon;

            }
            string imagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;
            if (imagePath != "")
            {
                if (File.Exists(imagePath))
                {
                    pbDriverImage.Load(imagePath);
                }
                else
                {
                    MessageBox.Show("Could not find this image :" + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            }
        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(InternationalLicenseID);
            if (_InternationalLicense == null)
            {
                MessageBox.Show("Could not find License with ID =" + InternationalLicenseID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }
            lblName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lbliLicenseID.Text =_InternationalLicense.InternationalLicenseID.ToString();
            lblNationalNO.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo.ToString();
            lblGender.Text = _InternationalLicense.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblDate.Text = clsFormat.DateToShort(_InternationalLicense.IssueDate);
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = _InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToString();
            lblDriverID.Text =_InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString();
            _LoadPersonImage();
        }
        private void gbDriverInternationalLicenseInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
