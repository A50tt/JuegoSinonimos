using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Shapes = System.Windows.Shapes;
using IO = System.IO;

using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Media;
using System.Reflection;
using ScrapySharp.Html.Parsing;


namespace JuegoSinonimos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> listSynonyms = new List<string>();
        public static List<string> listSynonymsWoSpecial = new List<string>();
        public static string askedWord;
        public static string songName;
        public static string nowPlaying;
        public static bool songAlreadyStarted = false;
        public static bool definitionShown = false;

        public MediaPlayer backgroundMusic_mediaPlayer = new MediaPlayer();
        public MediaPlayer checkQuestionSound_mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            WordSelectorTextBox.Focus();
            RandomWord();
        }

        public void RandomWord()
        {
            listSynonyms.Clear();
            listSynonymsWoSpecial.Clear();
            DefinitionConsoleText.Text = "";

            string Url = "https://www.palabrasaleatorias.com";

            HtmlWeb oWeb = new HtmlWeb(); //connection to Load (on HtmlDocument).
            HtmlDocument doc = oWeb.Load(Url); //URL a la página web fuente.

            HtmlNode Node = doc.DocumentNode.SelectSingleNode("//td/div");   //getting 'first row of synonims'.
            string finalRandomWord = Node.InnerText.Trim();
            RandomWordLabel.Content = finalRandomWord;
            askedWord = finalRandomWord.ToLower();

            string Url2 = "https://www.wordreference.com/sinonimos/" + askedWord;

            HtmlWeb oWeb2 = new HtmlWeb(); //connection to Load (on HtmlDocument).
            HtmlDocument doc2 = oWeb2.Load(Url2); //URL a la página web fuente.

            HtmlNode Node2 = doc2.DocumentNode.SelectSingleNode("//*[@id='article']/div[@id='otherDicts']/div/h3");
            try
            {
                if (askedWord == Node2.InnerText)
                {
                    RetrieveSynonims(finalRandomWord);
                    ShowDefinition(finalRandomWord);
                    CopyListToNoSpecial();
                }
                else
                {
                    RandomWord();
                }
            }
            catch (System.Exception e)
            {
                RandomWord();
            }
        }

        public void RetrieveSynonims(string word)
        {
            string rawUrl = "https://www.wordreference.com/sinonimos/";
            string primalWord = word.ToLower();
            string finalUrl = rawUrl + primalWord;
            UrlConsoleOutput(finalUrl);

            HtmlWeb oWeb = new HtmlWeb(); //connection to Load (on HtmlDocument).
            HtmlDocument doc = oWeb.Load(finalUrl); //URL a la página web fuente.

            try
            {
                foreach (HtmlAgilityPack.HtmlNode Nodo in doc.DocumentNode.SelectNodes("//*[@id='article']//div[1]//ul//li")) //getting 'first row of synonims'. 
                {
                    string nodeString = Nodo.CssSelect("").First().InnerText;
                    bool subDone = false;
                    int lastComma = 0;
                    while (subDone == false)
                    {
                        if (((nodeString.IndexOf(",", lastComma, nodeString.Length - lastComma) - lastComma)) >= 0)
                        {
                            string retrievedWord = nodeString.Substring(lastComma, nodeString.IndexOf(",", lastComma, nodeString.Length - lastComma) - lastComma).ToLower();
                            listSynonyms.Add(retrievedWord);
                            lastComma = nodeString.IndexOf(",", lastComma) + 3;
                        }
                        else
                        {
                            string retrievedWord = nodeString.Substring(lastComma, nodeString.Length - lastComma).ToLower();
                            listSynonyms.Add(retrievedWord);
                            subDone = true;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                RandomWord();
            }
        }

        public void ShowDefinition(string word)
        {
            string rawUrl = "https://www.wordreference.com/definicion/";
            string primalWord = word.ToLower();
            string finalUrl = rawUrl + primalWord;

            HtmlWeb oWeb = new HtmlWeb(); //connection to Load (on HtmlDocument).
            HtmlDocument doc = oWeb.Load(finalUrl); //URL a la página web fuente.

            int defNumber = 1;
            foreach (HtmlNode nodo in doc.DocumentNode.SelectNodes("//*[@id='otherDicts']//li")) //getting 'definitions'. 
            {
                string nodeString = defNumber.ToString() + ". ";
                defNumber++;
                nodeString += nodo.CssSelect("").First().InnerText;
                //Symbol ":" that appears in every definition.
                if (nodeString.Contains(":"))
                {
                    nodeString = nodeString.Replace(":", ".");
                }
                //Capital letters "tilde"
                if (nodeString.Contains("&#xC1;"))
                {
                    nodeString = nodeString.Replace("&#xC1;", "Á");
                }
                if (nodeString.Contains("&#xC9;"))
                {
                    nodeString = nodeString.Replace("&#xC9;", "É");
                }
                if (nodeString.Contains("&#xCD;"))
                {
                    nodeString = nodeString.Replace("&#xCD;", "Í");
                }
                if (nodeString.Contains("&#xD3;"))
                {
                    nodeString = nodeString.Replace("&#xD3;", "Ó");
                }
                if (nodeString.Contains("&#xDA;"))
                {
                    nodeString = nodeString.Replace("&#xDA;", "Ú");
                }

                // Lowercase letters "tilde"
                if (nodeString.Contains("&#xE1;"))
                {
                    nodeString = nodeString.Replace("&#xE1;", "á");
                }
                if (nodeString.Contains("&#xE9;"))
                {
                    nodeString = nodeString.Replace("&#xE9;", "é");
                }
                if (nodeString.Contains("&#xED;"))
                {
                    nodeString = nodeString.Replace("&#xED;", "í");
                }
                if (nodeString.Contains("&#xF3;"))
                {
                    nodeString = nodeString.Replace("&#xF3;", "ó");
                }
                if (nodeString.Contains("&#xFA;"))
                {
                    nodeString = nodeString.Replace("&#xFA;", "ú");
                }

                //Diéresis
                if (nodeString.Contains("&#xCF;"))
                {
                    nodeString = nodeString.Replace("&#xCF;", "Ï");
                }
                if (nodeString.Contains("&#xEF;"))
                {
                    nodeString = nodeString.Replace("&#xEF;", "ï");
                }
                if (nodeString.Contains("&#xD6;"))
                {
                    nodeString = nodeString.Replace("&#xD6;", "Ö");
                }
                if (nodeString.Contains("&#xF6;"))
                {
                    nodeString = nodeString.Replace("&#xF6;", "ö");
                }
                if (nodeString.Contains("&#xDC;"))
                {
                    nodeString = nodeString.Replace("&#xDC;", "Ü");
                }
                if (nodeString.Contains("&#xFC;"))
                {
                    nodeString = nodeString.Replace("&#xFC;", "ü");
                }

                //Other special characters
                if (nodeString.Contains("&#xD1;"))
                {
                    nodeString = nodeString.Replace("&#xD1;", "Ñ");
                }
                if (nodeString.Contains("&#xF1;"))
                {
                    nodeString = nodeString.Replace("&#xF1;", "ñ");
                }
                if (nodeString.Contains("&#x2016;"))
                {
                    nodeString = nodeString.Replace("&#x2016;", "");
                }
                if (nodeString.Contains("&#xBF;"))
                {
                    nodeString = nodeString.Replace("&#xBF;", "");
                }
                if (nodeString.Contains("&#x2014;"))
                {
                    nodeString = nodeString.Replace("&#x2014;", "");
                }
                if (nodeString.Contains("&#x221A;"))
                {
                    nodeString = nodeString.Replace("&#x221A;", "");
                }
                DefinitionConsoleText.Text += nodeString + "\n";
            }
        }
        public void CheckAnswer(string answer)
        {
            SynonymConsoleOutput(listSynonyms);
            string simpleAnswer = answer.Replace('á', 'a').Replace('à', 'a').Replace('é', 'e').Replace('è', 'e').Replace('í', 'i').Replace('ì', 'i').Replace('ó', 'o').Replace('ò', 'o').Replace('ú', 'u').Replace('ù', 'u').Replace('ñ', 'n').Replace('ä', 'a').Replace('ë', 'e').Replace('ï', 'i').Replace('ö', 'o').Replace('ü', 'u').Replace("ñ", "n");

            //pedigüeñería
            if (listSynonyms.Contains(answer))
            {
                PlayCorrectAnswerSound();
                MessageBox.Show("¡Correcto!");
            }
            //pedigüeneria
            else if (listSynonymsWoSpecial.Contains(simpleAnswer))
            {
                PlayCorrectAnswerSound();
                MessageBox.Show("¡Correcto! Pero ten cuidado con los signos de puntuación.");
            }
            else if (askedWord == simpleAnswer || askedWord == answer)
            {
                PlayIncorrectAnswerSound();
                MessageBox.Show("¿Te encuentras bien?");
            }
            else
            {
                PlayIncorrectAnswerSound();
                MessageBox.Show("INCORRECTO...");
            }
            WordSelectorTextBox.Text = "";
            SynonymsConsoleText.Text = "";
            DefinitionConsoleText.Text = "";
            RandomWord();
        }

        public void CopyListToNoSpecial()
        {
            string noSpecialWord;

            //Crating list NO SPECIAL TILDE
            foreach (string word in listSynonyms)
            {
                noSpecialWord = word;
                if (noSpecialWord.Contains("á"))
                {
                    noSpecialWord = noSpecialWord.Replace("á", "a");
                }
                if (noSpecialWord.Contains("é"))
                {
                    noSpecialWord = noSpecialWord.Replace("é", "e");
                }
                if (noSpecialWord.Contains("í"))
                {
                    noSpecialWord = noSpecialWord.Replace("í", "i");
                }
                if (noSpecialWord.Contains("ó"))
                {
                    noSpecialWord = noSpecialWord.Replace("ó", "o");
                }
                if (noSpecialWord.Contains("ú"))
                {
                    noSpecialWord = noSpecialWord.Replace("ú", "u");
                }
                ///////////////
                if (noSpecialWord.Contains("ñ"))
                {
                    noSpecialWord = noSpecialWord.Replace("ñ", "n");
                }
                ///////////////
                if (noSpecialWord.Contains("ä"))
                {
                    noSpecialWord = noSpecialWord.Replace("ä", "a");
                }
                if (noSpecialWord.Contains("ë"))
                {
                    noSpecialWord = noSpecialWord.Replace("ë", "e");
                }
                if (noSpecialWord.Contains("ï"))
                {
                    noSpecialWord = noSpecialWord.Replace("ï", "i");
                }
                if (noSpecialWord.Contains("ö"))
                {
                    noSpecialWord = noSpecialWord.Replace("ö", "o");
                }
                if (noSpecialWord.Contains("ü"))
                {
                    noSpecialWord = noSpecialWord.Replace("ü", "u");
                }
                listSynonymsWoSpecial.Add(noSpecialWord);
            }
        }

        public void PlayCorrectAnswerSound()
        {
            string executableFilePath = Assembly.GetExecutingAssembly().Location; //Ruta absoluta del .exe [A]
            string solutionFilePath = executableFilePath.Substring(0, executableFilePath.IndexOf("bin", 0, executableFilePath.Length - 1)); //Utilizamos 'A' para conseguir el parent de la carpeta "bin" [B]
            string musicDirectoryPath = solutionFilePath + "/EfectosSonido"; //Le añadimos a 'B' la entrada a la carpeta EfectosSonido [C]
            string audioFilePath = IO.Path.Combine(musicDirectoryPath, "CorrectAnswer.wav"); //Ruta absoluta de la música a reproducir, añadiendo el nombre del archivo .wav a C

            checkQuestionSound_mediaPlayer.Open(new Uri(audioFilePath));
            checkQuestionSound_mediaPlayer.Play();
        }

        public void PlayIncorrectAnswerSound()
        {
            string executableFilePath = Assembly.GetExecutingAssembly().Location; //Ruta absoluta del .exe [A]
            string solutionFilePath = executableFilePath.Substring(0, executableFilePath.IndexOf("bin", 0, executableFilePath.Length - 1)); //Utilizamos 'A' para conseguir el parent de la carpeta "bin" [B]
            string musicDirectoryPath = solutionFilePath + "/EfectosSonido"; //Le añadimos a 'B' la entrada a la carpeta EfectosSonido [C]
            string audioFilePath = IO.Path.Combine(musicDirectoryPath, "Bruh.wav"); //Ruta absoluta de la música a reproducir, añadiendo el nombre del archivo .wav a C

            checkQuestionSound_mediaPlayer.Open(new Uri(audioFilePath));
            checkQuestionSound_mediaPlayer.Play();
        }

        public void UrlConsoleOutput(string Url)
        {
            UrlConsoleText.Text = Url;
        }

        public void SynonymConsoleOutput(List<string> Synonims)
        {
            string omegaWord = "";
            foreach (var synonym in Synonims)
            {
                if (omegaWord != "")
                {
                    omegaWord += ", ";
                }
                omegaWord += synonym;
            }
            omegaWord += ".";
            SynonymsConsoleText.Text = omegaWord;
        }

        private void CheckAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer(WordSelectorTextBox.Text.ToLower());
        }

        private void WordSelectorTextBox_Click_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CheckAnswer(WordSelectorTextBox.Text.ToLower());
            }
        }

        private void RefreshRandomButton_Click(object sender, RoutedEventArgs e)
        {
            RandomWord();
        }

        private void ShowSynonymsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SynonymConsoleOutput(listSynonyms);
        }

        private void ShowSynonymsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SynonymsConsoleText.Text = "";
        }
    }
}