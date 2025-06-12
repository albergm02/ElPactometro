using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TrabajoFinal_ELPACTOMETRO.Modelo
{
    public class Partido : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int escañosPartido;
        private string nombrePartido;

        public Rectangle Barra { get; }
        public TextBlock EtiquetaPartido { get; }

        public Partido(int escaños, string nombre)
        {
            this.escañosPartido = escaños;
            this.nombrePartido = nombre;

            Barra = new Rectangle
            {
                Width = 30,
            };

            EtiquetaPartido = new TextBlock
            {
                FontSize = 8,
                FontWeight = FontWeights.Bold,
                Text = nombre,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            ToolTip t = new ToolTip();
            t.Content = NombrePartido + " " + EscañosPartido.ToString();
            Barra.ToolTip = t;


            switch (nombre)
            {
                case "PP": Barra.Fill = new SolidColorBrush(Colors.Blue); break;
                case "PSOE": Barra.Fill = new SolidColorBrush(Colors.Red); break;
                case "VOX": Barra.Fill = new SolidColorBrush(Colors.Green); break;
                case "SUMAR": Barra.Fill = new SolidColorBrush(Colors.Pink); break;
                case "ERC": Barra.Fill = new SolidColorBrush(Colors.Yellow); break;
                case "JUNTS": Barra.Fill = new SolidColorBrush(Colors.Turquoise); break;
                case "EH_BILDU": Barra.Fill = new SolidColorBrush(Colors.GreenYellow); break;
                case "EAJ_PNV": Barra.Fill = new SolidColorBrush(Colors.MediumVioletRed); break;
                case "BNG": Barra.Fill = new SolidColorBrush(Colors.CornflowerBlue); break;
                case "CCA": Barra.Fill = new SolidColorBrush(Colors.ForestGreen); break;
                case "UPN": Barra.Fill = new SolidColorBrush(Colors.DarkBlue); break;
                case "PODEMOS": Barra.Fill = new SolidColorBrush(Colors.Purple); break;
                case "CS": Barra.Fill = new SolidColorBrush(Colors.Orange); break;
                case "MASPAIS": Barra.Fill = new SolidColorBrush(Colors.LightPink); break;
                case "CUP_PR": Barra.Fill = new SolidColorBrush(Colors.LightSalmon); break;
                case "OTROS": Barra.Fill = new SolidColorBrush(Colors.Gray); break;
                case "XAV": Barra.Fill = new SolidColorBrush(Colors.Gold); break;
                case "SY": Barra.Fill = new SolidColorBrush(Colors.Brown); break;
                case "UPL": Barra.Fill = new SolidColorBrush(Colors.Khaki); break;
            }
        }
        public string NombrePartido { get { return nombrePartido; } set { nombrePartido = value; OnPropertyChanged("NombrePartido"); } }
        public int EscañosPartido
        {
            get { return escañosPartido; }
            set
            {
                escañosPartido = value;
                OnPropertyChanged("EscañosPartido");
                ModificarBarra(FactorVentana);
            }
        }
        public double FactorVentana { get; set; }

        public void ModificarBarra(double factorVentana)
        {
            FactorVentana = factorVentana;
            Barra.Height = EscañosPartido * FactorVentana;
        }

        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

    }
}
