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
        private Assignment[] _curvedAssignments;
        private Assignment _currentAssignment;
        private Curve _currentCurve;
        private int _currentTab;

        public Assignments(Main main, SchoolClass schoolClass)
        {
            InitializeComponent();
            _schoolClass = schoolClass;
            _assignments = _schoolClass.assignments;
            _main = main;
            _currentTab = 0;
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
            TabsAssignments.SelectedIndexChanged += delegate
            {
                TabChanged();
            };
            CheckedListBoxCategories.ItemCheck += delegate
            {
                FillAssgnCheckedListBox(false);
            };
        }

        private void InitializeUncurvedTab()
        {
            FillDataView();
            Clear();
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

        public void FillDataView(Assignment[] assgns = null, DataGridView dgv = null, bool sendToMain = true)
        {
            Assignment[] assignmentsToUse;
            DataGridView relevantDataGridView;
            if (dgv is null)
            {
                relevantDataGridView = DataGridView;
            }
            else
            {
                relevantDataGridView = DataGridViewCurved;
            }
            relevantDataGridView.Rows.Clear();
            if (assgns is null)
            {
                _schoolClass.LoadAssignments();
                _assignments = _schoolClass.assignments;
                assignmentsToUse = _assignments;
            }
            else
            {
                _schoolClass.ApplyCurves();
                _curvedAssignments = _schoolClass.curvedAssignments;
                assignmentsToUse = _curvedAssignments;
            }

            //code has been unspaghettied
            if (assignmentsToUse is null) return;
            foreach (Main.DataRow dataRow in _main.DataRows)
            {
                dataRow.SetDataToEmpty();
            }
            foreach (Assignment assgn in assignmentsToUse)
            {
                relevantDataGridView.Rows.Add(assgn.ToDataView(_schoolClass));
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

        private void TabChanged()
        {
            switch (_currentTab)
            {
                case 1 when TabsAssignments.SelectedTab == TabUncurved:
                case 2 when TabsAssignments.SelectedTab == TabCurved:
                    return;
            }

            if (TabsAssignments.SelectedTab == TabCurved)
            {
                _currentTab = 2;
                InitializeCurvedTab(true);
            }
            else
            {
                _currentTab = 1;
                InitializeUncurvedTab();
            }
        }

        private void InitializeCurvedTab(bool init = false)
        {
            FillCatCheckedListBox();
            DisplayCurvedData();
        }

        private void DisplayCurvedData()
        {
            _schoolClass.ApplyCurves();
            _curvedAssignments = _schoolClass.curvedAssignments;
            FillDataView(_curvedAssignments, dgv: DataGridViewCurved, sendToMain: true);
        }

        private void FillCatCheckedListBox(bool fillAssgns = true, bool init = false)
        {
            CheckedListBoxCategories.Items.Clear();
            foreach (string catName in _schoolClass.catNames)
            {
                CheckedListBoxCategories.Items.Add(catName);
            }
            if(fillAssgns) FillAssgnCheckedListBox(init);
        }

        private void FillAssgnCheckedListBox(bool init)
        {

            string[] prevUnchecked = new string[0];
            foreach (var item in CheckedListBoxAssignments.Items)
            {
                if (!CheckedListBoxAssignments.CheckedItems.Contains(item))
                {
                    Array.Resize(ref prevUnchecked, prevUnchecked.Length + 1);
                    prevUnchecked[prevUnchecked.Length - 1] = item.ToString();
                }
            }
            CheckedListBoxAssignments.Items.Clear();
            string[] assgnsInCats = new string[0];



            foreach (var item in CheckedListBoxCategories.CheckedIndices)
            {
                int index = Convert.ToInt32(item);
                Assignment[] tempAssgns = _schoolClass.GetAssgnsInCat(index);
                foreach (Assignment assgn in tempAssgns)
                {
                    Array.Resize(ref assgnsInCats, assgnsInCats.Length + 1);
                    assgnsInCats[assgnsInCats.Length - 1] = assgn.name;
                }
            }
            foreach (string assgnName in assgnsInCats)
            {
                CheckedListBoxAssignments.Items.Add(assgnName);
                CheckedListBoxAssignments.SetItemChecked(CheckedListBoxAssignments.Items.Count - 1,
                    !prevUnchecked.Contains(assgnName));
            }
        }

        internal class ErrorDisplay
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ButtonCurveSave_Click(object sender, EventArgs e)
        {

        }
    }
}
