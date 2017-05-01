using Flan411.Models;
using Flan411.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flan411console
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (!T411Service.VerifyToken())
            //    auth();

            //while (true)
            //{
            //Console.Write("Search: ");
            //string pattern = Console.ReadLine();

            //var task = T411Service.Search(pattern);
            //task.Wait();

            //List<Torrent> results = task.Result;

            //Console.WriteLine("Results: ");
            //foreach (var t in results)
            //{
            //    Console.WriteLine($"{t.Id} : {t.Name}");
            //}
            //Console.WriteLine("---");

            //const int ID = 4578050;
            //T411Service.Download(ID, "tamaman.torrent");

            //var task = TvMazeService.Search("The walking dead");
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
            //}

        }

        private static string auth()
        {
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
            return user.Token;
        }
    }
}
