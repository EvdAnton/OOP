using System;
using System.Windows.Forms;
using HSLibrary;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Json;


namespace Lab2OOP
{
    public partial class Form1 : Form
    {
        
        IEnumerable<Type> list;
        List<object> elements = new List<object>();
        bool CreatedObj = false;
        List<Point> position = new List<Point>();
        List<Spell> spells = new List<Spell>();
        List<HeroCard> heroCards = new List<HeroCard>();
        List<Weapon> weapons = new List<Weapon>();
        List<Minion> minions = new List<Minion>();
        List<Assembly> myPlagins = new List<Assembly>();
        List<Assembly> myPlaginsHierarchi = new List<Assembly>();
        List<Type> SpecificType = new List<Type>();
        string ImageName;

        public Form1()
        {
            InitializeComponent();


            label1.Left = 5;
            this.Width = 400;
            this.Height = 550;
            ChooseCard.Left = 70 + label1.Width;
            ChooseCard.Width = 120;
            label1.Top = ChooseCard.Top = 35;

            AddAtForm();
        }

        private void AddAtForm()
        {
            int i = 0;
            var ourtype = typeof(Cards);
            list = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));
            foreach (var itm in list)
            {
                var attributes = itm.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    if (i % 2 == 0)
                        ChooseCard.Items.Add(attribute);
                    i++;
                }
            }
        }

      

        public void but_Click(object sender, EventArgs e)
        {
            string image = "";
            bool IsSelect = true;
            var ChoosenType = GetMyType(ChooseCard);
            var properties = ChoosenType.GetProperties();


            object obj = CreateObject(ChoosenType, properties);

            foreach (var prop in properties)
            {
                if (prop.PropertyType.IsEnum)
                {
                    ComboBox MyCombo = FormForAdd.GlobForm.Controls["combo" + prop.Name] as ComboBox;
                    if (MyCombo.SelectedItem != null)
                    {
                        prop.SetValue(obj, MyCombo.SelectedItem);
                        if (prop.PropertyType.Name == typeof(CardsCategorize).Name)
                            image = MyCombo.SelectedItem.ToString();
                    }
                    else
                        IsSelect = false;
                }
                else
                {
                    TextBox Mybox = FormForAdd.GlobForm.Controls["edt" + prop.Name] as TextBox;
                    if (prop.Name != "Health")
                    {
                        if ((Mybox.Text.Length > 0) && (Convert.ToInt32(Mybox.Text) > -1) && (Convert.ToInt32(Mybox.Text) < 11))
                            prop.SetValue(obj, Convert.ToInt32(Mybox.Text));
                        else
                            IsSelect = false;
                    }
                }
            }
            if (IsSelect)
            {
                MessageBox.Show("You create new card!");
                CreatedObj = true;
                elements.Add(obj);
                ImageName = image;
            }
            else
                MessageBox.Show("Fill all fields or enter values low then 10 and more then 0");
        }

        private void ShowObject(string image)
        {
            if (image == null)
                return;
            if ((image == "Priest") || (image == "Neuteral"))
                newPB.Image = new Bitmap($"{image}.png", true);
            else if (image != "Warlock")
                newPB.Image = new Bitmap($"{image}.jpg", true);
            else
                newPB.Image = new Bitmap($"{image}.jpeg", true);
            this.Controls.Add(newPB);
            ImageName = "";
        }

        int numbers = 0;
        PictureBox newPB;

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (CreatedObj)
            {
                FormForAdd.GlobForm.Close();
                ChooseCard.Enabled = true;
                FormForAdd.GlobForm = null;

                CreatedObj = false;

                CreatePictureBox(e);
                ShowObject(ImageName);
            }
        }

        private void CreatePictureBox(MouseEventArgs e)
        {
            var field = new PictureBox();
            field.Click += this.field_Clickimg;
            field.SizeMode = PictureBoxSizeMode.StretchImage;
            field.Left = e.X;
            field.Width = 160;
            field.Height = 140;
            field.Top = e.Y;
            var point = new Point();
            point.X = field.Left;
            point.Y = field.Top;
            position.Add(point);
            field.Name = "picture" + position[numbers].X.ToString();
            numbers++;
            newPB = field;
        }

        public void field_Clickimg(object sender, EventArgs e)
        {
            int CursorX = Cursor.Position.X - this.Left - 8;
            int CursorY = Cursor.Position.Y - this.Top - 30;
            int count = 0;
            bool IsFound = false;
            foreach (var i in position)
            {
                if ((i.X + 150 >= CursorX) && (i.X <= CursorX) && (i.Y <= CursorY) && (i.Y + 140 >= CursorY))
                {
                    IsFound = true;
                    break;
                }
                else
                    count++;
            }
            if (IsFound)
            {
                var field = this.Controls["picture" + position[count].X.ToString()] as PictureBox;
                if (field != null)
                {
                    field.Dispose();
                    elements[count] = null;
                    numbers--;
                }
            }
        }

        private Type GetMyType(ComboBox Combo)
        {
            int i = 0;

            if (Combo.SelectedIndex > (list.Count() - 1))
                return SpecificType[Combo.SelectedIndex - list.Count()];

            foreach (var item in list)
            {
                if (i != Combo.SelectedIndex)
                    i++;
                else
                    return item;
            }

            return null;
        }


        private object CreateObject(Type ChoosenType, PropertyInfo[] properties)
        {
            var obj = Activator.CreateInstance(ChoosenType);
            if (ChoosenType == typeof(HeroCard))
            {
                foreach (var prop in properties)
                {
                    if ((prop.PropertyType.IsEnum) && (prop.PropertyType == typeof(CardsRarity)))
                    {
                        ComboBox MyCombo = FormForAdd.GlobForm.Controls["combo" + prop.Name] as ComboBox;
                        MyCombo.SelectedItem = CardsRarity.Legendary;
                        MyCombo.Enabled = false;
                    }
                    else
                    {
                        InitilizeTextBox("Health", prop, 30);
                        InitilizeTextBox("Armor", prop, 5);
                    }
                }
            }

            return obj;
        }

        private void InitilizeTextBox(string name, PropertyInfo prop, int param)
        {
            if (prop.Name.ToString() == name)
            {
                TextBox MyBox = FormForAdd.GlobForm.Controls["edt" + prop.Name] as TextBox;
                MyBox.Text = param.ToString();
                MyBox.Enabled = false;
            }
        }

        private void ChooceHero_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Controls["richTextBox"] != null)
                this.Controls.Remove(Controls["richTextBox"]);
            var ChoosenType = GetMyType(ChooseCard);
            FormForAdd.getInstance.CreateForm();
            var form = FormForAdd.GlobForm;

            var properties = ChoosenType.GetProperties();

            int Top = 25;
            foreach (var prop in properties)
            {
                if (prop.PropertyType.IsEnum)
                    CreateComboBox(prop.Name, Top, prop, form);
                else
                    FormForAdd.getInstance.CreateTextBox(prop.Name, Top, form);
                FormForAdd.getInstance.CreateLabel(prop.Name, Top, form);
                Top += 50;
            }

            var obj = CreateObject(ChoosenType, properties);
            CreateBtn(form, Top);
            if (FormForAdd.GlobForm != null)
                ChooseCard.Enabled = false;
            FormForAdd.GlobForm.FormClosing += this.EnabledTrue;
        }

        private void EnabledTrue(object sender, FormClosingEventArgs e)
        {
            FormForAdd.GlobForm = null;
            ChooseCard.Enabled = true;
        }

        private void CreateComboBox(string name, int Top, PropertyInfo info, Form form)
        {
            var field = new ComboBox();
            field.Name = "combo" + name;
            field.Left = 5;
            field.Top = Top;
            field.Width = 105;

            foreach (var itemenum in info.PropertyType.GetEnumValues())
            {
                field.Items.Add(itemenum);
            }
            form.Controls.Add(field);
            field.SelectedIndexChanged += this.field_SelectedIndexChange;
        }

        public void field_SelectedIndexChange(object sender, EventArgs e)
        {
            var ChoosenType = GetMyType(ChooseCard);
            var properties = ChoosenType.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType.IsEnum)
                {
                    ComboBox MyCombo = FormForAdd.GlobForm.Controls["combo" + prop.Name] as ComboBox;
                    if (((prop.Name == "Hero") || (prop.Name == "HeroAbility")) && ((MyCombo.SelectedItem != null)))
                    {
                        if (prop.Name == "Hero")
                        {
                            ComboBox MyBox = FormForAdd.GlobForm.Controls["comboHeroAbility"] as ComboBox;
                            MyBox.SelectedIndex = MyCombo.SelectedIndex;
                        }
                        else
                        {
                            ComboBox MyBox = FormForAdd.GlobForm.Controls["comboHero"] as ComboBox;
                            MyBox.SelectedIndex = MyCombo.SelectedIndex;
                        }
                    }
                }
            }
        }

        private void CreateBtn(Form form, int Top)
        {
            var but = new Button();
            but.Name = "button";
            but.Text = "Create";
            but.Top = Top - 10;
            but.Left = 5;
            but.Width = 105;
            but.Click += this.but_Click;
            form.Controls.Add(but);
        }


        DataContractJsonSerializer jsonFormatterSpell = jsonFormat(typeof(List<Spell>));
        DataContractJsonSerializer jsonFormatterHeroCard = jsonFormat(typeof(List<HeroCard>));
        DataContractJsonSerializer jsonFormatterWeapon = jsonFormat(typeof(List<Weapon>));
        DataContractJsonSerializer jsonFormatterMinion = jsonFormat(typeof(List<Minion>));

        private void CreateLists()
        {
            foreach (var type in elements)
            {
                if (type.GetType() == typeof(Spell))
                    spells.Add((Spell)type);
                if (type.GetType() == typeof(HeroCard))
                    heroCards.Add((HeroCard)type);
                if (type.GetType() == typeof(Weapon))
                    weapons.Add((Weapon)type);
                if (type.GetType() == typeof(Minion))
                    minions.Add((Minion)type);
            }
        }

        private void DeleteAllobjectsLists()
        {
            spells.Clear();
            minions.Clear();
            weapons.Clear();
            heroCards.Clear();
        }

        private void DeleteJsonFiles(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {


            CreateLists();

            DeleteJsonFiles("spells.json");
            using (var fStream = new FileStream("spells.json", FileMode.OpenOrCreate))
            {
                jsonFormatterSpell.WriteObject(fStream, spells);
            }

            DeleteJsonFiles("heroCards.json");
            using (var fStream = new FileStream("heroCards.json", FileMode.OpenOrCreate))
            {
                jsonFormatterHeroCard.WriteObject(fStream, heroCards);
            }

            DeleteJsonFiles("minions.json");
            using (var fStream = new FileStream("minions.json", FileMode.OpenOrCreate))
            {
                jsonFormatterMinion.WriteObject(fStream, minions);
            }

            DeleteJsonFiles("weapons.json");
            using (var fStream = new FileStream("weapons.json", FileMode.OpenOrCreate))
            {
                jsonFormatterWeapon.WriteObject(fStream, weapons);
            }


            DeleteAllobjectsLists();
        }

        private static DataContractJsonSerializer jsonFormat(Type type)
        {
            DataContractJsonSerializer jsonForm = new DataContractJsonSerializer(type);
            return jsonForm;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var memo = CreateMemo();
            for (var i = 0; i < position.Count; i++)
                this.Controls.Remove(Controls["picture" + position[i].X.ToString()]);

            using (var fStream = new FileStream("spells.json", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                {
                    spells = (List<Spell>)jsonFormatterSpell.ReadObject(fStream);
                    if (spells.Count > 0)
                    {
                        memo.Text += "Spells----->\n";
                        foreach (var el in spells)
                        {
                            elements.Add(el);
                            var type = el.GetType();
                            foreach (var prop in type.GetProperties())
                            {
                                if (prop.PropertyType.IsEnum)
                                    memo.Text += prop.GetValue(el) + "\n";
                                else
                                    memo.Text += prop.GetValue(el).ToString() + '\n';
                            }
                        }
                    }
                }
            }

            using (var fStream = new FileStream("heroCards.json", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                {
                    heroCards = (List<HeroCard>)jsonFormatterHeroCard.ReadObject(fStream);
                    if (heroCards.Count > 0)
                    {
                        memo.Text += "\nHeroCards----->\n";
                        foreach (var el in heroCards)
                        {
                            elements.Add(el);
                            var type = el.GetType();
                            foreach (var prop in type.GetProperties())
                            {
                                if (prop.PropertyType.IsEnum)
                                    memo.Text += prop.GetValue(el) + "\n";
                                else
                                    memo.Text += prop.GetValue(el).ToString() + '\n';
                            }
                        }
                    }
                }
            }

            using (var fStream = new FileStream("minions.json", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                {
                    minions = (List<Minion>)jsonFormatterMinion.ReadObject(fStream);
                    if (minions.Count > 0)
                    {
                        memo.Text += "\nMinions----->\n";
                        foreach (var el in minions)
                        {
                            elements.Add(el);
                            var type = el.GetType();
                            foreach (var prop in type.GetProperties())
                            {
                                if (prop.PropertyType.IsEnum)
                                    memo.Text += prop.GetValue(el) + "\n";
                                else
                                    memo.Text += prop.GetValue(el).ToString() + '\n';
                            }
                        }
                    }
                }
            }

            using (var fStream = new FileStream("weapons.json", FileMode.OpenOrCreate))
            {
                if (fStream.Length == 0)
                    return;

                weapons = (List<Weapon>)jsonFormatterWeapon.ReadObject(fStream);
                if (weapons.Count > 0)
                {
                    memo.Text += "\nWeapons----->\n";
                    foreach (var el in weapons)
                    {
                        elements.Add(el);
                        var type = el.GetType();
                        foreach (var prop in type.GetProperties())
                        {
                            if (prop.PropertyType.IsEnum)
                                memo.Text += prop.GetValue(el) + "\n";
                            else
                                memo.Text += prop.GetValue(el).ToString() + '\n';
                        }
                    }
                }
            }

            DeleteAllobjectsLists();
        }

        private RichTextBox CreateMemo()
        {
            var memo = new RichTextBox();
            memo.Name = "richTextBox";
            memo.Top = 80;
            memo.Left = 50;
            memo.Width = 300;
            memo.Height = 400;
            memo.ScrollBars = RichTextBoxScrollBars.Both;
            this.Controls.Add(memo);
            return memo;
        }

        private void getPlaginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Assembly ass = null;
            string dirName = "C:/Users/evdan/My_Project/Projects/OOPHS/Plaginus/Plagin/Plagin/bin/Debug/netstandard2.0/";
            string[] files = Directory.GetFiles(dirName);

            foreach (string file in files)
            {
                if (file.IndexOf(".dll") > 0)
                {
                    FileInfo fileInf = new FileInfo(file);
                    ass = Assembly.LoadFrom(fileInf.FullName);
                    if (!myPlagins.Contains(ass))
                        myPlagins.Add(ass);
                    foreach (Assembly plagin in myPlagins)
                        ViewPlagins(plagin);
                }
            }

        }

        private void ViewPlagins(Assembly ass)
        {
            Type[] types = ass.GetTypes();
            foreach (Type myType in types)
            {
                if (myType.IsClass)
                {
                    Object myObject = Activator.CreateInstance(myType);
                    MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (MethodInfo thisMethod in myArrayMethodInfo)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(thisMethod.Name);
                        item.Name = thisMethod.Name;
                        item.Click += new EventHandler(item_Click);
                        if (fileToolStripMenuItem.DropDownItems.Find(item.Name, false).Length == 0)
                            fileToolStripMenuItem.DropDownItems.Add(item);
                    }

                }
            }
        }

        private void item_Click(object sender, EventArgs e)
        {
            foreach (Assembly plagin in myPlagins)
            {
                Type[] types = plagin.GetTypes();
                foreach (Type myType in types)
                {
                    if (myType.IsClass)
                    { 
                        Object myObject = Activator.CreateInstance(myType);
                        MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly );
                        foreach (MethodInfo thisMethod in myArrayMethodInfo)
                            if (thisMethod.Name == sender.ToString())
                                thisMethod.Invoke(myObject, new object[] { });
                    }
                }
            }
        }

        private void getIeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Assembly ass = null;
            string dirName = "C:/Users/evdan/source/repos/PlaginHierarchi/PlaginHierarchi/bin/Debug/netstandard2.0";
            string[] files = Directory.GetFiles(dirName);

            foreach (string file in files)
            {
                if (file.IndexOf(".dll") > 0)
                {
                    FileInfo fileInf = new FileInfo(file);
                    ass = Assembly.LoadFrom(fileInf.FullName);
                    if (!myPlaginsHierarchi.Contains(ass))
                        myPlaginsHierarchi.Add(ass);
                    foreach (Assembly plagin in myPlaginsHierarchi)
                        ViewHierarchiPlagin(plagin);
                }
            }
        }

        private void ViewHierarchiPlagin(Assembly ass)
        {
            Type[] types = ass.GetTypes();
            foreach (Type myType in types)
            {
                if (myType.IsClass)
                {
                    SpecificType.Add(myType);
                    ChooseCard.Items.Add(myType.Name);
                }
            }
        }
    }
}
