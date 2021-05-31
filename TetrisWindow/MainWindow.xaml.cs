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
        private GameManager Manager { get; set; }
        private Timer TimersTimer;
        private double _time = 0.0;
        private ActionTypes _userAction;

        public MainWindow()
        {
            InitializeComponent();
            Manager = new GameManager(new Field(),new BlocksPoolManager());
            TimersTimer = new Timer();
            TimersTimer.Elapsed += new ElapsedEventHandler(OnElapsed_TimersTimer);
            TimersTimer.Interval = Manager.FrameRate;
        }

        void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
        {
            var doTimerAction = _time >= Manager.DownRate ? true : false;
            Manager.Update(_userAction, doTimerAction);

            if (Manager.IsGameOver)
            {
                _userAction = ActionTypes.nothing;

                Dispatcher.Invoke(() =>
                {
                    UpdateViewGameOver(Manager.CurrentBlockPoints, Manager.CurrentBlockType, Manager.FixedBlockPoints, Manager.FixedBlockTypes);
                });

                TimersTimer.Stop();
                return;
            }

            if (doTimerAction)
            {
                _time = 0.0;
            }
            else
            {
                _time += Manager.FrameRate;
            }

            Dispatcher.Invoke(() =>
            {
                UpdateView(Manager.CurrentBlockPoints, Manager.CurrentBlockType, Manager.FixedBlockPoints, Manager.FixedBlockTypes);
            });
        }

        private void UpdateViewGameOver(List<System.Drawing.Point> currentBlockPoints, BlockTypes currentBlockType, List<System.Drawing.Point> fixedBlockPoints, List<BlockTypes> fixedBlockTypes)
        {
            UpdateCurrnetBlock_GameOver(currentBlockPoints, currentBlockType);
            UpdateFixedBlock_GameOver(fixedBlockPoints, fixedBlockTypes);
        }

        private void UpdateFixedBlock_GameOver(List<System.Drawing.Point> fixedBlockPoints, List<BlockTypes> fixedBlockTypes)
        {
            _beforeFixedPoints.ForEach(p =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
            });

            fixedBlockPoints.ForEach(p =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
            });
        }

        private void UpdateCurrnetBlock_GameOver(List<System.Drawing.Point> currentBlockPoints, BlockTypes currentBlockType)
        {
            _beforeCurrnetBlockPoints.ForEach(p => 
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
            });

            currentBlockPoints.ForEach(p =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.Black;
            });
        }

        private void UpdateView(List<System.Drawing.Point> blockPoints, BlockTypes blockType, List<System.Drawing.Point> fixedPoints, List<BlockTypes> fixedBlockTypes)
        {
            UpdateCurrnetBlock(blockPoints, blockType);
            UpdateFixedBlock(fixedPoints, fixedBlockTypes);
        }

        List<System.Drawing.Point> _beforeCurrnetBlockPoints = new List<System.Drawing.Point>();

        private void UpdateCurrnetBlock(List<System.Drawing.Point> blockPoints, BlockTypes blockType)
        {
            _beforeCurrnetBlockPoints.ForEach(p =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.DarkGray;
            });

            blockPoints.ForEach(p => 
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = GetBlockColor(blockType);
            });

            _beforeCurrnetBlockPoints = blockPoints.ToList();
        }

        List<System.Drawing.Point> _beforeFixedPoints = new List<System.Drawing.Point>();

        private void UpdateFixedBlock(List<System.Drawing.Point> fixedPoints, List<BlockTypes> blockTypes)
        {
            _beforeFixedPoints.ForEach(p =>
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + p.Y + "_" + p.X);
                block.Rect.Fill = Brushes.DarkGray;
            });

            var list = fixedPoints.Zip(blockTypes, (p, t) => new { point = p, type = t }).ToList();

            list.ForEach(pair => 
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + pair.point.Y + "_" + pair.point.X);
                block.Rect.Fill = GetBlockColor(pair.type);
            });

            _beforeFixedPoints = fixedPoints.ToList();
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
            Manager.Start();
            TimersTimer.Start();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            TimersTimer.Stop();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            TimersTimer.Start();
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
