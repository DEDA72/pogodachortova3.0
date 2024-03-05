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
using System.IO;

namespace pogodachortova3_0
{
    public partial class FormMain : Form
    {
        public City city;
        public City objFavCity1;
        public City objFavCity2;
        string userName = "Guest";
        string favCitiesPath = "favCities.xml";
        string citiesListPath = "citiesList.txt";
        bool isFav1Empty = true;
        bool isFav2Empty = true;
        string favCity1 = "";
        string favCity2 = "";
        string weatherStatePict = "";

        public FormMain()
        {
            InitializeComponent();
            city = new City("Unknown");
        }

        private City FillObject(string cityName)
        {
            City filledObject = new City(cityName);

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load($"http://api.weatherapi.com/v1/current.xml?key=205f162dec5946729e3153756220707&q={cityName}&aqi=no");
                document.Save($"{cityName}.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load($"{cityName}.xml");

                XmlTextReader xmlTextReader = new XmlTextReader($"{cityName}.xml");
                xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
                while (xmlTextReader.Read())
                {
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "name")
                    {
                        xmlTextReader.Read();
                        filledObject.CityName = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "country")
                    {
                        xmlTextReader.Read();
                        filledObject.CountryName = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "lat")
                    {
                        xmlTextReader.Read();
                        filledObject.Latitude = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "lon")
                    {
                        xmlTextReader.Read();
                        filledObject.Longitude = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "temp_c")
                    {
                        xmlTextReader.Read();
                        filledObject.TemperatureCelsius = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "temp_f")
                    {
                        xmlTextReader.Read();
                        filledObject.TemperatureFahrenheit = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "condition")
                    {
                        xmlTextReader.Read();
                        xmlTextReader.Read();
                        filledObject.WeatherState = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "icon")
                    {
                        xmlTextReader.Read();
                        weatherStatePict = $"https:{xmlTextReader.Value}";
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "wind_dir")
                    {
                        xmlTextReader.Read();
                        filledObject.WindDirectionCardinal = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "wind_degree")
                    {
                        xmlTextReader.Read();
                        filledObject.WindDirectionDegrees = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "wind_kph")
                    {
                        xmlTextReader.Read();
                        filledObject.WindSpeed = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "pressure_mb")
                    {
                        xmlTextReader.Read();
                        filledObject.Pressure = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "humidity")
                    {
                        xmlTextReader.Read();
                        filledObject.Humidity = xmlTextReader.Value;
                    }
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "last_updated")
                    {
                        xmlTextReader.Read();
                        filledObject.LastUpdate = xmlTextReader.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return filledObject;
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuthorization fa = new FormAuthorization();
            if (fa.ShowDialog() == DialogResult.OK)
            {
                userName = fa.userName;
               
                logOutToolStripMenuItem.Enabled = true;
                loginToolStripMenuItem.Enabled = false;

                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(favCitiesPath);

                    XmlElement root = xmlDocument.DocumentElement;

                    if (root != null)
                    {
                        foreach (XmlElement node in root)
                        {
                            if (node.Name == userName)
                            {
                                foreach (XmlElement childnode in node.ChildNodes)
                                {
                                    if (childnode.Name == "FavCity1")
                                    {
                                        favCity1 = childnode.InnerText;
                                    }
                                    if (childnode.Name == "FavCity2")
                                    {
                                        favCity2 = childnode.InnerText;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (favCity1 != "")
                {
                    isFav1Empty = false;
                    labelEmpty1.Visible = false;
                    labelFavCity1.Visible = true;
                    linkLabelRemove1.Visible = true;
                    labelFavDegrees1.Visible = true;
                    pictureBoxFavFlag1.Visible = true;
                    pictureBoxFavWeatherState1.Visible = true;

                    city = FillObject(favCity1);
                    labelFavCity1.Text = city.CityName;
                    labelFavDegrees1.Text = $"{city.TemperatureCelsius}°";
                    pictureBoxFavFlag1.Image = Image.FromFile($"flags\\48\\{city.CountryName}.png");
                    pictureBoxFavWeatherState1.ImageLocation = weatherStatePict;
                }
                if (favCity2 != "")
                {
                    isFav2Empty = false;
                    labelEmpty2.Visible = false;
                    labelFavCity2.Visible = true;
                    linkLabelRemove2.Visible = true;
                    labelFavDegrees2.Visible = true;
                    pictureBoxFavFlag2.Visible = true;
                    pictureBoxFavWeatherState2.Visible = true;

                    city = FillObject(favCity2);
                    labelFavCity2.Text = city.CityName;
                    labelFavDegrees2.Text = $"{city.TemperatureCelsius}°";
                    pictureBoxFavFlag2.Image = Image.FromFile($"flags\\48\\{city.CountryName}.png");
                    pictureBoxFavWeatherState2.ImageLocation = weatherStatePict;
                }
                if (isFav1Empty == false && isFav2Empty == false)
                {
                    buttonFav.Enabled = false;
                }
                else
                {
                    buttonFav.Enabled = true;
                }
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("_kriss.sti", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string cityName = comboBoxCitiesList.Text;

            if (comboBoxCitiesList.Text == "")
            {
                MessageBox.Show("Выберите или введите название города", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            city = FillObject(cityName);
            pictureBoxWeatherState.ImageLocation = weatherStatePict;

            if (!comboBoxCitiesList.Items.Contains(comboBoxCitiesList.Text))
            {
                comboBoxCitiesList.Items.Add(comboBoxCitiesList.Text);
                File.AppendAllText(citiesListPath, $"\n{comboBoxCitiesList.Text}");
            }

            groupBoxInfo.Visible = true;

            labelCity.Text = city.CityName;
            labelCountry.Text = city.CountryName;
            labelLatitude.Text = $"{city.Latitude}°";
            labelLongitude.Text = $"{city.Longitude}°";
            if (celciusToolStripMenuItem.Checked)
            {
                labelTemperature.Text = $"{city.TemperatureCelsius}°C";
            }
            else if (fahrenheitFToolStripMenuItem.Checked)
            {
                labelTemperature.Text = $"{city.TemperatureFahrenheit}°F";
            }
            labelWeatherState.Text = city.WeatherState;
            labelUpdateDate.Text = city.LastUpdate;
            labelWindSpeed.Text = $"{city.WindSpeed} km/h";
            if (cardinalDirectionsToolStripMenuItem.Checked)
            {
                labelWindDirection.Text = city.WindDirectionCardinal;
            }
            else if (directionDegreesToolStripMenuItem.Checked)
            {
                labelWindDirection.Text = $"{city.WindDirectionDegrees}°";
            }
            labelPressure.Text = $"{city.Pressure} \"Hg";
            labelHumidity.Text = $"{city.Humidity}%";
            labelUpdateDate.Text = city.LastUpdate;
            pictureBoxFlag.Image = Image.FromFile($"flags\\48\\{city.CountryName}.png");
        }

        private void celciusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!celciusToolStripMenuItem.Checked)
            {
                celciusToolStripMenuItem.Checked = true;
                fahrenheitFToolStripMenuItem.Checked = false;
                labelTemperature.Text = $"{city.TemperatureCelsius}°C";
            }
        }

        private void fahrenheitFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fahrenheitFToolStripMenuItem.Checked)
            {
                celciusToolStripMenuItem.Checked = false;
                fahrenheitFToolStripMenuItem.Checked = true;
                labelTemperature.Text = $"{city.TemperatureFahrenheit}°F";
            }
        }

        private void directionDegreesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!directionDegreesToolStripMenuItem.Checked)
            {
                directionDegreesToolStripMenuItem.Checked = true;
                cardinalDirectionsToolStripMenuItem.Checked = false;
                labelWindDirection.Text = $"{city.WindDirectionDegrees}°";
            }
        }

        private void cardinalDirectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!cardinalDirectionsToolStripMenuItem.Checked)
            {
                cardinalDirectionsToolStripMenuItem.Checked = true;
                directionDegreesToolStripMenuItem.Checked = false;
                labelWindDirection.Text = city.WindDirectionCardinal;
            }
        }

        private void favouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!favouritesToolStripMenuItem.Checked)
            {
                favouritesToolStripMenuItem.Checked = true;
                groupBoxFav1.Visible = true;
                groupBoxFav2.Visible = true;
            }
            else if (favouritesToolStripMenuItem.Checked)
            {
                favouritesToolStripMenuItem.Checked = false;
                groupBoxFav1.Visible = false;
                groupBoxFav2.Visible = false;
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loginToolStripMenuItem.Enabled = true;
            logOutToolStripMenuItem.Enabled = false;
            buttonFav.Enabled = false;

            isFav1Empty = true;
            isFav2Empty = true;

            labelFavDegrees1.Visible = false;
            labelFavDegrees2.Visible = false;
            labelFavCity1.Visible = false;
            labelFavCity2.Visible = false;
            pictureBoxFavFlag1.Visible = false;
            pictureBoxFavFlag2.Visible = false;
            pictureBoxFavWeatherState1.Visible = false;
            pictureBoxFavWeatherState2.Visible = false;
            linkLabelRemove1.Visible = false;
            linkLabelRemove2.Visible = false;
            labelEmpty1.Visible = true;
            labelEmpty2.Visible = true;

            favCity1 = "";
            favCity2 = "";

            userName = "Guest";
            
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            
            string[] cities = File.ReadAllLines(citiesListPath);
            foreach (string city in cities)
            {
                comboBoxCitiesList.Items.Add(city);
            }
        }

        private void buttonFav_Click(object sender, EventArgs e)
        {
            if (comboBoxCitiesList.Text == "")
            {
                MessageBox.Show("Выберите город", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isFav1Empty == true)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(favCitiesPath);
                    XmlElement root = xmlDocument.DocumentElement;

                    if (root != null)
                    {
                        foreach (XmlElement node in root)
                        {
                            if (node.Name == userName)
                            {
                                if (node.HasChildNodes)
                                {
                                    foreach (XmlElement childnode in node.ChildNodes)
                                    {
                                        if (childnode.Name != "FavCity1")
                                        {
                                            XmlElement fc1 = xmlDocument.CreateElement("FavCity1");
                                            XmlNode fc1Name = xmlDocument.CreateTextNode(comboBoxCitiesList.Text);
                                            fc1.AppendChild(fc1Name);
                                            node.AppendChild(fc1);
                                            xmlDocument.Save(favCitiesPath);
                                            goto Skip;
                                        }
                                    }
                                }
                                else if (!node.HasChildNodes)
                                {
                                    XmlElement fc1 = xmlDocument.CreateElement("FavCity1");
                                    XmlNode fc1Name = xmlDocument.CreateTextNode(comboBoxCitiesList.Text);
                                    fc1.AppendChild(fc1Name);
                                    node.AppendChild(fc1);
                                    xmlDocument.Save(favCitiesPath);
                                    goto Skip;
                                }
                            }
                        }
                    }

                    XmlElement newUser = xmlDocument.CreateElement(userName);

                    XmlElement favC1 = xmlDocument.CreateElement("FavCity1");
                    XmlNode favC1Name = xmlDocument.CreateTextNode(comboBoxCitiesList.Text);

                    favC1.AppendChild(favC1Name);
                    newUser.AppendChild(favC1);
                    root.AppendChild(newUser);
                    xmlDocument.Save(favCitiesPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            Skip:
                isFav1Empty = false;
                labelEmpty1.Visible = false;
                labelFavCity1.Visible = true;
                linkLabelRemove1.Visible = true;
                labelFavDegrees1.Visible = true;
                pictureBoxFavFlag1.Visible = true;
                pictureBoxFavWeatherState1.Visible = true;

                city = FillObject(comboBoxCitiesList.Text);
                labelFavCity1.Text = city.CityName;
                labelFavDegrees1.Text = $"{city.TemperatureCelsius}°";
                pictureBoxFavFlag1.Image = Image.FromFile($"flags\\48\\{city.CountryName}.png");
                pictureBoxFavWeatherState1.ImageLocation = weatherStatePict;
            }
            else if (isFav1Empty == false && isFav2Empty == true)
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(favCitiesPath);
                    XmlElement root = xmlDocument.DocumentElement;

                    if (root != null)
                    {
                        foreach (XmlElement node in root)
                        {
                            if (node.Name == userName)
                            {
                                foreach (XmlElement childnode in node.ChildNodes)
                                {
                                    if (childnode.Name != "FavCity2")
                                    {
                                        XmlElement fc2 = xmlDocument.CreateElement("FavCity2");
                                        XmlNode fc2Name = xmlDocument.CreateTextNode(comboBoxCitiesList.Text);
                                        fc2.AppendChild(fc2Name);
                                        node.AppendChild(fc2);
                                        xmlDocument.Save(favCitiesPath);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                isFav2Empty = false;
                labelEmpty2.Visible = false;
                labelFavCity2.Visible = true;
                linkLabelRemove2.Visible = true;
                labelFavDegrees2.Visible = true;
                pictureBoxFavFlag2.Visible = true;
                pictureBoxFavWeatherState2.Visible = true;

                city = FillObject(comboBoxCitiesList.Text);
                labelFavCity2.Text = city.CityName;
                labelFavDegrees2.Text = $"{city.TemperatureCelsius}°";
                pictureBoxFavFlag2.Image = Image.FromFile($"flags\\48\\{city.CountryName}.png");
                pictureBoxFavWeatherState2.ImageLocation = weatherStatePict;
            }

            if (isFav1Empty == false && isFav2Empty == false)
            {
                buttonFav.Enabled = false;
            }
        }

        private void linkLabelRemove1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(favCitiesPath);
                XmlElement root = xmlDocument.DocumentElement;

                if (root != null)
                {
                    foreach (XmlElement node in root)
                    {
                        if (node.Name == userName)
                        {
                            foreach (XmlElement childnode in node.ChildNodes)
                            {
                                if (childnode.Name == "FavCity1")
                                {
                                    node.RemoveChild(childnode);
                                    xmlDocument.Save(favCitiesPath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            favCity1 = "";
            labelFavCity1.Visible = false;
            linkLabelRemove1.Visible = false;
            labelFavDegrees1.Visible = false;
            pictureBoxFavFlag1.Visible = false;
            pictureBoxFavWeatherState1.Visible = false;
            labelEmpty1.Visible = true;
            isFav1Empty = true;

            if (isFav1Empty == true || isFav2Empty == true)
            {
                buttonFav.Enabled = true;
            }
        }

        private void linkLabelRemove2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(favCitiesPath);
                XmlElement root = xmlDocument.DocumentElement;

                if (root != null)
                {
                    foreach (XmlElement node in root)
                    {
                        if (node.Name == userName)
                        {
                            foreach (XmlElement childnode in node.ChildNodes)
                            {
                                if (childnode.Name == "FavCity2")
                                {
                                    node.RemoveChild(childnode);
                                    xmlDocument.Save(favCitiesPath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            favCity2 = "";
            labelFavCity2.Visible = false;
            linkLabelRemove2.Visible = false;
            labelFavDegrees2.Visible = false;
            pictureBoxFavFlag2.Visible = false;
            pictureBoxFavWeatherState2.Visible = false;
            labelEmpty2.Visible = true;
            isFav2Empty = true;

            if (isFav1Empty == true || isFav2Empty == true)
            {
                buttonFav.Enabled = true;
            }
        }
    }
}
