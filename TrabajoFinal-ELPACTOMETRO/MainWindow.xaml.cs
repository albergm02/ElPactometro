using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TrabajoFinal_ELPACTOMETRO.Modelo;

namespace TrabajoFinal_ELPACTOMETRO
{
    // SECCIÓN DE EVENTOS //
    public class ProcesoEventArgs : EventArgs
    {
        public Proceso MiProceso { get; set; }

        public ProcesoEventArgs(Proceso proceso)
        {
            MiProceso = proceso;
        }
    }

    public class ListaPEventArgs : EventArgs
    {
        public ObservableCollection<Proceso> MiListaProcesos { get; set; }

        public ListaPEventArgs(ObservableCollection<Proceso> listaP)
        {
            MiListaProcesos = listaP;
        }
    }

    public class PartidoEventArgs : EventArgs
    {
        public Partido MiPartido { get; set; }

        public PartidoEventArgs(Partido partido)
        {
            MiPartido = partido;
        }
    }

    // SECCIÓN MAINPRINCIPAL //
    public partial class MainWindow : Window
    {

        private SecondWindow ventanaSecundaria;
        private readonly ObservableCollection<Proceso> listaProcesos = new ObservableCollection<Proceso>();
        readonly string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\Recursos\DatosProcesos.xml");
        private Proceso ProcesoSeleccion;

        public MainWindow()
        {
            listaProcesos = Proceso.UsingXmlDocumentWithXPath(path);
            ModificarBarras();
            ActualizarEscaños();
            InitializeComponent();

            ventanaSecundaria = new SecondWindow(listaProcesos);
            ventanaSecundaria.VentanaCerrada += VentanaSecundaria_VentanaCerrada;
            ventanaSecundaria.ProcesoCambiado += VentanaSecundaria_ProcesoCambiado;
            ventanaSecundaria.ListaCGenerada += VentanaSecundaria_ListaCGenerada;
            ventanaSecundaria.ListaPGenerada += VentanaSecundaria_ListaPGenerada;
            ventanaSecundaria.ListaPAutoGenerada += VentanaSecundaria_ListaPAutoGenerada;
            ventanaSecundaria.ProcesoEliminado += VentanaSecundaria_ProcesoEliminado;
            ventanaSecundaria.PartidoAñadido += VentanaSecundaria_PartidoAñadido;
            ventanaSecundaria.Show();

        }

        private void VentanaSecundaria_PartidoAñadido(object sender, PartidoEventArgs e)
        {
            DibujarGrafico1();
            ModificarBarras();
            ActualizarEscaños();
        }

        private void VentanaSecundaria_ProcesoEliminado(object sender, ProcesoEventArgs e)
        {
            LimpiarGrafico();
        }

        private void VentanaSecundaria_ListaPAutoGenerada(object sender, ListaPEventArgs e)
        {
            DibujarGrafico3(e.MiListaProcesos);
        }

        //////////////////////// Eventos de los procesos y las listas de procesos ////////////////////////

        private void VentanaSecundaria_ProcesoCambiado(object sender, ProcesoEventArgs e)
        {
            ProcesoSeleccion = e.MiProceso;
            ModificarBarras();
            DibujarGrafico1();
            tituloProceso.Text = ProcesoSeleccion.NombreProceso + " - " + ProcesoSeleccion.FechaProceso;
        }

        private void VentanaSecundaria_ListaPGenerada(object sender, ListaPEventArgs e)
        {
            ModificarBarras();
            DibujarGrafico3(e.MiListaProcesos);
        }

        private void VentanaSecundaria_ListaCGenerada(object sender, ListaPEventArgs e)
        {
            ModificarBarras();
            DibujarGrafico2(e.MiListaProcesos);
        }

        //


        //////////////////////// Eventos de ventana ////////////////////////

        private void VentanaSecundaria_VentanaCerrada(object sender, EventArgs e)
        {
            ButtonOpenSW.IsEnabled = true;
        }

        //

        //////////////////////// Eventos de ratón /////////////////////////

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //

        //////////////////////// Eventos de botones ///////////////////////
        private void ButtonFScreen_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                UIElement lineas = StackBarras.Children[0];
                lineas.Visibility = Visibility.Hidden;
                ModificarBarras();

            }
            else
            {
                this.WindowState = WindowState.Normal;
                UIElement lineas = StackBarras.Children[0];
                lineas.Visibility = Visibility.Visible;
                ModificarBarras();
            }


        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
            Application.Current.Shutdown();
        }

        private void ButtonOpenSW_Click(object sender, RoutedEventArgs e)
        {
            if (ventanaSecundaria == null)
            {
                ventanaSecundaria = new SecondWindow(listaProcesos);
            }
            ventanaSecundaria.Show();
            ButtonOpenSW.IsEnabled = false;
        }

        //


        //////////////////////// Tratamiento de gráficos //////////////////


        void DibujarGrafico1()
        {
            LimpiarGrafico();
            DibujarLineas();

            foreach (Partido partido in ProcesoSeleccion.ListaPartidos)
            {
                StackPanel sp = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(20, 0, 0, 20),
                };

                sp.Children.Add(partido.Barra);
                partido.EtiquetaPartido.Text = partido.NombrePartido;
                sp.Children.Add(partido.EtiquetaPartido);
                StackBarras.Children.Add(sp);
            }

        }

        void DibujarGrafico2(ObservableCollection<Proceso> listaProcesosAComparar)
        {
            LimpiarGrafico();
            DibujarLineas();
            foreach (Partido partido in listaProcesosAComparar.First().ListaPartidos)
            {
                StackPanel sp = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(20, 0, 0, 20),
                };

                sp.Children.Add(partido.Barra);
                partido.EtiquetaPartido.Text = partido.NombrePartido + " " + listaProcesosAComparar.First().FechaProceso;
                sp.Children.Add(partido.EtiquetaPartido);
                StackBarras.Children.Add(sp);

                foreach (Proceso proceso in listaProcesosAComparar.Skip(1))
                {
                    Partido partidoEnProceso = proceso.ListaPartidos.FirstOrDefault(p => p.NombrePartido == partido.NombrePartido);

                    if (partidoEnProceso != null)
                    {
                        StackPanel aux = new StackPanel
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Margin = new Thickness(20, 0, 0, 20),
                        };

                        aux.Children.Add(partidoEnProceso.Barra);
                        partidoEnProceso.EtiquetaPartido.Text = partido.NombrePartido + proceso.FechaProceso;
                        aux.Children.Add(partidoEnProceso.EtiquetaPartido);
                        StackBarras.Children.Add(aux);

                    }
                }
            }
        }

        void DibujarGrafico3(ObservableCollection<Proceso> listaPactometro)
        {
            LimpiarGrafico();
            DibujarLineasPactometro();
            tituloProceso.Text = "EL PACTÓMETRO: " + ProcesoSeleccion.FechaProceso;
            foreach (Proceso proceso in listaPactometro)
            {
                StackPanel sp = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(20, 0, 0, 20),
                };

                foreach (Partido p in proceso.ListaPartidos)
                {
                    sp.Children.Add(p.Barra);
                }

                StackBarras.Children.Add(sp);
            }
        }



        private void LimpiarGrafico()
        {
            foreach (StackPanel sp in StackBarras.Children)
            {
                sp.Children.Clear();
            }
            StackBarras.Children.Clear();

        }
        /// 


        //////////////////////// DIBUJO DE LINEAS ////////////////////

        private void DibujarLineas()
        {
            if (ProcesoSeleccion.ListaPartidos.Count <= 0) return;
            int contador;
            int valorSteps = 0;
            double escañosMax = ProcesoSeleccion.ListaPartidos.Max(x => x.EscañosPartido);
            double factorEsc = (StackBarras.ActualHeight - 200) / escañosMax;

            if (ProcesoSeleccion.EsAutonomico)
            {
                valorSteps = 5;
                contador = 10;
            }
            else
            {
                valorSteps = 20;
                contador = 20;
            }

            StackPanel spI = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(20, 0, 0, 33),
            };

            while (contador != 0)
            {

                Rectangle Barra = new Rectangle
                {
                    Width = 20,
                    Height = valorSteps * factorEsc,
                    Fill = new SolidColorBrush(Colors.Transparent),

                };

                TextBlock etiqueta = new TextBlock
                {
                    Text = (valorSteps * (contador - 1)).ToString() + "_",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                };


                Grid contenedor = new Grid();
                contenedor.Children.Add(Barra);
                contenedor.Children.Add(etiqueta);

                spI.Children.Add(contenedor);
                contador--;
            }

            StackBarras.Children.Add(spI);
        }

        private void DibujarLineasPactometro()
        {
            if (ProcesoSeleccion.ListaPartidos.Count <= 0) return;
            int contador;
            int valorSteps = 0;
            int valorMayoria;
            Boolean flagAlterno = false;
            double escañosMax = ProcesoSeleccion.ListaPartidos.Max(x => x.EscañosPartido);
            double factorEsc = (StackBarras.ActualHeight - 200) / escañosMax;

            if (ProcesoSeleccion.EsAutonomico)
            {
                valorSteps = 5;
                contador = 10;
                valorMayoria = 41;
            }
            else
            {
                valorSteps = 20;
                contador = 20;
                valorMayoria = 176;

            }

            StackPanel spI = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(20, 0, 0, 20),
            };

            while (contador != 0)
            {

                Rectangle Barra = new Rectangle
                {
                    Width = 20,
                    Height = valorSteps * factorEsc,

                };

                TextBlock etiqueta = new TextBlock
                {
                    Text = (valorSteps * (contador - 1)).ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                };

                if (flagAlterno == true)
                {
                    Barra.Fill = new SolidColorBrush(Colors.DarkGray);
                    flagAlterno = false;
                }
                else
                {
                    flagAlterno = true;
                    Barra.Fill = new SolidColorBrush(Colors.WhiteSmoke);
                }
                Grid contenedor = new Grid();
                contenedor.Children.Add(Barra);
                contenedor.Children.Add(etiqueta);

                spI.Children.Add(contenedor);
                contador--;
            }

            StackBarras.Children.Add(spI);

            StackPanel spMayoria = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(20, 0, 0, 20),
            };

            Rectangle BarraMayoria = new Rectangle
            {
                Width = 10,
                Height = valorMayoria * factorEsc,
                Fill = new SolidColorBrush(Colors.Green),

            };

            TextBlock etiquetaMayoria = new TextBlock
            {
                Text = valorMayoria.ToString(),
                Foreground = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
            };

            Grid contenedorMayoria = new Grid();
            contenedorMayoria.Children.Add(BarraMayoria);
            contenedorMayoria.Children.Add(etiquetaMayoria);

            spMayoria.Children.Add(contenedorMayoria);
            StackBarras.Children.Add(spMayoria);
        }

        //


        //////////////////////// Modificación de barras //////////////////

        private void ModificarBarras()
        {
            double escañosMax;
            double factorEsc = 0;

            if (ProcesoSeleccion != null && ProcesoSeleccion.ListaPartidos.Count > 0)
            {
                escañosMax = ProcesoSeleccion.ListaPartidos.Max(x => x.EscañosPartido);
                factorEsc = (StackBarras.ActualHeight - 200) / escañosMax;

                foreach (Proceso pro in listaProcesos)
                {
                    foreach (Partido p in pro.ListaPartidos)
                    {
                        p.ModificarBarra(factorEsc);
                    }
                }
            }
            else return;
        }


        //

        //////////////////////// Actualización del total de escaños //////////////////
        private void ActualizarEscaños()
        {
            foreach (Proceso pro in listaProcesos)
            {
                pro.ActualizarEscaños();
            }
        }

        //
    }
}
