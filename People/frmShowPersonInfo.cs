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
namespace DVLD_System
{
    public partial class frmShowPersonInfo : Form
    {
        clsPeopleBusiness _Person;
        int _PersonID;
        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
           ctrPersonDetails1.LoadPerson(  PersonID);
            

        }
        public frmShowPersonInfo(string NationalNo)
        {
            InitializeComponent();
            ctrPersonDetails1.LoadPerson(NationalNo);


        }

        private void frmPersonDetails_Load(object sender, EventArgs e)
        {
         


            //ctrPersonDetails1.LoadPerson(_PersonID);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
