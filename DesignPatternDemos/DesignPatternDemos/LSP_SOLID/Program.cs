using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSP_SOLID
{

    /*
     
        Liskov Substitution Principle possibilita substituir o tipo pela sua classe base.
        
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
        public virtual string Titulo { get; set; }
        public virtual string EquipeResponsavel { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual Prioridade Prioridade { get; set; }

        public Tarefa()
        { }

        public Tarefa(
            string titulo, 
            string equipe, 
            DateTime datacadastro, 
            DateTime datainicio, 
            Situacao situacao, 
            Prioridade prioridade)
        {
            if (titulo == null)
            {
                throw new ArgumentNullException(paramName: nameof(titulo));
            }
            Titulo = titulo;
            EquipeResponsavel = equipe;
            DataCadastro = datacadastro;
            DataInicio = datainicio;
            Situacao = situacao;
            Prioridade = prioridade;
        }

        public override string ToString()
        {
            return $"{nameof(Titulo)}: {Titulo}, {nameof(Situacao)}: {Situacao}, {nameof(Prioridade)}: {Prioridade}";
        }
    }

    public class CorrecaoErro : Tarefa
    {
        public override DateTime DataCadastro { 
            set
            {
                base.DataCadastro = value;
                base.DataInicio = base.DataCadastro.AddDays(1);
                base.Situacao = Situacao.Fila;
                base.Prioridade = Prioridade.Alta;
                base.EquipeResponsavel = "Suporte";
            }
        }
    }

    #endregion

    class Program
    {
        public static int TotalDias(Tarefa t)
        {
            DateTime hoje = DateTime.Now;
            return (int)((hoje - t.DataInicio).TotalDays);
        }

        static void Main(string[] args)
        {
            Tarefa tarefa1 = new Tarefa
            {
                Titulo = "Criar currículo online.",
                EquipeResponsavel = "Equipe Dev",
                DataCadastro = new DateTime(2019, 01, 20, 8, 00, 00),
                DataInicio = new DateTime(2019, 01, 22, 8, 00, 00),
                Prioridade = Prioridade.Alta,
                Situacao = Situacao.Fila
            };
            
            Console.WriteLine($"{tarefa1} iniciou há {TotalDias(tarefa1)} dia(s).");

            Tarefa tarefa2 = new CorrecaoErro()
            {
                Titulo = "Remover repositórios temporários do GitHub.",
                DataCadastro = new DateTime(2019, 01, 12, 8, 00, 00),
            };

            Console.WriteLine($"{tarefa2} iniciou há {TotalDias(tarefa2)} dia(s).");


            Console.ReadKey();
        }
    }
}
