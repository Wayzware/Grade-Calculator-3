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
        private readonly SchoolClass _schoolClass;
        private readonly Main _main;
        private Assignment[] _assignments;
        private Assignment _currentAssignment;

        public Assignments(Main main, SchoolClass schoolClass)
        {
            InitializeComponent();
            _schoolClass = schoolClass;
            _assignments = _schoolClass.assignments;
            _main = main;
            this.Closing += delegate
            {
                _main.SetAssignmentsToNull();
            };
            FillDataView();
            Clear();
            DataGridView.CurrentCellChanged += delegate
            {
                DisplayCellDataInEdit();
            };
        }

        public void Clear()
        {
            TextBox[] TBs = {TextBoxName, TextBoxPoints, TextBoxOutOf, TextBoxMeanPoints};
            foreach(TextBox textBox in TBs)
            {
                textBox.Clear();
            }
            FillCatComboBox();
            RadioButtonReal.Checked = true;
            RadioButtonTheo.Checked = false;
            CheckBoxActive.Checked = true;
        }

        public void FillDataView(bool sendToMain = true)
        {
            DataGridView.Rows.Clear();
            _schoolClass.LoadAssignments();
            _assignments = _schoolClass.assignments;

            //spaghetti code incoming
            if (_schoolClass.curves is null)
            {
                if (_assignments is null) return;
                foreach (Main.DataRow dataRow in _main.DataRows)
                {
                    dataRow.SetDataToEmpty();
                }
                foreach (Assignment assgn in _assignments)
                {
                    DataGridView.Rows.Add(assgn.ToDataView(_schoolClass));
                    if (sendToMain)
                    {
                        _main.AssgnToDataRow(assgn);
                    }
                }

                string arg1, arg2;
                bool arg3;
                (arg1, arg2, arg3) = _schoolClass.GetMeanGrade(_main.DataRows);
                _main.DisplayMean(arg1, arg2, arg3);
                _main.CalculateGrade();
            }
            else
            {
                Assignment[] curvedAssignments;

            }
        }

        private void FillCatComboBox()
        {
            ComboBoxCats.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxCats.Items.Clear();
            ComboBoxCats.Items.AddRange(_schoolClass.catNames);
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            SaveDataInForm();
        }

        private void SaveDataInForm()
        {
            if (VerifyData())
            {
                Assignment temp = new Assignment();
                temp.name = TextBoxName.Text;
                temp.catIndex = GetCatIndex(ComboBoxCats.Text);
                temp.points = Convert.ToDouble(TextBoxPoints.Text);
                temp.outOf = Convert.ToDouble(TextBoxOutOf.Text);
                temp.real = RadioButtonReal.Checked;
                temp.active = CheckBoxActive.Checked;
                temp.meanPoints = 0.0;
                if (ErrorChecking.TextIsType("double", TextBoxMeanPoints.Text))
                    temp.meanPoints = Convert.ToDouble(TextBoxMeanPoints.Text);

                //simple edit of the same assignment
                if (_currentAssignment != null && _currentAssignment.name.Equals(temp.name))
                {
                    XMLHandler.SaveAssignmentToFile(_schoolClass, temp, false);
                }
                else
                {
                    //this assignment name already exists, and it is NOT the one we are editing
                    if (XMLHandler.AssignmentFileExists(_schoolClass, temp))
                    {
                        XMLHandler.SaveAssignmentToFile(_schoolClass, temp);
                        var result = DialogResult.Yes;
                        if (!Settings.AlwaysDeleteOldAssignment)
                        {
                            result = MessageBox.Show("Would you like to delete the assignment with the old name?",
                                "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        }

                        if (result == DialogResult.Yes)
                        {
                            XMLHandler.DeleteAssignment(_schoolClass, _currentAssignment);
                        }
                    }
                    else
                    {
                        XMLHandler.SaveAssignmentToFile(_schoolClass, temp, warning:false);
                    }
                }
                _currentAssignment = temp;
                FillDataView();
                DisplayCellDataInEdit(temp);
            }
        }

        private bool VerifyData(bool warning = true)
        {
            if (TextBoxName.Text.Equals(""))
            {
                if (warning)
                {
                    MessageBox.Show(@"Name is invalid", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            if (ComboBoxCats.Text.Equals(""))
            {
                if (warning)
                {
                    MessageBox.Show(@"No category selected", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            if (!ErrorChecking.TextIsType("double", TextBoxPoints.Text))
            {
                if (warning)
                {
                    MessageBox.Show(@"Points earned is invalid", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            if (!ErrorChecking.TextIsType("double", TextBoxOutOf.Text))
            {
                if (warning)
                {
                    MessageBox.Show(@"Out of is invalid", @"Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }

            if (!ErrorChecking.TextIsType("double", TextBoxMeanPoints.Text))
            {
                if (!TextBoxMeanPoints.Text.Equals("") && warning)
                {
                    MessageBox.Show(@"Mean points earned is invalid, continuing anyway", @"Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return true;
        }

        private int GetCatIndex(string catName)
        {
            int c = 0;
            foreach (string cat in _schoolClass.catNames)
            {
                if (cat.Equals(catName))
                {
                    return c;
                }
                c++;
            }
            throw new ArgumentOutOfRangeException();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            _currentAssignment = null;
            Clear();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            Assignment temp = new Assignment();
            temp.name = TextBoxName.Text;
            if (XMLHandler.AssignmentFileExists(_schoolClass, temp))
            {
                var result = MessageBox.Show("Would you like to delete the assignment " + temp.name + "?",
                    "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    XMLHandler.DeleteAssignment(_schoolClass, temp);
                    if(Settings.WarningLevel > 1) 
                        MessageBox.Show("File deleted!", "Success!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FillDataView();
                }
            }
        }

        private void DisplayCellDataInEdit()
        {
            int row;
            if (DataGridView.CurrentCell != null)
            {
                row = DataGridView.CurrentCell.RowIndex;
            }
            else
            {
                return;
            }

            string name = DataGridView[1, row].Value.ToString();
            foreach (Assignment assgn in _assignments)
            {
                if (assgn.name.Equals(name))
                {
                    _currentAssignment = assgn;
                    FillRequired(assgn);
                    return;
                }
            }
            throw new DataException();
        }

        private void DisplayCellDataInEdit(Assignment assignment)
        {
            int len = DataGridView.RowCount;
            for(int c = 0; c < len; c++)
            {
                if (DataGridView[1, c].Value.ToString().Equals(assignment.name))
                {
                    DataGridView.CurrentCell = DataGridView[1, c];
                    return;
                }
            }
        }

        private void FillRequired(Assignment assgn)
        {
            TextBoxName.Text = assgn.name;
            ComboBoxCats.Text = _schoolClass.catNames[assgn.catIndex];
            TextBoxPoints.Text = assgn.points.ToString();
            TextBoxOutOf.Text = assgn.outOf.ToString();
            RadioButtonReal.Checked = assgn.real;
            RadioButtonTheo.Checked = !assgn.real;
            CheckBoxActive.Checked = assgn.active;
            TextBoxMeanPoints.Text = assgn.meanPoints.ToString();
        }
        
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillDataView();
        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void Assignments_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var _currentClass = _schoolClass;
            _currentClass.curves = new Curve[1];
            _currentClass.curves[0] = new Curve("BigXD");
            _currentClass.curves[0].kept = 1;
            _currentClass.curves[0].appliedCatIndexes = new int[1];
            _currentClass.curves[0].appliedCatIndexes[0] = 1;
            _assignments = _currentClass.curves[0].ApplyAll(_assignments);

            DataGridView.Rows.Clear();

            //spaghetti code incoming
            if (!(_schoolClass.curves is null))
            {
                if (_assignments is null) return;
                foreach (Main.DataRow dataRow in _main.DataRows)
                {
                    dataRow.SetDataToEmpty();
                }
                foreach (Assignment assgn in _assignments)
                {
                    DataGridView.Rows.Add(assgn.ToDataView(_schoolClass));
                    if (true)
                    {
                        _main.AssgnToDataRow(assgn);
                    }
                }

                string arg1, arg2;
                bool arg3;
                (arg1, arg2, arg3) = _schoolClass.GetMeanGrade(_main.DataRows);
                _main.DisplayMean(arg1, arg2, arg3);
                _main.CalculateGrade();
            }
            else
            {
                Assignment[] curvedAssignments;
            }
        }
    }
}
