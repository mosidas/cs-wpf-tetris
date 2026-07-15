using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Tetris.Application;
using Tetris.Avalonia.Input;
using Tetris.Avalonia.ViewModels;

namespace Tetris.Avalonia.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel? _viewModel;

        // 離散キー(ハードドロップ・回転・ホールド)を 1 押下 1 適用にするため、
        // OS のキーリピートによる連続 KeyDown を押下中セットで抑止する。
        private readonly HashSet<Key> _heldKeys = new();

        // デザイナ用の引数なしコンストラクタ。
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            var vm = _viewModel;
            if (vm is null)
            {
                return;
            }

            var key = e.Key;

            if (vm.IsGameOver)
            {
                if (key == Key.Space)
                {
                    vm.Start();
                    e.Handled = true;
                }
                else if (key == Key.Escape)
                {
                    Close();
                    e.Handled = true;
                }

                return;
            }

            if (vm.IsPaused)
            {
                switch (key)
                {
                    case Key.Space:
                        vm.Resume();
                        e.Handled = true;
                        break;
                    case Key.R:
                        vm.Reset();
                        e.Handled = true;
                        break;
                    case Key.Escape:
                        Close();
                        e.Handled = true;
                        break;
                }

                return;
            }

            // プレイ中。
            if (key == Key.P || key == Key.Escape)
            {
                vm.Pause();
                e.Handled = true;
                return;
            }

            var command = KeyMapper.ToGameCommand(key);
            if (command == GameCommand.nothing)
            {
                return;
            }

            if (KeyMapper.IsDiscrete(key))
            {
                if (!_heldKeys.Add(key))
                {
                    e.Handled = true;
                    return; // 同一押下中の OS リピートは無視。
                }
            }

            vm.Apply(command);
            e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            _heldKeys.Remove(e.Key);
        }
    }
}
