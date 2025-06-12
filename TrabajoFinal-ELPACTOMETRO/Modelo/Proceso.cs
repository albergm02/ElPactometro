using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml;

namespace TrabajoFinal_ELPACTOMETRO.Modelo
{
    public class Proceso : INotifyPropertyChanged
    {
        private string nombreProceso, fechaProceso;
        private ObservableCollection<Partido> listaPartidos;
        private Boolean esAutonomico;
        private int mayoriaAbsoluta, numeroEscañosTotales;

        public TextBlock EtiquetaProceso { get; set; }

        public Proceso(string nombre, string fecha)
        {
            this.nombreProceso = nombre;
            this.fechaProceso = fecha;
            listaPartidos = new ObservableCollection<Partido>();

            switch (nombre)
            {
                case "ELECCIONES GENERALES": esAutonomico = false; mayoriaAbsoluta = 176; break;
                case "ELECCIONES AUTONÓMICAS": esAutonomico = true; mayoriaAbsoluta = 71; break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public int MayoriaAbsoluta { get { return mayoriaAbsoluta; } set { mayoriaAbsoluta = value; OnPropertyChanged("MayoriaAbsoluta"); } }
        public int NumeroEscañosTotales
        {
            get { return numeroEscañosTotales; }
            set
            {
                numeroEscañosTotales = value;
                OnPropertyChanged("NumeroEscañosTotales");
            }
        }
        public string NombreProceso { get { return nombreProceso; } set { nombreProceso = value; OnPropertyChanged("NombreProceso"); } }
        public string FechaProceso { get { return fechaProceso; } set { fechaProceso = value; OnPropertyChanged("FechaProceso"); } }
        public Boolean EsAutonomico { get { return esAutonomico; } set { esAutonomico = value; OnPropertyChanged("EsAutonomico"); } }
        public ObservableCollection<Partido> ListaPartidos { get { return listaPartidos; } set { listaPartidos = value; OnPropertyChanged("ListaPartidos"); } }



        public static ObservableCollection<Proceso> UsingXmlDocumentWithXPath(string path)
        {
            ObservableCollection<Proceso> listaProcesos = new ObservableCollection<Proceso>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            XmlNodeList itemNodes = xmlDocument.SelectNodes("//Pactometro//Procesos//Proceso");

            foreach (XmlNode itemNode in itemNodes)
            {
                string nombre = itemNode.Attributes["nombreProceso"].Value;
                string fecha = itemNode.Attributes["fechaProceso"].Value;
                Proceso nuevoProceso = new Proceso(nombre, fecha);

                foreach (XmlNode item in itemNode.SelectSingleNode("partidos"))
                {
                    string nombrePartido = item.Attributes["nombrePartido"].Value;
                    int escaños = int.Parse(item.Attributes["escañosPartido"].Value);

                    Partido nuevoPartido = new Partido(escaños, nombrePartido);
                    nuevoProceso.ListaPartidos.Add(nuevoPartido);
                }

                listaProcesos.Add(nuevoProceso);
            }
            return listaProcesos;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ActualizarEscaños()
        {
            int total = 0;
            foreach (Partido p in listaPartidos)
            {
                total += p.EscañosPartido;
            }
            NumeroEscañosTotales = total;
        }

    }


}
