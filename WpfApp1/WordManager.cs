using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WpfApp1
{
    public class Word
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
    }

    public class WordManager
    {
        private List<Word> words;

        public WordManager()
        {
            words = new List<Word>();
            LoadWordsFromXml();
        }

        private void LoadWordsFromXml()
        {
            
            XDocument doc = XDocument.Load(@"C:\Users\Nechita Bianca\Desktop\AN2\sem2\MAP\WpfApp1\WpfApp1\Dictionar.xml");
            
            words = doc.Root.Elements("Word").Select(wordElement => new Word
            {
                Name = wordElement.Element("Name").Value,
                Description = wordElement.Element("Description").Value,
                Category = wordElement.Element("Category").Value,
                ImagePath = wordElement.Element("ImagePath").Value
            }).ToList();
        }
        public List<Word> GetWords()
        {
            return words;
        }
        public void AddWord(Word word)
        {
            words.Add(word);
        }

    }
}
