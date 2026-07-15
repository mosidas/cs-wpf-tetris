using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Tetris.Avalonia.Composition;
using Tetris.Avalonia.Views;

namespace Tetris.Avalonia
{
    // 基底は Avalonia の Application。素の `Application` は外側名前空間の Tetris.Application と
    // 衝突するため完全修飾する。
    public partial class App : global::Avalonia.Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var services = ServiceRegistration.Build();
                desktop.MainWindow = services.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
