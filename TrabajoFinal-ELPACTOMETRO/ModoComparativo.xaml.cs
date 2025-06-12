using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrabajoFinal_ELPACTOMETRO.Modelo;

namespace TrabajoFinal_ELPACTOMETRO
{
    /// <summary>
    /// Lógica de interacción para ModoComparativo.xaml
    /// </summary>
    public partial class ModoComparativo : Window
    {
        private ObservableCollection<Proceso> listaSalida;
        private ObservableCollection<Proceso> listaFiltrada;
        private Proceso ProcesoSeleccionado;
        private Proceso ProcesoElemental;

        public event EventHandler<ListaPEventArgs> ListaCGenerada;

        public ModoComparativo(ObservableCollection<Proceso> listaProcesos, Proceso procesoSeleccionado)
        {
            InitializeComponent();
            ProcesoElemental = procesoSeleccionado;
            listaProcesos = FiltrarProcesos(listaProcesos, procesoSeleccionado);
            listaSalida = new ObservableCollection<Proceso> { ProcesoElemental };
            TablaProcesosSeleccion.ItemsSource = listaProcesos;
            TablaProcesosSalida.ItemsSource = listaSalida;

            listaSalida.CollectionChanged += ListaSalida_CollectionChanged;
        }

        private ObservableCollection<Proceso> FiltrarProcesos(ObservableCollection<Proceso> procesoLista, Proceso procesoSeleccionado)
        {
            listaFiltrada = new ObservableCollection<Proceso>();

            if (procesoSeleccionado.EsAutonomico)
            {
                foreach (Proceso proceso in procesoLista)
                {
                    if (proceso.EsAutonomico && procesoSeleccionado != proceso) listaFiltrada.Add(proceso);
                }
            }
            else if (!procesoSeleccionado.EsAutonomico)
            {
                foreach (Proceso proceso in procesoLista)
                {
                    if (!proceso.EsAutonomico && procesoSeleccionado != proceso) listaFiltrada.Add(proceso);
                }
            }
            return listaFiltrada;
        }


        private void ButtonCloseMC_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }


        private void ButtonFScreenMC_Click(object sender, RoutedEventArgs e)
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

        private void ButtonMinimizeMC_Click(object sender, RoutedEventArgs e)
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

        private void AgregarSeleccion_Click(object sender, RoutedEventArgs e)
        {
            listaSalida.Add(ProcesoSeleccionado);
            listaFiltrada.Remove(ProcesoSeleccionado);
        }

        private void EliminarSeleccion_Click(object sender, RoutedEventArgs e)
        {
            if (ProcesoSeleccionado != ProcesoElemental)
            {
                listaSalida.Remove(ProcesoSeleccionado);
                listaFiltrada.Add(ProcesoSeleccionado);
            }
            else
            {
                MessageBox.Show("No se puede eliminar el elemento actual debido a que es el elemental", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void GenerarGrafico_Click(object sender, RoutedEventArgs e)
        {
            OnListaCGenerada(listaSalida);
            Hide();
        }

        private void OnListaCGenerada(ObservableCollection<Proceso> listaProcesos)
        {
            ListaCGenerada?.Invoke(this, new ListaPEventArgs(listaProcesos));
        }

        private void TablaProcesosSeleccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablaProcesosSeleccion.SelectedItems != null && TablaProcesosSeleccion.SelectedItems.Count > 0)
            {
                TablaProcesosSalida.SelectedItems.Clear();
                AgregarSeleccion.IsEnabled = true;
                EliminarSeleccion.IsEnabled = false;
                ProcesoSeleccionado = (Proceso)TablaProcesosSeleccion.SelectedItem;
            }
            else
            {
                AgregarSeleccion.IsEnabled = false;
            }

        }

        private void TablaProcesosSalida_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablaProcesosSalida.SelectedItems != null && TablaProcesosSalida.SelectedItems.Count > 0)
            {
                TablaProcesosSeleccion.SelectedItems.Clear();
                EliminarSeleccion.IsEnabled = true;
                AgregarSeleccion.IsEnabled = false;
                ProcesoSeleccionado = (Proceso)TablaProcesosSalida.SelectedItem;

            }
            else
            {
                EliminarSeleccion.IsEnabled = false;
            }
        }

        private void ListaSalida_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GenerarGrafico.IsEnabled = listaSalida.Count > 1;
        }
    }
}
