using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Trazable.Engine.Base;
using Trazable.Engine.System;

namespace Trazable.App
{
    public partial class FormMain : Form
    {
        private Panel selected = null;

        private OptSystem system = null;

        private UserScripts scripts;

        private List<Panel> panelList = new List<Panel>();

        private Dictionary<Panel, UserScript> panels = new Dictionary<Panel, UserScript>();

        private Dictionary<UserScript, Panel> panelsInverse = new Dictionary<UserScript, Panel>();

        private List<Control> currentControls = new List<Control>();

        public FormMain()
        {
            InitializeComponent();
            this.LoadApplications();
            this.ShowApplications();
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadApplications()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Trazable";
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Trazable\\Trazable.xml";
            this.scripts = UserScripts.Load(fileName);
            if (this.scripts.Count == 0)
            {
                UserScript script = new UserScript();
                script.Name = "Primera prueba";
                script.Script = "Input(SCA, spectacle, \"Spectacle\", -9, 3.75, 177);\n"+
                    "Input(PAM, cornea, \"Cornea\", 48.7, 83, 44.94, 177);\n"+
                    "Input(double, bvd, \"BVD\", 12);\n"+
                    "Input(double, ct, \"CT\", 0.497);\n"+
                    "Input(double, acd, \"ACD\", 3.19);\n" +
                    "Input(double, sf, \"SF\", -0.304);\n" +
                    "Output(SCA, ev, \"EV\");\n" +
                    "Output(SCA, theoricalIOL, \"Theorical IOL\");\n" +
                    "Input(SCA, realIOL, \"Real IOL\", -14.50, 3.5, 93);\n" +
                    "Output(SCA, resultSpectacle, \"Result Spectacle\");\n" +
                    "\n" +
                    "ev = spectacle.prop(bvd).ref(cornea).prop(ct).prop(acd).prop(sf);\n" +
                    "theoricalIOL = cornea.prop(ct).prop(acd).prop(sf).refinv(ev);\n" +
                    "resultSpectacle = cornea.refinv(realIOL.refinv(ev).propinv(ct).propinv(acd).propinv(sf)).propinv(bvd);\n";
                this.scripts.Add(script);
                UserScripts.Save(this.scripts);
            }
        }

        private void SaveApplications()
        {
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Trazable\\Trazable.xml";
            UserScripts.Save(this.scripts, fileName);
        }

        private void ShowApplications()
        {
            foreach (Panel panel in this.panelList)
            {
                panel.Dispose();
            }
            panelList.Clear();
            this.panels.Clear();
            this.panelsInverse.Clear();
            for (int i = this.scripts.Count-1; i >=0; i--)
            {
                Panel panel = this.NewProjectPanel(this.scripts[i].Name);
                this.panelList.Add(panel);
                this.panels.Add(panel, this.scripts[i]);
                this.panelsInverse.Add(this.scripts[i], panel);
            }
        }

        private void ExecuteCurrent()
        {
            this.ClearDynamicComponents();
            if (this.selected != null)
            {
                UserScript script = this.panels[this.selected];
                try
                {
                    this.system = script.Process();
                    this.system.Initialize();
                    this.CreateSystemComponents();
                }
                catch
                {
                    MessageBox.Show("Hay errores en el script");
                }
            }
        }

        private void CalculateCurrent()
        {
            this.RecoverVariables();
            this.system.Execute();
            this.SetOutputVariables();
        }

        private void ClearDynamicComponents()
        {
            foreach (Control control in currentControls)
            {
                control.Dispose();
            }
            this.currentControls.Clear();

        }

        private Panel NewProjectPanel(string name)
        {
            Panel result = new Panel();
            result.BackColor = Color.DeepSkyBlue;
            result.Dock = DockStyle.Top;
            result.Size = new Size(182, 36);
            result.BorderStyle = BorderStyle.FixedSingle;
            result.Cursor = Cursors.Hand;
            result.MouseClick += panel1_MouseClick;
            this.pnlProjects.Controls.Add(result);
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = Color.Black;
            label.Location = new Point(11, 5);
            label.Size = new Size(56, 23);
            label.Text = name;
            label.MouseClick += panel1_MouseClick;
            result.Controls.Add(label);
            return result;
        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.selected != null)
            {
                this.selected.BackColor = Color.DeepSkyBlue;
            }
            if (sender is Label)
            {
                this.selected = (Panel)((Label)sender).Parent;
            }
            else
            {
                this.selected = (Panel)sender;
            }
            this.selected.BackColor = Color.LightSkyBlue;
            this.ExecuteCurrent();
        }

        private MaskedTextBox NewTextBox(int x, int y, VarDirection direction, string name)
        {
            MaskedTextBox result = new MaskedTextBox();
            this.pnlMain.Controls.Add(result);
            result.Location = new Point(100 + x * 70, 30 + y * 26);
            result.Name = "tb" + name;
            result.Size = new Size(65, 20);
            if (direction == VarDirection.Output)
            {
                result.BackColor = Color.PaleTurquoise;
                result.BorderStyle = BorderStyle.None;
            }
            this.currentControls.Add(result);
            return result;
        }

        private Label NewLabel(int y, string name, string text)
        {
            Label label = new Label();
            label.AutoSize = false;
            label.Location = new Point(5, 30 + y * 26 + 2);
            this.pnlMain.Controls.Add(label);
            label.Name = "lbl" + name;
            label.Size = new Size(90, 13);
            label.Text = text;
            label.TextAlign = ContentAlignment.TopRight;
            this.currentControls.Add(label);
            return label;
        }

        private void CreateSCAComponents(int i, VarDef variable)
        {
            MaskedTextBox component = NewTextBox(0, i, variable.Direction, variable.Name + "_Sphere");
            component.Text = variable.Parameters.Count > 0 ? variable.Parameters[0] : "";
            component = NewTextBox(1, i, variable.Direction, variable.Name + "_Cylinder");
            component.Text = variable.Parameters.Count > 1 ? variable.Parameters[1] : "";
            component = NewTextBox(2, i, variable.Direction, variable.Name + "_Axis");
            component.Text = variable.Parameters.Count > 2 ? variable.Parameters[2] : "";
        }

        private void CreateADComponents(int i, VarDef variable)
        {
            MaskedTextBox component = NewTextBox(0, i, variable.Direction, variable.Name + "_M");
            component.Text = variable.Parameters.Count > 0 ? variable.Parameters[0] : "";
            component = NewTextBox(1, i, variable.Direction, variable.Name + "_C0");
            component.Text = variable.Parameters.Count > 1 ? variable.Parameters[1] : "";
            component = NewTextBox(2, i, variable.Direction, variable.Name + "_C45");
            component.Text = variable.Parameters.Count > 2 ? variable.Parameters[2] : "";
        }

        private void CreatePAMComponents(int i, VarDef variable)
        {
            MaskedTextBox component = NewTextBox(0, i, variable.Direction, variable.Name + "_Power1");
            component.Text = variable.Parameters.Count > 0 ? variable.Parameters[0] : "";
            component = NewTextBox(1, i, variable.Direction, variable.Name + "_Axis1");
            component.Text = variable.Parameters.Count > 1 ? variable.Parameters[1] : "";
            component = NewTextBox(2, i, variable.Direction, variable.Name + "_Power2");
            component.Text = variable.Parameters.Count > 2 ? variable.Parameters[2] : "";
            component = NewTextBox(3, i, variable.Direction, variable.Name + "_Axis2");
            component.Text = variable.Parameters.Count > 3 ? variable.Parameters[3] : "";
        }

        private void CreateDoubleComponents(int i, VarDef variable)
        {
            MaskedTextBox component = NewTextBox(0, i, variable.Direction, variable.Name + "_Double");
            component.Text = variable.Parameters.Count > 0 ? variable.Parameters[0] : "";
        }

        private void CreateVariableComponents(int i, VarDef variable)
        {
            this.NewLabel(i, variable.Name, variable.Text);
            switch (variable.Type)
            {
                case VarType.SCA:
                    this.CreateSCAComponents(i, variable);
                    break;
                case VarType.Double:
                    this.CreateDoubleComponents(i, variable);
                    break;
                case VarType.AD:
                    this.CreateADComponents(i, variable);
                    break;
                case VarType.PAM:
                    this.CreatePAMComponents(i, variable);
                    break;
                default:
                    break;
            }
        }


        private void CreateSystemComponents()
        {
            for (int i = 0; i < system.Vars.Variables.Count; i++)
            {
                this.CreateVariableComponents(i, system.Vars.Variables[i]);
            }
            //this.Height = system.Vars.Variables.Count * 26 + 200;
            btnCalculate.Top = system.Vars.Variables.Count * 26 + 50;
        }

        private void btnCalculate_Click(object sender, System.EventArgs e)
        {
            this.CalculateCurrent();
        }

        private string GetTextFrom(string name)
        {
            Control[] controls = this.Controls.Find(name, true);
            if (controls != null && controls.Length == 1)
            {
                return controls[0].Text;
            }
            return string.Empty;
        }

        private void SetTextTo(string name, string text)
        {
            Control[] controls = this.Controls.Find(name, true);
            if (controls != null && controls.Length == 1)
            {
                controls[0].Text = text;
            }
        }

        private string FixDecimalSeparator(string str)
        {
            return str.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }

        private double GetDoubleFrom(string name)
        {
            string s = this.GetTextFrom(name);
            s = this.FixDecimalSeparator(s);
            return Convert.ToDouble(s);
        }

        private void SetDoubleTo(string name, double value)
        {
            this.SetTextTo(name, value.ToString("F2"));
        }

        private void RecoverADVariable(VarDef variable)
        {
            double m = this.GetDoubleFrom("tb" + variable.Name + "_M");
            double c0 = this.GetDoubleFrom("tb" + variable.Name + "_C0");
            double c45 = this.GetDoubleFrom("tb" + variable.Name + "_C45");
            OpticalRepresentation or = new OpticalRepresentation(new AstigmaticDecomposition(m, c0, c45));
            variable.Value = or;
        }

        private void SetADVariable(VarDef variable)
        {
            if (variable.Value == null)
            {
               
            }
            else
            {
                AstigmaticDecomposition ad = ((OpticalRepresentation)variable.Value).AstigmaticDecomposition;
                this.SetDoubleTo("tb" + variable.Name + "_M", ad.M);
                this.SetDoubleTo("tb" + variable.Name + "_C0", ad.C0);
                this.SetDoubleTo("tb" + variable.Name + "_C45", ad.C45);
            }
        }

        private void RecoverPAMVariable(VarDef variable)
        {
            double power1 = this.GetDoubleFrom("tb" + variable.Name + "_Power1");
            double axis1 = this.GetDoubleFrom("tb" + variable.Name + "_Axis1");
            double power2 = this.GetDoubleFrom("tb" + variable.Name + "_Power2");
            double axis2 = this.GetDoubleFrom("tb" + variable.Name + "_Axis2");
            OpticalRepresentation or = new OpticalRepresentation(power1, axis1, power2, axis2);
            variable.Value = or;
        }

        private void SetPAMVariable(VarDef variable)
        {
            if (variable.Value == null)
            {

            }
            else
            {
                PowersAndMeridians pam = ((OpticalRepresentation)variable.Value).PowersAndMeridians;
                this.SetDoubleTo("tb" + variable.Name + "_Power1", pam.Power1);
                this.SetDoubleTo("tb" + variable.Name + "_Axis1", pam.Meridian1.Degree);
                this.SetDoubleTo("tb" + variable.Name + "_Power2", pam.Power2);
                this.SetDoubleTo("tb" + variable.Name + "_Axis2", pam.Meridian2.Degree);
            }
        }

        private void RecoverSCAVariable(VarDef variable)
        {
            double sphere = this.GetDoubleFrom("tb" + variable.Name + "_Sphere");
            double cylinder = this.GetDoubleFrom("tb" + variable.Name + "_Cylinder");
            double axis = this.GetDoubleFrom("tb" + variable.Name + "_Axis");
            OpticalRepresentation or = new OpticalRepresentation(sphere, cylinder, axis);
            variable.Value = or;
        }

        private void SetSCAVariable(VarDef variable)
        {
            if (variable.Value == null)
            {

            }
            else
            {
                SphereCylinderAxis sca = ((OpticalRepresentation)variable.Value).SphereCylinderAxis;
                this.SetDoubleTo("tb" + variable.Name + "_Sphere", sca.Sphere);
                this.SetDoubleTo("tb" + variable.Name + "_Cylinder", sca.Cylinder);
                this.SetDoubleTo("tb" + variable.Name + "_Axis", sca.Axis.Degree);
            }
        }

        private void RecoverDoubleVariable(VarDef variable)
        {
            double value = this.GetDoubleFrom("tb" + variable.Name + "_Double");
            variable.Value = value;
        }


        private void RecoverVariables()
        {
            foreach (VarDef variable in this.system.Vars.Variables)
            {
                if (variable.Direction == VarDirection.Input)
                {
                    switch (variable.Type)
                    {
                        case VarType.AD:
                            RecoverADVariable(variable);
                            break;
                        case VarType.PAM:
                            RecoverPAMVariable(variable);
                            break;
                        case VarType.SCA:
                            RecoverSCAVariable(variable);
                            break;
                        case VarType.Double:
                            RecoverDoubleVariable(variable);
                            break;
                    }
                }
            }
        }

        private void SetOutputVariables()
        {
            foreach (VarDef variable in this.system.Vars.Variables)
            {
                if (variable.Direction == VarDirection.Output)
                {
                    switch (variable.Type)
                    {
                        case VarType.AD:
                            this.SetADVariable(variable);
                            break;
                        case VarType.PAM:
                            this.SetPAMVariable(variable);
                            break;
                        case VarType.SCA:
                            this.SetSCAVariable(variable);
                            break;
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserScript script = new UserScript();
            FormScript fs = new FormScript(script);
            if (fs.ShowDialog() == DialogResult.OK)
            {
                this.scripts.Add(script);
                this.ShowApplications();
                this.selected = null;
                this.panel1_MouseClick(this.panelsInverse[script], null);
                this.SaveApplications();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.selected != null)
            {
                UserScript script = this.panels[this.selected];
                FormScript fs = new FormScript(script);
                if (fs.ShowDialog() == DialogResult.OK)
                {
                    this.ShowApplications();
                    this.selected = null;
                    this.panel1_MouseClick(this.panelsInverse[script], null);
                    this.SaveApplications();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.selected != null)
            {
                UserScript script = this.panels[this.selected];
                this.panels.Remove(this.selected);
                //this.panelList.Remove(this.selected);
                this.panelsInverse.Remove(script);
                this.scripts.Remove(script);
                this.SaveApplications();
                this.ShowApplications();
                this.selected = null;
                this.ClearDynamicComponents();
            }
        }
    }
}
