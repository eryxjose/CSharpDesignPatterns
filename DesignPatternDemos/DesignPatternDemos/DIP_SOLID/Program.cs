using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIP_SOLID
{
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Tarefa
    {
        public string Titulo;
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Tarefa> FindAllChildrenOf(string name);
    }

    // low-level
    public class Relationships : IRelationshipBrowser
    {
        // Tuples C# 7 
        private List<(Tarefa, Relationship, Tarefa)> relations = new List<(Tarefa, Relationship, Tarefa)>();

        public void AddParentAndChild(Tarefa parent, Tarefa child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }

        public IEnumerable<Tarefa> FindAllChildrenOf(string name)
        {
            return relations
                    .Where(o => o.Item1.Titulo == name && o.Item2 == Relationship.Parent)
                    .Select(o => o.Item3);
            
            //foreach (var item in relations.Where(o => o.Item1.Titulo == name && o.Item2 == Relationship.Parent))
            //{
            //    yield return item.Item3;
            //}
        }

        // public List<(Tarefa, Relationship, Tarefa)> Relations => relations;

    }

    // high level
    public class Research
    {
        public Research(IRelationshipBrowser browser)
        {
            foreach (var item in browser.FindAllChildrenOf("Análise sistema agenda online."))
            {
                Console.WriteLine($"Subtarefa: {item.Titulo}");
            }
        }

        //public Research(Relationships relationships)
        //{
        //    var relations = relationships.Relations;
        //    foreach (var r in relations.Where(
        //        p =>
        //            p.Item1.Titulo.Contains("Análise") &&
        //            p.Item2 == Relationship.Parent)
        //        )
        //    {
        //        Console.WriteLine($"Tarefa relacionada a Análise: {r.Item3.Titulo}");
        //    }
        //}
    }

    class Program
    {
        static void Main(string[] args)
        {
            var parent = new Tarefa { Titulo = "Análise sistema agenda online." };
            var child1 = new Tarefa { Titulo = "Elaborar userstories." };
            var child2 = new Tarefa { Titulo = "Identificar tópicos para prototipação." };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);
            new Research(relationships);
            Console.ReadKey();
        }
    }
}
