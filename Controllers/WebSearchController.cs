using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSearchRefactorExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSearchController : ControllerBase
    {
        [HttpGet]
        public Response Get([FromQuery] string searchTerms = "")
        {
            var response = new Response
            {
                SearchTerms = searchTerms
            };

            var googleSearchResult = new GoogleSearch().Search(searchTerms);
            response.Results.Add(googleSearchResult);

            var bingSearchResult = new BingSearch().Search(searchTerms);
            response.Results.Add(bingSearchResult);

            return response;
        }
    }

    public class GoogleSearch
    {
        public SearchResponse Search(string searchTerms)
        {
            var response = new SearchResponse
            {
                Provider = "Google",
            };

            var browserService = new BrowserService();
            var content = browserService.GetContent($"https://www.google.com.au/search?q={searchTerms}").Result;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            var fetchedNodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"rso\"]/div/div/div/div/div/a[@href] | //*[@id=\"rso\"]/div/div/div/div/a[@href]");

            // get the top 5
            response.Urls = fetchedNodes.Take(5).Select(x => x.GetAttributeValue("href", string.Empty)).ToArray();

            return response;
        }
    }

    public class BingSearch
    {
        public SearchResponse Search(string searchTerms)
        {
            var response = new SearchResponse
            {
                Provider = "Bing",
            };

            var browserService = new BrowserService();
            var content = browserService.GetContent($"https://www.bing.com/search?q={searchTerms}").Result;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            var fetchedNodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"b_results\"]/li/h2/a[@href]");

            // get the top 5
            response.Urls = fetchedNodes.Take(5).Select(x => x.GetAttributeValue("href", string.Empty)).ToArray();

            return response;
        }
    }

    public class BrowserService
    {
        public async Task<string> GetContent(string url)
        {
            // Download the Chromium revision if it does not already exist
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

            // Create an instance of the browser and configure launch options
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            // Create a new page and go to url
            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            // Return the HTML of the current page
            return await page.GetContentAsync();
        }
    }

    public class Response
    {
        public Response()
        {
            Results = new List<SearchResponse>();
        }
        public List<SearchResponse> Results { get; set; }
        public string SearchTerms { get; set; }
    }

    public class SearchResponse
    {
        public string[] Urls { get; set; }
        public string Provider { get; set; }
    }
}
