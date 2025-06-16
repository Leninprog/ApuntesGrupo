using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApuntesGrupo.Models;

namespace ApuntesGrupo.ViewModels;

public class RecordatoriosViewModel : INotifyPropertyChanged
{
    private string _nuevoTexto = "";
    public string NuevoTexto
    {
        get => _nuevoTexto;
        set
        {
            if (_nuevoTexto != value)
            {
                _nuevoTexto = value;
                OnPropertyChanged(nameof(NuevoTexto));
            }
        }
    }

    private TimeSpan _nuevaHora = DateTime.Now.TimeOfDay;
    public TimeSpan NuevaHora
    {
        get => _nuevaHora;
        set
        {
            if (_nuevaHora != value)
            {
                _nuevaHora = value;
                OnPropertyChanged(nameof(NuevaHora));
            }
        }
    }

    private bool _nuevoActivo = true;
    public bool NuevoActivo
    {
        get => _nuevoActivo;
        set
        {
            if (_nuevoActivo != value)
            {
                _nuevoActivo = value;
                OnPropertyChanged(nameof(NuevoActivo));
            }
        }
    }

    public ObservableCollection<Recordatorio> Recordatorios { get; set; } = new();
    private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "recordatorios.json");

    public Command AgregarCommand { get; }
    public Command<Recordatorio> EliminarCommand { get; }

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

        var nuevo = new Recordatorio
        {
            Texto = NuevoTexto,
            FechaHora = NuevaHora,
            Activo = NuevoActivo
        };

        Recordatorios.Add(nuevo);
        Guardar();

        NuevoTexto = "";
        NuevaHora = DateTime.Now.TimeOfDay;
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
            var lista = JsonSerializer.Deserialize<List<Recordatorio>>(json);
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
        var json = JsonSerializer.Serialize(Recordatorios);
        File.WriteAllText(filePath, json);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
