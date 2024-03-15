using JuPi.Helpers;
using Pastel;

namespace JuPi
{
    internal class Program {
        private static string ConfigDir = @"./config.txt";

        // Program vars
        private const string DisplayPrefix = "PI: ";
        
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
            Console.Write(DisplayPrefix);
            while (DoMainLoop(GuessHelper))
                continue;

            // Logging on completion
            config.Log(GuessHelper.CorrectGuess);
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

            return DisplayPIGuess(GuessHelper);
        }

        private static bool DisplayPIGuess(GuessHelper GuessHelper) {
            if (GuessHelper.Guess.Length >= GuessHelper.Pi.Length) {
                Console.WriteLine("\nCongratulations! You won. There are no more digits to PI.".Pastel("#55FF55"));
                return false;
            }
            if (GuessHelper.Guess == GuessHelper.LastGuess)
                return true;

            Console.SetCursorPosition(0, 0);
            Console.Write($"{DisplayPrefix}{GuessHelper.CorrectGuess.Pastel("#BBFFBB")}{GuessHelper.IncorrectGuess.Pastel("#FF3333")}  ");
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            return true;
        }
    }
}
