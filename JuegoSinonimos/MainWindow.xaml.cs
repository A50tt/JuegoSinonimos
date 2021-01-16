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
            ChooseSong();
            PlayMusic();
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

            HtmlNode Node2 = doc2.DocumentNode.SelectSingleNode("//*[@id='article']//div[1]/h3");
            try
            {
                if (askedWord == Node2.InnerText)
                {
                    RetrieveSynonims(finalRandomWord);
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
                    if (!Nodo.InnerHtml.Contains("span"))
                    {
                        string NodeString = Nodo.CssSelect("").First().InnerText;
                        bool subDone = false;
                        int lastComma = 0;
                        while (subDone == false)
                        {
                            if (((NodeString.IndexOf(",", lastComma, NodeString.Length - lastComma) - lastComma)) >= 0)
                            {
                                string retrievedWord = NodeString.Substring(lastComma, NodeString.IndexOf(",", lastComma, NodeString.Length - lastComma) - lastComma).ToLower();
                                listSynonyms.Add(retrievedWord);
                                lastComma = NodeString.IndexOf(",", lastComma) + 3;
                            }
                            else
                            {
                                string retrievedWord = NodeString.Substring(lastComma, NodeString.Length - lastComma).ToLower();
                                listSynonyms.Add(retrievedWord);
                                subDone = true;
                            }
                        }
                    }
                }
                ShowDefinition(word);
            }
            catch (System.Exception e)
            {
                RandomWord();
            }
        }

        public void ShowDefinition(string word)
        {
            string rawUrl = "https://dle.rae.es/";
            string primalWord = word.ToLower();
            string finalUrl = rawUrl + primalWord;

            HtmlWeb oWeb = new HtmlWeb(); //connection to Load (on HtmlDocument).
            HtmlDocument doc = oWeb.Load(finalUrl); //URL a la página web fuente.

            foreach (HtmlAgilityPack.HtmlNode Nodo in doc.DocumentNode.SelectNodes("//*[@class='j']")) //getting 'first row of synonims'. 
            {
                string NodeString = Nodo.CssSelect("").First().InnerText.Substring(0,Nodo.InnerText.Length-1) + ".";
                
                //Capital letters "tilde"
                if (NodeString.Contains("&#xC1;"))
                {
                    NodeString = NodeString.Replace("&#xC1;", "Á");
                }                       
                if (NodeString.Contains("&#xC9;"))
                {
                    NodeString = NodeString.Replace("&#xC9;", "É");
                }                
                if (NodeString.Contains("&#xCD;"))
                {
                    NodeString = NodeString.Replace("&#xCD;", "Í");
                }                
                if (NodeString.Contains("&#xD3;"))
                {
                    NodeString = NodeString.Replace("&#xD3;", "Ó");
                }                
                if (NodeString.Contains("&#xDA;"))
                {
                    NodeString = NodeString.Replace("&#xDA;", "Ú");
                }                
                
                // Lowercase letters "tilde"
                if (NodeString.Contains("&#xE1;"))
                {
                    NodeString = NodeString.Replace("&#xE1;", "á");
                }                       
                if (NodeString.Contains("&#xE9;"))
                {
                    NodeString = NodeString.Replace("&#xE9;", "é");
                }                
                if (NodeString.Contains("&#xED;"))
                {
                    NodeString = NodeString.Replace("&#xED;", "í");
                }                
                if (NodeString.Contains("&#xF3;"))
                {
                    NodeString = NodeString.Replace("&#xF3;", "ó");
                }                
                if (NodeString.Contains("&#xFA;"))
                {
                    NodeString = NodeString.Replace("&#xFA;", "ú");
                }

                //Diéresis
                if (NodeString.Contains("&#xCF;"))
                {
                    NodeString = NodeString.Replace("&#xCF;", "Ï");
                }                
                if (NodeString.Contains("&#xEF;"))
                {
                    NodeString = NodeString.Replace("&#xEF;", "ï");
                }     
                if (NodeString.Contains("&#xD6;"))
                {
                    NodeString = NodeString.Replace("&#xD6;", "Ö");
                }                
                if (NodeString.Contains("&#xF6;"))
                {
                    NodeString = NodeString.Replace("&#xF6;", "ö");
                }                
                if (NodeString.Contains("&#xDC;"))
                {
                    NodeString = NodeString.Replace("&#xDC;", "Ü");
                }                
                if (NodeString.Contains("&#xFC;"))
                {
                    NodeString = NodeString.Replace("&#xFC;", "ü");
                }

                //Other special characters
                if (NodeString.Contains("&#xD1;"))
                {
                    NodeString = NodeString.Replace("&#xD1;", "Ñ");
                }                
                if (NodeString.Contains("&#xF1;"))
                {
                    NodeString = NodeString.Replace("&#xF1;", "ñ");
                }                
                if (NodeString.Contains("&#x2016;"))
                {
                    NodeString = NodeString.Replace("&#x2016;", "");
                }                
                if (NodeString.Contains("&#xBF;"))
                {
                    NodeString = NodeString.Replace("&#xBF;", "");
                }                
                if (NodeString.Contains("&#x2014;"))
                {
                    NodeString = NodeString.Replace("&#x2014;", "");
                }                
                if (NodeString.Contains("&#x221A;"))
                {
                    NodeString = NodeString.Replace("&#x221A;", "");
                }
                DefinitionConsoleText.Text += NodeString + "\n";
            }            
        }
        public void CheckAnswer(string answer)        
        {
            SynonymConsoleOutput(listSynonyms);
            string simpleAnswer = answer.Replace('á', 'a').Replace('à', 'a').Replace('é', 'e').Replace('è', 'e').Replace('í', 'i').Replace('ì', 'i').Replace('ó', 'o').Replace('ò', 'o').Replace('ú', 'u').Replace('ù', 'u').Replace('ñ', 'n').Replace('ä', 'a').Replace('ë', 'e').Replace('ï', 'i').Replace('ö', 'o').Replace('ü', 'u').Replace("ñ","n");

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

        public void CopyListToNoSpecial ()
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

        public void ChooseSong()
        {
            int songChoosing;
            if (songAlreadyStarted == false)
            {
                Random random = new Random();
                songChoosing = random.Next(1, 7);
                switch (songChoosing)
                {
                    case 1:
                        songName = "ApocalipsisAquarius.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.08;
                        break;
                    case 2:
                        songName = "CareeningIntoDanger.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.2;
                        break;
                    case 3:
                        songName = "Cleigne.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.1;
                        break;
                    case 4:
                        songName = "StandYourGround.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.08;
                        break;
                    case 5:
                        songName = "VeiledInBlack.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.04;
                        break;
                    case 6:
                        songName = "Hellfire.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.17;
                        break;
                    default:
                        songName = "Cleigne.wav";
                        backgroundMusic_mediaPlayer.Volume = 0.1;
                        break;            
                }
                songAlreadyStarted = true;
            }
            else
            {
                int numSong = Convert.ToInt32(Convert.ToString(nowPlaying).Substring(0, 1));
                if (numSong == 1)
                {
                    songName = "CareeningIntoDanger.wav";
                    backgroundMusic_mediaPlayer.Volume = 0.2;
                    songChoosing = 2;
                }
                else if (numSong == 2)
                {
                    songName = "Cleigne.wav";
                    backgroundMusic_mediaPlayer.Volume = 0.1;
                    songChoosing = 3;
                }
                else if (numSong == 3)
                {
                    songName = "StandYourGround.wav";
                    backgroundMusic_mediaPlayer.Volume = 0.08;
                    songChoosing = 4;
                }
                else if (numSong == 4)
                {
                    songName = "VeiledInBlack.wav";
                    backgroundMusic_mediaPlayer.Volume = 0.04;
                    songChoosing = 5;
                }
                else if (numSong == 5)
                {
                    songName = "Hellfire.wav";
                    backgroundMusic_mediaPlayer.Volume = 0.17;
                    songChoosing = 6;
                }
                else if (numSong == 6)
                {
                    songName = "ApocalipsisAquarius.wav";
                    backgroundMusic_mediaPlayer.Volume = 0.08;
                    songChoosing = 1;
                }
                else
                {
                    songChoosing = 1;
                }
            }
            nowPlaying = Convert.ToString(songChoosing) + ". " + songName.Substring(0, songName.Length - 4);
            SongNameLabel.Content = nowPlaying;
        }

        public void PlayMusic()
        {
            string executableFilePath = Assembly.GetExecutingAssembly().Location; //Ruta absoluta del .exe [A]
            string solutionFilePath = executableFilePath.Substring(0,executableFilePath.IndexOf("bin",0,executableFilePath.Length - 1)); //Ruta arreglada del padre de "bin"
            string musicDirectoryPath = solutionFilePath + "/Music"; //Ruta absoluta de la carpeta del .exe [B]
            string audioFilePath = IO.Path.Combine(musicDirectoryPath, songName); //Ruta absoluta de la música a reproducir (combinando B y songName)

            backgroundMusic_mediaPlayer.Open(new Uri(audioFilePath));      //(new Uri(@"..\..\Music\" + songName + ""))
            backgroundMusic_mediaPlayer.MediaEnded += delegate { ChooseSong(); PlayMusic(); };
            backgroundMusic_mediaPlayer.Play();
        }

        public void StopMusic()
        {
            backgroundMusic_mediaPlayer.Stop();
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

        private void MusicButton_Click(object sender, RoutedEventArgs e)
        {
            
            ChooseSong();
            StopMusic();
            PlayMusic();
        }

        private void StopMusicButton_Click_1(object sender, RoutedEventArgs e)
        {
            StopMusic();
            SongNameLabel.Content = "";
        }

        private void ShowSynonymsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SynonymConsoleOutput(listSynonyms);
        }

        private void ShowSynonymsButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SynonymsConsoleText.Text = "";
        }

        private void DefinitionButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (definitionShown == false)
            {
                ShowDefinition(askedWord);
                definitionShown = true;
            }     
            
            else
            {
                DefinitionConsoleText.Text = "";
                definitionShown = false;
            }
            */
        }
    }
}