using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrabajoFinal_ELPACTOMETRO.Modelo;

namespace TrabajoFinal_ELPACTOMETRO
{
    /// <summary>
    /// Lógica de interacción para AñadirProceso.xaml
    /// </summary>
    public partial class AñadirProceso : Window
    {
        private ObservableCollection<Proceso> ProcesoListas;
        private string nombreProceso;

        public AñadirProceso(ObservableCollection<Proceso> listaProcesos)
        {
            InitializeComponent();

            ProcesoListas = listaProcesos;
            PickerFecha.SelectedDate = DateTime.Today;

        }

        public event EventHandler<ProcesoEventArgs> ProcesoAñadido;

        public void OnProcesoAñadido(Proceso proceso)
        {
            ProcesoAñadido(this, new ProcesoEventArgs(proceso));
        }
        private void ButtonAPClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
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
            DateTime? selectedDate = PickerFecha.SelectedDate;

            if (selectedDate.HasValue && NombreProceso.SelectedItem != null)
            {
                nombreProceso = ((ComboBoxItem)NombreProceso.SelectedItem).Content.ToString();

                Proceso nuevoProceso = new Proceso(nombreProceso, selectedDate.Value.ToString("yyyy/MM/dd"));
                ProcesoListas.Add(nuevoProceso);
                OnProcesoAñadido(nuevoProceso);
            }
            else
            {
                DialogResult = false;
                MessageBox.Show("Debes seleccionar un proceso antes de aceptar.");
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
