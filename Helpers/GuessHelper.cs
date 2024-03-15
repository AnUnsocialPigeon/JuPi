using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuPi.Helpers {
    internal class GuessHelper {
        public GuessHelper(string Pi) {
            this.Pi = Pi;
            Guess = "";
        }

        public string Pi { get; set; }
        public string Guess {
            get => _Guess;
            set {
                LastGuess = Guess;
                _Guess = value; 
            }
        }
        private string _Guess { get; set; } = string.Empty;

        public string LastGuess { get; private set; } = string.Empty;

        public int ErrorIndex {
            get {
                for (int i = 0; i < Guess.Length; i++)
                    if (Guess[i] != Pi[i])
                        return i;
                return Guess.Length;
            }
        }

        public string CorrectGuess {
            get {
                string correct = "";
                for (int i = 0; i < ErrorIndex; i++)
                    correct += Guess[i];
                return correct;
            }
        }

        public string IncorrectGuess {
            get {
                string incorrect = "";
                for (int i = ErrorIndex; i < Guess.Length; i++)
                    incorrect += Guess[i];
                return incorrect;
            }
        }

        public void Pop() {
            Guess = Guess.Remove(Guess.Length - 1);
        }
    }
}
