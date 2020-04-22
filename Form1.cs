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

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            XMLHandler.DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Grade Calculator/Classes/";
            XMLHandler.Data = XMLHandler.ReadSchoolClasses();
        }

        private void addClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClass addClassForm = new AddClass();
            addClassForm.Show();
        }
    }
}
