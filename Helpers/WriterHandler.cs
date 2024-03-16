
using Pastel;

namespace JuPi.Helpers
{
    public class WriterHandler {
        private const string DisplayPrefix = "PI: ";

        public static void UpdateScreen(GuessHelper guessHelper, bool reWrite) {
            // TODO: Make this just rewrite the last couple characters, with a check for console size changes.
            int cursor_x = GetCursorLeft(guessHelper);
            int cursor_y = GetCursorRight(guessHelper);

            if (reWrite) {
                Console.Clear();
                Console.Write(GetRegularOutput(guessHelper));
                Console.SetCursorPosition(cursor_x, cursor_y);
                return;
            }

            Console.SetCursorPosition(0, 0);
            int extraSpaces = Console.WindowWidth - cursor_x;
            Console.Write($"{DisplayPrefix}{guessHelper.CorrectGuess.Pastel("#BBFFBB")}{guessHelper.IncorrectGuess.Pastel("#FF3333")}{new string(' ', extraSpaces)}" +
                $"\nDigits: {guessHelper.DigitCount}/{guessHelper.TheoreticalMaxDigits}");
            Console.SetCursorPosition(cursor_x, cursor_y);
        }


        #region Yucky code
        private static string GetRegularOutput(GuessHelper guessHelper)
            => GetOutput(guessHelper.CorrectGuess, guessHelper.IncorrectGuess, guessHelper.DigitCount, guessHelper.TheoreticalMaxDigits);

        private static string GetOutput(string correct, string wrong, int digits, int maxDigits)
            => $"{DisplayPrefix}{correct.Pastel("#BBFFBB")}{wrong.Pastel("#FF3333")}\nDigits: {digits}/{maxDigits}";


        private static int GetCursorLeft(GuessHelper guessHelper) {
            int width = Console.WindowWidth;
            string toPutAfter = PureText(guessHelper);
            return toPutAfter.Length % width;
        }
        
        private static int GetCursorRight(GuessHelper guessHelper) {
            int width = Console.WindowWidth;
            string toPutAfter = PureText(guessHelper);
            return toPutAfter.Length / width;
        }

        private static string PureText(GuessHelper guessHelper) 
            => $"{DisplayPrefix}{guessHelper.CorrectGuess}{guessHelper.IncorrectGuess}";   // Keep relative to GetOutput()
        #endregion

    }
}