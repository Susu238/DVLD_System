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
namespace DVLD_System.Applications.Test_Types
{
    public partial class frmEditTestType : Form
    {

        private clsTestTypes.enTestTypes _TestID = clsTestTypes.enTestTypes.VisionTest;
        clsTestTypes _TestType;

        public frmEditTestType(clsTestTypes.enTestTypes testID)
        {
            InitializeComponent();
            _TestID = testID;
        }
        public void LoadData()
        {
            _TestType = clsTestTypes.FindByTestTypeID( _TestID);
            if (_TestType == null)
            {
                MessageBox.Show("No application  with ID = " + _TestID, " Test not found ", MessageBoxButtons.OK);
                this.Close();
                return;
            }
            lblID.Text = ((int)_TestID).ToString();

            txtTitle.Text = _TestType.TestTypeTitle;
            txtDescription.Text = _TestType.TestTypeDescription;
            txtFees.Text = _TestType.TestTypeFees.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _TestType.TestTypeTitle = txtTitle.Text.Trim();
            _TestType.TestTypeDescription= txtDescription.Text.Trim();

            _TestType.TestTypeFees = Convert.ToSingle(txtFees.Text.Trim());


            if (_TestType.Save())
            {

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title can not be empty");

            }
            else
                errorProvider1.SetError(txtTitle, null);

        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "Title can not be empty");

            }
            else
                errorProvider1.SetError(txtDescription, null);

        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees can not be empty");

            }
            else
                errorProvider1.SetError(txtFees, null);
            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Not a valid number");

            }
            else
                errorProvider1.SetError(txtFees, null);

        }
    }
}
