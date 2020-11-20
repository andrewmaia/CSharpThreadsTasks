using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ContaBancaria conta ;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded; 
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IniciarConta();
        }

        private void IniciarConta()
        {
            conta = new ContaBancaria(100);
            labelSaldo.Content = conta.Saldo.ToString("C");
        }

        private void ButtonReiniciar_Click(object sender, RoutedEventArgs e)
        {
            IniciarConta();
        }

        private void ButtonSaqueComLock_Click(object sender, RoutedEventArgs e)
        {

            var progress = new Progress<bool>(sacou => ProcessarRetornoSaque(sacou));
            Task t = Task.Run(() => conta.SacarComLock(100, progress));
        }



        private void ButtonSaqueSemLock_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<bool>(sacou => ProcessarRetornoSaque(sacou));
            Task t = Task.Run(() => conta.SacarSemLock(100, progress));
        }

        private void ProcessarRetornoSaque(bool sacou)
        {
            if (sacou)
                labelSaldo.Content = conta.Saldo.ToString("C");
            else
                MessageBox.Show("Não há saldo suficiente para realizar o saque!", "Sem Saldo");
        }


    }

    public class ContaBancaria
    {
        private object trava = new object();
        public double Saldo
        {
            get;
            private set;
        }
        public ContaBancaria(double saldoInicial)
        {
            Saldo = saldoInicial;
        }
        private bool Sacar(double valor, IProgress<bool> progress)
        {
            bool sacou = false;
            if (Saldo >= valor)
            {
                Thread.Sleep(3000);
                Saldo -= valor;
                sacou = true;
            }
            progress.Report(sacou);
            return sacou;
        }

        public bool SacarSemLock(double valor,IProgress<bool> progress)
        {
            return Sacar(valor, progress);
        }        

        public bool SacarComLock(double valor, IProgress<bool> progress)
        {
            lock (trava)
            {
                return Sacar(valor, progress);
            }
        }

    }

        

}
