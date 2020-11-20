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

namespace ProgressBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        CancellationTokenSource _tokenSource = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            int contagem = 100;
            progress1.Maximum = contagem;
            progress1.Value = 0;
            var progresso = new Progress<int>(value =>
             {
                 progress1.Value = value;
                 if (progress1.Value >= progress1.Maximum)
                 {
                     FinalizarProcesso("Processo Finalizado");
                 }
                 else
                     label1.Content = $"{value}%";
             });


            //Utilizando task:
            Task.Run(() => IniciarProcesso(contagem, progresso, token));
            //Utilizando Thread (Mesmo resultado)
            //Thread t = new Thread(() => IniciarProcesso(contagem, progresso, token));
            //t.Start();

            label1.Content = $"Processo Iniciado";
            button.IsEnabled = false;
            buttonCancelar.IsEnabled = true;
        }

        private void IniciarProcesso(int contagem,IProgress<int> progresso, CancellationToken token)
        {
            for (int i= 1; i<= contagem;i++)
            {
                Thread.Sleep(100);
                if (token.IsCancellationRequested)
                    return;
                var percentual = (i * 100) / contagem;
                progresso.Report(percentual);
            }
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            _tokenSource.Cancel();
            FinalizarProcesso("Processo Cancelado");
        }

        private void FinalizarProcesso(string mensagem)
        {
            _tokenSource.Dispose();
            label1.Content = mensagem;
            button.IsEnabled = true;
            buttonCancelar.IsEnabled = false;
        }
    }
}
