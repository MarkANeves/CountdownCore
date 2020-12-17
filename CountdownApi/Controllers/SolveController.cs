using CountdownEngine;
using CountdownEngine.Solver3;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CountdownApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolveController : Controller
    {
        public ContentResult Get()
        {
            //List<int> numbers = new List<int> { 25, 75, 7, 4, 5, 3 }; var target = 640;
            List<int> numbers = new List<int> { 75, 100, 5, 4, 7, 8 }; var target = 964;
            //List<int> numbers = new List<int> { 25, 75, 7 }; var target=107;

            Stopwatch stopWatch = new Stopwatch();

            IEnumerable<Solution> solutions = null;
            //IEnumerable<string> solutions=null;
            stopWatch.Start();
            for (int i = 0; i < 10; i++)
            {
                var solver = new Solver();
                solutions = solver.Solve(numbers, target);
            }
            stopWatch.Stop();



            var solutionsString = $"Num solutions={solutions.Count()}<br>";
            solutionsString += $"Num calls={Solver.numCalls}<br>";
            solutionsString += $"Num skipped={Solver.numSkipped}<br>";
            solutionsString += $"Time taken={FormatStopWatch(stopWatch)}<br>";

            solutionsString += "<br><br>";

//            foreach (var s in solutions.OrderBy(x=>x.RpnString.Length))
//                solutionsString += s.RpnString + " : " + s.InlineString + "<br>";

            return base.Content(solutionsString, "text/html");
        }

        private string FormatStopWatch(Stopwatch stopWatch)
        {
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            return elapsedTime;
        }
    }
}
