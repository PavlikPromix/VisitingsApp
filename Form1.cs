using System.Text.Json;

namespace VisitingsApp
{
    public partial class Form1 : Form
    {
        List<Person>? people;
        string path = @"data.json";
        public Form1()
        {
            InitializeComponent();
            editModeToolStripMenuItem.Checked = true; // debug
            people = new List<Person>();
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisitingApp\data.json";
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VisitingApp");
            if (File.Exists(path))
                LoadData();
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

        private void CreateName(string name, bool c)
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

            panel.Controls.Add(new Label
            {
                Text = name,
                Bounds = new Rectangle(30, 3, 140, 25)
            });

            panel.Controls.Add(new CheckBox
            {
                Text = "",
                Bounds = new Rectangle(170, 3, 25, 25),
                Checked = c
            });

            flowLayoutPanel1.Controls.Add(panel);
        }

        private void GetNames()
        {
            people.Clear();
            if (flowLayoutPanel1.Controls.Count == 0)
                return;
            foreach (var panel in flowLayoutPanel1.Controls)
            {
                string name = "";
                bool check = false;
                foreach (var item in (panel as Panel).Controls)
                {
                    if (item.GetType() == typeof(Label))
                    {
                        name = (item as Label).Text;
                    }
                    if (item.GetType() == typeof(CheckBox))
                    {
                        check = (item as CheckBox).Checked;
                    }
                }
                people.Add(new Person(name, check));
            }
        }

        private void UpdateNames()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var p in people)
                CreateName(p.Name, p.Checked);
        }

        private void SortNames()
        {
            people = people.OrderBy(p => p.Name).ToList();
            UpdateNames();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!editModeToolStripMenuItem.Checked)
                HideEditMenu();
            else UnhideEditMenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateName(textBox1.Text, false);
            GetNames();
            SortNames();
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            Button s = sender as Button;
            s.Click -= Button_Click;
            foreach (var item in s.Parent.Controls)
            {
                if (item.GetType() == typeof(Label))
                {
                    people.RemoveAt(people.FindIndex(p => p.Name == (item as Label).Text));
                }
            }
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

        private async void saveToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            await SaveData();
        }

        private async Task SaveData()
        {
            GetNames();
            await File.WriteAllTextAsync(path, string.Empty);
            using (FileStream f = new FileStream(path, FileMode.OpenOrCreate))
                await JsonSerializer.SerializeAsync<List<Person>>(f, people);
            
        }

        private async void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            using (FileStream f = new FileStream(path, FileMode.OpenOrCreate))
                people = await JsonSerializer.DeserializeAsync<List<Person>>(f);
            UpdateNames();
        }
    }
}