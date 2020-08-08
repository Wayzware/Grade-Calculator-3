using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Grade_Calculator_3
{
    public partial class Assignments : Form
    {
        private readonly SchoolClass _schoolClass;
        private readonly Main _main;
        private Assignment[] _assignments;
        private Assignment _currentAssignment;
        public int currentTab;

        public Assignments(Main main, SchoolClass schoolClass)
        {
            InitializeComponent();
            _schoolClass = schoolClass;
            _assignments = _schoolClass.assignments;
            _main = main;
            currentTab = 0;
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
            CheckedListBoxCurves.SelectedIndexChanged += delegate
            {

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
            relevantDataGridView = dgv is null ? DataGridView : DataGridViewCurved;
            relevantDataGridView.Rows.Clear();
            if (assgns is null)
            {
                _schoolClass.LoadAssignments();
                _assignments = _schoolClass.assignments;
                assignmentsToUse = _assignments;
            }
            else
            {
                assignmentsToUse = assgns;
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
            (arg1, arg2, arg3) = _schoolClass.GetMeanGrade(_main.DataRows, assignmentsToUse);
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

        private void Assignments_Load(object sender, EventArgs e)
        {
        }

        private void TabChanged()
        {
            switch (currentTab)
            {
                case 1 when TabsAssignments.SelectedTab == TabUncurved:
                case 2 when TabsAssignments.SelectedTab == TabCurved:
                    return;
            }

            if (TabsAssignments.SelectedTab == TabCurved)
            {
                currentTab = 2;
                InitializeCurvedTab(true);
            }
            else
            {
                currentTab = 1;
                InitializeUncurvedTab();
            }
        }

        private void ClearCurvedForm(bool clearName = true, bool deselect = true)
        {
            TextBox[] textBoxes =
            {
                TextBoxConDropPercent, TextBoxConDropPoints, TextBoxCurveToMean, TextBoxAdditivePercent,
                TextBoxAdditivePoints, TextBoxMultiplicative
            };
            RadioButton[] radioButtons =
            {
                RadioButtonDrop, RadioButtonKeep, RadioButtonConDropPercent, RadioButtonConDropPoints,
                RadioButtonCurveToMean, RadioButtonAdditivePercent, RadioButtonAdditivePoints, RadioButtonMultiplicative
            };
            NumericUpDown[] numericUpDowns = { NumericUpDownDrop, NumericUpDownKeep };

            foreach (TextBox tb in textBoxes)
            {
                tb.Text = "";
            }
            foreach (RadioButton rb in radioButtons)
            {
                rb.Checked = false;
            }
            foreach (NumericUpDown nUD in numericUpDowns)
            {
                nUD.Value = 0;
            }
            if (clearName)
            {
                TextBoxCurveName.Text = "";
                CheckBoxCurveActive.Checked = true;
            }
            if(deselect) CheckedListBoxCurves.ClearSelected();
        }

        private void InitializeCurvedTab(bool init = false)
        {
            ClearCurvedForm();
            _schoolClass.LoadCurves();
            DisplayCurvedData();
            FillCatCheckedListBox();
            FillCurvesCheckedListBox();
            CheckedListBoxAssignments.Items.Clear();
        }

        private void DisplayCurvedData()
        {
            _schoolClass.ApplyCurves();
            FillDataView(_schoolClass.curvedAssignments, dgv: DataGridViewCurved, sendToMain: true);
        }

        private void FillCatCheckedListBox()
        {
            CheckedListBoxCategories.Items.Clear();
            foreach (string catName in _schoolClass.catNames)
            {
                CheckedListBoxCategories.Items.Add(catName);
            }
        }

        private void FillAssgnCheckedListBox(IEnumerable<string> activeCats)
        {
            List<string> prevUnchecked = new List<string>();
            List<string> prevExisted = new List<string>();
            foreach (var item in CheckedListBoxAssignments.Items)
            {
                prevExisted.Add(item.ToString());
                if (!CheckedListBoxAssignments.CheckedItems.Contains(item))
                {
                    prevUnchecked.Add(item.ToString());
                }
            }
            CheckedListBoxAssignments.Items.Clear();

            List<string> assgnsInCats = new List<string>();

            foreach (var item in activeCats)
            {
                int index = _schoolClass.CatExists(item);
                Assignment[] tempAssgns = _schoolClass.GetAssgnsInCat(index);
                foreach (Assignment assgn in tempAssgns)
                {
                    assgnsInCats.Add(assgn.name);
                }
            }
            foreach (string assgnName in assgnsInCats)
            {
                CheckedListBoxAssignments.Items.Add(assgnName);
                if (prevExisted.Contains(assgnName))
                {
                    CheckedListBoxAssignments.SetItemChecked(CheckedListBoxAssignments.Items.IndexOf(assgnName),
                        !prevUnchecked.Contains(assgnName));
                }
                else
                {
                    CheckedListBoxAssignments.SetItemChecked(CheckedListBoxAssignments.Items.IndexOf(assgnName), true);
                }
            }
        }

        private void InitAssgnCheckedListBox(string curveName)
        {
            CheckedListBoxAssignments.Items.Clear();

            Curve curve = _schoolClass.curves[_schoolClass.CurveExists(curveName)];
            List<string> assgnsInCats = new List<string>();
            foreach (int index in curve.appliedCatIndexes)
            {
                Assignment[] tempAssgns = _schoolClass.GetAssgnsInCat(index);
                foreach (Assignment assgn in tempAssgns)
                {
                    assgnsInCats.Add(assgn.name);
                }
            }
            foreach (string assgnName in assgnsInCats)
            {
                CheckedListBoxAssignments.Items.Add(assgnName);
                CheckedListBoxAssignments.SetItemChecked(CheckedListBoxAssignments.Items.IndexOf(assgnName),
                    curve.appliedAssgnNames.Contains(assgnName));
            }
        }

        private void FillCurvesCheckedListBox()
        {
            CheckedListBoxCurves.Items.Clear();
            if (_schoolClass.curves is null) return;
            foreach (Curve curve in _schoolClass.curves)
            {
                CheckedListBoxCurves.Items.Add(curve.name);
                CheckedListBoxCurves.SetItemChecked(CheckedListBoxCurves.Items.IndexOf(curve.name), curve.active);
            }
        }

        private void ButtonCurveSave_Click(object sender, EventArgs e)
        {
            SaveCurve();
        }

        private void ButtonCurveHelp_Click(object sender, EventArgs e)
        {
            
        }

        private void CheckedListBoxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCatComboBox();
        }

        private void CheckedListBoxCategories_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<string> checkedItems = new List<string>();
            foreach (var item in CheckedListBoxCategories.CheckedItems)
                checkedItems.Add(item.ToString());
            if (e.NewValue == CheckState.Checked)
                checkedItems.Add(CheckedListBoxCategories.Items[e.Index].ToString());
            else
                checkedItems.Remove(CheckedListBoxCategories.Items[e.Index].ToString());
            FillAssgnCheckedListBox(checkedItems);
        }

        private void ButtonCurveNew_Click(object sender, EventArgs e)
        {
            ClearCurvedForm(true, true);
            FillCatCheckedListBox();
            FillAssgnCheckedListBox(new string[0]);
        }

        private void ButtonCurveClear_Click(object sender, EventArgs e)
        {
            ClearCurvedForm(false, false);
            FillCatCheckedListBox();
            FillAssgnCheckedListBox(new string[0]);
        }

        private void SaveCurve(bool warning = true)
        {
            //everything in the required group box
            if (TextBoxCurveName.Text.StartsWith("$$$ADJUST$$$"))
            {
                MessageBox.Show(
                    "You sly dog! You can't start a curve with \"$$$ADJUST$$$\" because it has a special meaning" +
                    " in this program.", "Someone read the code!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            Curve newCurve = new Curve(TextBoxCurveName.Text);
            newCurve.active = CheckBoxCurveActive.Checked;
            List<int> enabledCatIndexes = new List<int>();
            foreach (var item in CheckedListBoxCategories.CheckedIndices)
            {
                enabledCatIndexes.Add(Convert.ToInt32(item));
            }
            newCurve.appliedCatIndexes = enabledCatIndexes.ToArray();
            List<string> enabledAssgns = new List<string>();
            foreach (var item in CheckedListBoxAssignments.CheckedItems)
            {
                enabledAssgns.Add(item.ToString());
            }
            newCurve.appliedAssgnNames = enabledAssgns.ToArray();

            //the actual curve method
            if (RadioButtonDrop.Checked)
            {
                newCurve.kept = -1 * (int) NumericUpDownDrop.Value;
                if (!Settings.unrestrictedCurves)
                {
                    if (enabledCatIndexes.Count > 1)
                    {
                        MessageBox.Show("This curve can only be applied to 1 category. Cannot apply/save curve.", "Error!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else if (RadioButtonKeep.Checked)
            {
                newCurve.kept = (int) NumericUpDownKeep.Value;
                if (!Settings.unrestrictedCurves)
                {
                    if (enabledCatIndexes.Count > 1)
                    {
                        MessageBox.Show("This curve can only be applied to 1 category. Cannot apply/save curve.", "Error!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else if (RadioButtonConDropPercent.Checked)
            {
                double val;
                if (Double.TryParse(TextBoxConDropPercent.Text, out val))
                {
                    newCurve.conDropPercent = val;
                }
                else
                {
                    MessageBox.Show("Value entered for drop if below % is invalid.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else if (RadioButtonConDropPoints.Checked)
            {
                double val;
                if (double.TryParse(TextBoxConDropPoints.Text, out val))
                {
                    newCurve.conDropPoints = val;
                }
                else
                {
                    MessageBox.Show("Value entered for drop if below x points is invalid.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else if (RadioButtonCurveToMean.Checked)
            {
                double val;
                if (double.TryParse(TextBoxCurveToMean.Text, out val))
                {
                    newCurve.goalMeanPercent = val;
                }
                else
                {
                    MessageBox.Show("Value entered for curve to mean % is invalid.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (!Settings.unrestrictedCurves)
                {
                    if (enabledCatIndexes.Count > 1)
                    {
                        MessageBox.Show("This curve can only be applied to 1 category. Cannot apply/save curve.", "Error!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else if (RadioButtonAdditivePercent.Checked)
            {
                double val;
                if (double.TryParse(TextBoxAdditivePercent.Text, out val))
                {
                    newCurve.additivePercent = val / 100;
                }
                else
                {
                    MessageBox.Show("Value entered for add x % is invalid.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else if (RadioButtonAdditivePoints.Checked)
            {
                double val;
                if (double.TryParse(TextBoxAdditivePoints.Text, out val))
                {
                    newCurve.additive = val;
                }
                else
                {
                    MessageBox.Show("Value entered for add x points is invalid.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else if (RadioButtonMultiplicative.Checked)
            {
                double val;
                if (double.TryParse(TextBoxMultiplicative.Text, out val))
                {
                    newCurve.multiplicative = val;
                }
                else
                {
                    MessageBox.Show("Value entered for multiply by x is invalid.", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            else if(!Settings.unrestrictedCurves)
            {
                MessageBox.Show("No curve method selected. Cannot save.", "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if ( _schoolClass.CurveExists(newCurve.name) != -1) //if the curve will be overwritten
            {
                var result = DialogResult.Yes;
                if(warning) result = MessageBox.Show("Data already exists for " + newCurve.name + ". Overwrite the file?",
                        "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    XMLHandler.DeleteCurve(_schoolClass, newCurve);
                }
                else
                {
                    return;
                }
            }
            try
            {
                XMLHandler.SaveCurveToFile(_schoolClass, newCurve, true);
            }
            catch
            {
                if(warning) MessageBox.Show("Curve name is not valid.", "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            _schoolClass.LoadCurves();
            DisplayCurvedData();
            FillCurvesCheckedListBox();
            CheckedListBoxCurves.SetSelected(CheckedListBoxCurves.Items.IndexOf(newCurve.name), true);
        }

        private void CheckedListBoxCurves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(CheckedListBoxCurves.SelectedItem is null))
            {
                string workaround = CheckedListBoxCurves.SelectedItem.ToString();
                FillCurveForm(_schoolClass.curves[_schoolClass.CurveExists(CheckedListBoxCurves.SelectedItem.ToString())]);
                InitAssgnCheckedListBox(workaround);
            }
        }
        
        private void FillCurveForm(Curve curve)
        {
            ClearCurvedForm();
            TextBoxCurveName.Text = curve.name;
            CheckBoxCurveActive.Checked = curve.active;
            FillCatCheckedListBox();
            List<string> appliedCatNames = new List<string>();
            foreach (int x in curve.appliedCatIndexes)
            {
                appliedCatNames.Add(_schoolClass.catNames[x]);
            }
            FillAssgnCheckedListBox(appliedCatNames);
            if (curve.kept > 0)
            {
                RadioButtonKeep.Checked = true;
            }
            else if (curve.kept < 0)
            {
                RadioButtonDrop.Checked = true;
            }
            else if (curve.conDropPercent != 0)
            {
                RadioButtonConDropPercent.Checked = true;
            }
            else if (curve.conDropPoints != 0)
            {
                RadioButtonConDropPoints.Checked = true;
            }
            else if (curve.goalMeanPercent != 0)
            {
                RadioButtonCurveToMean.Checked = true;
            }
            else if (curve.additivePercent != 0)
            {
                RadioButtonAdditivePercent.Checked = true;
            }
            else if (curve.additive != 0)
            {
                RadioButtonAdditivePercent.Checked = true;
            }
            else if (curve.multiplicative != 0)
            {
                RadioButtonMultiplicative.Checked = true;
            }


            if (RadioButtonDrop.Checked)
            {
                NumericUpDownDrop.Value = curve.kept * -1;
            }
            else if (RadioButtonKeep.Checked)
            {
                NumericUpDownKeep.Value = curve.kept;
            }
            else if (RadioButtonConDropPercent.Checked)
            {
                TextBoxConDropPercent.Text = curve.conDropPercent.ToString();
            }
            else if (RadioButtonConDropPoints.Checked)
            {
                TextBoxConDropPoints.Text = curve.conDropPoints.ToString();
            }
            else if (RadioButtonCurveToMean.Checked)
            {
                TextBoxCurveToMean.Text = curve.goalMeanPercent.ToString();
            }
            else if (RadioButtonAdditivePercent.Checked)
            {
                TextBoxAdditivePercent.Text = (100 * curve.additivePercent).ToString();
            }
            else if (RadioButtonAdditivePoints.Checked)
            {
                TextBoxAdditivePoints.Text = curve.additive.ToString();
            }
            else if (RadioButtonMultiplicative.Checked)
            {
                TextBoxMultiplicative.Text = curve.multiplicative.ToString();
            }

            foreach (int x in curve.appliedCatIndexes)
            {
                CheckedListBoxCategories.SetItemChecked(x, true);
            }
        }

        private void ButtonCurveDelete_Click(object sender, EventArgs e)
        {
            if (_schoolClass.CurveExists(TextBoxCurveName.Text) == -1) return;
            var result = MessageBox.Show("Delete data for " + TextBoxCurveName.Text + "?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                XMLHandler.DeleteCurve(_schoolClass, _schoolClass.curves[_schoolClass.CurveExists(TextBoxCurveName.Text)]);
            }
            InitializeCurvedTab();
        }
    }

}
