using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

public class ExecutionTimeActionFilter : IActionFilter
{
    private Stopwatch _stopwatch;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();
        var actionName = context.ActionDescriptor.DisplayName;
        var elapsedTime = _stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Action '{actionName}' executed in {elapsedTime} ms.");
    }
}
