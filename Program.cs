using Pastel;

namespace JuPi {
    internal class Program {
        private static string ConfigDir = @"./config.txt";

        // Program vars
        private const string DisplayPrefix = "PI: ";
        private static string PIGuess = "";
        private static string LastGuess = "";
        
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

            // MAIN SHIT
            while (DoMainLoop(config))
                continue;
        }

        private static bool DoMainLoop(Config config) {
            ConsoleKeyInfo keyInput = Console.ReadKey(true);
            // Exit
            if (keyInput.Key == ConsoleKey.Enter) 
                return false;

            if (keyInput.Key == ConsoleKey.OemPeriod && !PIGuess.Contains('.'))
                PIGuess += '.';
            else if (keyInput.Key == ConsoleKey.Backspace)
                PIGuess = PIGuess.Remove(PIGuess.Length - 1);
            else if (keyInput.KeyChar >= 48 && keyInput.KeyChar <= 57)
                PIGuess += keyInput.KeyChar.ToString();

            return DisplayPIGuess(PIGuess, config.PiData ?? "");
        }

        private static bool DisplayPIGuess(string guess, string data) {
            if (guess.Length >= data.Length) {
                Console.WriteLine("\nCongratulations! You won".Pastel("#55FF55"));
                return false;
            }
            if (PIGuess == LastGuess)
                return true;

            string correct = "";
            string incorrect = "";
            for (int i = 0; i < guess.Length; i++) {
                if (guess[i] == data[i] && incorrect == "") {
                    correct += guess[i];
                    continue;
                }
                incorrect += guess[i];
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"{DisplayPrefix}{correct}{incorrect.Pastel("#FF3333")} ");
            LastGuess = PIGuess;
            
            return true;
        }
    }
}
