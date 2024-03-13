using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp1
{
    public partial class ModulAdministrativ : Window
    {
        private WordManager wordManager;
        private string name;
        private string description;
        private Methods methods;

        public ModulAdministrativ()
        {
            InitializeComponent();
            wordManager = new WordManager();
            LoadCategoriesFromXml();
            methods = new Methods(@"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml");
        }

        private void LoadCategoriesFromXml()
        {
            try
            {
                string dictionaryFilePath = @"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml";
                XDocument dictionaryDoc = XDocument.Load(dictionaryFilePath);

                var categories = dictionaryDoc.Root.Elements("Word").Select(wordElement => wordElement.Element("Category").Value).Distinct().ToList();
                if (!categories.Contains("Adăugare categorie"))
                {
                    categories.Insert(0, "Adăugare categorie");
                }

                foreach (var category in categories)
                {
                    cmbCategory.Items.Add(category);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul cu cuvinte și categorii nu a fost găsit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea categoriilor: " + ex.Message);
            }
        }

        private void UpdateXmlFile(Word newWord)
        {
            try
            {
                string filePath = @"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml";
                XDocument xmlDoc = XDocument.Load(filePath);
                xmlDoc.Root.Add(
                    new XElement("Word",
                        new XElement("Name", newWord.Name),
                        new XElement("Description", newWord.Description),
                        new XElement("Category", newWord.Category),
                        new XElement("ImagePath", newWord.ImagePath)
                    )
                );
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea fișierului XML: " + ex.Message);
            }
        }

        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            string name = txtWord.Text;
            string description = txtDescription.Text;
            string category = cmbCategory.Text;
            string imagePath = "C:/Users/Nechita Bianca/Desktop/AN2/sem2/MAP/WpfApp1/poze/noimage.png";

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Vă rugăm să completați toate câmpurile.");
                return;
            }
            if (category.Equals("Adăugare categorie"))
            {
                category = Microsoft.VisualBasic.Interaction.InputBox("Introduceți noua categorie:", "Adăugare categorie", "");
                
                if (string.IsNullOrWhiteSpace(category))
                {
                    cmbCategory.SelectedIndex = -1;
                    return;
                }
            }

            Word newWord = new Word
            {
                Name = name,
                Description = description,
                Category = category,
                ImagePath = imagePath
            };

            wordManager.AddWord(newWord);

            UpdateXmlFile(newWord);
            MessageBox.Show("Cuvântul a fost adăugat cu succes!");
        }
        private void AddCategoryToWord(string name, string description, string newCategory)
        {
            try
            {
                string filePath = @"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml";
                XDocument xmlDoc = XDocument.Load(filePath);


                XElement wordElement = xmlDoc.Root.Elements("Word")
                    .FirstOrDefault(word =>
                        word.Element("Name")?.Value == name &&
                        word.Element("Description")?.Value == description);

                if (wordElement != null)
                {

                    wordElement.Element("Category").Value = newCategory;


                    xmlDoc.Save(filePath);

                    MessageBox.Show($"Categoria '{newCategory}' a fost adăugată cu succes la cuvântul '{name}'.");
                }
                else
                {
                    MessageBox.Show($"Cuvântul cu numele '{name}' și descrierea '{description}' nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea fișierului XML: " + ex.Message);
            }
        }
        private void cmbCategory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbCategory.SelectedIndex == 0)
            {
                string newCategory = Microsoft.VisualBasic.Interaction.InputBox("Introduceți noua categorie:", "");

                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    cmbCategory.Items.Add(newCategory);
                    cmbCategory.SelectedItem = newCategory;
                }
                else
                {
                    cmbCategory.SelectedIndex = -1;
                }
            }
            else if (cmbCategory.SelectedIndex > 0)
            {
                string selectedCategory = cmbCategory.SelectedItem.ToString();

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description))
                {
                    AddCategoryToWord(name, description, selectedCategory);
                }
            }
        }
        private void ModifyWord_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtWord.Text))
            {
                MessageBox.Show("Vă rugăm să introduceți numele cuvântului pe care doriți să-l modificați.");
                return;
            }


            string newName = Microsoft.VisualBasic.Interaction.InputBox("Introduceți noul nume:", "Modificare nume", "");
            string newDescription = Microsoft.VisualBasic.Interaction.InputBox("Introduceți noua descriere:", "Modificare descriere", "");
            string newCategory = Microsoft.VisualBasic.Interaction.InputBox("Introduceți noua categorie:", "Modificare categorie", "");
            string newImagePath = Microsoft.VisualBasic.Interaction.InputBox("Introduceți noua cale către imagine:", "Modificare imagine", "");


            UpdateWord(txtWord.Text, newName, newDescription, newCategory, newImagePath);

            MessageBox.Show("Cuvântul a fost modificat cu succes!");
        }



        private void DeleteWord(string wordName)
        {
            try
            {
                string filePath = @"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml";
                XDocument xmlDoc = XDocument.Load(filePath);

              
                XElement wordElement = xmlDoc.Root.Elements("Word")
                    .FirstOrDefault(word =>
                        word.Element("Name")?.Value == wordName);

                if (wordElement != null)
                {
                    
                    wordElement.Remove();
                    xmlDoc.Save(filePath);

                    MessageBox.Show($"Cuvântul '{wordName}' a fost șters cu succes!");
                }
                else
                {
                    MessageBox.Show($"Cuvântul '{wordName}' nu a fost găsit în fișierul XML.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea fișierului XML: " + ex.Message);
            }
        }

        private void DeleteWord_Click(object sender, RoutedEventArgs e)
        {
            string wordName = txtWord.Text;

            if (string.IsNullOrWhiteSpace(wordName))
            {
                MessageBox.Show("Vă rugăm să introduceți numele cuvântului pe care doriți să-l ștergeți.");
                return;
            }
            DeleteWord(wordName);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ModulDeCautare modulDeCautare = new ModulDeCautare();
            modulDeCautare.Show();
            this.Close();
        }




        private void UpdateWord(string oldName, string newName, string newDescription, string newCategory, string newImagePath)
        {
            try
            {
                string filePath = @"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml";
                XDocument xmlDoc = XDocument.Load(filePath);

                
                XElement wordElement = xmlDoc.Root.Elements("Word")
                    .FirstOrDefault(word =>
                        word.Element("Name")?.Value == oldName);

                if (wordElement != null)
                {
                    wordElement.Element("Name").Value = newName;
                    wordElement.Element("Description").Value = newDescription;
                    wordElement.Element("Category").Value = newCategory;
                    wordElement.Element("ImagePath").Value = newImagePath;

                    
                    xmlDoc.Save(filePath);
                }
                else
                {
                    MessageBox.Show($"Cuvântul '{oldName}' nu a fost găsit în fișierul XML.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea fișierului XML: " + ex.Message);
            }
        }

    }
}