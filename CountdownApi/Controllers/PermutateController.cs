using CountdownEngine;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CountdownApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermutateController : ControllerBase
    {
        public string Get()
        {
            List<int> numbers = new List<int>{ 1, 2, 3, 4, 5, 6 };

            var permutations = Permutater.Permutate(numbers);
            var permsJson = JsonConvert.SerializeObject(permutations, Formatting.Indented);
            return permsJson.ToString();
        }
    }
}
