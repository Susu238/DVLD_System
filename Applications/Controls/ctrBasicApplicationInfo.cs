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
using DVLD_System.Global_Classes;
namespace DVLD_System.Applications.Controls
{
    public partial class ctrBasicApplicationInfo : UserControl
    {
        private clsApplications Application;
        private int _ApplicatinID = -1;
        public int ApplicationID
        {
            get
            {
                return _ApplicatinID;
            }
        }
        public void LoadApplicationInfo(int applicationID)
        {
            Application = clsApplications.FindBaseApplication(applicationID);
            if(Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with application ID " + applicationID.ToString(), "Error", MessageBoxButtons.OK);
            }
            _FillApplicationInfo();
        }
        public void ResetApplicationInfo()
        {
            _ApplicatinID = -1;
            lblID.Text = "[???]";
            lblStatus.Text = "[???]";
            lblFees.Text = "[???]";
            lblApplicantt.Text = "[???]";
            lblType.Text = "[???]";
            lblDate.Text = "[???]";
            lblStatusDate.Text = "[???]";
            lblCreatedBy.Text = "[???]";

        }
        private void _FillApplicationInfo()
        {
            _ApplicatinID = Application.ApplicationID;
            lblID.Text = Application.ApplicationID.ToString();
            lblStatus.Text = Application.StatusText;
            lblType.Text = Application.ApplicationTypeInfo.ApplicationTypeTitle;
            lblFees.Text = Application.PaidFees.ToString();
            lblApplicantt.Text = Application.ApplicantFullName;
            lblDate.Text = clsFormat.DateToShort(Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(Application.LastStatusDate);
            lblCreatedBy.Text = Application.CreatedByUserInfo.userName;
        }
        public ctrBasicApplicationInfo()
        {
            InitializeComponent();
        }

        private void ctrBasicApplicationInfo_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(Application.ApplicantPersonID);
            frm.ShowDialog();
            LoadApplicationInfo(_ApplicatinID);
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }
    }
}
