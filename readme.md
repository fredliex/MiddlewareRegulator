```csharp
services.AddMiddlewareRegulator(appBuilder =>
{
    // ensure StaticFile and move to first
    var staticFileMiddleware = appBuilder.Find<StaticFileMiddleware>();
    if (staticFileMiddleware == null) 
    {
        appBuilder.UseStaticFiles();
        staticFileMiddleware = appBuilder.Find<StaticFileMiddleware>();
    }
    appBuilder.Remove(staticFileMiddleware);
    appBuilder.Insert(0, staticFileMiddleware);
});

services.AddMiddlewareRegulator(appBuilder =>
{
    // remove DeveloperExceptionPage
    var developerExceptionPageMiddleware = appBuilder.Find<DeveloperExceptionPageMiddleware>();
    if (developerExceptionPageMiddleware != null) appBuilder.Remove(developerExceptionPageMiddleware);
});
```