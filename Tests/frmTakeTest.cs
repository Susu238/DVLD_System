using DVLD_Business;
using DVLD_System.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _TestID = -1;
        private clsTestTypes.enTestTypes _TestType;
        private int _TestAppointmentID = -1;
        private clsTests _Test;
        public frmTakeTest(int TestAppointmentID,clsTestTypes.enTestTypes TestType)
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _TestType = TestType;
     

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
             ctrScheduledTest1.TestType = _TestType;
            ctrScheduledTest1.LoadInfo(_TestAppointmentID);
            if(ctrScheduledTest1.TestAppointmentID == -1)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
            _TestID = ctrScheduledTest1.TestId;
            if(_TestID != -1)
            {
                _Test = clsTests.Find(_TestID);
                if (_Test.TestResult)

                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                txtNotes.Text = _Test.Notes;
                lblResultMessage.Visible = true;
                rbPass.Enabled = false;
                rbFail.Enabled = false;

            }
            else
            {
                _Test = new clsTests();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                "Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            _Test.TestAppointmentID = _TestAppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.userID;
            _Test.Notes = txtNotes.Text.Trim();
            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully ", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Data not Saved !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrScheduledTest1_Load(object sender, EventArgs e)
        {

        }
    }
}
