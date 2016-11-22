using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace _2048
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            CreateNewGame();
        }

        public void CreateNewGame()
        {
            game = new Game();
            Score.Content = game.Score;
            UpdateGraphics(game.Board);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Directions dir;
            if (e.Key == Key.Up)
                dir = Directions.Up;
            else if (e.Key == Key.Down)
                dir = Directions.Down;
            else if (e.Key == Key.Left)
                dir = Directions.Left;
            else if (e.Key == Key.Right)
                dir = Directions.Right;
            else
                return;

            game.Update(dir);
            Score.Content = game.Score;

            UpdateGraphics(game.Board);

            if (game.GameOver)
            {
                MessageBox.Show("Game Over!!!");
            }
        }

        private void UpdateGraphics(Tile[,] values)
        {
            Playground.Children.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Border b = new Border();
                    b.Style = (Style)FindResource("TileBack");
                    b.Background = GetBGColorFromValue(values[i, j].value) as Brush;
                    b.Margin = new Thickness(j * 130 + 10, i * 130 + 10, 0, 0);

                    Label lb = new Label();
                    lb.Foreground = GetFGColorFromValue(values[i, j].value) as Brush;
                    lb.Style = (Style)FindResource("TileLabel");
                    lb.Content = values[i, j].value == 0? string.Empty : values[i, j].value.ToString();

                    b.Child = lb;

                    Playground.Children.Add(b);
                }
            }
        }

        private SolidColorBrush GetBGColorFromValue(uint val)
        {
            val = val > 4096 ? 8192 : val;
            return (SolidColorBrush)FindResource("TileBG" + val); 
        }

        private SolidColorBrush GetFGColorFromValue(uint val)
        {
            val = val <= 4 ? 2u : 8u;
            return (SolidColorBrush)FindResource("TileFG" + val);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            CreateNewGame();
        }
    }
}
