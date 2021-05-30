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

        public MainWindow()
        {
            InitializeComponent();
            Manager = new GameManager();
            TimersTimer = new Timer();
            TimersTimer.Elapsed += new ElapsedEventHandler(OnElapsed_TimersTimer);
            TimersTimer.Interval = Manager.FrameRate;
        }

        void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
        {
            var doTimerAction = _time >= Manager.DownRate ? true : false;
            Manager.Update(ActionType.nothing, doTimerAction);

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
                UpdateView();
            });
        }
        private void UpdateView()
        {
            UpdateCurrnetBlock(Manager.CurrentBlockPoints, Manager.CurrentBlockType);
            UpdateFixedBlock(Manager.FixedBlockPoints, Manager.FixedBlockTypes);
        }

        private void UpdateCurrnetBlock(List<System.Drawing.Point> blockPoints, BlockType blockType)
        {
            for (var i = 0; i < blockPoints.Count; i++)
            {
                var block = (BlockRectangle)MainField.FindName("currentBlock" + i);
                if(block == null)
                {
                    block = new BlockRectangle();
                    block.Name = "currentBlock" + i;
                    MainGrid.Children.Add(block);
                    MainGrid.RegisterName("currentBlock" + i, block);
                }

                block.SetValue(Grid.RowProperty, blockPoints[i].Y);
                block.SetValue(Grid.ColumnProperty, blockPoints[i].X);

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
        }

        private int _beforeFixedBlockCount = 0;

        private void UpdateFixedBlock(List<System.Drawing.Point> fixedPoints, List<BlockType> blockTypes)
        {

            for (var i = 0; i < _beforeFixedBlockCount; i++)
            {
                var block = (BlockRectangle)MainField.FindName("fixedBlock" + i);
                MainGrid.Children.Remove(block);
            }

            for (var i = 0;i < fixedPoints.Count;i++)
            {
                var block = new BlockRectangle
                {
                    Name = "fixedBlock" + i
                };
                block.SetValue(Grid.ColumnProperty, fixedPoints[i].X);
                block.SetValue(Grid.RowProperty, fixedPoints[i].Y);
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

                MainGrid.Children.Add(block);
                //MainGrid.RegisterName("fixedBlock" + i, block);
            }

            _beforeFixedBlockCount = fixedPoints.Count();
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
    }
}
