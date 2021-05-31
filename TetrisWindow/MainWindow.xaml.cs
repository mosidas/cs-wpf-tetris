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
            _timer.Elapsed += new ElapsedEventHandler(OnElapsed_TimersTimer);
            _timer.Interval = _gameManager.FrameRate;
        }

        void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
        {
            var doTimerAction = _time >= _gameManager.DownRate ? true : false;
            _gameManager.Update(_userAction, doTimerAction);

            if (_gameManager.IsGameOver)
            {
                _userAction = ActionTypes.nothing;
                Dispatcher.Invoke(() =>
                {
                    UpdateViewMod_GameOver();
                });

                _timer.Stop();
                return;
            }

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
                UpdateViewMod();
            });
        }

        private void UpdateViewMod()
        {
            for (var x = 0; x < _gameManager.FieldWidth;x++)
            {
                for (var y = 0; y < _gameManager.FieldHeight; y++)
                {
                    var block = (BlockRectangle)MainField.FindName("Cell_" + y + "_" + x);
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }

            var fieldPointAndTypePairs = _gameManager.FieldPointAndTypePairs;
            fieldPointAndTypePairs.ForEach(pair =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + pair.Item1.Y + "_" + pair.Item1.X);
                block.Rect.Fill = GetBlockColor(pair.Item2);
            });
        }

        private void UpdateViewMod_GameOver()
        {
            for (var x = 0; x < _gameManager.FieldWidth; x++)
            {
                for (var y = 0; y < _gameManager.FieldHeight; y++)
                {
                    var block = (BlockRectangle)MainField.FindName("Cell_" + y + "_" + x);
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }

            var fieldBlockPoints = _gameManager.FieldBlockPoints;
            //System.Threading.Tasks.Parallel.ForEach(fieldBlockPoints, p => 
            //{
            //    Delay();
            //    Dispatcher.Invoke(() =>
            //    {
            //        var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
            //        block.Rect.Fill = Brushes.Black;
            //    });
            //});
            fieldBlockPoints.ForEach(p =>
            {
                Delay();
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
            });
        }

        private async void Delay()
        {
            await System.Threading.Tasks.Task.Delay(new Random().Next(100, 1000));
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
