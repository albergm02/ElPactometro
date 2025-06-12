using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrabajoFinal_ELPACTOMETRO.Modelo;

namespace TrabajoFinal_ELPACTOMETRO
{
    /// <summary>
    /// Lógica de interacción para AñadirPartido.xaml
    /// </summary>
    public partial class AñadirPartido : Window
    {
        private readonly Proceso ListaProceso;


        public AñadirPartido(Proceso procesoSeleccion)
        {
            InitializeComponent();
            ListaProceso = procesoSeleccion;
        }

        public event EventHandler<PartidoEventArgs> PartidoAñadido;
        private void ButtonAPClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ButtonAPFScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void ButtonAPMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            int contador = 0;
            if (comboPartidos.SelectedItem == null)
            {
                DialogResult = false;
                MessageBox.Show("Debe de seleccionar qué partido quiere añadir");
            }

            if (txtNumero.Text == null)
            {
                DialogResult = false;
                MessageBox.Show("Debes añadir un número de partidos antes de aceptar.");
            }

            if (int.TryParse(txtNumero.Text, out int escaños))
            {
                foreach (Partido p in ListaProceso.ListaPartidos)
                {
                    contador += p.EscañosPartido;
                    if (p.NombrePartido == ((ComboBoxItem)comboPartidos.SelectedItem).Content.ToString())
                    {
                        DialogResult = false;
                        MessageBox.Show("Este partido ya existe en los procesos.");
                        return;
                    }
                }

                if (ListaProceso.EsAutonomico)
                {
                    if (contador + escaños > 81)
                    {
                        DialogResult = false;
                        MessageBox.Show("Se ha superado el numero máximo de escaños, el máximo es." + (81 - contador));
                    }
                    else
                    {
                        Partido nuevoPartido = new Partido(int.Parse(txtNumero.Text), ((ComboBoxItem)comboPartidos.SelectedItem).Content.ToString());
                        ListaProceso.ListaPartidos.Add(nuevoPartido);
                        OnPartidoAñadido(nuevoPartido);
                    }
                }
                else
                {
                    if (contador + escaños > 350)
                    {
                        DialogResult = false;
                        MessageBox.Show("Se ha superado el numero máximo de escaños, el máximo es." + (350 - contador));

                    }
                    else
                    {
                        Partido nuevoPartido = new Partido(int.Parse(txtNumero.Text), ((ComboBoxItem)comboPartidos.SelectedItem).Content.ToString());
                        ListaProceso.ListaPartidos.Add(nuevoPartido);
                        OnPartidoAñadido(nuevoPartido);
                    }

                }
            }
            else
            {
                DialogResult = false;
                MessageBox.Show("Numero incorrecto");
                return;
            }



        }

        private void OnPartidoAñadido(Partido nuevoPartido)
        {
            PartidoAñadido(this, new PartidoEventArgs(nuevoPartido));
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TxtNumero_GotFocus(object sender, RoutedEventArgs e)
        {
            txtNumero.Text = string.Empty;
        }
    }
}
