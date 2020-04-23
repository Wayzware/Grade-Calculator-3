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
    public partial class Main : Form
    {
        private readonly int PAGE_LEN = 5;
        int currentPage, pages;
        private DataRow[] DataRows;
        private SchoolClass CurrentClass;


        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            XMLHandler.DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Grade Calculator/Classes/";
            XMLHandler.Data = XMLHandler.ReadSchoolClasses();
            comboBoxClasses.DropDownStyle = ComboBoxStyle.DropDownList;
            currentPage = 1;
            pages = 1;
            CurrentClass = null;
            LoadClassData("");
            RefreshClassList();
        }

        private void addClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClass addClassForm = new AddClass();
            addClassForm.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        public void RefreshClassList()
        {
            XMLHandler.Data = XMLHandler.ReadSchoolClasses();
            comboBoxClasses.Items.Clear();
            foreach(SchoolClass schoolClass in XMLHandler.Data)
            {
                comboBoxClasses.Items.Add(schoolClass.className);
            }
        }

        private void comboBoxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClassData(comboBoxClasses.Text);
        }

        public void LoadClassData(string className)
        {
            TextBox[] CatTextBoxes = { TextBoxCat1, TextBoxCat2, TextBoxCat3, TextBoxCat4, TextBoxCat5 };

            //if the class if blank (initialization), add PAGE_LEN blank categories to DataRows
            if (className.Equals(""))
            {
                DataRows = new DataRow[PAGE_LEN];
                int c = 0;
                foreach(DataRow DataRow in DataRows)
                {
                    DataRow temp = new DataRow();
                    temp.SetAllToEmpty();
                    DataRows[c] = temp;
                    c++;
                }
                setVisibility(PAGE_LEN, 1);
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
                        CurrentClass = workingClass;
                    }
                    c++;
                }
                if (!found)
                {
                    throw new ArgumentException("The class selected does not exist. This should never happen.");
                }

                //fill DataRows with category data
                DataRows = new DataRow[CurrentClass.catNames.Length];


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
            pages = (int)Math.Ceiling(Convert.ToDouble(DataRows.Length) / Convert.ToDouble(PAGE_LEN));

            //DataRows is now populated with non-point data
            //Display the data in DataRows in the textboxes
            DisplayPage(currentPage);

        }

        private void DisplayPage(int page)
        {
            TextBox[] TextBoxes1 = { TextBoxCat1, TextBoxP1, TextBoxOutOf1, TextBoxW1, TextBoxPer1, TextBoxT1 };
            TextBox[] TextBoxes2 = { TextBoxCat2, TextBoxP2, TextBoxOutOf2, TextBoxW2, TextBoxPer2, TextBoxT2 };
            TextBox[] TextBoxes3 = { TextBoxCat3, TextBoxP3, TextBoxOutOf3, TextBoxW3, TextBoxPer3, TextBoxT3 };
            TextBox[] TextBoxes4 = { TextBoxCat4, TextBoxP4, TextBoxOutOf4, TextBoxW4, TextBoxPer4, TextBoxT4 };
            TextBox[] TextBoxes5 = { TextBoxCat5, TextBoxP5, TextBoxOutOf5, TextBoxW5, TextBoxPer5, TextBoxT5 };

            TextBox[][] TextBox2D = { TextBoxes1, TextBoxes2, TextBoxes3, TextBoxes4, TextBoxes5 };

            int start = (page - 1) * PAGE_LEN; //start INDEX
            int end = start + PAGE_LEN - 1;
            if(end > (DataRows.Length - 1))
            {
                end = DataRows.Length - 1;
            }
            setVisibility(DataRows.Length, page);
            int c = 0;
            int offset = (page - 1) * PAGE_LEN;
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

        private void SaveDataToMem(int page)
        {
            TextBox[] TextBoxes1 = { TextBoxCat1, TextBoxP1, TextBoxOutOf1, TextBoxW1, TextBoxPer1, TextBoxT1 };
            TextBox[] TextBoxes2 = { TextBoxCat2, TextBoxP2, TextBoxOutOf2, TextBoxW2, TextBoxPer2, TextBoxT2 };
            TextBox[] TextBoxes3 = { TextBoxCat3, TextBoxP3, TextBoxOutOf3, TextBoxW3, TextBoxPer3, TextBoxT3 };
            TextBox[] TextBoxes4 = { TextBoxCat4, TextBoxP4, TextBoxOutOf4, TextBoxW4, TextBoxPer4, TextBoxT4 };
            TextBox[] TextBoxes5 = { TextBoxCat5, TextBoxP5, TextBoxOutOf5, TextBoxW5, TextBoxPer5, TextBoxT5 };

            TextBox[][] TextBox2D = { TextBoxes1, TextBoxes2, TextBoxes3, TextBoxes4, TextBoxes5 };

            int start = (page - 1) * PAGE_LEN; //start INDEX
            int end = start + PAGE_LEN - 1;
            if (end > (DataRows.Length - 1))
            {
                end = DataRows.Length - 1;
            }
            int c = 0;
            int offset = (page - 1) * PAGE_LEN;
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

        private void ClearInput()
        {
            //clear the actual textboxes
            for(int c = 0; c < 5; c++)
            {
                ClearInputInRow(c + 1);
            }
            TextBoxTotalPer.Text = "";
            TextBoxGrade.Text = "";

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

        private void setVisibility(int elements, int currentPage)
        {
            int start = (currentPage - 1) * PAGE_LEN;
            int end = elements;
            int c = 1;
            while(c <= PAGE_LEN) //should set row's visibility
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
                Button[] Buttons = { ButtonA1, ButtonP1 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 2)
            {
                TextBox[] TextBoxes = { TextBoxCat2, TextBoxP2, TextBoxOutOf2, TextBoxW2, TextBoxPer2, TextBoxT2 };
                Button[] Buttons = { ButtonA2, ButtonP2 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 3)
            {
                TextBox[] TextBoxes = { TextBoxCat3, TextBoxP3, TextBoxOutOf3, TextBoxW3, TextBoxPer3, TextBoxT3 };
                Button[] Buttons = { ButtonA3, ButtonP3 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 4)
            {
                TextBox[] TextBoxes = { TextBoxCat4, TextBoxP4, TextBoxOutOf4, TextBoxW4, TextBoxPer4, TextBoxT4 };
                Button[] Buttons = { ButtonA4, ButtonP4 };
                ChangeRowVisHelper(TextBoxes, Buttons, visible);
            }
            else if (row == 5)
            {
                TextBox[] TextBoxes = { TextBoxCat5, TextBoxP5, TextBoxOutOf5, TextBoxW5, TextBoxPer5, TextBoxT5 };
                Button[] Buttons = { ButtonA5, ButtonP5 };
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

        private void CalculateGrade()
        {
            double[] points = new double[DataRows.Length];
            double[] outOf = new double[DataRows.Length];
            double[] weight = new double[DataRows.Length];
            double[] percent = new double[DataRows.Length];
            double[] total = new double[DataRows.Length];


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
            CalculateGrade();
        }

        private class DataRow
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


}
