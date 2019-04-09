using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRP_SOLID
{
    public class Tarefa
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string descricao)
        {
            entries.Add($"{++count}: {descricao}");
            return count; // memento
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, entries);
        }
    }

    public class Persistence
    {
        public void SaveToFile(Tarefa tarefa, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
            {
                File.WriteAllText(filename, tarefa.ToString());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tarefa = new Tarefa();
            tarefa.AddEntry("Atualizar currículo.");
            tarefa.AddEntry("Preparar aula AngularJS.");
            Console.WriteLine(tarefa);

            var p = new Persistence();
            var filename = @"c:\temp\tarefas.txt";
            p.SaveToFile(tarefa, filename, true);

            Process.Start(filename);

            Console.ReadKey();
        }
    }
}
