using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanserMenu.v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();          
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 mainMenu = new Form2();
            mainMenu.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 settingMenu = new Form3();
            settingMenu.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
