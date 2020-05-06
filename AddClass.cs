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
    public partial class AddClass : Form
    {
        Main main;

        string[] categoryNames;
        string[] categoryWorthsS;
        int pages, currentPage;

        public AddClass(Main sender)
        {
            InitializeComponent();
            main = sender;

            categoryNames = new string[1];
            categoryNames[0] = "";
            categoryWorthsS = new string[1];
            categoryWorthsS[0] = "";
            pages = 1;
            DisplayPage(1);

            CheckBox[] gradeScaleCheckBoxes = new CheckBox[11];
            gradeScaleCheckBoxes[0] = checkBoxA;
            gradeScaleCheckBoxes[1] = checkBoxAM;
            gradeScaleCheckBoxes[2] = checkBoxBP;
            gradeScaleCheckBoxes[3] = checkBoxB;
            gradeScaleCheckBoxes[4] = checkBoxBM;
            gradeScaleCheckBoxes[5] = checkBoxCP;
            gradeScaleCheckBoxes[6] = checkBoxC;
            gradeScaleCheckBoxes[7] = checkBoxCM;
            gradeScaleCheckBoxes[8] = checkBoxDP;
            gradeScaleCheckBoxes[9] = checkBoxD;
            gradeScaleCheckBoxes[10] = checkBoxDM;
            foreach (CheckBox chkbx in gradeScaleCheckBoxes)
            {
                chkbx.Checked = true;
            }
        }

        public AddClass(Main sender, SchoolClass schoolClass)
        {
            InitializeComponent();
            main = sender;
            DisplayData(schoolClass);
        }

        private void DisplayData(SchoolClass schoolClass)
        {
            int c = 0;

            textBoxClassName.Text = schoolClass.className;
            textBoxProfessor.Text = schoolClass.professor;
            comboBoxTermSeason.Text = schoolClass.termSeason;
            numericUpDownTermYear.Value = schoolClass.termYear;
            numericUpDownCredits.Value = schoolClass.credits;

            if (schoolClass.gradeScaleFormat == 1)
            {
                radioButtonAF.Checked = true;
                TextBox[] GradesAF = { TextBoxA, TextBoxAM, TextBoxBP, TextBoxB, TextBoxBM, TextBoxCP, TextBoxC, TextBoxCM, TextBoxDP, TextBoxD, TextBoxDM, textBoxF };
                CheckBox[] GradesAFEnabled = { checkBoxA, checkBoxAM, checkBoxBP, checkBoxB, checkBoxBM, checkBoxCP, checkBoxC, checkBoxCM, checkBoxDP, checkBoxD, checkBoxDM, checkBoxF };

                c = 0;
                foreach (double x in schoolClass.gradeScale)
                {
                    if(schoolClass.gradeScale[c] != -1)
                    {
                        GradesAFEnabled[c].Checked = true;
                        GradesAF[c].Text = schoolClass.gradeScale[c].ToString();
                    }
                    else
                    {
                        GradesAFEnabled[c].Checked = false;
                    }
                    c++;
                }
            }

            categoryNames = schoolClass.catNames;
            categoryWorthsS = new string[categoryNames.Length];
            pages = categoryNames.Length / 5 + 1;
            currentPage = 1;
            c = 0;
            foreach(double worth in schoolClass.catWorths)
            {
                categoryWorthsS[c] = worth.ToString();
                c++;
            }
            DisplayPage(1);
            numericUpDownCategories.Value = categoryNames.Length;
        }

        private void AddClass_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveCurrentData();
        }

        public void SaveCurrentData()
        {
            SchoolClass schoolClass = new SchoolClass();
            Double[] gradeScaleValues;

            //Class Information
            //Class Name
            if (textBoxClassName.Text != "")
            {
                schoolClass.className = textBoxClassName.Text;
            }
            else
            {
                MessageBox.Show("Class Name is invalid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Professor
            if (textBoxProfessor.Text != "")
            {
                schoolClass.professor = textBoxProfessor.Text;
            }
            else
            {
                schoolClass.professor = null;
            }

            //Term
            if (comboBoxTermSeason.Text != "")
            {
                schoolClass.termSeason = comboBoxTermSeason.Text;
                schoolClass.termYear = (int)numericUpDownTermYear.Value;
            }
            else
            {
                schoolClass.termSeason = null;
                schoolClass.termYear = -1;
            }

            //Credits
            if (((int)numericUpDownCredits.Value) != 0)
            {
                schoolClass.credits = (int)numericUpDownCredits.Value;
            }
            else
            {
                schoolClass.credits = -1;
            }

            //Grade Scale
            //Grade Scale Format
            if (radioButtonAF.Checked == true)
            {
                schoolClass.gradeScaleFormat = 1;
            }
            else if (radioButtonSN.Checked == true)
            {
                schoolClass.gradeScaleFormat = 2;
            }
            else //note: this should never happen as radio buttons are being used.
            {
                MessageBox.Show("Grade Scale Format is invalid. (This should never happen)", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Grade Scale Values
            //A-F
            int c = 0;
            if (schoolClass.gradeScaleFormat == 1)
            {
                TextBox[] gradeScaleTextBoxes = new TextBox[11];
                CheckBox[] gradeScaleCheckBoxes = new CheckBox[11];
                Boolean[] gradeScaleEnableds = new Boolean[11];

                gradeScaleTextBoxes[0] = TextBoxA;
                gradeScaleTextBoxes[1] = TextBoxAM;
                gradeScaleTextBoxes[2] = TextBoxBP;
                gradeScaleTextBoxes[3] = TextBoxB;
                gradeScaleTextBoxes[4] = TextBoxBM;
                gradeScaleTextBoxes[5] = TextBoxCP;
                gradeScaleTextBoxes[6] = TextBoxC;
                gradeScaleTextBoxes[7] = TextBoxCM;
                gradeScaleTextBoxes[8] = TextBoxDP;
                gradeScaleTextBoxes[9] = TextBoxD;
                gradeScaleTextBoxes[10] = TextBoxDM;

                gradeScaleCheckBoxes[0] = checkBoxA;
                gradeScaleCheckBoxes[1] = checkBoxAM;
                gradeScaleCheckBoxes[2] = checkBoxBP;
                gradeScaleCheckBoxes[3] = checkBoxB;
                gradeScaleCheckBoxes[4] = checkBoxBM;
                gradeScaleCheckBoxes[5] = checkBoxCP;
                gradeScaleCheckBoxes[6] = checkBoxC;
                gradeScaleCheckBoxes[7] = checkBoxCM;
                gradeScaleCheckBoxes[8] = checkBoxDP;
                gradeScaleCheckBoxes[9] = checkBoxD;
                gradeScaleCheckBoxes[10] = checkBoxDM;

                foreach (CheckBox chkbx in gradeScaleCheckBoxes)
                {
                    gradeScaleEnableds[c] = chkbx.Checked;
                    c++;
                }

                gradeScaleValues = new double[11];

                c = 0;
                Double prevVal = Double.MaxValue;
                Boolean flag = false; //if no checkbox is enabled (other than F)
                foreach (Double element in gradeScaleValues)
                {
                    if (gradeScaleEnableds[c])
                    {
                        flag = true;
                        if (ErrorChecking.textIsType("Double", gradeScaleTextBoxes[c].Text))
                        {
                            Double currentGradeScaleValue = Convert.ToDouble(gradeScaleTextBoxes[c].Text);
                            if (prevVal > currentGradeScaleValue)
                            {
                                gradeScaleValues[c] = currentGradeScaleValue;
                                prevVal = currentGradeScaleValue;
                            }
                            else
                            {
                                MessageBox.Show("Grade scale has impossible values!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Non-numeric value entered in grade scale!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        gradeScaleValues[c] = -1; //if this grade value is disabled
                    }
                    c++;
                }
                if (!flag)
                {
                    MessageBox.Show("At least one grade must be enabled!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                schoolClass.gradeScale = gradeScaleValues;
            }

            //S/N (TODO)
            else
            {
                return;
            }

            //Categories
            SaveCurrentCatData();
            schoolClass.catNames = categoryNames;
            foreach(string cat in schoolClass.catNames)
            {
                if (cat.Equals(""))
                {
                    MessageBox.Show("One or more categories have no name!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            schoolClass.catWorths = new double[categoryWorthsS.Length];
            c = 0;
            Double total = 0;
            foreach(string worthString in categoryWorthsS)
            {
                if(ErrorChecking.textIsType("Double", worthString))
                {
                    schoolClass.catWorths[c] = Convert.ToDouble(worthString);
                    total += schoolClass.catWorths[c];
                    c++;
                }
                else
                {
                    MessageBox.Show("Category total percentage contains non-numeric value!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if((int) Math.Round(total) != 100)
            {
                var result = MessageBox.Show("% of total adds to " + total.ToString() + ". Continue?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(result == DialogResult.No)
                {
                    return;
                }
            }

            //Data has been verified and is ready to be written to a file!
            XMLHandler.SaveSchoolClassToFile(schoolClass, XMLHandler.SCHEMA_VER);
            main.InitialSetup();
            this.Close();
        }


        private void SaveCurrentCatData()
        {
            TextBox[] nameTextBoxes = new TextBox[4];
            TextBox[] worthTextBoxes = new TextBox[4];
            nameTextBoxes[0] = TextBoxName1;
            nameTextBoxes[1] = TextBoxName2;
            nameTextBoxes[2] = TextBoxName3;
            nameTextBoxes[3] = TextBoxName4;
            worthTextBoxes[0] = TextBoxWorth1;
            worthTextBoxes[1] = TextBoxWorth2;
            worthTextBoxes[2] = TextBoxWorth3;
            worthTextBoxes[3] = TextBoxWorth4;

            int c = 0;
            foreach(TextBox tb in nameTextBoxes)
            {
                if (tb.Visible)
                {
                    categoryNames[c + ((currentPage - 1)  * 4)] = tb.Text;
                }
                c++;
            }
            c = 0;
            foreach(TextBox tb in worthTextBoxes)
            {
                if (tb.Visible)
                {
                    categoryWorthsS[c + ((currentPage - 1) * 4)] = tb.Text;
                }
                c++;
            }
            return;

        }

        private void numericUpDownCategories_ValueChanged(object sender, EventArgs e)
        {
            SaveCurrentCatData();
            //transfer old data to new 
            int oldCategories = categoryNames.Length;
            int categories = (int)numericUpDownCategories.Value;
            string[] newNames = new string[(int)numericUpDownCategories.Value];
            string[] newWorths = new string[(int)numericUpDownCategories.Value];
            if(newNames.Length > categoryNames.Length)
            {
                int c = 0;
                foreach(string name in categoryNames)
                {
                    newNames[c] = name;
                    c++;
                }
                c = 0;
                foreach(string worth in categoryWorthsS)
                {
                    newWorths[c] = worth;
                    c++;
                }
            }
            else
            {
                int c = 0;
                foreach(string name in newNames)
                {
                    newNames[c] = categoryNames[c];
                    c++;
                }
                c = 0;
                foreach(string worth in newWorths)
                {
                    newWorths[c] = categoryWorthsS[c];
                    c++;
                }
            }
            categoryNames = newNames;
            categoryWorthsS = newWorths;
            int oldPages = pages;
            pages = (int) Math.Ceiling(Convert.ToDouble(categories) / 4.0);

            //Page display
            //if the number of pages increases
            if (oldPages < pages){
                if (currentPage < pages)
                {
                    buttonPgForward.Visible = true;
                }
            }
            //if the number of pages decreases or stays the same
            else
            {
                if(currentPage > pages)
                {
                    currentPage = pages;
                }
            }
            DisplayPage(currentPage);
        }

        private void buttonPgForward_Click(object sender, EventArgs e)
        {
            SaveCurrentCatData();
            DisplayPage(currentPage + 1);
        }

        private void buttonPgBack_Click(object sender, EventArgs e)
        {
            SaveCurrentCatData();
            DisplayPage(currentPage - 1);
        }

        private void DisplayPage(int page)
        {
            if(page > pages || page < 1)
            {
                throw new System.ArgumentException("Invalid page passed to DisplayPage");
            }
            Label[] nameLabels = new Label[4];
            TextBox[] nameTextBoxes = new TextBox[4];
            Label[] worthLabels = new Label[4];
            TextBox[] worthTextBoxes = new TextBox[4];

            nameLabels[0] = LabelName1;
            nameLabels[1] = LabelName2;
            nameLabels[2] = LabelName3;
            nameLabels[3] = LabelName4;
            nameTextBoxes[0] = TextBoxName1;
            nameTextBoxes[1] = TextBoxName2;
            nameTextBoxes[2] = TextBoxName3;
            nameTextBoxes[3] = TextBoxName4;
            worthLabels[0] = LabelWorth1;
            worthLabels[1] = LabelWorth2;
            worthLabels[2] = LabelWorth3;
            worthLabels[3] = LabelWorth4;
            worthTextBoxes[0] = TextBoxWorth1;
            worthTextBoxes[1] = TextBoxWorth2;
            worthTextBoxes[2] = TextBoxWorth3;
            worthTextBoxes[3] = TextBoxWorth4;

            int bottomEnabled = (page - 1) * 4;
            int topEnabled = bottomEnabled + 3;

            if(topEnabled > categoryNames.Length - 1) //if top bound is greater than the index of the last category
            {
                topEnabled = categoryNames.Length - 1;
            }

            int enabledIndex = bottomEnabled;
            int c = 0;
            //fill the textboxes
            while(enabledIndex <= topEnabled)
            {
                nameLabels[c].Visible = true;
                nameTextBoxes[c].Visible = true;
                worthLabels[c].Visible = true;
                worthTextBoxes[c].Visible = true;
                nameTextBoxes[c].Text = categoryNames[enabledIndex];
                worthTextBoxes[c].Text = categoryWorthsS[enabledIndex];
                c++;
                enabledIndex++;
            }
            //disable the unneeded labels and textboxes
            while(c <= 3)
            {
                nameLabels[c].Visible = false;
                nameTextBoxes[c].Visible = false;
                worthLabels[c].Visible = false;
                worthTextBoxes[c].Visible = false;
                c++;
            }

            if(page > 1)
            {
                buttonPgBack.Visible = true;
            }
            else
            {
                buttonPgBack.Visible = false;
            }

            if(page < pages)
            {
                buttonPgForward.Visible = true;
            }
            else
            {
                buttonPgForward.Visible = false;
            }
            if(pages == 1)
            {
                LabelPage.Visible = false;
            }
            else
            {
                LabelPage.Visible = true;
                string pageString = "Page " + page.ToString();
                LabelPage.Text = pageString;
            }

            currentPage = page;
        }
    }

    public class SchoolClass
    {
        public string className, professor, termSeason;
        public int termYear, credits, gradeScaleFormat;
        public Double[] gradeScale;
        public string[] catNames;
        public Double[] catWorths;

    }
}
