using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TrabajoFinal_ELPACTOMETRO.Modelo;

namespace TrabajoFinal_ELPACTOMETRO
{
    /// <summary>
    /// Lógica de interacción para ModoPactometro.xaml
    /// </summary>
    public partial class ModoPactometro : Window
    {
        private ObservableCollection<Proceso> ListaSalidaP;
        private ObservableCollection<Partido> ListaFiltrada1, ListaFiltrada2;
        private Partido PartidoSeleccionado;
        private Proceso ProcesoSalida1, ProcesoSalida2;
        private int MayoriaEscaños, ContadorL1, ContadorL2;

        public ModoPactometro(Proceso procesoSeleccion)
        {

            InitializeComponent();

            if (procesoSeleccion.NombreProceso == "ELECCIONES GENERALES")
                MayoriaEscaños = 176;
            else
                MayoriaEscaños = 41;

            textoPactometro.Text = "EL PACTÓMETRO: " + procesoSeleccion.NombreProceso + " " + procesoSeleccion.FechaProceso;

            ListaFiltrada1 = new ObservableCollection<Partido>();
            ListaFiltrada2 = new ObservableCollection<Partido>();
            ListaSalidaP = new ObservableCollection<Proceso>();

            foreach (Partido p in procesoSeleccion.ListaPartidos)
            {
                Partido instanciaPartido = p;
                ContadorL1 += instanciaPartido.EscañosPartido;
                ListaFiltrada1.Add(instanciaPartido);
            }

            ActualizarTexto(ContadorL1, ContadorL2);

            ProcesoSalida1 = new Proceso(procesoSeleccion.NombreProceso + "A FAVOR", procesoSeleccion.FechaProceso);
            ProcesoSalida2 = new Proceso(procesoSeleccion.NombreProceso + "EN CONTRA", procesoSeleccion.FechaProceso);

            TablaPartidos1.ItemsSource = ListaFiltrada1;
            TablaPartidos2.ItemsSource = ListaFiltrada2;

            ListaFiltrada1.CollectionChanged += ListaFiltrada1_CollectionChanged;
            ListaFiltrada2.CollectionChanged += ListaFiltrada2_CollectionChanged;


        }

        public event EventHandler<ListaPEventArgs> ListaPGenerada;

        private void ListaFiltrada2_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerarPactometro.IsEnabled = ListaFiltrada1.Count > 0 && ListaFiltrada2.Count > 0;
        }

        private void ListaFiltrada1_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerarPactometro.IsEnabled = ListaFiltrada1.Count > 0 && ListaFiltrada2.Count > 0;
        }

        private void ButtonClosePC_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }


        private void ButtonFScreenPC_Click(object sender, RoutedEventArgs e)
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

        private void ButtonMinimizePC_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TablaPartidos2_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TablaPartidos2.SelectedItems != null && TablaPartidos2.SelectedItems.Count > 0)
            {
                TablaPartidos1.SelectedItems.Clear();
                MoverATabla1.IsEnabled = true;
                MoverATabla2.IsEnabled = false;
                PartidoSeleccionado = (Partido)TablaPartidos2.SelectedItem;
            }
            else
            {
                MoverATabla2.IsEnabled = false;
            }
        }

        private void TablaPartidos1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TablaPartidos1.SelectedItems != null && TablaPartidos1.SelectedItems.Count > 0)
            {
                TablaPartidos2.SelectedItems.Clear();
                MoverATabla2.IsEnabled = true;
                MoverATabla1.IsEnabled = false;
                PartidoSeleccionado = (Partido)TablaPartidos1.SelectedItem;
            }
            else
            {
                MoverATabla1.IsEnabled = false;
            }
        }

        private void GenerarPactometro_Click(object sender, RoutedEventArgs e)
        {
            if (MayoriaEscaños == 176)
            {
                if (ContadorL1 < 200 && ContadorL2 < 200 && (ContadorL1 >= 176 || ContadorL2 >= 176))
                {
                    foreach (Partido p1 in ListaFiltrada1)
                    {
                        ProcesoSalida1.ListaPartidos.Add(p1);
                    }

                    ListaSalidaP.Add(ProcesoSalida1);

                    foreach (Partido p2 in ListaFiltrada2)
                    {
                        ProcesoSalida2.ListaPartidos.Add(p2);
                    }

                    ListaSalidaP.Add(ProcesoSalida2);

                    OnListaPGenerada(ListaSalidaP);
                    Hide();
                }
                else
                {
                    MessageBox.Show("La cifra de uno de los bandos supera la permitida (200) " +
                        "o ninguno de los bandos alcanza la mayoría (176)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (MayoriaEscaños == 41)
            {
                if (ContadorL1 < 70 && ContadorL2 < 70 && (ContadorL1 >= 41 || ContadorL2 >= 41))
                {
                    foreach (Partido p1 in ListaFiltrada1)
                    {
                        ProcesoSalida1.ListaPartidos.Add(p1);
                    }

                    ListaSalidaP.Add(ProcesoSalida1);

                    foreach (Partido p2 in ListaFiltrada2)
                    {
                        ProcesoSalida2.ListaPartidos.Add(p2);
                    }

                    ListaSalidaP.Add(ProcesoSalida2);

                    OnListaPGenerada(ListaSalidaP);
                    Hide();
                }
                else
                {
                    MessageBox.Show("La cifra de uno de los bandos supera la permitida (70) " +
                        "o ninguno de los bandos alcanza la mayoría (41)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        private void OnListaPGenerada(ObservableCollection<Proceso> listaPactometro)
        {
            ListaPGenerada?.Invoke(this, new ListaPEventArgs(listaPactometro));
        }

        private void MoverATabla2_Click(object sender, RoutedEventArgs e)
        {
            if (PartidoSeleccionado != null)
            {
                ListaFiltrada2.Add(PartidoSeleccionado);
                ContadorL1 -= PartidoSeleccionado.EscañosPartido;
                ListaFiltrada1.Remove(PartidoSeleccionado);
                ContadorL2 += PartidoSeleccionado.EscañosPartido;

                ActualizarTexto(ContadorL1, ContadorL2);
            }

            PartidoSeleccionado = null;
        }

        private void TablaPartidos2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PartidoSeleccionado != null)
            {
                ListaFiltrada1.Add(PartidoSeleccionado);
                ContadorL1 += PartidoSeleccionado.EscañosPartido;
                ListaFiltrada2.Remove(PartidoSeleccionado);
                ContadorL2 -= PartidoSeleccionado.EscañosPartido;

                ActualizarTexto(ContadorL1, ContadorL2);
            }
            PartidoSeleccionado = null;

        }


        private void TablaPartidos1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PartidoSeleccionado != null)
            {
                ListaFiltrada2.Add(PartidoSeleccionado);
                ContadorL1 -= PartidoSeleccionado.EscañosPartido;
                ListaFiltrada1.Remove(PartidoSeleccionado);
                ContadorL2 += PartidoSeleccionado.EscañosPartido;

                ActualizarTexto(ContadorL1, ContadorL2);
            }
            PartidoSeleccionado = null;

        }

        private void MoverATabla1_Click(object sender, RoutedEventArgs e)
        {
            if (PartidoSeleccionado != null)
            {
                ListaFiltrada1.Add(PartidoSeleccionado);
                ContadorL1 += PartidoSeleccionado.EscañosPartido;
                ListaFiltrada2.Remove(PartidoSeleccionado);
                ContadorL2 -= PartidoSeleccionado.EscañosPartido;

                ActualizarTexto(ContadorL1, ContadorL2);
                PartidoSeleccionado = null;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ActualizarTexto(int Contador1, int Contador2)
        {
            Escaños1.Text = "Escaños totales bando 1: " + ContadorL1.ToString();
            Escaños2.Text = "Escaños totales bando 2: " + ContadorL2.ToString();
        }


    }
}
