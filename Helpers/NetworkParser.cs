using HtmlAgilityPack;

namespace JuPi.Helpers {
    public class NetworkParser
    {

        // The HTTP client engine used for this app.
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Base constructor for the Network Parser class. Initiates the engine
        /// </summary>
        public NetworkParser()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Gets the HTTP contents from a given URL
        /// </summary>
        /// <param name="url">the URL the contents are from</param>
        /// <returns></returns>
        private async Task<string> GetHttpContents(string url)
        {
            Console.WriteLine($"Fetching data from {url}");
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching data: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get the PI data from the given URL
        /// TODO: Make this function parse any URL's, not just PiDay's URL.
        /// </summary>
        /// <param name="url">URL to parse</param>
        /// <returns>PI as a string, or "" if failure</returns>
        public string GetPiData(string url) {
            // Bleugh
            // TODO: Make this nice :)
            if (url.Contains("newton.ex.ac.uk"))
                return GetPiDataFromNewton(url) ?? "";
            
            if (url.Contains("cecm.sfu.ca")) 
                return GetPiDataFromCescm(url) ?? "";
            
            return GetPiDataFromPiDay(url) ?? "";
        }

        private string? GetPiDataFromNewton(string url) {
            string httpData = GetHttpContents(url).Result;
            return new string(httpData.Where(x => (x >= 48 && x <= 57) || x == 46).ToArray());
        }

        private string? GetPiDataFromCescm(string url) {
            string httpData = GetHttpContents(url).Result;
            // Doesnt work
            return GetDataAtFullXPath(httpData, @"//*")?.Replace(" ", "").Replace("\n", "");
        }

        /// <summary>
        /// Gets pi data for PiDay website
        /// </summary>
        /// <param name="url">URL where PI is found</param>
        /// <returns>Pi, or null if not found.</returns>
        private string? GetPiDataFromPiDay(string url)
        {
            string httpData = GetHttpContents(url).Result;
            return GetDataAtFullXPath(httpData, @"//div[@id='million_pi']");
        }

        /// <summary>
        /// Gets the text found at a given Full XPath
        /// </summary>
        /// <param name="htmlContent">The HTTP to parse</param>
        /// <param name="xPath">The XPath to parse</param>
        /// <returns>The text at that XPath - or null if failure.</returns>
        private string? GetDataAtFullXPath(string htmlContent, string xPath)
        {
            try
            {
                HtmlDocument htmlDoc = new();
                htmlDoc.LoadHtml(htmlContent);

                HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(xPath);
                //Console.WriteLine($"Got: {node.InnerText}");

                return node?.InnerText ?? null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error parsing XPath {xPath} for given URL.\nFull Error:{e.Message}");
                throw;
            }
        }
    }
}
