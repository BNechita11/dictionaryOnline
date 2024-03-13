using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WpfApp1
{
    internal class Methods
    {
        public string dictionaryFilePath;
        public Dictionary<string, Tuple<string, string>> userAccounts;
        public string loggedInUsername;
        public ModulDeCautare modulDeCautare;

        public Methods(string filePath)
        {
            dictionaryFilePath = filePath;
            userAccounts = new Dictionary<string, Tuple<string, string>>();
            loggedInUsername = string.Empty;
            modulDeCautare = null;
        }

        //functii MC
        public List<string> LoadCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                XDocument dictionaryDoc = XDocument.Load(dictionaryFilePath);

                categories = dictionaryDoc.Root.Elements("Word")
                    .Select(wordElement => wordElement.Element("Category").Value)
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea categoriilor: " + ex.Message);
            }

            return categories;
        }
        public void DisplayWordSuggestions(ListBox lstSearchResults, string category, string searchTerm)
        {
            try
            {
                XDocument dictionaryDoc = XDocument.Load(dictionaryFilePath);

                var filteredWords = SearchWord(searchTerm, category);

                lstSearchResults.Items.Clear();
                foreach (string word in filteredWords)
                {
                    lstSearchResults.Items.Add(word);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea sugestiilor de cuvinte: " + ex.Message);
            }
        }


        public List<string> SearchWord(string searchTerm, string category)
        {
            List<string> filteredWords = new List<string>();

            try
            {
                XDocument dictionaryDoc = XDocument.Load(dictionaryFilePath);

                filteredWords = dictionaryDoc.Root.Elements("Word")
                    .Where(wordElement =>
                        (string.IsNullOrEmpty(searchTerm) || wordElement.Element("Name").Value.Contains(searchTerm)) &&
                        (string.IsNullOrEmpty(category) || wordElement.Element("Category").Value == category))
                    .Select(wordElement => wordElement.Element("Name").Value)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la căutarea cuvintelor: " + ex.Message);
            }

            return filteredWords;
        }

        public Dictionary<string, string> GetWordDetails(string selectedWord)
        {
            Dictionary<string, string> wordDetails = new Dictionary<string, string>();

            try
            {
                XDocument dictionaryDoc = XDocument.Load(dictionaryFilePath);

                var wordElement = dictionaryDoc.Root.Elements("Word")
                    .FirstOrDefault(word => word.Element("Name").Value == selectedWord);

                if (wordElement != null)
                {
                    wordDetails.Add("Description", wordElement.Element("Description").Value);
                    wordDetails.Add("Category", wordElement.Element("Category").Value);
                    wordDetails.Add("ImagePath", wordElement.Element("ImagePath").Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea detaliilor cuvântului: " + ex.Message);
            }

            return wordDetails;
        }

        public void AuthenticateUser(string username, string password)
        {
            if (userAccounts.ContainsKey(username))
            {
                if (userAccounts[username].Item1 == password)
                {
                    string roles = userAccounts[username].Item2;

                    if (roles.Contains("Utilizator") || roles.Contains("Administrator"))
                    {
                        loggedInUsername = username;
                    }
                    else
                    {
                        MessageBox.Show("Accesul interzis! Acest cont nu are un rol valid!");
                    }
                }
                else
                {
                    MessageBox.Show("Parola introdusă este incorectă!");
                }
            }
            else
            {
                MessageBox.Show("Numele de utilizator introdus nu există!");
            }
        }
    }
}
