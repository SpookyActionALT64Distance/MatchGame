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
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        readonly DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text += " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()   //Creates a list of 8 pair of animalEmojis
            {
               "🐙","🐙",
               "🐡","🐡",   //An exception is C#'s way of telling you that something went wrong when your code was running. Every exception
               "🐘","🐘",   //has a type: this one is an ArgumentOutOfRangeException. Exceptions also have useful messages to help you figure
               "🐳","🐳",   //out what went wrong. This exceptions's message says, "Index was out of range." That's useful information to help
               "🐪","🐪",   //us figure out what went wrong.
               "🦕","🦕",
               "🦘","🦘",
               "🦔","🦔",
            };
            Random random = new Random();                   //Creates a new random number generator

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())  //Find every TextBlock in the main grid and
            {                                                                       //repeat the following statements for each of them
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count); //Pick a random # between 0 and the # of emoji left in the list and call it "index"
                    string nextEmoji = animalEmoji[index];      //Use the random number called “index” to get a random emoji from the list
                    textBlock.Text = nextEmoji;                 //Update the TextBlock with the random emoji from the list
                    animalEmoji.RemoveAt(index);                //Remove the random emoji from the list
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;     // These are "fields". They’re variables that live inside the Class but
        bool findingMatch = false;          // outside the Methods, so all of the Methods in the window can access them

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {                                                   // findingMatch=Keeps track of wether or not the player just clicker                                                            
            TextBlock textBlock = sender as TextBlock;      // on the 1st animal in a pair and is now trying to find it's match
            if (findingMatch == false)       // the player just clicked the 1st animal in a pair, so it makes that animal invisible
            {                                // and keeps track of it's TextBlock in case it needs to make it visible again
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)   // The player found a match! Makes the 2nd animal in the pair invisible
            {                                                       // (& unclickable), & resets findingMatch so the next animal clicked is
                matchesFound++;                                     // the 1st in a pair again
                textBlock.Visibility = Visibility.Hidden;           
                findingMatch = false;
            }
            else                                                    // The player clicked on an animal that doesn't match, so it makes the 1st
            {                                                       // animal that was clicked visible again & resets findingMatch
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)      // This resets the game if all 8 matched
            {                           // pairs have been found (otherwise it does
                SetUpGame();            // nothing because the game is still running)
            }
        }
    }
}
