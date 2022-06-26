
using System.Collections.Generic;

namespace CountdownEngine
{
    public static class Permutater
    {
        public static IEnumerable<List<int>> Permutate(List<int> input)
        {
            if (input.Count == 2) // these are permutations of array of size 2
            {
                yield return new List<int>(input);
                yield return new List<int> { input[1], input[0] };
            }
            else
            {
                foreach (var elem in input) // going through array
                {
                    var rlist = new List<int>(input); // creating subarray = array
                    rlist.Remove(elem); // removing element
                    foreach (var retlist in Permutate(rlist))
                    {
                        retlist.Insert(0, elem); // inserting the element at pos 0

                        yield return retlist;
                    }
                }
            }
        }
    }
}
