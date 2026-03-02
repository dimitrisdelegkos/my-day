# My Day

## Introduction

**MyDay** is a .NET 8 web API that aims to provide useful data to any end user in order to carry on their day. 

More specifically **MyDay** integrates with three different external APIs, fetches their data and returns the details back to the user, providing useful insights on the day's weather report, the top news headlines and finally suggesting some music playlists based on the end user's taste.

## Solution Design

**MyDay** consists of two .NET 8 projects:
- `MyDay.API`: an API that exposes an endpoint for accepting the user query in order to forward the respective web requests to the three external APIs and finally fetch the respective data.
- `MyDay.Core `: a library projects consisting of three layers:
  -   `Infrastructure`, which serves as the base of the project offering reusable services for the upper solution architectural layers.
  -   `Services`, which uses the services exposed by the `Infrastructure` in order to integrate with any external system.
  -   and `Application`, where all the proper business logic (i.e. data handling, caching) is applied to the data received by the `Services` layer.

### Endpoints
**MyDay** exposes two endpoints from its `API` project:
- `/MyDailyInfo` GET operation, which accepts the user criteria and perfoms the respective requests to the external systems for fetching related data.
- `/Performance` GET operation, which returns the performance metrics per external system, allocated the respective requests as below: fast <200ms, average 200-400ms, slow > 400ms

**MyDay** accepts and returns data that comply to the JSON format.

The above endpoints, their inputs and outputs are described in details in the respective swagger file: [/swagger/index.html](https://github.com/dimitrisdelegkos/my-day/blob/master/myday-api-definition.json).

### Configuration
The basic objects included in the configuration of the system are the following:
- `NewsAPISettings`: it contains the endpoints and the API key for integrating with `News API`
- `CacheSettings`: it contains the settings for caching functionality. `CacheSettings:Mode` setting can be set either to `InMemory` or to `Distributed`. If `InMemory` is used caching is implemented with the use of the application memory, else a distributed cache (i.e. a Redis Cache) must be in place for utilization. 
- `OpenWeatherAPISettings`: it contains the endpoints and the API key for integrating with `Open Weaher Map API`
- `TidalAPISettings`: it contains the endpoints and the client credentials for integrating with `Tidal API`
- `DailyTipsSettings`: it contains properties for altering the results returned by the API (e.g. the count of top headlines)

## Integrations

**MyDay** integrates with the systems that are mentioned in the following table:
|Source| Destination | Integration Protocol  | Authentication Method | API Documentation | API Endpoint(s) Consumed | Comments |
|--|--|--|--|--|--|--|
| MyDay | News API | REST | API Key |[API Documentation](https://newsapi.org/docs) |[Top Headlines](https://newsapi.org/docs/endpoints/top-headlines) |- 
| MyDay |Open Weather MAP API | REST | API Key |[API documentation](https://openweathermap.org/api/one-call-3?collection=one_call_api_3.0) | [Daily Aggregation](https://openweathermap.org/api/one-call-3?collection=one_call_api_3.0&collection=one_call_api_3.0#history_daily_aggregation) | - |
| MyDay | Tidal API| REST | OAuth 2.0 | [API documentation](https://tidal-music.github.io/tidal-api-reference/#/), [API overview](https://developer.tidal.com/documentation/api-sdk/api-sdk-overview) | [Token Endpoint](https://developer.tidal.com/documentation/api-sdk/api-sdk-authorization), [Get Playlists by term](https://tidal-music.github.io/tidal-api-reference/#/searchResults/get_searchResults__id__relationships_playlists), [Get Playlist by ID](https://tidal-music.github.io/tidal-api-reference/#/playlists/get_playlists__id_) |- |
| MyDay | Redis Cache | TCP| Connection string with credentials| - |- |Optional, `MemoryCache` can be used instead for caching

## How to run locally

In order to run the project locally the following are required:
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) must be installed
- Visual Studio v22 (or later)
- Make sure to create a `secrets.json` file as per the example `secrets.json.example` of the project `MyDay.API`. Any missing secrets must be provided by the system owner.
