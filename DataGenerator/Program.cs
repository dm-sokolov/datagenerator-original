using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerator.BL;
using DataGenerator.Data;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Repository repository = new Repository();
            repository.Init();

            ScriptGenerator generator = new ScriptGenerator(repository);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            ScriptWriter writer = new ScriptWriter(generator);
            writer.WriteAsync(@"D:\testScript.sql", 1000);

            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
