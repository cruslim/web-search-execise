# Pairing exercise
This (C#) API project is not maintainable and it is poorly written. 
Please review and improve in appropriate areas. 

Consider all aspects of good software engineering and show us how you'll make it a production ready code.


### To run the API project:
- using **VS Code**, in the terminal, run `dotnet build; dotnet run` 
- using **VisualStudio**, open the solution as per usual (F5 or the 'play' button)
- on your browser, navigate to `https://localhost:44378/websearch?searchterms=google`


This API has 1 endpoint only which is: `GET /websearch?searchterms={search-terms}` 

On execution, it will perform web search on both Google and Bing and it will return the top **5** url results from each search provider, such as:

```json
{
    "results": [
        {
            "urls": [
                "https://www.google.com/",
                "http://www.google.com.au/",
                "https://www.facebook.com/GoogleDownUnder/",
                "https://about.google/products/",
                "https://apps.apple.com/us/app/google/id284815942"
            ],
            "provider": "Google"
        },
        {
            "urls": [
                "https://www.google.com.au/",
                "https://www.google.com.au/webhp?gl=AU",
                "https://www.google.com/search",
                "https://maps.google.com.au/",
                "https://twitter.com/Google"
            ],
            "provider": "Bing"
        }
    ],
    "searchTerms": "google"
}
```
