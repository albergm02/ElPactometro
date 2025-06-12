using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrabajoFinal_ELPACTOMETRO.Modelo;

namespace TrabajoFinal_ELPACTOMETRO
{
    /// <summary>
    /// Lógica de interacción para SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        private Proceso ProcesoSeleccion;
        private Partido PartidoSeleccion;
        private ObservableCollection<Proceso> ProcesoListas { get; set; }
        private int Mayoria = 0;

        private Proceso ListaUnica, ListaRestante;
        private ObservableCollection<Proceso> ListaSalidaP;

        public SecondWindow(ObservableCollection<Proceso> listaProcesos)
        {
            InitializeComponent();
            ProcesoListas = listaProcesos;
            TablaProcesos.ItemsSource = ProcesoListas;
        }

        public event EventHandler VentanaCerrada;
        public event EventHandler<ProcesoEventArgs> ProcesoCambiado;
        public event EventHandler<ProcesoEventArgs> ProcesoEliminado;
        public event EventHandler<ListaPEventArgs> ListaCGenerada;
        public event EventHandler<ListaPEventArgs> ListaPGenerada;
        // DIFERENCIA ENTRE LAS DOS: EN ESTA SE PASA DE FORMA DIRECTA: EXISTE UN PARTIDO CON MAYORIA ABSOLUTA //
        public event EventHandler<ListaPEventArgs> ListaPAutoGenerada;
        public event EventHandler<PartidoEventArgs> PartidoAñadido;

        private void ButtonCloseSW_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            OnWindowClosed();
        }


        private void ButtonFScreenSW_Click(object sender, RoutedEventArgs e)
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

        private void ButtonMinimizeSW_Click(object sender, RoutedEventArgs e)
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

        private void TablaProcesos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TablaProcesos.SelectedItems != null)
            {
                ProcesoSeleccion = (Proceso)TablaProcesos.SelectedItem;
                if (ProcesoSeleccion == null) return;
                TablaPartidos.ItemsSource = ProcesoSeleccion.ListaPartidos;
                botonComparador.IsEnabled = true;
                botonPactometro.IsEnabled = true;
                botonAñadirPartido.IsEnabled = true;
                OnProcesoCambiado(ProcesoSeleccion);
            }
            else
            {
                TablaPartidos.ItemsSource = null;
            }


        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            OnProcesoEliminado(ProcesoSeleccion);
        }

        private void OnProcesoEliminado(Proceso procesoSeleccion)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Está seguro de que quiere eliminar el proceso?", "Confirmacion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes)
            {
                ProcesoListas.Remove(procesoSeleccion);
                ProcesoEliminado?.Invoke(this, new ProcesoEventArgs(procesoSeleccion));
                procesoSeleccion.ListaPartidos.Clear();
                TablaProcesos.SelectedItem = null;
            }
            else return;

        }

        internal void OnWindowClosed()
        {
            VentanaCerrada(this, new EventArgs());
        }
        internal void OnProcesoCambiado(Proceso proceso)
        {
            ProcesoCambiado(this, new ProcesoEventArgs(proceso));
            Mayoria = 0;
            if (proceso.EsAutonomico) Mayoria = 176;
            else Mayoria = 41;
        }

        private void BotonComparador_Click(object sender, RoutedEventArgs e)
        {

            ModoComparativo modoComparativo = new ModoComparativo(ProcesoListas, (Proceso)ProcesoSeleccion);
            modoComparativo.ListaCGenerada += ModoComparativo_ListaCGenerada;
            modoComparativo.ShowDialog();

        }

        private void ModoComparativo_ListaCGenerada(object sender, ListaPEventArgs e)
        {
            ListaCGenerada?.Invoke(this, e);
        }

        private void BotonPactometro_Click(object sender, RoutedEventArgs e)
        {

            ListaUnica = new Proceso(ProcesoSeleccion.NombreProceso, ProcesoSeleccion.FechaProceso);
            ListaRestante = new Proceso(ProcesoSeleccion.NombreProceso, ProcesoSeleccion.FechaProceso);
            ListaSalidaP = new ObservableCollection<Proceso>();

            PartidoSeleccion = ComprobarSiExisteMayoriaDe1Partido(ProcesoSeleccion);

            if (PartidoSeleccion != null)
            {
                ListaUnica.ListaPartidos.Add(PartidoSeleccion);
                ListaSalidaP.Add(ListaUnica);
                foreach (Partido p in ProcesoSeleccion.ListaPartidos)
                {
                    ListaRestante.ListaPartidos.Add(p);
                }
                ListaRestante.ListaPartidos.Remove(PartidoSeleccion);
                ListaSalidaP.Add(ListaRestante);
                OnListaAutomaticaGenerada(ListaSalidaP);
            }


            else
            {
                if (ProcesoSeleccion.ListaPartidos.Count > 1)
                {
                    MessageBox.Show("Se va a proceder a abrir una ventana " +
                    "para elección debido a que ningun partido supera la mayoría.\n", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    ModoPactometro modoPactometro = new ModoPactometro((Proceso)ProcesoSeleccion);
                    modoPactometro.ListaPGenerada += ModoPactometro_ListaPGenerada;
                    modoPactometro.Show();
                }
                else
                {
                    MessageBox.Show("Error: no existen al menos 2 partidos en el proceso.", "Error de proceso.", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void OnListaAutomaticaGenerada(ObservableCollection<Proceso> listaAutomatica)
        {
            ListaPAutoGenerada?.Invoke(this, new ListaPEventArgs(listaAutomatica));
        }

        private void ModoPactometro_ListaPGenerada(object sender, ListaPEventArgs e)
        {
            ListaPGenerada?.Invoke(this, e);
        }

        private void BotonAñadirProceso_Click(object sender, RoutedEventArgs e)
        {
            AñadirProceso addProceso = new AñadirProceso(ProcesoListas);
            addProceso.ProcesoAñadido += AddProceso_ProcesoAñadido;
            addProceso.ShowDialog();
        }

        private void AddProceso_ProcesoAñadido(object sender, ProcesoEventArgs e)
        {
            TablaProcesos.ScrollIntoView(e.MiProceso);
            TablaProcesos.SelectedItem = e.MiProceso;
        }

        private void BotonAñadirPartido_Click(object sender, RoutedEventArgs e)
        {
            AñadirPartido addPartido = new AñadirPartido(ProcesoSeleccion);
            addPartido.PartidoAñadido += AddPartido_PartidoAñadido;
            addPartido.ShowDialog();
        }

        private void AddPartido_PartidoAñadido(object sender, PartidoEventArgs e)
        {
            PartidoAñadido?.Invoke(this, e);
        }

        private Partido ComprobarSiExisteMayoriaDe1Partido(Proceso proceso)
        {
            if (proceso.EsAutonomico)
            {
                foreach (Partido p in proceso.ListaPartidos)
                {
                    if (p.EscañosPartido >= 41)
                    {
                        return p;
                    }
                }
            }
            else
            {
                foreach (Partido p in proceso.ListaPartidos)
                {
                    if (p.EscañosPartido >= 176) return p;
                }
            }
            return null;
        }
    }
}
