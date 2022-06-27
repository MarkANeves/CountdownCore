using CountdownEngine;
using CountdownEngine.Solvers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

//using System.Web.Http;

namespace CountdownApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolveController : Controller
    {
        private ISolver _solver;

        public SolveController(ISolver solver)
        {
            _solver = solver;
        }

        [HttpGet("test")]
        public ContentResult Get()
        {
            //List<int> numbers = new List<int> { 25, 75, 7, 4, 5, 3 }; var target = 640;
            //List<int> numbers = new List<int> { 75, 100, 5, 4, 7, 8 }; var target = 947;// 964;// 100;
            //List<int> numbers = new List<int> { 25, 50, 75, 100, 3, 6 }; var target = 952;// 964;// 100; // hardest ever
            //List<int> numbers = new List<int> { 25, 50, 75, 100, 3, 6 }; var target = 106;// 102;// 952;// 964;// 100;
            //List<int> numbers = new List<int> { 25, 50, 99, 100, 7, 6 }; var target = 106;// 102;// 952;// 964;// 100;
            //List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 75 }; var target = 947;// can't be done
            //List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 75 }; var target = 100;
            //List<int> numbers = new List<int> { 25, 75, 7 }; var target=107;
            List<int> numbers = new List<int> { 2, 5, 9, 25, 75, 10 }; var target=109;
            //http://localhost:55599/api/solve?n=2&n=5&n=9&n=25&n=75&n=10&n=109

            Stopwatch stopWatch = new Stopwatch();

            IEnumerable<Solution> solutions = null;
            //IEnumerable<string> solutions=null;
            stopWatch.Start();
            for (int i = 0; i < 1; i++)
            {
                solutions = _solver.Solve(numbers, target);
            }
            stopWatch.Stop();

            var solutionsString = $"Num solutions={solutions.Count()}<br>";
            solutionsString += $"Num calls={_solver.NumCalls()}<br>";
            solutionsString += $"Num skipped={_solver.NumSkipped()}<br>";
            solutionsString += $"Time taken={FormatStopWatch(stopWatch)}<br>";
            solutionsString += $"Target={target}<br>";
            solutionsString += $"Numbers={string.Join(",", numbers)}<br>";

            solutionsString += "<br><br>";

            foreach (var s in solutions.OrderBy(x => x.RpnString.Length))
            {
                solutionsString += s.RpnString + " : " + s.InlineString;
                foreach (var calcStr in s.SeparateCalculations)
                {
                    solutionsString += "<br>"+calcStr;
                }

                solutionsString += "<br>--------------------------------------<br>";
            }

            return base.Content(solutionsString, "text/html");
        }

        public IActionResult Get([FromQuery] List<int> n)
        {
            if (n.Count < 2)
            {
                return BadRequest("At least 2 numbers have to be supplied");
            }

            int target = n[^1];
            n.RemoveAt(n.Count - 1);
            var solutions = _solver.Solve(n, target);
            var json = JsonConvert.SerializeObject(solutions.OrderBy(x => x.RpnNodesLength));
            return Ok(json);
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
