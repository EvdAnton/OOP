using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab2OOP
{
    public sealed class FormForAdd
    {
        private static FormForAdd instance = null;
        public static Form GlobForm = null;

        private FormForAdd() { }

        public static FormForAdd getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormForAdd();
                }

                return instance;
            }
        }
        

        public void CreateForm()
        {
            Form form = new Form();
            form.AutoSize = true;
            form.BackColor = Color.LightSlateGray;
            form.Show();
            GlobForm = form;
            
        }

        public void CreateLabel(string name, int Top, Form form)
        {
            var field = new Label();
            field.Name = "lbl" + name;
            field.Left = 5;
            field.Top = Top - 15;
            field.Width = 105;
            field.Text = name;
            form.Controls.Add(field);
        }

        public void CreateTextBox(string name, int Top, Form form)
        {
            var field = new TextBox();
            field.Name = "edt" + name;
            field.Left = 5;
            field.Top = Top;
            field.Width = 105;
            form.Controls.Add(field);
        }

    }
}
