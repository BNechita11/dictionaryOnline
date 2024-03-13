using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class ModulDeCautare : Window
    {
        private Dictionary<string, Tuple<string, string>> userAccounts;
        private Methods methods;

        public ModulDeCautare()
        {
            InitializeComponent();
            methods = new Methods(@"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml");
            LoadCategories();
        }

        private void LoadCategories()
        {
            cmbCategory.ItemsSource = methods.LoadCategories();
           
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtWord.Text;
            string category = cmbCategory.SelectedItem as string;

            if (string.IsNullOrEmpty(searchTerm) && string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Introduceți un termen de căutare sau selectați o categorie.");
                return;
            }
            lstSearchResults.ItemsSource = methods.SearchWord(searchTerm, category);
            
        }

        private void lstSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSearchResults.SelectedItem != null)
            {
                string selectedWord = lstSearchResults.SelectedItem.ToString();
                DisplayWordDetails(selectedWord);
            }
            else
            {
                ClearWordDetails();
            }
        }

        private void DisplayWordDetails(string selectedWord)
        {
            Dictionary<string, string> wordDetails = methods.GetWordDetails(selectedWord);

            if (wordDetails.Count > 0)
            {
                txtWordDetails.Text = $"Cuvânt: {selectedWord}";
                txtDescription.Text = $"Descriere: {wordDetails["Description"]}";
                txtCategory.Text = $"Categorie: {wordDetails["Category"]}";

                if (!string.IsNullOrEmpty(wordDetails["ImagePath"]))
                {
                    imgWord.Source = new BitmapImage(new Uri(wordDetails["ImagePath"]));
                }
                else
                {
                    imgWord.Source = null;
                }
            }
            else
            {
                ClearWordDetails();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            txtWord.Text = string.Empty;
            cmbCategory.SelectedItem = null;
            lstSearchResults.ItemsSource = null;
            imgWord.Source = null;
            ClearWordDetails();
        }

        private void ClearWordDetails()
        {
            txtWordDetails.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtCategory.Text = string.Empty;
            lstSearchResults.Items.Clear();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ModulDivertisment modulDivertisment = new ModulDivertisment();
            modulDivertisment.Show();
            this.Hide();
        }


        //private void BackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(methods.loggedInUsername) && methods.userAccounts.ContainsKey(methods.loggedInUsername))
        //    {
        //        string roles = methods.userAccounts[methods.loggedInUsername].Item2;

        //        if (roles != null && roles.Contains("Administrator"))
        //        {
        //            ModulAdministrativ modulAdminWindow = new ModulAdministrativ();
        //            modulAdminWindow.Show();
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Nu aveți permisiuni pentru a accesa modulul administrator!");
        //        }
        //    }
        //    else
        //    {
        //        ModulAdministrativ modulAdminWindow = new ModulAdministrativ();
        //        modulAdminWindow.Show();
        //        this.Close();
        //    }
        //}



        private void AuthenticateUser(string username, string password)
        {
            methods.AuthenticateUser(username, password);
        }


        private void DisplayWordSuggestions(string category, string searchTerm)
        {
            if (cmbCategory.SelectedItem != null)
            {
                methods.DisplayWordSuggestions(lstSearchResults, category, searchTerm);
            }
        }

        private void txtWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayWordSuggestions(cmbCategory.SelectedItem as string, txtWord.Text);
        }


    }
}
