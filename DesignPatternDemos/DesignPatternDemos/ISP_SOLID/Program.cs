using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_SOLID
{

    /*
     
        Interface Segregation Principle especifica a criação de múltiplas interfaces simples ou 
        atômicas que podem ser combinadas para compor diferentes funcionalidades.

        O padrão Decorator está em conformidade com o princípio Interface Segregation Principle.

    */

    public class Tarefa
    {
    }

    public interface INotificador
    {
        void EnviarEmail(Tarefa tarefa);
        void EnviarSMS(Tarefa tarefa);
    }

    public class NotificadorEquipe : INotificador
    {
        private IEnviarEmail enviarEmail;
        private IEnviarSMS enviarSMS;

        public NotificadorEquipe(IEnviarEmail enviarEmail, IEnviarSMS enviarSMS)
        {
            if (enviarEmail == null)
            {
                throw new ArgumentNullException(paramName: nameof(enviarEmail));
            }
            if (enviarSMS == null)
            {
                throw new ArgumentNullException(paramName: nameof(enviarSMS));
            }
            this.enviarEmail = enviarEmail;
            this.enviarSMS = enviarSMS;
        }

        public void EnviarEmail(Tarefa tarefa)
        {
            enviarEmail.EnviarEmail(tarefa);
        }

        public void EnviarSMS(Tarefa tarefa)
        {
            enviarSMS.EnviarSMS(tarefa);
        }
    }
    
    public interface IEnviarEmail
    {
        void EnviarEmail(Tarefa tarefa);
    }
    public interface IEnviarSMS
    {
        void EnviarSMS(Tarefa tarefa);
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
