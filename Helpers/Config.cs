using System.Net.NetworkInformation;

namespace JuPi.Helpers {
    public class Config {
        // Engines
        NetworkParser _NetworkParser;

        // Config
        private bool _ConfigFileChanged = false;
        public string ConfigPath = @"config.txt";
        private const string _piFileInit = "Pi_File";
        private const string _piDownloadInit = "Pi_Download";
        private const string _leaderboardInit = "Leaderboard";

        // User settings (parsed from config)
        public string? PiPath;
        public string? PiURL;
        public string? LeaderboardPath;
        public string? Username = "Ju";     // TODO: Add this
        public bool RetypeMode = true;      // TODO: Add this
        public int MinPiCount = 150;        // TODO: Add this
        public string CorrectColourHex = "#AAFFAA";     // TODO: Add this
        public string IncorrectColourHex = "#FF3333";   // TODO: Add this

        // Data
        public string? PiData;

        #region Config
        /// <summary>
        /// Base constructor for Config Class
        /// </summary>
        public Config() {
            _NetworkParser = new();
        }
        
        /// <summary>
        /// Constructor for Config Class
        /// </summary>
        /// <param name="configPath">The path of the Config file</param>
        public Config(string configPath) : this() {
            ConfigPath = configPath;
        }

        /// <summary>
        /// Parse Command line arguments 
        /// </summary>
        /// <param name="arg">The arg to be parsed</param>
        public void ParseArg(string arg) {
            // Correct parse: help window
            if (arg == "-h" || arg == "--help") {
                Console.WriteLine("Help window activated:");
                Console.WriteLine("\t-h or --help \tHelp!");
                Console.WriteLine("\tfile path    \tThe file path of the Config file associated with this program");
                Environment.Exit(0);
            }
            else if (_ConfigFileChanged) {
                Console.WriteLine("Multiple arguments detected. Exiting.");
                Environment.Exit(1);
            }
            else if (!File.Exists(arg)) {
                Console.WriteLine($"File {arg} does not exist. Exiting.");
                Environment.Exit(1);
            }

            // Deal with config file given            
            _ConfigFileChanged = true;
            ConfigPath = arg;
        }


        /// <summary>
        /// Initialise the current config. Parse it out etc.
        /// </summary>
        internal void LoadConfig() {
            // Create new config, if not exist
            if (!File.Exists(ConfigPath))
                File.WriteAllText(ConfigPath, $"{_piFileInit}: pi.txt\n{_piDownloadInit}: https://www.piday.org/million\n{_leaderboardInit}: leaderboard.csv");

            // Read the config file
            string[] configFile = File.ReadAllLines(ConfigPath);
            foreach (string line in configFile) {
                ParseConfigLine(line);
            }

            if ((PiData == null || PiData.Length < MinPiCount) && (PiURL == "" || PiURL == null)) {
                Console.WriteLine("There is no way of me getting PI! The inbuilt stuff does not suffice. \nPlease correct the config file.");
                Environment.Exit(1);
            }
            if (PiURL == null) {
                Console.WriteLine("URL is null. I need that URL!");
                Environment.Exit(1);
            }

            // Downloading PI data from website
            if (PiData == null || PiData.Length < MinPiCount) {
                PiData = _NetworkParser.GetPiData(PiURL);
                if (PiPath != null)
                    File.WriteAllTextAsync(PiPath, PiData);  // Put pi into 
            }

            // If still less than the minimum requirement for length of pi
            if (PiData.Length < MinPiCount) {
                Console.WriteLine($"I do not have enough PI to continue.\nRequired PI Length: {MinPiCount}\nPi: {PiData}");
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Parses a single line of the config file
        /// </summary>
        /// <param name="line">The line to parse</param>
        private void ParseConfigLine(string line) {
            // Validity
            string prefix = line.Split(':')[0];
            if (line.Split(':').Length < 2 || !new string[] { _piFileInit, _piDownloadInit, _leaderboardInit }.Contains(prefix)) {
                Console.WriteLine($"Config file at {ConfigPath} is corrupted.\nLine: {line}");
                Environment.Exit(1);
            }

            // Parse Pi_File
            if (prefix == _piFileInit)
                ParsePiFile(line);
            else if (prefix == _piDownloadInit)
                ParsePiDownload(line);
            else if (prefix == _leaderboardInit)
                ParseLeaderboardPath(line);
        }

        /// <summary>
        /// Parses the leaderboard path from the config file
        /// </summary>
        /// <param name="line">Line of the config file</param>
        private void ParseLeaderboardPath(string line) => LeaderboardPath = GetConfigLineData(line);


        /// <summary>
        /// Parses PI directory from the file string. 
        /// If the directory doesnt exist or is empty, PiData remains null or ""
        ///     If it does; PiData will become this data too.
        /// </summary>
        /// <param name="line">The config file's line to parse</param>
        private void ParsePiFile(string line) {
            string data = GetConfigLineData(line);
            if (data == null || data == "") {
                Console.WriteLine("Config is invalid.");
                Environment.Exit(1);
            }
            if (!File.Exists(data)) {
                File.WriteAllText(data, "");
            }
            PiPath = data;
            PiData = File.ReadAllText(data);
        }

        /// <summary>
        /// Parses PI download URL from the Config file
        /// </summary>
        /// <param name="line">Config file's line</param>
        private void ParsePiDownload(string line) => PiURL = GetConfigLineData(line);

        /// <summary>
        /// Gets a config line's data
        /// </summary>
        /// <param name="line">The config line</param>
        /// <returns>The data after the prefix. Prefix does not care.</returns>
        private string GetConfigLineData(string line) => line.Split(':').Skip(1).Aggregate((x, y) => x += $":{y}").Trim();

        #endregion

        /// <summary>
        /// Logs the current score to the leaderboard
        /// </summary>
        /// <param name="guess">Current run</param>
        internal void Log(string guess) {
            if (LeaderboardPath == null) {
                Console.WriteLine("Leaderboard does not exist");
                return;
            }

            // Add to leaderboard
            File.AppendAllText(LeaderboardPath, $"{DateTime.Now},{Username},{guess.Length},{guess}");
        }
    }
}
