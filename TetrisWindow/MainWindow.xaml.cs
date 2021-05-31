using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TetrisLogic;
using TetrisLogic.UserAction;

namespace TetrisWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        private GameManager _gameManager;
        private Timer _timer;
        private double _time;
        private ActionTypes _userAction;

        public MainWindow()
        {
            InitializeComponent();
            _gameManager = new GameManager(new Field(),new BlocksPoolManager());
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnElapsed_Timer);
            _timer.Interval = _gameManager.FrameRate;
        }

        void OnElapsed_Timer(object sender, ElapsedEventArgs e)
        {
            var doTimerAction = _time >= _gameManager.DownRate ? true : false;
            _gameManager.Update(_userAction, doTimerAction);

            if (doTimerAction)
            {
                _time = 0.0;
            }
            else
            {
                _time += _gameManager.FrameRate;
            }

            Dispatcher.Invoke(() =>
            {
                if (_gameManager.IsGameOver)
                {
                    _userAction = ActionTypes.nothing;
                    UpdateView_GameOver();
                    _timer.Stop();
                }
                else
                {
                    UpdateView_Update();
                }
                    
            });
        }

        private void UpdateView_Init()
        {
            for (var x = 0; x < _gameManager.FieldWidth; x++)
            {
                for (var y = 0; y < _gameManager.FieldHeight; y++)
                {
                    var block = (BlockRectangle)MainField.FindName("Cell_" + y + "_" + x);
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }
        }

        private void UpdateView_Update()
        {
            UpdateView_Init();

            var fieldPointAndTypePairs = _gameManager.FieldPointAndTypePairs;
            fieldPointAndTypePairs.ForEach(pair =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + pair.Item1.Y + "_" + pair.Item1.X);
                block.Rect.Fill = GetBlockColor(pair.Item2);
            });
        }

        private async void UpdateView_GameOver()
        {
            UpdateView_Update();

            await System.Threading.Tasks.Task.Delay(1500);

            var fieldBlockPoints = _gameManager.FieldBlockPoints;

            fieldBlockPoints.ForEach(p =>
            {
                System.Threading.Tasks.Task.Run(() => UpdateView_GameOver_FillBlack(p) );

            });
        }

        private async void UpdateView_GameOver_FillBlack(System.Drawing.Point p)
        {
            await System.Threading.Tasks.Task.Delay(new Random().Next(100, 500));

            Dispatcher.Invoke(() =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
            });
        }

        private Brush GetBlockColor(BlockTypes type)
        {
            if (type == BlockTypes.I)
            {
                return Brushes.LightBlue;
            }
            else if (type == BlockTypes.O)
            {
                return Brushes.Yellow;
            }
            else if (type == BlockTypes.S)
            {
                return Brushes.Green;
            }
            else if (type == BlockTypes.Z)
            {
                return Brushes.Red;
            }
            else if (type == BlockTypes.J)
            {
                return Brushes.Blue;
            }
            else if (type == BlockTypes.L)
            {
                return Brushes.Orange;
            }
            else if (type == BlockTypes.T)
            {
                return Brushes.Purple;
            }
            else
            {
                return Brushes.DarkGray;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            _time = 0.0;
            _gameManager.Start();
            _timer.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void MainGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case System.Windows.Input.Key.Down:
                    _userAction = ActionTypes.moveDown;
                    break;
                case System.Windows.Input.Key.Left:
                    _userAction = ActionTypes.moveLeft;
                    break;
                case System.Windows.Input.Key.Right:
                    _userAction = ActionTypes.moveRight;
                    break;
                case System.Windows.Input.Key.Up:
                    _userAction = ActionTypes.hardDrop;
                    break;
                case System.Windows.Input.Key.Z:
                    _userAction = ActionTypes.rotateLeft;
                    break;
                case System.Windows.Input.Key.X:
                    _userAction = ActionTypes.rotateRight;
                    break;
                case System.Windows.Input.Key.Space:
                    _userAction = ActionTypes.hold;
                    break;
                default:
                    _userAction = ActionTypes.nothing;
                    break;
            }
        }

        private void MainGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _userAction = ActionTypes.nothing;
        }
    }
}
