using System;
using System.Collections.Generic;
using System.Linq;

namespace stone_scissors_paper
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (!CheckArguments(args))
                return;
            var game = new Game(args.ToList());
            game.Run();
        }

        private static bool CheckArguments(IEnumerable<string> arguments)
        {
            var data = arguments.ToList();
            if (data.Count % 2 == 0 || data.Count < 3)
            {
                Console.WriteLine("Error. Amount of arguments must be >= 3.");
                return false;
            }
            var duplicates = data.GroupBy(x => x)
                .SelectMany(g => g.Skip(1));
            if (!duplicates.Any()) return true;
            Console.WriteLine("Error. There should be no duplicates.");
            return false;
        }
    }
}