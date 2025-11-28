using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DVLD_Business;
using DVLD_System.Global_Classes;

namespace DVLD_System
{
    public partial class frmAddEditPerson : Form
    {
        //Declare a delegate .

        public delegate void DataBackEventHandler(object sender, int PersonID);
        //declare an event using delegate.
        public event DataBackEventHandler DataBack;
        public enum enMode { AddNew = 0 , Update = 1};
        private enMode _Mode;
        public enum enGender { Male = 0, Female = 1 };
        int _PersonID = -1;
        clsPeopleBusiness _Person;

        public frmAddEditPerson()
        {
            InitializeComponent();
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            _Mode = enMode.AddNew;
           

        }
        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
           
           _Mode = enMode.Update;
            _PersonID = PersonID;

        }
        private void _FillCountriesInComoboBox()
        {
            cbCountry.Items.Clear();
            DataTable dtCountries = clsCountryBusiness.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {

                cbCountry.Items.Add(row["CountryName"]);

            }
        }
        public void _ResetDefaultValues()
        {
            _FillCountriesInComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";

                _Person = new clsPeopleBusiness();
            }
            else
            {
                lblMode.Text = "Update Person";

            }

            rbMale.Checked = true;
            if (rbMale.Checked)
            {
                pbPicture.Image = Properties.Resources.maleIconpng;
            }
            else
            {
                pbPicture.Image = Properties.Resources.femaleIcon;

            }
            llRemoveImage.Visible = (pbPicture.ImageLocation != null);

            dtDate.MaxDate = DateTime.Now.AddYears(-18);
            dtDate.Value = dtDate.MaxDate;
            dtDate.MinDate = DateTime.Now.AddYears(-100);
            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            rbMale.Checked = true;
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";







        }
        public void _LoadData()
        {
            //_FillCountriesInComoboBox();
            _Person = clsPeopleBusiness.Find(_PersonID);
            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person not found ", MessageBoxButtons.OK);
                this.Close();
                return;
            }


            txtNationalNo.Text = _Person.NationalNo;

            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;

            txtLastName.Text = _Person.LastName;
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            txtAddress.Text = _Person.Address;
            dtDate.Value = _Person.DateOfBirth;
            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;


            if (_Person.ImagePath != "")
            {
                pbPicture.ImageLocation = _Person.ImagePath;
            }
            llRemoveImage.Visible = (_Person.ImagePath != "");
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);



        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
           _ResetDefaultValues();
            if (_Mode == enMode.Update)
            {
                _LoadData();
                lblPersonID.Text = _Person.ID.ToString();

            }
        }

        private bool _HandelPersonImage()
        {

            if (_Person.ImagePath != pbPicture.ImageLocation)
            {

                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {

                    }
                }
            }
            if (pbPicture.ImageLocation != null)
            {
                // then we copy the image to the folder after we name it 
                string SourceImageFile = pbPicture.ImageLocation.ToString();
                if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                {

                    pbPicture.ImageLocation = SourceImageFile;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error copying file ", "Error", MessageBoxButtons.OK);
                    return false;
                }



            }
            return true;
        }
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ValidateEmptyTexBox(object sender, CancelEventArgs e)
        {
            // Fierst set AutoValidate property of thr form to Enable Allow Focus change in design
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txtEmailValidating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;
            if (!clsValidation.ValidatEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);

            }
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPeopleBusiness.IsNationalNoExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number  is used  for another person");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }
        public bool SavePerson()
        {
            if (!this.ValidateChildren())
            {
                //here we do not continue because the form is invalid
                MessageBox.Show("Some fields are not valid , put the mouse over the red icon ");
                return false;
            }
            if (!_HandelPersonImage())
            {
                return false;
            }
            int CountryID = clsCountryBusiness.Find(cbCountry.Text).ID;
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtDate.Value;
            _Person.NationalityCountryID = CountryID;
            _Person.Gendor = (short)(rbMale.Checked ? enGender.Male : enGender.Female);
            _Person.ImagePath = pbPicture.ImageLocation ?? "";

            _Person.Mode = (_Mode == enMode.AddNew) ? clsPeopleBusiness.enMode.AddNew : clsPeopleBusiness.enMode.Update;

            if (_Person.Save())
            {
                _PersonID = _Person.ID;
                _Mode = enMode.Update;
                llRemoveImage.Visible = true;
                //MessageBox.Show("Data Saved Successfully.");
                return true;
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.");
                return false ;

            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SavePerson())
            {

                lblPersonID.Text = _Person.ID.ToString();

                _Mode = enMode.Update;

                lblMode.Text = "Update Person";
                MessageBox.Show("Data Saved Successfully .", "Saved", MessageBoxButtons.OK);
                lblNationalNo.Text = _Person.NationalNo.ToString();

                //Tigger the event to send data back to the caller form
                DataBack?.Invoke(this, _Person.ID);
            }
             
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void lblMode_Click(object sender, EventArgs e)
        {

        }

        private void llImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif";
            //openFileDialog1.FilterIndex = 1;
            //openFileDialog1.RestoreDirectory = true;

            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{


            //    // Process the selected file
            //    string selectedFilePath = openFileDialog1.FileName;
            //    pbPicture.Load(selectedFilePath);
            //    llRemoveImage.Visible = true;

            //}
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;

                try
                {
                    // Validate image by opening it
                    using (var imgTemp = Image.FromFile(selectedFilePath))
                    {
                        pbPicture.Image = new Bitmap(imgTemp);
                    }

                    pbPicture.ImageLocation = selectedFilePath;
                    llRemoveImage.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid image file. Please select a valid picture.\n\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPicture.ImageLocation = null;
            if (rbMale.Checked)
                pbPicture.Image = Properties.Resources.maleIconpng;
            else
                pbPicture.Image = Properties.Resources.femaleIcon;

            //currentImagePath = "";
            llRemoveImage.Visible = false;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_Person.ImagePath))
                pbPicture.Image = Properties.Resources.maleIconpng;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_Person.ImagePath))
                pbPicture.Image = Properties.Resources.femaleIcon;
        }

        private void frmAddEditPerson_AutoValidateChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
