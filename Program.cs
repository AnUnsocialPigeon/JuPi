using JuPi.Helpers;
using Pastel;

namespace JuPi
{
    internal class Program {
        private static string ConfigDir = @"./config.txt";

        internal static void Main(string[] args) {
            // Load args and config
            Config config = new(ConfigDir);
            foreach (string arg in args)
                config.ParseArg(arg);
            config.LoadConfig();

            // Get PI from the config
            string pi = config.PiData ?? "";
            if (pi == "") {
                Console.WriteLine($"Final check resulted in PI being nothing :(");
                Environment.Exit(1);
            }

            GuessHelper GuessHelper = new(pi);

            // MAIN SHIT
            DisplayPIGuess(GuessHelper, true);
            while (DoMainLoop(GuessHelper))
                continue;

            // Logging on completion
            config.Log(GuessHelper);
        }

        private static bool DoMainLoop(GuessHelper GuessHelper) {
            ConsoleKeyInfo keyInput = Console.ReadKey(true);
            // Exit
            if (keyInput.Key == ConsoleKey.Enter) 
                return false;

            if (keyInput.Key == ConsoleKey.OemPeriod && !GuessHelper.Guess.Contains('.'))
                GuessHelper.Guess += '.';
            else if (keyInput.Key == ConsoleKey.Backspace)
                GuessHelper.Pop();
            else if (keyInput.KeyChar >= 48 && keyInput.KeyChar <= 57)
                GuessHelper.Guess += keyInput.KeyChar.ToString();
            else if (keyInput.KeyChar == 'h')
                GuessHelper.Hint();

            return DisplayPIGuess(GuessHelper);
        }

        private static bool DisplayPIGuess(GuessHelper GuessHelper, bool ForceWrite = false) {
            if (!ForceWrite && GuessHelper.Guess.Length >= GuessHelper.Pi.Length) {
                Console.WriteLine("\nCongratulations! You won. There are no more digits to PI.".Pastel("#55FF55"));
                return false;
            }
            if (!ForceWrite && GuessHelper.Guess == GuessHelper.LastGuess)
                return true;

            // Screen writing
            WriterHandler.UpdateScreen(GuessHelper, ForceWrite);

            return true;
        }

        
    }
}
