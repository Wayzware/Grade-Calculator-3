using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace Grade_Calculator_3
{
    public partial class Main : Form
    {
        private const int PageLen = 5;
        public int currentPage, pages;
        public DataRow[] DataRows;
        private SchoolClass _currentClass;
        private AddPoints[] _addPoints;
        private Assignments _assignments;
        public bool blank = true;


        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            XMLHandler.DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Grade Calculator/";
            comboBoxClasses.DropDownStyle = ComboBoxStyle.DropDownList;
            InitialSetup();
        }

        public void InitialSetup()
        {
            currentPage = 1;
            pages = 1;
            _currentClass = null;
            _addPoints = new AddPoints[0];
            advancedModeToolStripMenuItem.Enabled = false;
            LoadClassData("");
            LockWeights(false);
            ChangeInputMode(1);
            ClearInput();
            RefreshClassList();
            LoadAllAssignments();
        }

        private void addClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClass addClassForm = new AddClass(this);
            addClassForm.Show();
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        public void RefreshClassList(bool refreshDependents=true)
        {
            XMLHandler.Data = XMLHandler.ReadSchoolClasses();
            comboBoxClasses.Items.Clear();
            if(XMLHandler.Data == null)
            {
                return;
            }
            foreach(SchoolClass schoolClass in XMLHandler.Data)
            {
                comboBoxClasses.Items.Add(schoolClass.className);
            }

            if (refreshDependents)
            {
                RefreshEditClass();
                RefreshRemoveClass();
            }
        }

        private void RefreshEditClass()
        {
            editClassToolStripMenuItem.DropDownItems.Clear();
            if (XMLHandler.Data == null)
            {
                editClassToolStripMenuItem.Enabled = false;
                return;
            }
            editClassToolStripMenuItem.Enabled = true;
            foreach (SchoolClass schoolClass in XMLHandler.Data)
            {
                ToolStripItem temp = editClassToolStripMenuItem.DropDownItems.Add(schoolClass.className);
                temp.Click += delegate
                {
                    EditClassHandler(temp.Text);
                };
            }
        }

        private void RefreshRemoveClass()
        {
            removeClassToolStripMenuItem.DropDownItems.Clear();
            if (XMLHandler.Data == null)
            {
                removeClassToolStripMenuItem.Enabled = false;
                return;
            }
            removeClassToolStripMenuItem.Enabled = true;
            foreach (SchoolClass schoolClass in XMLHandler.Data)
            {
                ToolStripItem temp = removeClassToolStripMenuItem.DropDownItems.Add(schoolClass.className);
                temp.Click += delegate
                {
                    DeleteClassHandler(temp.Text);
                };
            }
        }

        private void comboBoxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            blank = false;
            advancedModeToolStripMenuItem.Enabled = true;
            CloseAllAddWindows();
            ClearInput();
            LoadClassData(comboBoxClasses.Text);
            _assignments?.Close();
            _assignments = null;
            LabelMeanZero.Visible = false;
            if (advancedModeToolStripMenuItem.Checked)
            {
                _assignments = new Assignments(this, _currentClass);
                _assignments.Show();
            }
            LockWeights(true);
        }

        private void LockWeights(bool locked)
        {
            TextBox[] wBoxes = {TextBoxW1, TextBoxW2, TextBoxW3, TextBoxW4, TextBoxW5};
            foreach (TextBox t in wBoxes)
            {
                t.ReadOnly = locked;
            }
        }

        public void LoadClassData(string className)
        {
            TextBox[] catTextBoxes = { TextBoxCat1, TextBoxCat2, TextBoxCat3, TextBoxCat4, TextBoxCat5 };

            //if the class is blank (initialization), add PAGE_LEN blank categories to DataRows
            if (className.Equals(""))
            {
                DataRows = new DataRow[PageLen];
                int c = 0;
                foreach(DataRow DataRow in DataRows)
                {
                    DataRow temp = new DataRow();
                    temp.SetAllToEmpty();
                    DataRows[c] = temp;
                    c++;
                }
                SetVisibility(PageLen, 1);
            }
            else
            {
                bool found = false;
                int c = 0;
                SchoolClass workingClass = null;
                while (!found && c != XMLHandler.Data.Length)
                {
                    workingClass = XMLHandler.Data[c];
                    if (workingClass.className.Equals(className))
                    {
                        found = true;
                        _currentClass = workingClass;
                    }
                    c++;
                }
                if (!found)
                {
                    throw new ArgumentException("The class selected does not exist. This should never happen.");
                }

                //fill DataRows with category data
                DataRows = new DataRow[_currentClass.catNames.Length];

                c = 0;
                foreach(DataRow dataRow in DataRows)
                {
                    DataRows[c] = new DataRow();
                    DataRows[c].SetAllToEmpty();
                    DataRows[c].CatName = workingClass.catNames[c];
                    DataRows[c].Weight = workingClass.catWorths[c].ToString();
                    c++;
                }
            }
            currentPage = 1;
            pages = (int)Math.Ceiling(Convert.ToDouble(DataRows.Length) / Convert.ToDouble(PageLen));

            //DataRows is now populated with non-point data
            //Display the data in DataRows in the textboxes
            DisplayPage(currentPage);

        }

        public void DisplayPage(int page)
        {
            TextBox[] TextBoxes1 = { TextBoxCat1, TextBoxP1, TextBoxOutOf1, TextBoxW1, TextBoxPer1, TextBoxT1 };
            TextBox[] TextBoxes2 = { TextBoxCat2, TextBoxP2, TextBoxOutOf2, TextBoxW2, TextBoxPer2, TextBoxT2 };
            TextBox[] TextBoxes3 = { TextBoxCat3, TextBoxP3, TextBoxOutOf3, TextBoxW3, TextBoxPer3, TextBoxT3 };
            TextBox[] TextBoxes4 = { TextBoxCat4, TextBoxP4, TextBoxOutOf4, TextBoxW4, TextBoxPer4, TextBoxT4 };
            TextBox[] TextBoxes5 = { TextBoxCat5, TextBoxP5, TextBoxOutOf5, TextBoxW5, TextBoxPer5, TextBoxT5 };

            TextBox[][] TextBox2D = { TextBoxes1, TextBoxes2, TextBoxes3, TextBoxes4, TextBoxes5 };

            int start = (page - 1) * PageLen; //start INDEX
            int end = start + PageLen - 1;
            if(end > (DataRows.Length - 1))
            {
                end = DataRows.Length - 1;
            }
            SetVisibility(DataRows.Length, page);
            int c = 0;
            int offset = (page - 1) * PageLen;
            while(start <= end)
            {
                TextBox2D[c][0].Text = DataRows[c + offset].CatName;
                TextBox2D[c][1].Text = DataRows[c + offset].Points;
                TextBox2D[c][2].Text = DataRows[c + offset].OutOf;
                TextBox2D[c][3].Text = DataRows[c + offset].Weight;
                TextBox2D[c][4].Text = DataRows[c + offset].Percent;
                TextBox2D[c][5].Text = DataRows[c + offset].Total;
                c++;
                start++;
            }
            currentPage = page;
        }

        public void SaveDataToMem(int page)
        {
            TextBox[] TextBoxes1 = { TextBoxCat1, TextBoxP1, TextBoxOutOf1, TextBoxW1, TextBoxPer1, TextBoxT1 };
            TextBox[] TextBoxes2 = { TextBoxCat2, TextBoxP2, TextBoxOutOf2, TextBoxW2, TextBoxPer2, TextBoxT2 };
            TextBox[] TextBoxes3 = { TextBoxCat3, TextBoxP3, TextBoxOutOf3, TextBoxW3, TextBoxPer3, TextBoxT3 };
            TextBox[] TextBoxes4 = { TextBoxCat4, TextBoxP4, TextBoxOutOf4, TextBoxW4, TextBoxPer4, TextBoxT4 };
            TextBox[] TextBoxes5 = { TextBoxCat5, TextBoxP5, TextBoxOutOf5, TextBoxW5, TextBoxPer5, TextBoxT5 };

            TextBox[][] TextBox2D = { TextBoxes1, TextBoxes2, TextBoxes3, TextBoxes4, TextBoxes5 };

            int start = (page - 1) * PageLen; //start INDEX
            int end = start + PageLen - 1;
            if (end > (DataRows.Length - 1))
            {
                end = DataRows.Length - 1;
            }
            int c = 0;
            int offset = (page - 1) * PageLen;
            while (start <= end)
            {
                DataRows[c + offset].CatName = TextBox2D[c][0].Text;
                DataRows[c + offset].Points = TextBox2D[c][1].Text;
                DataRows[c + offset].OutOf = TextBox2D[c][2].Text;
                DataRows[c + offset].Weight = TextBox2D[c][3].Text;
                DataRows[c + offset].Percent = TextBox2D[c][4].Text;
                DataRows[c + offset].Total = TextBox2D[c][5].Text;
                c++;
                start++;
            }
        }

        public bool SaveIndexToMem(int index, string points_i, string outOf_i, bool neg)
        {
            double coe = 1;
            if (neg)
            {
                coe = -1;
            }
            string points, outOf;
            if (points_i.Equals(""))
            {
                points = "0";
            }
            else
            {
                points = points_i;
            }

            if (outOf_i.Equals(""))
            {
                outOf = "0";
            }
            else
            {
                outOf = outOf_i;
            }
            SaveDataToMem(currentPage);
            if (ErrorChecking.TextIsType("Double", points) && ErrorChecking.TextIsType("Double", outOf))
            {
                if(ErrorChecking.TextIsType("Double", DataRows[index].Points) && ErrorChecking.TextIsType("Double", DataRows[index].OutOf))
                {
                    DataRows[index].Points = (Convert.ToDouble(DataRows[index].Points) + coe * Convert.ToDouble(points)).ToString();
                    DataRows[index].OutOf = (Convert.ToDouble(DataRows[index].OutOf) + coe * Convert.ToDouble(outOf)).ToString();
                }
                else
                {
                    DataRows[index].Points = (coe * Convert.ToDouble(points)).ToString();
                    DataRows[index].OutOf = (coe * Convert.ToDouble(outOf)).ToString();
                }
                return true;
            }
            return false;
        }

        private void ClearInput()
        {
            //clear the actual textboxes
            for(int c = 0; c < 5; c++)
            {
                ClearInputInRow(c + 1);
            }
            TextBoxTotalPer.Text = "";
            TextBoxGrade.Text = "";
            TextBoxMeanPercent.Text = "";
            TextBoxMeanGrade.Text = "";

            foreach(DataRow dataRow in DataRows)
            {
                dataRow.SetDataToEmpty();
            }
            //DisplayPage(currentPage);
        }
        private void ClearInputInRow(int row)
        {
            if (row == 1)
            {
                TextBox[] TextBoxes = { TextBoxP1, TextBoxOutOf1, TextBoxPer1, TextBoxT1 };
                ClearInputInRow(TextBoxes);
            }
            else if (row == 2)
            {
                TextBox[] TextBoxes = { TextBoxP2, TextBoxOutOf2, TextBoxPer2, TextBoxT2 };
                ClearInputInRow(TextBoxes);
            }
            else if (row == 3)
            {
                TextBox[] TextBoxes = { TextBoxP3, TextBoxOutOf3, TextBoxPer3, TextBoxT3 };
                ClearInputInRow(TextBoxes);
            }
            else if (row == 4)
            {
                TextBox[] TextBoxes = { TextBoxP4, TextBoxOutOf4,TextBoxPer4, TextBoxT4 };
                ClearInputInRow(TextBoxes);
            }
            else if (row == 5)
            {
                TextBox[] TextBoxes = { TextBoxP5, TextBoxOutOf5, TextBoxPer5, TextBoxT5 };
                ClearInputInRow(TextBoxes);
            }
            else
            {
                throw new Exception("Row does not exist");
            }
        }
        private void ClearInputInRow(TextBox[] TBs)
        {
            foreach (TextBox TB in TBs)
            {
                TB.Text = "";
            }
        }

        private void SetVisibility(int elements, int currentPage)
        {
            int start = (currentPage - 1) * PageLen;
            int end = elements;
            int c = 1;
            while(c <= PageLen) //should set row's visibility
            {
                ChangeRowVisibility(c, start + c <= end);
                c++;
            }

            //button visibility
            if(currentPage == 1)
            {
                ButtonBack.Enabled = false;
            }
            else
            {
                ButtonBack.Enabled = true;
            }
            if(currentPage < pages)
            {
                ButtonForward.Enabled = true;
            }
            else
            {
                ButtonForward.Enabled = false;
            }
        }

        private void ChangeRowVisibility(int row, bool visible)
        {
            if (row == 1)
            {
                TextBox[] TextBoxes = { TextBoxCat1, TextBoxP1, TextBoxOutOf1, TextBoxW1, TextBoxPer1, TextBoxT1 };
                Button[] Buttons = { ButtonA1 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 2)
            {
                TextBox[] TextBoxes = { TextBoxCat2, TextBoxP2, TextBoxOutOf2, TextBoxW2, TextBoxPer2, TextBoxT2 };
                Button[] Buttons = { ButtonA2 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 3)
            {
                TextBox[] TextBoxes = { TextBoxCat3, TextBoxP3, TextBoxOutOf3, TextBoxW3, TextBoxPer3, TextBoxT3 };
                Button[] Buttons = { ButtonA3 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 4)
            {
                TextBox[] TextBoxes = { TextBoxCat4, TextBoxP4, TextBoxOutOf4, TextBoxW4, TextBoxPer4, TextBoxT4 };
                Button[] Buttons = { ButtonA4 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 5)
            {
                TextBox[] TextBoxes = { TextBoxCat5, TextBoxP5, TextBoxOutOf5, TextBoxW5, TextBoxPer5, TextBoxT5 };
                Button[] Buttons = { ButtonA5 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else
            {
                throw new Exception("Row does not exist");
            }
        }
        private void ChangeRowVisHelper(TextBox[] TBs, Button[] BTNs, bool vis)
        {
            foreach(TextBox TB in TBs)
            {
                TB.Visible = vis;
            }
            foreach(Button BTN in BTNs)
            {
                BTN.Visible = vis;
            }
        }

        public void CalculateGrade()
        {
            double[] points = new double[DataRows.Length];
            double[] outOf = new double[DataRows.Length];
            double[] weight = new double[DataRows.Length];
            double[] percent = new double[DataRows.Length];
            double[] total = new double[DataRows.Length];
            double finalPercentage = 0;
            string finalGrade = "";

            int c = 0;
            foreach(DataRow dataRow in DataRows)
            {
                //convert the strings in the text boxes to numerical values (if possible)
                if(dataRow.Points.Equals(""))
                {
                    points[c] = 0;
                }
                else
                {
                    try
                    {
                        points[c] = Convert.ToDouble(dataRow.Points);
                    }
                    catch
                    {
                        MessageBox.Show("At least one entry in points is invalid.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (dataRow.OutOf.Equals(""))
                {
                    outOf[c] = 0;
                }
                else
                {
                    try
                    {
                        outOf[c] = Convert.ToDouble(dataRow.OutOf);
                    }
                    catch
                    {
                        MessageBox.Show("At least one entry in out of is invalid.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (dataRow.Weight.Equals(""))
                {
                    weight[c] = 0;
                }
                else
                {
                    try
                    {
                        weight[c] = Convert.ToDouble(dataRow.Weight);
                    }
                    catch
                    {
                        MessageBox.Show("At least one entry in weight is invalid.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if(weight[c] != 0)
                {
                    if(outOf[c] == 0)
                    {
                        if (Settings.WarningLevel >= 1)
                        {
                            MessageBox.Show("At least one entry for Out Of is invalid!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        percent[c] = 0;
                        total[c] = 0;
                    }
                    else
                    {
                        percent[c] = 100 * points[c] / outOf[c];
                        total[c] = percent[c] * weight[c] / 100;
                        finalPercentage += total[c];
                    }
                }
                c++;
            }


            //get the grade scale
            double[] gradeScaleVals = new double[0];
            string[] gradeScaleGrade = new string[0];
            if(_currentClass == null)
            {
                //will not find a letter grade for a non-class
            }
            else if(_currentClass.gradeScaleFormat == 1)
            {
                c = 0;
                foreach(double val in _currentClass.gradeScale)
                {
                    if(val != -1) //if the grade is enabled
                    {
                        Array.Resize(ref gradeScaleVals, gradeScaleVals.Length + 1);
                        Array.Resize(ref gradeScaleGrade, gradeScaleGrade.Length + 1);
                        gradeScaleVals[gradeScaleVals.Length - 1] = val;
                        gradeScaleGrade[gradeScaleGrade.Length - 1] = GetGradeFromIndexAF(c);
                    }
                    c++;
                }
                //for adding F at the end
                Array.Resize(ref gradeScaleVals, gradeScaleVals.Length + 1);
                Array.Resize(ref gradeScaleGrade, gradeScaleGrade.Length + 1);
                gradeScaleVals[gradeScaleVals.Length - 1] = 0;
                gradeScaleGrade[gradeScaleGrade.Length - 1] = GetGradeFromIndexAF(11);

                bool found = false;
                c = 0;
                while (!found && (c != gradeScaleVals.Length))
                {
                    if(gradeScaleVals[c] <= finalPercentage)
                    {
                        finalGrade = gradeScaleGrade[c];
                        found = true;
                    }
                    c++;
                }
            }
            else
            {
                throw new NotImplementedException("S/N grading is not yet supported");
            }
            TextBoxGrade.Text = finalGrade;
            TextBoxTotalPer.Text = finalPercentage.ToString();

            c = 0;
            foreach(DataRow dataRow in DataRows)
            {
                DataRows[c].Points = points[c].ToString();
                DataRows[c].OutOf = outOf[c].ToString();
                DataRows[c].Weight = weight[c].ToString();
                DataRows[c].Percent = percent[c].ToString();
                DataRows[c].Total = total[c].ToString();
                c++;
            }
            DisplayPage(currentPage);

        }

        public void DisplayMean(string percent, string grade, bool zero)
        {
            TextBoxMeanPercent.Text = percent;
            TextBoxMeanGrade.Text = grade;
            LabelMeanZero.Visible = zero;
        }

        private string GetGradeFromIndexAF(int index)
        {
            string[] grades = { "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F" };
            return grades[index];
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void ButtonForward_Click(object sender, EventArgs e)
        {
            SaveDataToMem(currentPage);
            DisplayPage(currentPage + 1);
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            SaveDataToMem(currentPage);
            DisplayPage(currentPage - 1);
        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            SaveDataToMem(currentPage);
            CalculateGrade();
        }

        private void ButtonA1_Click(object sender, EventArgs e)
        {
            LaunchAddWindow(1);
        }

        private void ButtonA2_Click(object sender, EventArgs e)
        {
            LaunchAddWindow(2);
        }

        private void ButtonA3_Click(object sender, EventArgs e)
        {
            LaunchAddWindow(3);
        }

        private void ButtonA4_Click(object sender, EventArgs e)
        {
            LaunchAddWindow(4);
        }

        private void ButtonA5_Click(object sender, EventArgs e)
        {
            LaunchAddWindow(5);
        }

        private void LaunchAddWindow(int button)
        {
            int offset = button - 1;
            int index = (currentPage - 1) * PageLen + offset;
            Array.Resize(ref _addPoints, _addPoints.Length + 1);
            _addPoints[_addPoints.Length - 1] = new AddPoints(index, this);
            _addPoints[_addPoints.Length - 1].Show();
        }

        private void CloseAllAddWindows()
        {
            foreach(AddPoints form in _addPoints)
            {
                form.Close();
            }
            _addPoints = new AddPoints[0];
        }

        private void ButtonCurve_Click(object sender, EventArgs e)
        {
            CurveForm CF = new CurveForm();
            CF.Show();
        }

        private void refreshClassListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialSetup();
        }

        public void EditClassHandler(string className)
        {
            bool found = false;
            int c = 0;
            SchoolClass workingClass = null;
            while (!found && c != XMLHandler.Data.Length)
            {
                workingClass = XMLHandler.Data[c];
                if (workingClass.className.Equals(className))
                {
                    found = true;
                }
                c++;
            }
            if (!found)
            {
                throw new ArgumentException("The class selected does not exist. This should never happen.");
            }

            AddClass edit = new AddClass(this, workingClass);
            edit.Show();
        }

        public void DeleteClassHandler(string className)
        {
            //if class was deleted
            if (XMLHandler.DeleteClass(className))
            {
                InitialSetup();
            }
        }

        private void ChangeInputMode(int mode)
        {
            //basic = 1, advanced = 2
            Button[] buttons = { ButtonA1, ButtonA2, ButtonA3, ButtonA4, ButtonA5, ButtonClear, ButtonCalculate };
            TextBox[] pointsTextBoxes = { TextBoxP1, TextBoxP2, TextBoxP3, TextBoxP4, TextBoxP5 };
            TextBox[] outOfTextBoxes = { TextBoxOutOf1, TextBoxOutOf2, TextBoxOutOf3, TextBoxOutOf4, TextBoxOutOf5 };
            TextBox[] meanTextBoxes = {TextBoxMeanGrade, TextBoxMeanPercent};

            if(mode == 1)
            { 
                _assignments?.Close();
                _assignments = null;
                basicModeToolStripMenuItem.Checked = true;
                advancedModeToolStripMenuItem.Checked = false;
                foreach(Button btn in buttons)
                {
                    btn.Enabled = true;
                }
                foreach(TextBox tb in pointsTextBoxes)
                {
                    tb.ReadOnly = false;
                }
                foreach(TextBox tb in outOfTextBoxes)
                {
                    tb.ReadOnly = false;
                }
                foreach (TextBox tb in meanTextBoxes)
                {
                    tb.Text = "";
                    tb.Enabled = false;
                }
                LabelMeanZero.Visible = false;
                DisplayPage(currentPage);
                return;
            }
            else if(mode == 2)
            {
                basicModeToolStripMenuItem.Checked = false;
                advancedModeToolStripMenuItem.Checked = true;
                foreach (Button btn in buttons)
                {
                    btn.Enabled = false;
                }
                foreach (TextBox tb in pointsTextBoxes)
                {
                    tb.ReadOnly = true;
                }
                foreach (TextBox tb in outOfTextBoxes)
                {
                    tb.ReadOnly = true;
                }
                if (_assignments == null)
                {
                    _assignments = new Assignments(this, _currentClass);
                    _assignments.Show();
                }
                foreach (TextBox tb in meanTextBoxes)
                {
                    tb.Enabled = true;
                }
                DisplayPage(currentPage);
                return;
            }
            throw new NotImplementedException("Invalid mode passed to ChangeInputMode");
        }

        public void AssgnToDataRow(Assignment assgn)
        {
            if (assgn.active)
            {
                double temp = Convert.ToDouble("0" + DataRows[assgn.catIndex].Points);
                temp += assgn.points;
                DataRows[assgn.catIndex].Points = temp.ToString();

                temp = Convert.ToDouble("0" + DataRows[assgn.catIndex].OutOf);
                temp += assgn.outOf;
                DataRows[assgn.catIndex].OutOf = temp.ToString();
                DisplayPage(currentPage);
            }
        }

        private void basicModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeInputMode(1);
        }

        private void advancedModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeInputMode(2);
        }

        public void LoadAllAssignments()
        {
            foreach (SchoolClass schoolClass in XMLHandler.Data)
            {
                schoolClass.LoadAssignments();
            }
        }

        public void SetAssignmentsToNull()
        {
            _assignments = null;
        }

        public class DataRow
        {
            public string CatName, Points, OutOf, Weight, Percent, Total;

            public void SetAllToEmpty()
            {
                CatName = "";
                Points = "";
                OutOf = "";
                Weight = "";
                Percent = "";
                Total = "";
            }
            public void SetDataToEmpty()
            {
                Points = "";
                OutOf = "";
                Percent = "";
                Total = "";
            }
        }

    }

    public class SchoolClass
    {
        public string className, professor, termSeason;
        public int termYear, credits, gradeScaleFormat;
        public Double[] gradeScale;
        public string[] catNames;
        public Double[] catWorths;
        public Assignment[] assignments;

        public void LoadAssignments()
        {
            assignments = XMLHandler.ReadAssignments(this);
        }

        public void RemapAssignments(SchoolClass newClass, bool deleteUnmapped = true)
        {
            if (assignments is null || assignments.Length == 0)
            {
                return;
            }

            Assignment[] unchanged = new Assignment[0];
            Assignment[] updated = new Assignment[0];
            Assignment[] deleted = new Assignment[0];
            Dictionary<int, int> map = new Dictionary<int, int>();

            int c = 0;
            foreach (string oldCatName in catNames)
            {
                int newIndex = newClass.CatExists(oldCatName);
                if (newIndex > -1)
                {
                    map.Add(c, newIndex);
                }

                c++;
            }

            foreach (Assignment a in assignments)
            {
                int oldCatIndex = a.catIndex;
                try
                {
                    int newCatIndex = map[oldCatIndex];
                    if (newCatIndex == oldCatIndex)
                    {
                        Array.Resize(ref unchanged, unchanged.Length + 1);
                        unchanged[unchanged.Length - 1] = a;
                    }
                    else
                    {
                        a.catIndex = newCatIndex;
                        Array.Resize(ref updated, updated.Length + 1);
                        updated[updated.Length - 1] = a;
                    }
                }
                catch (KeyNotFoundException)
                {
                    Array.Resize(ref deleted, deleted.Length + 1);
                    deleted[deleted.Length - 1] = a;
                }
            }

            foreach (Assignment a in updated)
            {
                XMLHandler.SaveAssignmentToFile(this, a, false);
            }

            if (!deleteUnmapped) return;
            foreach (Assignment a in deleted)
            {
                XMLHandler.DeleteAssignment(this, a, false);
            }
        }

        private int CatExists(string catName)
        {
            int c = 0;
            foreach (string oldCatName in catNames)
            {
                if (oldCatName.Equals(catName))
                    return c;
                c++;
            }

            return -1;
        }

        public bool IsParseable()
        {
            try
            {
                var temp = new XElement("ClassData",
                    new XElement("ClassName", className),
                    new XElement("Professor", professor),
                    new XElement("Term",
                        new XElement("Year", termYear),
                        new XElement("Season", termSeason)),
                    new XElement("Credits", credits),
                    new XElement("GradeScaleFormat", gradeScaleFormat),
                    new XElement("GradeScale", gradeScale),
                    new XElement("Categories", catNames));
                var xDoc = new XDocument(temp);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool ActivesHaveMeans()
        {
            return assignments.Where(a => a.active).All(a => a.meanPoints != 0);
        }

        private string GetGradeFromIndexAF(int index)
        {
            string[] grades = {"A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F"};
            return grades[index];
        }

        public (string, string, bool) GetMeanGrade(Main.DataRow[] data)
        {
            bool zero = false;
            (string, string, bool) errorVal = ("Error in file!", "0.0", false);
            double[] meanPoints = new double[catNames.Length];
            double[] outOf = new double[catNames.Length];
            double[] weight = new double[catNames.Length];
            string[] retVal = new string[2];
            int c = 0;
            foreach (Main.DataRow dr in data)
            {
                if (double.TryParse(dr.Weight, out double tempWeight))
                {
                    weight[c] = tempWeight;
                    c++;
                }
                else
                {
                    return errorVal;
                }
            }

            foreach (Assignment a in assignments)
            {
                if (a.active)
                {
                    meanPoints[a.catIndex] += a.meanPoints;
                    outOf[a.catIndex] += a.outOf;
                }

                if (a.meanPoints == 0) zero = true;
            }

            double meanPercent = 0.0;
            c = 0;
            foreach (double x in meanPoints)
            {
                if (outOf[c] == 0)
                {
                    c++;
                    continue;
                }
                meanPercent += (meanPoints[c] / outOf[c]) * weight[c];
                c++;
            }

            double[] gradeScaleVals = new double[0];
            string[] gradeScaleGrade = new string[0];
            if (this.gradeScaleFormat == 1)
            {
                c = 0;
                foreach (double val in this.gradeScale)
                {
                    if (val != -1) //if the grade is enabled
                    {
                        Array.Resize(ref gradeScaleVals, gradeScaleVals.Length + 1);
                        Array.Resize(ref gradeScaleGrade, gradeScaleGrade.Length + 1);
                        gradeScaleVals[gradeScaleVals.Length - 1] = val;
                        gradeScaleGrade[gradeScaleGrade.Length - 1] = GetGradeFromIndexAF(c);
                    }

                    c++;
                }

                //for adding F at the end
                Array.Resize(ref gradeScaleVals, gradeScaleVals.Length + 1);
                Array.Resize(ref gradeScaleGrade, gradeScaleGrade.Length + 1);
                gradeScaleVals[gradeScaleVals.Length - 1] = 0;
                gradeScaleGrade[gradeScaleGrade.Length - 1] = GetGradeFromIndexAF(11);

                bool found = false;
                c = 0;
                while (!found && (c != gradeScaleVals.Length))
                {
                    if (gradeScaleVals[c] <= meanPercent)
                    {
                        retVal[0] = gradeScaleGrade[c];
                        found = true;
                    }
                    c++;
                }
            }

            retVal[1] = meanPercent.ToString();
            return (retVal[0], retVal[1], zero);
        }
    }

    public class Assignment
    {
        public bool active = false, real = true;
        public int catIndex;
        public string name;
        public double points, outOf, meanPoints;

        public Object[] ToDataView(SchoolClass schoolClass)
        {
            double percent = 0.0;
            if (!(points == 0.0 || outOf == 0.0))
                 percent = points / outOf * 100.0;
            Object[] retVal = {active, name, schoolClass.catNames[catIndex], points, outOf, percent, real};
            return retVal;
        }
    }

    public static class ErrorChecking
    {
        public static bool TextIsType(string type, Object value)
        {
            try
            {
                if (type == "Double" || type == "double")
                {
                    Double temp = Convert.ToDouble(value);
                    return true;
                }
                else if (type == "int")
                {
                    int temp = Convert.ToInt32(value);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            throw new System.ArgumentException("Argument passed to textIsType has not been implemented");
        }
    }
}
