# Narato.Correlations
This library contains middleware for setting a Correlation Id on responses, and methods to use them cross-app

When working with a "micro service" architecture, you want to be able to follow the flow of your entire application in log files.  
That means that when something goes wrong when calling a service with a http request, you want to be able to link logging messages from the failing service 
to logging messages from the main request on the main API.  
This library can be used for this type of scenario

What this library does correlation-wise in a nutshell:
1) Every request gets a correlation Id. There are 2 scenarios here
    * There was a correlation Id on the incoming request. This one will be used for the current request as well
    * There is no correlation Id on the incoming request. A new one will be used for the current request
2) Every **outgoing** request gets the correlation Id set as a request header, thanks to the HttpClientFactory.

Getting started
==========
### 1. Add dependency in your project's csproj file

```xml
<PackageReference Include="Narato.Correlations" Version="2.0.0" />
```

### 2. Configure Startup.cs
In the ConfigureServices method, add following line:
```C#
app.AddCorrelations();
```

In the Configure method, add following line:
```C#
app.UseCorrelations();
```
This line has to be **above** `app.UseMvc();`, preferably as high as possible.

### 3. Use HttpClientFactory
Everywhere where you use a HttpClient, **use** the HttpClientFactory to instantiate the HttpClient.
The factory has a `Create()` method with 1 overload with a callback `Action<HttpClient>`.
An example on how to use it to instantiate a HttpClient with a BaseAddress
```C#
var client = factory.Create((c) =>
{
    c.BaseAddress = new Uri("http://www.google.com/");
});
```

### 4. Use the correlation Id
For logging, you can find an [NLog Extension here](https://github.com/Narato/Narato.Correlations.NlogExtensions) and a [SeriLog Extension here](https://github.com/Narato/Narato.Correlations.SerilogMiddleware)

If you want to use the correlation Id anywhere yourself, just inject `ICorrelationIdProvider` in your service, and call `GetCorrelationId()` on the provider.

# Helping out

If you want to help out, please read [this wiki page](https://github.com/Narato/Narato.Correlations/wiki/Helping-out)