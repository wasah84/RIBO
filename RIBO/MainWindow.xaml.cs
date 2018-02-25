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

            refreshValues();

            //result.Text = proc.ExitCode.ToString();

        }

        private void launch_Click(object sender, RoutedEventArgs e)
        {
            refreshValues();
        }

        private void refreshValues()
        {
            using (new WaitCursor())
            {
                // Mode
                mode.Text = getModeStringValue(getValue(10));

                // Consigne générale
                consigne.Text = getTemperature(getValue(9), false);

                // Code défaut
                defaut.Text = getValue(13);

                // Consigne pièces
                piece1_value.Text = getTemperature(getValue(29), false);
                piece2_value.Text = getTemperature(getValue(30), false);
                piece3_value.Text = getTemperature(getValue(31), false);
                piece4_value.Text = getTemperature(getValue(32), false);
                piece5_value.Text = getTemperature(getValue(33), false);
                piece6_value.Text = getTemperature(getValue(34), false);

                // Températures pièces
                piece1_value_Copy.Text = getTemperature(getValue(37), true);
                piece2_value_Copy.Text = getTemperature(getValue(38), true);
                piece3_value_Copy.Text = getTemperature(getValue(39), true);
                piece4_value_Copy.Text = getTemperature(getValue(40), true);
                piece5_value_Copy.Text = getTemperature(getValue(41), true);
                piece6_value_Copy.Text = getTemperature(getValue(42), true);

                refreshTime.Text = DateTime.Now.ToString();
            }
        }

        public class WaitCursor : IDisposable
        {
            private Cursor _previousCursor;

            public WaitCursor()
            {
                _previousCursor = Mouse.OverrideCursor;

                Mouse.OverrideCursor = Cursors.Wait;
            }

            #region IDisposable Members

            public void Dispose()
            {
                Mouse.OverrideCursor = _previousCursor;
            }

            #endregion
        }

        private String getValue(int register)
        {

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "c:\\users\\bureau\\documents\\visual studio 2017\\Projects\\RIBO\\RIBO\\Resources\\modpoll.exe",
                    Arguments = "-m enc -r " + register + " -t 4 -p 1 -1 -d 8 127.0.0.1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            Boolean start = false;
            String currentLine = String.Empty;
            String values = String.Empty;
            while (!proc.StandardOutput.EndOfStream)
            {
                currentLine = proc.StandardOutput.ReadLine();
                if (start)
                {
                    values += currentLine + System.Environment.NewLine;
                }
                if (currentLine.Contains("Polling slave"))
                {
                    start = true;
                }
            }

            proc.Dispose();
            proc.Close();

            return values.Split(':')[1].Trim();
        }

        private String getModeStringValue(string intMode)
        {
            String stringMode = "Unknown";
            switch (intMode)
            {
                case "0":
                    stringMode = "Automatique";
                    break;
                case "1":
                    stringMode = "Arrêt";
                    break;
                case "2":
                    stringMode = "Chauffage";
                    break;
                case "3":
                    stringMode = "Climatisation";
                    break;
                case "4":
                    stringMode = "Ventilation";
                    break;
                case "5":
                    stringMode = "Déshumidification";
                    break;
                default:
                    stringMode = "Inconnu";
                    break;
            }

            return stringMode;
        }

        private String getTemperature(String temperature, Boolean trailingZero)
        {
            double intTemp = -1;
            double.TryParse(temperature, out intTemp);

            if (intTemp != 0)
            {
                intTemp = intTemp / 100.0;
                if (trailingZero)
                {
                    temperature = intTemp.ToString("N2");
                }
                else
                {
                    temperature = intTemp.ToString();
                }
                temperature += "°C";
            }
            else
            {
                temperature = "Eteint";
            }

            return temperature;
        }
    }
}
