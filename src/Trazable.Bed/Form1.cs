using System;
using System.Windows.Forms;
using Trazable.Engine.Base;

namespace Trazable.Bed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SphereCylinderAxis sca = new SphereCylinderAxis(-9, 3.75, 177);
            AstigmaticDecomposition ad = RepresentationConversion.ToAstigmaticDecomposition(sca);
            ad = ad.Propagate(12);
            PowersAndMeridians pam = new PowersAndMeridians(48.70, 83.00, 44.94, 173.00);
            ad = ad.Refracte(pam.ToAstigmaticDecomposition());
            ad = ad.Propagate(0.497);
            ad = ad.Propagate(3.19);
            ad = ad.Propagate(-0.304);
            sca = ad.ToSphereCylinderAxis();
            richTextBox1.AppendText(sca.ToString());
        }
    }
}
