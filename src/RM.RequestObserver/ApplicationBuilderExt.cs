using Microsoft.AspNetCore.Builder;
using System.Web;
using UAParser;

namespace RM.RequestObserver;

public static class ApplicationBuilderExt
{
    public static IApplicationBuilder UseRMRequestObserver(this IApplicationBuilder app, StatisticConfig config) =>
        app.Use(async (ctx, next) =>
        {
            await next();

            if (config.IgnoreResponseStatusCodes.Contains(ctx.Response.StatusCode)
            || !config.RequestTypes.Contains(ctx.Request.Method)
            || config.IgnoreUserNames.Any(x => x == ctx.User.Identity?.Name)
            || config.IgnorePaths.Any(x => ctx.Request.Path.Value.Contains(x, StringComparison.OrdinalIgnoreCase))
            )
                return;

            string uaString = ctx.Request.Headers["User-Agent"];


            //-- get a parser using externally supplied yaml definitions
            // var uaParser = Parser.FromYaml(yamlString);
            // ClientInfo clientInfo = uaParser.Parse(uaString);

            //-- get a parser with the embedded regex patterns
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(uaString);

            StatisticsLog statisticsLog = new()
            {
                User = ctx.User.Identity?.Name,
                Ip = $"{ctx.Connection.RemoteIpAddress}",//+$":{ctx.Connection.RemotePort}",
                Country = $"http://ip-api.com/json/{ctx.Connection.RemoteIpAddress}",//https://ip-api.com/
                Referer = HttpUtility.UrlDecode(ctx.Request.Headers["Referer"]),
                Path = HttpUtility.UrlDecode(ctx.Request.Path),
            };

            config?.CallBack?.Invoke(statisticsLog, clientInfo, ctx.Response.StatusCode);

        });
}
