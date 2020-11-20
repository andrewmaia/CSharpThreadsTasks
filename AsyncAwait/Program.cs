using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Chamou Task 1 SEM await, assim ela irá executar e não irá travar thread principal permitindo qua Task 2 seja chamada");
            Task<string> task1 = Task.Run(() => Task1());

            Console.WriteLine("Chamou Task 2 COM await, a Thread Principal será forçada aguardar a finalização da Task 2");
            string retornoTask2 = await Task.Run(() => Task2());
            Console.WriteLine(retornoTask2);

            //Obtendo o retorno da task1
            task1.Wait();//Checa se a task1 já finalizou para obter seu retorno, no caso é 
            //desnessário porque a task2 leva mais tempo para executar que a task1 e quanto o código chega neste ponto
            //a task1 já está finalizada mas é uma boa prática utilizar o Wait antes de obter o Result 
            Console.WriteLine(task1.Result);


            Console.WriteLine("Repare que apesar de ter terminado antes, a Task1 teve que aguarda a Task2 para exibir seu resultado no console");
            Console.ReadLine();
        }

        private static string Task1()
        {
            Thread.Sleep(2000);
            return $"Task 1 terminou às {DateTime.Now:HH:mm:ss}";
        }

        private static string Task2()
        {
            Thread.Sleep(6000);
            return $"Task 2 terminou às {DateTime.Now:HH:mm:ss}"; 
        }
    }
}
