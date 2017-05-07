using Flan411.Models;
using Flan411.Tools;
using System;
using System.Collections.Generic;

namespace Flan411console
{
    class Program
    {
        static void Main(string[] args)
        {
            t411();
            //tvMaze();
        }

        private static void t411()
        {
            if (!T411Service.VerifyToken())
                auth();

            while (true)
            {
                Console.Write("Search: ");
                string pattern = Console.ReadLine();

                var task = T411Service.Search(pattern, 10, T411Service.CID_SERIES);
                try
                {
                    task.Wait();
                }catch(Exception e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    auth();
                    continue;
                }

                List<Torrent> results = task.Result;

                Console.WriteLine("Results: ");
                foreach (var t in results)
                {
                    Console.WriteLine($"{t.Id} : {t.Name}");
                }
                Console.WriteLine("---");
            }
        }

        private static string auth()
        {

            //var task = TvMazeService.Search("The walking dead");

            Console.Write("User: ");
            string userName = Console.ReadLine();

            // read password without outputing
            // thanks to http://stackoverflow.com/a/36332407
            Console.Write("Password: ");
            string pwd = null;
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                pwd += key.KeyChar;
            }
            Console.WriteLine();

            User user = T411Service.AuthenticateUser(userName, pwd).Result;
            if (user == null)
                return null;
            return user.Token;
        }

        private static void tvMaze()
        {
            try
            {
                var test = TvMazeService.Search("The walking dead").Result;
            }
            catch (AggregateException)
            {
                Console.WriteLine("Aucune information trouvée.");
            }

            Console.WriteLine("FIN");
            Console.ReadKey();
        }
    }
}
