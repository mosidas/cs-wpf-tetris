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
                Dispatcher.Invoke(() =>
                {
                    UpdateViewGameOver(Manager.CurrentBlockPoints, Manager.CurrentBlockType, Manager.FixedBlockPoints, Manager.FixedBlockTypes);
                });

                TimersTimer.Stop();
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
            for (var i = 0; i < _beforeFixedPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeFixedPoints[i].Y + "_" + _beforeFixedPoints[i].X);
                block.Rect.Fill = Brushes.Black;
            }

            for (var i = 0; i < fixedBlockPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeFixedPoints[i].Y + "_" + _beforeFixedPoints[i].X);
                block.Rect.Fill = Brushes.Black;
            }
        }

        private void UpdateCurrnetBlock_GameOver(List<System.Drawing.Point> currentBlockPoints, BlockTypes currentBlockType)
        {
            for (var i = 0; i < _beforeCurrnetBlockPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeCurrnetBlockPoints[i].Y + "_" + _beforeCurrnetBlockPoints[i].X);
                block.Rect.Fill = Brushes.Black;
            }

            for (var i = 0; i < currentBlockPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeCurrnetBlockPoints[i].Y + "_" + _beforeCurrnetBlockPoints[i].X);
                block.Rect.Fill = Brushes.Black;
            }
        }

        private void UpdateView(List<System.Drawing.Point> blockPoints, BlockTypes blockType, List<System.Drawing.Point> fixedPoints, List<BlockTypes> fixedBlockTypes)
        {
            UpdateCurrnetBlock(blockPoints, blockType);
            UpdateFixedBlock(fixedPoints, fixedBlockTypes);
        }

        List<System.Drawing.Point> _beforeCurrnetBlockPoints = new List<System.Drawing.Point>();

        private void UpdateCurrnetBlock(List<System.Drawing.Point> blockPoints, BlockTypes blockType)
        {
            for (var i = 0; i < _beforeCurrnetBlockPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeCurrnetBlockPoints[i].Y + "_" + _beforeCurrnetBlockPoints[i].X);
                block.Rect.Fill = Brushes.DarkGray;
            }

            for (var i = 0; i < blockPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + blockPoints[i].Y + "_" + blockPoints[i].X);
                if (block == null)
                {
                    continue;
                }

                if (blockType == BlockTypes.I)
                {
                    block.Rect.Fill = Brushes.LightBlue;
                }
                else if (blockType == BlockTypes.O)
                {
                    block.Rect.Fill = Brushes.Yellow;
                }
                else if (blockType == BlockTypes.S)
                {
                    block.Rect.Fill = Brushes.Green;
                }
                else if (blockType == BlockTypes.Z)
                {
                    block.Rect.Fill = Brushes.Red;
                }
                else if (blockType == BlockTypes.J)
                {
                    block.Rect.Fill = Brushes.Blue;
                }
                else if (blockType == BlockTypes.L)
                {
                    block.Rect.Fill = Brushes.Orange;
                }
                else if (blockType == BlockTypes.T)
                {
                    block.Rect.Fill = Brushes.Purple;
                }
                else
                {
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }

            _beforeCurrnetBlockPoints = blockPoints.ToList();
        }

        List<System.Drawing.Point> _beforeFixedPoints = new List<System.Drawing.Point>();

        private void UpdateFixedBlock(List<System.Drawing.Point> fixedPoints, List<BlockTypes> blockTypes)
        {
            for (var i = 0; i < _beforeFixedPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeFixedPoints[i].Y + "_" + _beforeFixedPoints[i].X);
                block.Rect.Fill = Brushes.DarkGray;
            }

            for (var i = 0; i < fixedPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + fixedPoints[i].Y + "_" + fixedPoints[i].X);

                if (blockTypes[i] == BlockTypes.I)
                {
                    block.Rect.Fill = Brushes.LightBlue;
                }
                else if (blockTypes[i] == BlockTypes.O)
                {
                    block.Rect.Fill = Brushes.Yellow;
                }
                else if (blockTypes[i] == BlockTypes.S)
                {
                    block.Rect.Fill = Brushes.Green;
                }
                else if (blockTypes[i] == BlockTypes.Z)
                {
                    block.Rect.Fill = Brushes.Red;
                }
                else if (blockTypes[i] == BlockTypes.J)
                {
                    block.Rect.Fill = Brushes.Blue;
                }
                else if (blockTypes[i] == BlockTypes.L)
                {
                    block.Rect.Fill = Brushes.Orange;
                }
                else if (blockTypes[i] == BlockTypes.T)
                {
                    block.Rect.Fill = Brushes.Purple;
                }
                else
                {
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }

            _beforeFixedPoints = fixedPoints.ToList();
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
