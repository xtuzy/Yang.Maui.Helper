using System.Threading.Tasks;
using WindowsClipboard = System.Windows.Clipboard;

namespace Xamarin.Essentials
{
    /// <summary>
    /// copy from 
    /// https://github.com/xamarin/Essentials/tree/aab19bb928332e737c6957e8df45c1c7f2e722cb/Xamarin.Essentials
    /// </summary>
    public static partial class Clipboard
    {
        static Task PlatformSetTextAsync(string text)
        {
            WindowsClipboard.SetText(text);
            return Task.CompletedTask;
        }

        static bool PlatformHasText
            => WindowsClipboard.ContainsText();

        static Task<string> PlatformGetTextAsync() => Task.FromResult(WindowsClipboard.GetText());

        static void StartClipboardListeners()
        {
        }

        static void StopClipboardListeners()
        {
        }
    }
}
