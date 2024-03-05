using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace pogodachortova3_0
{
    public partial class FormAuthorization : Form
    {
        public string userName;
        string path = "accounts.xml";
        private bool isLogin = false;
        private bool isPassword = false;

        public FormAuthorization()
        {
            InitializeComponent();
        }

        private void linkLabelRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRegistration fr = new FormRegistration();
            if (fr.ShowDialog() == DialogResult.OK)
            {
                userName = fr.userName;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (File.Exists(path))
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
                            foreach (XmlElement childNode in node.ChildNodes)
                            {
                                if (childNode.Name == "UserName" && childNode.InnerText == textBoxLogin.Text)
                                {
                                    isLogin = true;
                                }
                                if (childNode.Name == "Password" && childNode.InnerText == textBoxPassword.Text)
                                {
                                    isPassword = true;
                                }
                            }
                        }
                    }

                    if (isLogin == true && isPassword == true)
                    {
                        userName = textBoxLogin.Text;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }
    }
}
