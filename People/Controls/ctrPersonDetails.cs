using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using System.IO;

namespace DVLD_System
{
    public partial class ctrPersonDetails : UserControl
    {

        private clsPeopleBusiness _Person;
       private int _PersonID = -1 ;

        public int PersonID
        {
            get { return _PersonID; }
        }
        public clsPeopleBusiness selectedPersonInfo
        {
            get { return _Person; }
        }
        public ctrPersonDetails()
        {
            InitializeComponent();
            //_PersonID = _Person.ID;


        }
        private string currentImagePath = "";
        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 0)
                pbPiture.BackgroundImage = Properties.Resources.maleIconpng;
            else
                pbPiture.BackgroundImage = Properties.Resources.femaleIcon;

            string ImagePath = _Person.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPiture.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image :" + ImagePath, "Error", MessageBoxButtons.OK);

        }
        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.ID;
            lblPersonID.Text = _Person.ID.ToString();
            lblName.Text = $"{_Person.FirstName} {_Person.SecondName} {_Person.ThirdName} {_Person.LastName}";
            lblNationalNo.Text = _Person.NationalNo;
            panel3.BackgroundImage = _Person.Gendor == 0
        ? Properties.Resources.maleIconpng
        : Properties.Resources.femaleIcon;
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";

            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString();
            lblPhone.Text = _Person.Phone;
            //if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            //{
            //    pbPiture.ImageLocation = _Person.ImagePath;
            //    currentImagePath = _Person.ImagePath;
            //}
            //else
            //{
            //    currentImagePath = "";
            //}
            lblCountry.Text = clsCountryBusiness.Find(_Person.NationalityCountryID).CountryName;
            _LoadPersonImage();
            //var country = clsCountryBusiness.Find(_Person.NationalityCountryID);
            //lblCountry.Text = (country != null && !string.IsNullOrEmpty(country.CountryName))
            //    ? country.CountryName
            //    : "Unknown Country";



        }
        public void ResetPersonInfo() {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            panel3.BackgroundImage =
          Properties.Resources.maleIconpng;
         
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
           pbPiture.BackgroundImage =  Properties.Resources.Name;

        }
        public void LoadPerson(int PersonID)
        {


            _Person = clsPeopleBusiness.Find(PersonID);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No person with PersonID  ="+ PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.FindForm()?.Close();
                return;

            }
            _FillPersonInfo();

               
        }

        public void LoadPerson(string NationalNo)
        {


            _Person = clsPeopleBusiness.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No National Number with NationalNo =" + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.FindForm()?.Close();
                return;

            }
            _FillPersonInfo();


        }
        private void ctrPersonDetails_Load(object sender, EventArgs e)
        {

        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Person != null)
            {

                _PersonID = _Person.ID;

                frmAddEditPerson frm = new frmAddEditPerson(_PersonID);

                frm.ShowDialog();

                LoadPerson(_PersonID);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }
    }
}
