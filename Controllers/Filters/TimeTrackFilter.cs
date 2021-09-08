using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Controllers.Filters
{
    public class TimeTrackFilter : Attribute, IActionFilter
    {
        private Stopwatch stopwatch;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch.Stop();

            var miliseconds = stopwatch.ElapsedMilliseconds;
            var action = context.ActionDescriptor.DisplayName;

            Debug.WriteLine($"Action [{action}], executed in: {miliseconds} miliseconds");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
    }
}
