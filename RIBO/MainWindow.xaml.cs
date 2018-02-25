using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace RIBO
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "c:\\users\\bureau\\documents\\visual studio 2017\\Projects\\RIBO\\RIBO\\Resources\\modpoll.exe",
                    Arguments = "-m enc -r 1 -c 40 -t 4 -p 1 -1 -d 8 127.0.0.1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string line = String.Empty;
            while (!proc.StandardOutput.EndOfStream)
            {
                line += proc.StandardOutput.ReadLine() + System.Environment.NewLine;
                // do something with line
            }

            valeur.Text = line;
        }
    }
}
