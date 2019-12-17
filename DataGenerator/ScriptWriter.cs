using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerator.BL;
using DataGenerator.Data;

namespace DataGenerator
{
    public class ScriptWriter
    {
        private readonly IScriptGenerator _generator;

        public ScriptWriter(IScriptGenerator generator)
        {
            _generator = generator;
        }

        public void Write(string filePath, int entityCount)
        {
            using (TextWriter writer = new StreamWriter(filePath))
            {

                string insertLine = _generator.GetInsertLine();
                writer.WriteLine(insertLine);

                for (int i = 1; i <= entityCount; i++)
                {
                    if (i > 1) writer.Write(",");

                    UserEntity user = _generator.GenerateUser();
                    string valueLine = _generator.GetValueLine(user);
                    writer.WriteLine(valueLine);
                }
            }
        }

        public void WriteAsync(string filePath, int entityCount)
        {
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            bool isComplete = false;

            Task task = new Task(() =>
            {
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    string insertLine = _generator.GetInsertLine();
                    writer.WriteLine(insertLine);
                    string valueLine;

                    int i = 1;
                    while (!isComplete)
                    {
                        bool isNew = queue.TryDequeue(out valueLine);
                        if (isNew)
                        {
                            if (i > 1) writer.Write(",");
                            writer.WriteLine(valueLine);
                            i++;
                        }
                    }
                }
            });

            task.Start();

            Parallel.For(1, entityCount, i =>
            {
                UserEntity user = _generator.GenerateUser();
                string valueLine = _generator.GetValueLine(user);
                queue.Enqueue(valueLine);
            });

            isComplete = true;

            task.Wait();
        }
    }
}
