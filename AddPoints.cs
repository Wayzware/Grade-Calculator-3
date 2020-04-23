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
    public partial class AddPoints : Form
    {
        Main main;
        int Index;

        public AddPoints(int index, Main sender)
        {
            InitializeComponent();
            main = sender;
            Index = index;
            LabelCat.Text = main.DataRows[Index].CatName;
        }

        private void AddPoints_Load(object sender, EventArgs e)
        {
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddSubPoints(false);
        }
        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            AddSubPoints(true);
        }

        private void AddSubPoints(bool neg)
        {
            if(ErrorChecking.textIsType("double", TextBoxPE.Text) && ErrorChecking.textIsType("double", TextBoxPP.Text))
            {
                double PE = Convert.ToDouble(TextBoxPE.Text);
                double PP = Convert.ToDouble(TextBoxPP.Text);
                if (neg)
                {
                    PE = -PE;
                    PP = -PP;
                }
                if(ErrorChecking.textIsType("double", main.DataRows[Index].Points)){
                    main.DataRows[Index].Points = (Convert.ToDouble(main.DataRows[Index].Points) + PE).ToString();
                }
                else
                {
                    main.DataRows[Index].Points = PE.ToString();
                }
                if (ErrorChecking.textIsType("double", main.DataRows[Index].OutOf))
                {
                    main.DataRows[Index].OutOf = (Convert.ToDouble(main.DataRows[Index].OutOf) + PP).ToString();
                }
                else
                {
                    main.DataRows[Index].OutOf = PP.ToString();
                }
                main.DisplayPage(main.currentPage);
            }
        }
    }
}
