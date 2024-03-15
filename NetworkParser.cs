﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JuPi {
    public class NetworkParser {

        // The HTTP client engine used for this app.
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Base constructor for the Network Parser class. Initiates the engine
        /// </summary>
        public NetworkParser() {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Gets the HTTP contents from a given URL
        /// </summary>
        /// <param name="url">the URL the contents are from</param>
        /// <returns></returns>
        private async Task<string> GetHttpContents(string url) {
            Console.WriteLine($"Fetching data from {url}");
            try {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e) {
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
        public string GetPiData(string url) => GetPiDataFromPiDayAsync(url) ?? "";

        /// <summary>
        /// Gets pi data for PiDay website
        /// </summary>
        /// <param name="url">URL where PI is found</param>
        /// <returns>Pi, or null if not found.</returns>
        private string? GetPiDataFromPiDayAsync(string url) {
            string httpData = GetHttpContents(url).Result;
            return GetDataAtFullXPath(httpData, @"//div[@id='million_pi']"); 
        }

        /// <summary>
        /// Gets the text found at a given Full XPath
        /// </summary>
        /// <param name="htmlContent">The HTTP to parse</param>
        /// <param name="xPath">The XPath to parse</param>
        /// <returns>The text at that XPath - or null if failure.</returns>
        private string? GetDataAtFullXPath(string htmlContent, string xPath) {
            try {
                HtmlDocument htmlDoc = new();
                htmlDoc.LoadHtml(htmlContent);

                HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode(xPath);
                //Console.WriteLine($"Got: {node.InnerText}");
                
                return node?.InnerText ?? null;
            } catch (Exception e) {
                Console.WriteLine($"Error parsing XPath {xPath} for given URL.\nFull Error:{e.Message}");
                throw;
            }
        }
    }
}
