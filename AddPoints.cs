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
                string PE = TextBoxPE.Text;
                string PP = TextBoxPP.Text;
                main.SaveIndexToMem(Index, PE, PP, neg);
                main.DisplayPage(main.currentPage);
            }
        }
    }
}
