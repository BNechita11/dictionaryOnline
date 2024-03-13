using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class ModulDivertisment : Window
    {
        private int currentIndex = 0;
        private WordManager wordManager;
        private int correctAnswers = 0;
        private List<Word> words;
        private int rounds = 0;
        private Word previousWord;
        private List<int> usedWords = new List<int>();
        private string currentWord;
        private bool isImageRound = false;

        public ModulDivertisment()
        {
            InitializeComponent();
            wordManager = new WordManager();
            words = wordManager.GetWords();
            ShowRandomWord();
        }

        private void ShowRandomWord()
        {
            txtAnswer.Text = string.Empty;

            int index = -1;
            Random random = new Random();
            do
            {
                index = random.Next(0, words.Count);
            } while (usedWords.Contains(index));

            usedWords.Add(index);

            string imagePath = words[index].ImagePath;
            if (!string.IsNullOrEmpty(imagePath) && !imagePath.Equals("C:/Users/Nechita Bianca/Desktop/AN2/sem2/MAP/WpfApp1/WpfApp1/Poze/no_image.jpg", StringComparison.OrdinalIgnoreCase))
            {
               
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    imgWord.Source = bitmap;
                    imgWord.Visibility = Visibility.Visible;
                    wordDefinition.Text = string.Empty; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare la incarcarea imaginii: " + ex.Message);
                    
                    wordDefinition.Text = words[index].Description;
                    wordDefinition.Visibility = Visibility.Visible;
                    imgWord.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                
                wordDefinition.Text = words[index].Description;
                wordDefinition.Visibility = Visibility.Visible;
                imgWord.Visibility = Visibility.Hidden;
            }

            currentWord = words[index].Name.ToLower();
        }



        private void CheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            string userAnswer = txtAnswer.Text.Trim();
            string correctAnswer = currentWord;

            if (userAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Raspuns corect!");
                correctAnswers++;
            }
            else
            {
                MessageBox.Show($"Raspuns incorect! Raspunsul corect era : {correctAnswer}");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;

            if (rounds >= 4)
            {
                btnNext.Content = "Finish";

                if (btnNext.Content.ToString() == "Finish")
                {
                    MessageBox.Show($"Joc incheiat! Ai ghicit {correctAnswers} cuvinte! Jocul se va reseta.");
                    ResetGame();
                    return;
                }
            }

            rounds++;
            txtAnswer.Text = null;
            ShowRandomWord();
        }

        private void ResetGame()
        {
            currentIndex = 0;
            correctAnswers = 0;
            rounds = 0;
            txtAnswer.Text = null;
            btnNext.Content = "Next";
            ShowRandomWord();
        }

        private void BackButton1_Click(object sender, RoutedEventArgs e)
        {
            ModulDeCautare modulDeCautare = new ModulDeCautare();
            modulDeCautare.Show();
            this.Close();
        }
    }
}
