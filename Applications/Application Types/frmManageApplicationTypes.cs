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
using DVLD_System.Applications.Application_Types;
namespace DVLD_System
{
    public partial class frmManageApplicationTypes : Form
    {

        private static DataTable _dtAllApplicationTypes;
        private void _RefreshApplicationsList()
        {
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();

            dgvAllApplications.DataSource = _dtAllApplicationTypes;

            lblRecord.Text = _dtAllApplicationTypes.Rows.Count.ToString();

        }

        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvAllApplications.DataSource = _dtAllApplicationTypes;
            lblRecord.Text = _dtAllApplicationTypes.Rows.Count.ToString();
            dgvAllApplications.Columns[0].HeaderText = "ID";
            dgvAllApplications.Columns[0].Width = 110;
            dgvAllApplications.Columns[1].HeaderText = "Title";
            dgvAllApplications.Columns[1].Width = 400;
            dgvAllApplications.Columns[2].HeaderText = "Fees";
            dgvAllApplications.Columns[2].Width = 100;





        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
          frmEditApplicationTypes frm = new frmEditApplicationTypes((int)dgvAllApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshApplicationsList();
        }
    }
}
