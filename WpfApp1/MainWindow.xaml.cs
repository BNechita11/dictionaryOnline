using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using WpfApp1;

namespace DictionarExplicativ
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, Tuple<string, string>> userAccounts; 

        public MainWindow()
        {
            InitializeComponent();
            LoadUserAccounts();
        }

        private void LoadUserAccounts()
        {
            try
            {
                string filePath = @"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\userAccounts.xml";

                XDocument doc = XDocument.Load(filePath);

                userAccounts = new Dictionary<string, Tuple<string, string>>();

                foreach (XElement userElement in doc.Root.Elements("User"))
                {
                    string username = userElement.Element("Username").Value;
                    string password = userElement.Element("Password").Value;
                    string roles = userElement.Element("Role")?.Value;
                    userAccounts.Add(username, Tuple.Create(password, roles));
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul userAccounts.xml nu a fost găsit.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea conturilor de utilizatori: " + ex.Message);
                Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            
            if (userAccounts.ContainsKey(username))
            {
               
                if (userAccounts[username].Item1 == password)
                {
                    string roles = userAccounts[username].Item2;

                   
                    if (roles != null && roles.Contains("Administrator"))
                    {
                        MessageBox.Show("Autentificare reușită! Bine ai venit administratore, " + username + "!");
                        ModulAdministrativ modulAdminWindow = new ModulAdministrativ();
                        modulAdminWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nu aveți permisiuni pentru a accesa modulul administrator, dar vă puteți conecta ca și utilizator! Bine ai venit utilizatorule, " + username + "!");
                        ModulDeCautare modulDeCautare = new ModulDeCautare();
                        modulDeCautare.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Parolă incorectă! Vă rugăm să încercați din nou.");
                }
            }
            else
            {
                MessageBox.Show("Numele de utilizator nu există! Vă rugăm să încercați din nou sau să creați un cont nou.");
            }
        }


    }
}
