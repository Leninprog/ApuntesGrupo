using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ApuntesGrupo.Models;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;

namespace ApuntesGrupo.ViewModels
{
    public class RecordatoriosViewModel : INotifyPropertyChanged
    {
        private string _nuevoTexto = string.Empty;
        private TimeSpan _nuevaHora = TimeSpan.Zero;
        private bool _nuevoActivo = true;

        public string NuevoTexto
        {
            get => _nuevoTexto;
            set { _nuevoTexto = value; OnPropertyChanged(); }
        }

        public TimeSpan NuevaHora
        {
            get => _nuevaHora;
            set { _nuevaHora = value; OnPropertyChanged(); }
        }

        public bool NuevoActivo
        {
            get => _nuevoActivo;
            set { _nuevoActivo = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Recordatorio> Recordatorios { get; set; } = new();

        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "recordatorios.json");

        public ICommand AgregarCommand { get; }
        public ICommand EliminarCommand { get; }

        public RecordatoriosViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EliminarCommand = new Command<Recordatorio>(Eliminar);
            Cargar();
        }

        public void Agregar()
        {
            if (string.IsNullOrWhiteSpace(NuevoTexto))
                return;

            var recordatorio = new Recordatorio
            {
                Texto = NuevoTexto,
                FechaHora = NuevaHora,
                Activo = NuevoActivo
            };

            Recordatorios.Add(recordatorio);
            Guardar();

            // Reset form
            NuevoTexto = string.Empty;
            NuevaHora = TimeSpan.Zero;
            NuevoActivo = true;
        }

        public void Eliminar(Recordatorio r)
        {
            if (Recordatorios.Contains(r))
            {
                Recordatorios.Remove(r);
                Guardar();
            }
        }

        public void Cargar()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var lista = JsonConvert.DeserializeObject<List<Recordatorio>>(json);
                if (lista != null)
                {
                    Recordatorios.Clear();
                    foreach (var r in lista)
                        Recordatorios.Add(r);
                }
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Recordatorios, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
