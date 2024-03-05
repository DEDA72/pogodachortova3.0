using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace pogodachortova3_0
{
    public partial class FormRegistration : Form
    {
        public string userName;

        public FormRegistration()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string path = "accounts.xml";

            if (textBoxPassword.Text != textBoxPasswordRepeat.Text)
            {
                MessageBox.Show("Пароли не совпадают! Попробуйте еще раз", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxPassword.Text == textBoxPasswordRepeat.Text)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(path);

                    XmlNode root = xmlDocument.DocumentElement;

                    if (root != null)
                    {
                        foreach (XmlElement node in root)
                        {
                            foreach (XmlElement childnode in node.ChildNodes)
                            {
                                if (childnode.Name == "UserName" && childnode.InnerText == textBoxLogin.Text)
                                {
                                    MessageBox.Show("Пользователь с данным именем уже существует! Попробуйте сменить имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }

                    XmlElement newUser = xmlDocument.CreateElement("User");

                    XmlElement n1 = xmlDocument.CreateElement("UserName");
                    XmlElement n2 = xmlDocument.CreateElement("Password");

                    XmlNode t1 = xmlDocument.CreateTextNode(textBoxLogin.Text);
                    XmlNode t2 = xmlDocument.CreateTextNode(textBoxPassword.Text);

                    n1.AppendChild(t1);
                    n2.AppendChild(t2);

                    newUser.AppendChild(n1);
                    newUser.AppendChild(n2);

                    root.AppendChild(newUser);
                    xmlDocument.Save(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            userName = textBoxLogin.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
