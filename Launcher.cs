using System;
using System.Windows.Forms;

namespace DanserMenu.v2
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandMaker mainMenu = new CommandMaker();
            mainMenu.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
