using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TetrisLogic;
using static TetrisLogic.SystemProperty;

namespace TetrisWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        private GameManager Manager { get; set; }
        private System.Timers.Timer TimersTimer;
        private double _time = 0.0;
        private ActionType _userAction;

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
        private void UpdateView(List<System.Drawing.Point> blockPoints, BlockType blockType, List<System.Drawing.Point> fixedPoints, List<BlockType> fixedBlockTypes)
        {
            UpdateCurrnetBlock(blockPoints, blockType);
            UpdateFixedBlock(fixedPoints, fixedBlockTypes);
        }

        List<System.Drawing.Point> _beforeCurrnetBlockPoints = new List<System.Drawing.Point>();

        private void UpdateCurrnetBlock(List<System.Drawing.Point> blockPoints, BlockType blockType)
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

                if (blockType == BlockType.I)
                {
                    block.Rect.Fill = Brushes.LightBlue;
                }
                else if (blockType == BlockType.O)
                {
                    block.Rect.Fill = Brushes.Yellow;
                }
                else if (blockType == BlockType.S)
                {
                    block.Rect.Fill = Brushes.Green;
                }
                else if (blockType == BlockType.Z)
                {
                    block.Rect.Fill = Brushes.Red;
                }
                else if (blockType == BlockType.J)
                {
                    block.Rect.Fill = Brushes.Blue;
                }
                else if (blockType == BlockType.L)
                {
                    block.Rect.Fill = Brushes.Orange;
                }
                else if (blockType == BlockType.T)
                {
                    block.Rect.Fill = Brushes.Purple;
                }
                else
                {
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }

            _beforeCurrnetBlockPoints = blockPoints.ToList();

            //for (var i = 0; i < blockPoints.Count; i++)
            //{
            //    var block = (BlockRectangle)MainField.FindName("currentBlock" + i);
            //    if(block == null)
            //    {
            //        block = new BlockRectangle();
            //        block.Name = "currentBlock" + i;
            //        MainGrid.Children.Add(block);
            //        MainGrid.RegisterName("currentBlock" + i, block);
            //    }

            //    block.SetValue(Grid.RowProperty, blockPoints[i].Y);
            //    block.SetValue(Grid.ColumnProperty, blockPoints[i].X);

            //    if (blockType == BlockType.I)
            //    {
            //        block.Rect.Fill = Brushes.LightBlue;
            //    }
            //    else if (blockType == BlockType.O)
            //    {
            //        block.Rect.Fill = Brushes.Yellow;
            //    }
            //    else if (blockType == BlockType.S)
            //    {
            //        block.Rect.Fill = Brushes.Green;
            //    }
            //    else if (blockType == BlockType.Z)
            //    {
            //        block.Rect.Fill = Brushes.Red;
            //    }
            //    else if (blockType == BlockType.J)
            //    {
            //        block.Rect.Fill = Brushes.Blue;
            //    }
            //    else if (blockType == BlockType.L)
            //    {
            //        block.Rect.Fill = Brushes.Orange;
            //    }
            //    else if (blockType == BlockType.T)
            //    {
            //        block.Rect.Fill = Brushes.Purple;
            //    }
            //    else
            //    {
            //        block.Rect.Fill = Brushes.DarkGray;
            //    }
            //}
        }

        List<System.Drawing.Point> _beforeFixedPoints = new List<System.Drawing.Point>();

        private void UpdateFixedBlock(List<System.Drawing.Point> fixedPoints, List<BlockType> blockTypes)
        {
            for (var i = 0; i < _beforeFixedPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + _beforeFixedPoints[i].Y + "_" + _beforeFixedPoints[i].X);
                block.Rect.Fill = Brushes.DarkGray;
            }

            for (var i = 0; i < fixedPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("Cell_" + fixedPoints[i].Y + "_" + fixedPoints[i].X);

                if (blockTypes[i] == BlockType.I)
                {
                    block.Rect.Fill = Brushes.LightBlue;
                }
                else if (blockTypes[i] == BlockType.O)
                {
                    block.Rect.Fill = Brushes.Yellow;
                }
                else if (blockTypes[i] == BlockType.S)
                {
                    block.Rect.Fill = Brushes.Green;
                }
                else if (blockTypes[i] == BlockType.Z)
                {
                    block.Rect.Fill = Brushes.Red;
                }
                else if (blockTypes[i] == BlockType.J)
                {
                    block.Rect.Fill = Brushes.Blue;
                }
                else if (blockTypes[i] == BlockType.L)
                {
                    block.Rect.Fill = Brushes.Orange;
                }
                else if (blockTypes[i] == BlockType.T)
                {
                    block.Rect.Fill = Brushes.Purple;
                }
                else
                {
                    block.Rect.Fill = Brushes.DarkGray;
                }
            }

            _beforeFixedPoints = fixedPoints.ToList();

            //for (var i = 0; i < _beforeFixedBlockCount; i++)
            //{
            //    //var block = (BlockRectangle)MainField.FindName("fixedBlock" + i);
            //    var block = (BlockRectangle)MainGrid.Children.OfType<FrameworkElement>().FirstOrDefault(x => x.Name == "fixedBlock" + i);
            //    MainGrid.Children.Remove(block);
            //}

            //for (var i = 0; i < fixedPoints.Count; i++)
            //{
            //    var block = new BlockRectangle
            //    {
            //        Name = "fixedBlock" + i
            //    };
            //    block.SetValue(Grid.ColumnProperty, fixedPoints[i].X);
            //    block.SetValue(Grid.RowProperty, fixedPoints[i].Y);

            //    if (blockTypes[i] == BlockType.I)
            //    {
            //        block.Rect.Fill = Brushes.LightBlue;
            //    }
            //    else if (blockTypes[i] == BlockType.O)
            //    {
            //        block.Rect.Fill = Brushes.Yellow;
            //    }
            //    else if (blockTypes[i] == BlockType.S)
            //    {
            //        block.Rect.Fill = Brushes.Green;
            //    }
            //    else if (blockTypes[i] == BlockType.Z)
            //    {
            //        block.Rect.Fill = Brushes.Red;
            //    }
            //    else if (blockTypes[i] == BlockType.J)
            //    {
            //        block.Rect.Fill = Brushes.Blue;
            //    }
            //    else if (blockTypes[i] == BlockType.L)
            //    {
            //        block.Rect.Fill = Brushes.Orange;
            //    }
            //    else if (blockTypes[i] == BlockType.T)
            //    {
            //        block.Rect.Fill = Brushes.Purple;
            //    }
            //    else
            //    {
            //        block.Rect.Fill = Brushes.DarkGray;
            //    }

            //    MainGrid.Children.Add(block);
            //    //MainGrid.RegisterName(block.Name + "Z", block);
            //}

            //_beforeFixedBlockCount = fixedPoints.Count();
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
                    _userAction = ActionType.moveDown;
                    break;
                case System.Windows.Input.Key.Left:
                    _userAction = ActionType.moveLeft;
                    break;
                case System.Windows.Input.Key.Right:
                    _userAction = ActionType.moveRight;
                    break;
                case System.Windows.Input.Key.Up:
                    _userAction = ActionType.hardDrop;
                    break;
                case System.Windows.Input.Key.Z:
                    _userAction = ActionType.rotateLeft;
                    break;
                case System.Windows.Input.Key.X:
                    _userAction = ActionType.rotateRight;
                    break;
                case System.Windows.Input.Key.Space:
                    _userAction = ActionType.hold;
                    break;
                default:
                    _userAction = ActionType.nothing;
                    break;
            }
        }

        private void MainGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _userAction = ActionType.nothing;
        }
    }
}
