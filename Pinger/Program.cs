using Pinger.UI;

namespace Pinger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IUi ui = new ConsoleWorkProcessUi();
            ui.RunGui();
        }
    }
}
 