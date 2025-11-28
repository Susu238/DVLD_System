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
using DVLD_System.Applications.Test_Types;
namespace DVLD_System
{
    public partial class frmTestTypesList : Form
    {
        private static DataTable _dtAllTestTypes;
        private void _RefreshTestsList()
        {
            _dtAllTestTypes = clsTestTypes.GetAllTestTypes();

            dgvAllTestTypes.DataSource = _dtAllTestTypes;

            lblRecord.Text = _dtAllTestTypes.Rows.Count.ToString();

        }
        public frmTestTypesList()
        {
            InitializeComponent();
        }

        private void frmTestTypesList_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestTypes.GetAllTestTypes();
            dgvAllTestTypes.DataSource = _dtAllTestTypes;
            lblRecord.Text = _dtAllTestTypes.Rows.Count.ToString();
            dgvAllTestTypes.Columns[0].HeaderText = "ID";
            dgvAllTestTypes.Columns[0].Width = 110;
            dgvAllTestTypes.Columns[1].HeaderText = "Title";
            dgvAllTestTypes.Columns[1].Width = 150;
            dgvAllTestTypes.Columns[2].HeaderText = "Description";
            dgvAllTestTypes.Columns[2].Width= 300;

            dgvAllTestTypes.Columns[3].HeaderText = "Fees";
            dgvAllTestTypes.Columns[3].Width = 100;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType frm = new frmEditTestType((clsTestTypes.enTestTypes)dgvAllTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshTestsList();
        }
    }
}
