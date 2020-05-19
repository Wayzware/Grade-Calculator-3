using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grade_Calculator_3
{
    public partial class Assignments : Form
    {
        private SchoolClass _schoolClass;
        private Main _main;
        private Assignment[] _assignments;

        public Assignments(Main main, SchoolClass schoolClass)
        {
            _schoolClass = schoolClass;
            _assignments = _schoolClass.assignments;
            _main = main;
            InitializeComponent();
            FillDataView();
        }

        public void ResetAddEdit()
        {

        }

        public void FillDataView()
        {
            if (!(_assignments is null))
            {
                foreach (Assignment assgn in _assignments)
                {
                    DataGridView.Rows.Add(assgn.ToDataView(_schoolClass));
                }
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Assignment test = new Assignment();
            test.active = true;
            test.name = "Test";
            test.catIndex = 1;
            test.points = 5;
            test.outOf = 6;
            test.real = true;
            test.meanPoints = 1;
            XMLHandler.SaveAssignmentToFile(_schoolClass, test, true);
        }
    }
}
