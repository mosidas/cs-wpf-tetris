using Microsoft.Extensions.DependencyInjection;
using Tetris.Application;
using Tetris.Avalonia.ViewModels;
using Tetris.Avalonia.Views;
using Tetris.Domain;
using Tetris.Infrastructure;

namespace Tetris.Avalonia.Composition
{
    /// <summary>
    /// 合成ルート。依存(ゲーム進行・プールマネージャ・ViewModel・Window)を組み立てる単一箇所。
    /// Window と ViewModel は単一の <see cref="GameSession"/> を共有する。
    /// </summary>
    public static class ServiceRegistration
    {
        public static ServiceProvider Build()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IBlocksPoolManager, BlocksPoolManager>();
            services.AddSingleton<Field>();
            services.AddSingleton<GameSession>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton(sp => new MainWindow(sp.GetRequiredService<MainViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
