namespace VisitingsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void HideEditMenu()
        {
            button1.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
            flowLayoutPanel1.Height = this.Height - 78;
            foreach (var panel in flowLayoutPanel1.Controls)
            {
                foreach (var item in (panel as Panel).Controls)
                {
                    if (item.GetType() == typeof(Button))
                    {
                        (item as Button).Visible = false;
                    }
                }
            }
        }

        private void UnhideEditMenu()
        {
            button1.Visible = true;
            label1.Visible = true;
            textBox1.Visible = true;
            flowLayoutPanel1.Height = this.Height - 116;
            foreach (var panel in flowLayoutPanel1.Controls)
            {
                foreach (var item in (panel as Panel).Controls)
                {
                    if (item.GetType() == typeof(Button))
                    {
                        (item as Button).Visible = true;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!editModeToolStripMenuItem.Checked)
                HideEditMenu();
            else UnhideEditMenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Panel panel = new Panel() { Size = new Size(200, 30), };

            Button button = new Button
            {
                Text = "X",
                Bounds = new Rectangle(3, 3, 25, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
            };
            button.Click += Button_Click;
            panel.Controls.Add(button);
            
            panel.Controls.Add(new Label { 
                Text = textBox1.Text,
                Bounds = new Rectangle(30, 3, 140, 25)
            });
            
            panel.Controls.Add(new CheckBox { 
                Text = "", 
                Bounds = new Rectangle(170, 3, 25, 25)
            });
            
            flowLayoutPanel1.Controls.Add(panel);
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            Button s = sender as Button;
            s.Click -= Button_Click;
            s.Parent.Dispose();
        }

        private void editModeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (editModeToolStripMenuItem.Checked)
                UnhideEditMenu();
            else HideEditMenu();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (editModeToolStripMenuItem.Checked)
                flowLayoutPanel1.Height = this.Height - 116;
            else flowLayoutPanel1.Height = this.Height - 78;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1.PerformClick();
                (sender as TextBox).Text = "";
            }
        }
    }
}