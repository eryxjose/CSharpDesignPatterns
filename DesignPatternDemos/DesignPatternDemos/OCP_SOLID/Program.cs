using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCP_SOLID
{
    /*
    Exemplo de implementação do princípio Open Closed Principle
    */

    /*
    Specification Pattern é classificado um tipo de Enterprise Pattern
    Este padrão garante que o princípio Open Closed Principle não seja quebrado.
    */

    #region Tarefa 
    public enum Situacao
    {
        Analise, Ativa, Impedida, Concluida, Fila
    }

    public enum Prioridade
    {
        Baixa, Media, Alta
    }

    public class Tarefa
    {
        public string Titulo;
        public Situacao Situacao;
        public Prioridade Prioridade;
        public Tarefa(string titulo, Situacao situacao, Prioridade prioridade)
        {
            if (titulo == null)
            {
                throw new ArgumentNullException(paramName: nameof(titulo));
            }
            Titulo = titulo;
            Situacao = situacao;
            Prioridade = prioridade;
        }
    }

    #endregion

    #region Implementação tradicional Filter

    /*
    A implementação tradicional (abaixo) quebra o princípio Open Closed Principle.
    TarefaFilter deve ser extendida e não alterada para respeitar o padrão OCP.
    */

    public class TarefaFilter
    {
        public IEnumerable<Tarefa> FilterBySituacao(
            IEnumerable<Tarefa> tarefas,
            Situacao situacao)
        {
            foreach (var t in tarefas)
            {
                if (t.Situacao == situacao)
                    yield return t;
            }
        }

        public IEnumerable<Tarefa> FilterByPrioridade(
            IEnumerable<Tarefa> tarefas,
            Prioridade prioridade)
        {
            foreach (var t in tarefas)
            {
                if (t.Prioridade == prioridade)
                    yield return t;
            }
        }

        public IEnumerable<Tarefa> FilterBySituacaoAndPrioridade(
            IEnumerable<Tarefa> tarefas,
            Prioridade prioridade,
            Situacao situacao)
        {
            foreach (var t in tarefas)
            {
                if (t.Prioridade == prioridade && t.Situacao == situacao)
                    yield return t;
            }
        }
    }

    #endregion

    #region Interfaces para implementar padrão Specification

    /*
     Interfaces para implementar padrão Specification
    */

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> itens, ISpecification<T> spec);
    }

    #endregion

    #region Specifications

    // AndSpecification implementa o padrão Combinator

    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;
        
        // TODO: implementar array de parâmetros: ISpecification<T> params[]

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);
        }
    }

    public class SituacaoSpecification : ISpecification<Tarefa>
    {
        private Situacao situacao;

        public SituacaoSpecification(Situacao situacao)
        {
            this.situacao = situacao;
        }

        public bool IsSatisfied(Tarefa t)
        {
            return t.Situacao == situacao;
        }
    }

    public class PrioridadeSpecification : ISpecification<Tarefa>
    {
        private Prioridade prioridade;

        public PrioridadeSpecification(Prioridade prioridade)
        {
            this.prioridade = prioridade;
        }
        public bool IsSatisfied(Tarefa t)
        {
            return t.Prioridade == prioridade;
        }
    }

    public class TarefaFilterSpecification : IFilter<Tarefa>
    {
        public IEnumerable<Tarefa> Filter(IEnumerable<Tarefa> itens, ISpecification<Tarefa> spec)
        {
            foreach (var item in itens)
            {
                if (spec.IsSatisfied(item))
                    yield return item;
            }
        }
    }


    #endregion


    public class Program
    {
        static void Main(string[] args)
        {
            var tarefa1 = new Tarefa("Preparar aula AngularJS.", Situacao.Fila, Prioridade.Media);
            var tarefa2 = new Tarefa("Revisar curtidas e compartilhamentos twitter.", Situacao.Fila, Prioridade.Baixa);
            var tarefa3 = new Tarefa("Andamento curso Design Patterns C#.", Situacao.Ativa, Prioridade.Alta);

            Tarefa[] tarefas = { tarefa1, tarefa2, tarefa3 };

            var f = new TarefaFilter();
            Console.WriteLine("Tarefas com situação Ativa (old): ");
            foreach (var item in f.FilterBySituacao(tarefas, Situacao.Ativa))
            {
                Console.WriteLine($" - {item.Titulo} é uma tarefa ativa.");
            }

            var tf = new TarefaFilterSpecification();
            Console.WriteLine("Tarefas com situação Ativa (new): ");
            foreach (var item in tf.Filter(tarefas, new SituacaoSpecification(Situacao.Ativa)))
            {
                Console.WriteLine($" - {item.Titulo} é uma tarefa ativa.");
            }

            Console.WriteLine("Tarefas situação Fila Prioridade Alta");
            foreach (var item in tf.Filter(
                tarefas, 
                new AndSpecification<Tarefa>(
                    new SituacaoSpecification(Situacao.Fila), 
                    new PrioridadeSpecification(Prioridade.Alta
                ))))
            {
                Console.WriteLine($" - {item.Titulo} - Situação: Fila, Prioridade: Alta.");
            }

            Console.ReadKey();
        }
    }
}
