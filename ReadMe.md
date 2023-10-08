# RMRequestObserver 

## Description
observe details of request and powefull config to have custome bussiness what to do whith them.

#### sample usage source:
```csharp
    app.UseRMRequestObserver(new()
    {
        IgnoreUserNames = new() { "a2@gmail.com" },
        IgnorePaths = new() { "/Account/ExternalLogin", "account/login", "/Home/Error", "/_content", "/admin", "/api/" },
        IgnoreResponseStatusCodes = new() { 404 },
        RequestTypes = new() { "GET" },
        CallBack = async (logItem, clientInfo, statusCode) =>
        {
            using var scope = app.Services.CreateAsyncScope();
            var repo = scope.ServiceProvider.GetRequiredService<IStatisticsItemLogRepository>();
            var dbLog = new StatisticsLogItem(
                logItem.User,
                logItem.Path,
                logItem.Referer,
                logItem.Ip,
                logItem.Country,
                clientInfo.UA.Family,
                $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}",
                clientInfo.OS.Family,
                $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}",
                $"{clientInfo.Device.Family}({clientInfo.Device.Brand} {clientInfo.Device.Model})"
            );
            await repo.AddAsync(dbLog).ConfigureAwait(false);
            var res = await repo.UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    });
```

