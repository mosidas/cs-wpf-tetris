using System;
using System.Collections.Generic;
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
        private readonly GameManager _gameManager;
        private readonly Timer _timer;
        private ActionTypes _userAction;

        public MainWindow()
        {
            InitializeComponent();
            _gameManager = new GameManager(new Field(), new BlocksPoolManager());
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnElapsed_Timer);
            _timer.Interval = _gameManager.FrameRate;
        }

        private void OnElapsed_Timer(object sender, ElapsedEventArgs e)
        {
            _gameManager.Update(_userAction);

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
                    block.Rect.Opacity = 1.0;
                }
            }
        }

        private void UpdateView_Update()
        {
            UpdateView_Init();

            SetGhostBlock(_gameManager.GhostBlockPoints, _gameManager.CurrentBlocktype);
            SetFieldBlock(_gameManager.FieldPointAndTypePairs);
            SetHoldBlock(_gameManager.HoldBlockType);
            SetNextBlock(_gameManager.NextBlockTypes[0]);
            SetScore(_gameManager.Score);
        }

        private void SetScore(int score)
        {
            txtScore.Text = score.ToString();
        }

        private void SetFieldBlock(List<(System.Drawing.Point, BlockTypes)> fieldPointAndTypePairs)
        {
            fieldPointAndTypePairs.ForEach(pair =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + pair.Item1.Y + "_" + pair.Item1.X);
                block.Rect.Fill = GetBlockColor(pair.Item2);
                block.Rect.Opacity = 1;
            });
        }

        private void SetGhostBlock(List<System.Drawing.Point> ghostBlockPoints, BlockTypes ghostBlocktype)
        {
            ghostBlockPoints.ForEach(p =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = GetBlockColor(ghostBlocktype);
                block.Rect.Opacity = 0.5;
            });
        }

        private void SetNextBlock(BlockTypes bt)
        {
            NextBlock1.Rect.Fill = GetBlockColor(bt);
            NextBlock2.Rect.Fill = GetBlockColor(bt);
            NextBlock3.Rect.Fill = GetBlockColor(bt);
            NextBlock4.Rect.Fill = GetBlockColor(bt);

            if (bt == BlockTypes.I)
            {
                Grid.SetColumn(NbBox1, 1); Grid.SetRow(NbBox1, 0);
                Grid.SetColumn(NbBox2, 1); Grid.SetRow(NbBox2, 1);
                Grid.SetColumn(NbBox3, 1); Grid.SetRow(NbBox3, 2);
                Grid.SetColumn(NbBox4, 1); Grid.SetRow(NbBox4, 3);
            }
            else if (bt == BlockTypes.T)
            {
                Grid.SetColumn(NbBox1, 0); Grid.SetRow(NbBox1, 1);
                Grid.SetColumn(NbBox2, 1); Grid.SetRow(NbBox2, 1);
                Grid.SetColumn(NbBox3, 2); Grid.SetRow(NbBox3, 1);
                Grid.SetColumn(NbBox4, 1); Grid.SetRow(NbBox4, 2);
            }
            else if (bt == BlockTypes.J)
            {
                Grid.SetColumn(NbBox1, 1); Grid.SetRow(NbBox1, 0);
                Grid.SetColumn(NbBox2, 1); Grid.SetRow(NbBox2, 1);
                Grid.SetColumn(NbBox3, 1); Grid.SetRow(NbBox3, 2);
                Grid.SetColumn(NbBox4, 0); Grid.SetRow(NbBox4, 2);
            }
            else if (bt == BlockTypes.L)
            {
                Grid.SetColumn(NbBox1, 1); Grid.SetRow(NbBox1, 0);
                Grid.SetColumn(NbBox2, 1); Grid.SetRow(NbBox2, 1);
                Grid.SetColumn(NbBox3, 1); Grid.SetRow(NbBox3, 2);
                Grid.SetColumn(NbBox4, 2); Grid.SetRow(NbBox4, 2);
            }
            else if (bt == BlockTypes.S)
            {
                Grid.SetColumn(NbBox1, 1); Grid.SetRow(NbBox1, 1);
                Grid.SetColumn(NbBox2, 2); Grid.SetRow(NbBox2, 1);
                Grid.SetColumn(NbBox3, 0); Grid.SetRow(NbBox3, 2);
                Grid.SetColumn(NbBox4, 1); Grid.SetRow(NbBox4, 2);
            }
            else if (bt == BlockTypes.Z)
            {
                Grid.SetColumn(NbBox1, 0); Grid.SetRow(NbBox1, 1);
                Grid.SetColumn(NbBox2, 1); Grid.SetRow(NbBox2, 1);
                Grid.SetColumn(NbBox3, 1); Grid.SetRow(NbBox3, 2);
                Grid.SetColumn(NbBox4, 2); Grid.SetRow(NbBox4, 2);
            }
            else if (bt == BlockTypes.O)
            {
                Grid.SetColumn(NbBox1, 1); Grid.SetRow(NbBox1, 1);
                Grid.SetColumn(NbBox2, 1); Grid.SetRow(NbBox2, 2);
                Grid.SetColumn(NbBox3, 2); Grid.SetRow(NbBox3, 1);
                Grid.SetColumn(NbBox4, 2); Grid.SetRow(NbBox4, 2);
            }
        }

        private void SetHoldBlock(BlockTypes bt)
        {
            if(bt != BlockTypes.nothing)
            {
                HbBox1.Visibility = Visibility.Visible;
                HbBox2.Visibility = Visibility.Visible;
                HbBox3.Visibility = Visibility.Visible;
                HbBox4.Visibility = Visibility.Visible;
            }

            HoldBlock1.Rect.Fill = GetBlockColor(bt);
            HoldBlock2.Rect.Fill = GetBlockColor(bt);
            HoldBlock3.Rect.Fill = GetBlockColor(bt);
            HoldBlock4.Rect.Fill = GetBlockColor(bt);

            if (bt == BlockTypes.I)
            {
                Grid.SetColumn(HbBox1, 1); Grid.SetRow(HbBox1, 0);
                Grid.SetColumn(HbBox2, 1); Grid.SetRow(HbBox2, 1);
                Grid.SetColumn(HbBox3, 1); Grid.SetRow(HbBox3, 2);
                Grid.SetColumn(HbBox4, 1); Grid.SetRow(HbBox4, 3);
            }
            else if (bt == BlockTypes.T)
            {
                Grid.SetColumn(HbBox1, 0); Grid.SetRow(HbBox1, 1);
                Grid.SetColumn(HbBox2, 1); Grid.SetRow(HbBox2, 1);
                Grid.SetColumn(HbBox3, 2); Grid.SetRow(HbBox3, 1);
                Grid.SetColumn(HbBox4, 1); Grid.SetRow(HbBox4, 2);
            }
            else if (bt == BlockTypes.J)
            {
                Grid.SetColumn(HbBox1, 1); Grid.SetRow(HbBox1, 0);
                Grid.SetColumn(HbBox2, 1); Grid.SetRow(HbBox2, 1);
                Grid.SetColumn(HbBox3, 1); Grid.SetRow(HbBox3, 2);
                Grid.SetColumn(HbBox4, 0); Grid.SetRow(HbBox4, 2);
            }
            else if (bt == BlockTypes.L)
            {
                Grid.SetColumn(HbBox1, 1); Grid.SetRow(HbBox1, 0);
                Grid.SetColumn(HbBox2, 1); Grid.SetRow(HbBox2, 1);
                Grid.SetColumn(HbBox3, 1); Grid.SetRow(HbBox3, 2);
                Grid.SetColumn(HbBox4, 2); Grid.SetRow(HbBox4, 2);
            }
            else if (bt == BlockTypes.S)
            {
                Grid.SetColumn(HbBox1, 1); Grid.SetRow(HbBox1, 1);
                Grid.SetColumn(HbBox2, 2); Grid.SetRow(HbBox2, 1);
                Grid.SetColumn(HbBox3, 0); Grid.SetRow(HbBox3, 2);
                Grid.SetColumn(HbBox4, 1); Grid.SetRow(HbBox4, 2);
            }
            else if (bt == BlockTypes.Z)
            {
                Grid.SetColumn(HbBox1, 0); Grid.SetRow(HbBox1, 1);
                Grid.SetColumn(HbBox2, 1); Grid.SetRow(HbBox2, 1);
                Grid.SetColumn(HbBox3, 1); Grid.SetRow(HbBox3, 2);
                Grid.SetColumn(HbBox4, 2); Grid.SetRow(HbBox4, 2);
            }
            else if (bt == BlockTypes.O)
            {
                Grid.SetColumn(HbBox1, 1); Grid.SetRow(HbBox1, 1);
                Grid.SetColumn(HbBox2, 1); Grid.SetRow(HbBox2, 2);
                Grid.SetColumn(HbBox3, 2); Grid.SetRow(HbBox3, 1);
                Grid.SetColumn(HbBox4, 2); Grid.SetRow(HbBox4, 2);
            }
        }

        private async void UpdateView_GameOver()
        {
            UpdateView_Update();

            await System.Threading.Tasks.Task.Delay(1500);

            var fieldBlockPoints = _gameManager.FieldBlockPoints;
            fieldBlockPoints.AddRange(_gameManager.GhostBlockPoints);

            fieldBlockPoints.ForEach(p =>
            {
                System.Threading.Tasks.Task.Run(() => UpdateView_GameOver_FillBlack(p) );
            });

            StartMessage();
        }

        private async void UpdateView_GameOver_FillBlack(System.Drawing.Point p)
        {
            await System.Threading.Tasks.Task.Delay(new Random().Next(100, 1000));

            Dispatcher.Invoke(() =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
                block.Rect.Opacity = 1.0;
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

        private void MainGrid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(_gameManager.IsGameOver)
            {
                switch(e.Key)
                {
                    case System.Windows.Input.Key.Space:
                        Msg.Visibility = Visibility.Hidden;
                        Pause = false;
                        _gameManager.Start(1);
                        _timer.Start();
                        break;
                    case System.Windows.Input.Key.Escape:
                        Close();
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.Space:
                        Msg.Visibility = Visibility.Hidden;
                        Pause = false;
                        _timer.Start();
                        break;
                    case System.Windows.Input.Key.P:
                        _timer.Stop();
                        Pause = true;
                        PauseMessage();
                        return;
                    case System.Windows.Input.Key.R:
                        if(Pause)
                        {
                            Msg.Visibility = Visibility.Hidden;
                            _gameManager.Start(1);
                            _timer.Start();
                        }
                        return;
                    case System.Windows.Input.Key.Escape:
                        if (Pause)
                        {
                            Close();
                        }
                        else
                        {
                            _timer.Stop();
                            Pause = true;
                            PauseMessage();
                        }
                        return;
                }

                _userAction = e.Key switch
                {
                    System.Windows.Input.Key.Down => ActionTypes.moveDown,
                    System.Windows.Input.Key.Left => ActionTypes.moveLeft,
                    System.Windows.Input.Key.Right => ActionTypes.moveRight,
                    System.Windows.Input.Key.Up => ActionTypes.hardDrop,
                    System.Windows.Input.Key.Z => ActionTypes.rotateLeft,
                    System.Windows.Input.Key.X => ActionTypes.rotateRight,
                    System.Windows.Input.Key.Space => ActionTypes.hold,
                    _ => ActionTypes.nothing,
                };
            }
        }

        private void MainGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _userAction = ActionTypes.nothing;
        }

        private void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            StartMessage();
        }
        private bool Pause;

        private void StartMessage()
        {
            Msg.Visibility = Visibility.Visible;
            Msg.Background = Brushes.White;
            Msg.Content = "ゲーム開始：Space P:一時停止 終了：Esc";
        }

        private void PauseMessage()
        {
            Msg.Visibility = Visibility.Visible;
            Msg.Background = Brushes.White;
            Msg.Content = "ゲーム再開：Spase リセット：R 終了：Esc";
        }
    }
}
