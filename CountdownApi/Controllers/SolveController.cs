using CountdownEngine;
using CountdownEngine.Solvers;
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
            //List<int> numbers = new List<int> { 75, 100, 5, 4, 7, 8 }; var target = 947;// 964;// 100;
            //List<int> numbers = new List<int> { 25, 50, 75, 100, 3, 6 }; var target = 952;// 964;// 100; // hardest ever
            List<int> numbers = new List<int> { 25, 50, 75, 100, 3, 6 }; var target = 106;// 102;// 952;// 964;// 100;
            //List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 75 }; var target = 947;// can't be done
            //List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 75 }; var target = 100;
            //List<int> numbers = new List<int> { 25, 75, 7 }; var target=107;

            Stopwatch stopWatch = new Stopwatch();

            IEnumerable<Solution> solutions = null;
            //IEnumerable<string> solutions=null;
            stopWatch.Start();
            var solver = new Solver3();
            for (int i = 0; i < 1; i++)
            {
                solutions = solver.Solve(numbers, target);
            }
            stopWatch.Stop();



            var solutionsString = $"Num solutions={solutions.Count()}<br>";
            solutionsString += $"Num calls={solver.NumCalls()}<br>";
            solutionsString += $"Num skipped={solver.NumSkipped()}<br>";
            solutionsString += $"Time taken={FormatStopWatch(stopWatch)}<br>";
            solutionsString += $"Target={target}<br>";
            solutionsString += $"Numbers={string.Join(",", numbers)}<br>";

            solutionsString += "<br><br>";

            foreach (var s in solutions.OrderBy(x => x.RpnString.Length))
//            foreach (var s in solutions.OrderBy(x=>x.CallNum))
                solutionsString += s.RpnString + " : " + s.InlineString + s.SeparateCalculationsString + "<br>";

            return base.Content(solutionsString, "text/html");
        }

        private string FormatStopWatch(Stopwatch stopWatch)
        {
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            return elapsedTime;
        }
    }
}
