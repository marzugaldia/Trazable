using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trazable.Engine.System;

namespace Trazable.App
{
    public partial class FormScript : Form
    {
        private UserScript script;

        public FormScript(UserScript script)
        {
            InitializeComponent();
            this.script = script;
            this.textBox1.Text = script.Name;
            this.richTextBox1.Text = script.Script;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("Necesita un nombre");
            }
            else
            {
                script.Name = this.textBox1.Text;
                script.Script = this.richTextBox1.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
