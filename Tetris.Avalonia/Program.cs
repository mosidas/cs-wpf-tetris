using Avalonia;

namespace Tetris.Avalonia
{
    internal static class Program
    {
        // 初期化コード。SynchronizationContext やロギングが整う前に呼ばれる可能性があるため、
        // AppMain より前で Avalonia・サードパーティ API を使わないこと。
        [System.STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia の構成。デザイナも参照するためここに置く。
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
