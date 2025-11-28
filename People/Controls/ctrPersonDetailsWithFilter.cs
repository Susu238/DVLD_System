using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Controls
{
    public partial class ctrPersonDetailsWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        //create a method to raise the event with parameter
        protected virtual void PersonSelected(int PersonID)
        {
            Action <int> handler = OnPersonSelected;
            if (handler != null) {
                handler(PersonID);// raise the event with parameter

            }
        }
        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get { return _ShowAddPerson; }
            set { _ShowAddPerson = value;
                btnAddPerson.Visible = _ShowAddPerson;
            }
        }
        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set { _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        public ctrPersonDetailsWithFilter()
        {
            InitializeComponent();
        }
        private int _PersonID = -1;
        public int PersonID
        {
            get { return ctrPersonDetails1.PersonID; }
        }
        public clsPeopleBusiness SelectedPersonInfo
        {
            get { return ctrPersonDetails1.selectedPersonInfo; }
        }
        public void LoadPersonInfo(int PersonID)
        {
            cbxFilter.SelectedIndex = 1;
            txtFilter.Text = PersonID.ToString();
            FindNow();
        }
        private void FindNow()
        {
            switch (cbxFilter.Text)
            {
                case "Person ID" :
                    ctrPersonDetails1.LoadPerson(int.Parse(txtFilter.Text));
                    break;
                case "National No":
                    ctrPersonDetails1.LoadPerson(txtFilter.Text);
                    break;
                default:
                    break;

            }
            if(OnPersonSelected != null && FilterEnabled)
            {
                OnPersonSelected(ctrPersonDetails1.PersonID);
            }
            }
        private void ctrPersonDetailsWithFilter_Load(object sender, EventArgs e)
        {
            
           
        }

        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            txtFilter.Text = "";
            txtFilter.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields arenot valid, put the mouse over the red icon");

                return;
            }
            FindNow();
        }

        private void ctrPersonDetails1_Load(object sender, EventArgs e)
        {
            if(cbxFilter.Items.Count > 0 &&  cbxFilter.SelectedIndex == -1)
                cbxFilter.SelectedIndex = 0;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilter.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilter, "This field is required!");


            }
            else
            {
                errorProvider1.SetError(txtFilter, null);
            }

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm1 = new frmAddEditPerson();
            frm1.DataBack += DataBackEvent;
            frm1.ShowDialog();
        }
        private void DataBackEvent(object sender, int PersonID)
        {
            //Handle the data recieved
            cbxFilter.SelectedIndex = 1;
            txtFilter.Text = PersonID.ToString();

            ctrPersonDetails1.LoadPerson(PersonID);
        }
        public void FilterFocus()
        {
            txtFilter.Focus();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //check if the pressed key is Enter(character 13)
            if (e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }
            //this will allow only digit if person id is selected
            if (cbxFilter.Text == "Person ID")
                e.Handled = ! char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void gbFilter_Enter(object sender, EventArgs e)
        {

        }
    }
}
