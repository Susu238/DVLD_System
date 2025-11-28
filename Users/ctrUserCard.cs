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
namespace DVLD_System.Users
{
    public partial class ctrUserCard : UserControl
    {
        private clsUser _User;
       private  int  _UserID = -1;
        public int UserID
        {
            get { return _UserID; }
        }
        private void _FillUserInfo()
        {
            ctrPersonDetails1.LoadPerson(_User.PersonID);
            lblUserID.Text = _User.userID.ToString();
            lblUserName.Text =_User.userName;
            if (_User.IsActive)
            {
                lblIsActive.Text = "Yes";

            }
            else
            {
                lblIsActive.Text = "No";
            }


        }
        public void ResetUserInfo()
        {
            ctrPersonDetails1.ResetPersonInfo();
            lblUserID.Text = "[????]";
            lblUserName.Text = "[????]";
            lblIsActive.Text = "[????]";

        }
        public ctrUserCard()
        {
            InitializeComponent();
        }
        public void LoadUser(int UserID)
        {

            _UserID =  UserID;
            _User = clsUser.FindByUserID(UserID);

            if (_User == null)
            {
                ResetUserInfo();
                MessageBox.Show("No user with UserID  =" + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.FindForm()?.Close();
                return;

            }
            _FillUserInfo();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void ctrUserCard_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ctrPersonDetails1_Load(object sender, EventArgs e)
        {

        }
    }
}
