using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;

//public class TrackActionTimeFilter : IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        Console.WriteLine("Track Action Time Filter Started");
//        context.HttpContext.Items["ActionStartTime"] = DateTime.UtcNow;
//        await next();
//        var startTime = (DateTime)context.HttpContext.Items["ActionStartTime"]!;
//        var elapsed = DateTime.UtcNow - startTime;
//        context.HttpContext.Response.Headers.Append("X-Elapsed-Time", $"{elapsed.TotalMilliseconds}ms");
//        Console.WriteLine($"Track Action Time Filter Took {elapsed.TotalMilliseconds}ms")
//    }
//}

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class TrackActionTimeFilter : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        Console.WriteLine("Track Action Time Filter Started");
//        context.HttpContext.Items["ActionStartTime"] = DateTime.UtcNow;
//        await next();
//        var startTime = (DateTime)context.HttpContext.Items["ActionStartTime"]!;
//        var elapsed = DateTime.UtcNow - startTime;
//        context.HttpContext.Response.Headers.Append("X-Elapsed-Time", $"{elapsed.TotalMilliseconds}ms");
//        Console.WriteLine($"Track Action Time Filter Took {elapsed.TotalMilliseconds}ms");
//    }
//}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TrackActionTimeFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("Track Action Time Filter Started");
        context.HttpContext.Items["ActionStartTime"] = DateTime.UtcNow;
        await next();
        var startTime = (DateTime)context.HttpContext.Items["ActionStartTime"]!;
        var elapsed = DateTime.UtcNow - startTime;
        context.HttpContext.Response.Headers.Append("X-Elapsed-Time", $"{elapsed.TotalMilliseconds}ms");
        Console.WriteLine($"Track Action Time Filter Took {elapsed.TotalMilliseconds}ms");
    }
}

