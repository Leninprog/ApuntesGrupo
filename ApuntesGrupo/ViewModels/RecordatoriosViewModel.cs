using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApuntesGrupo.Models;

namespace ApuntesGrupo.ViewModels;


public class RecordatoriosViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Recordatorio> Recordatorios { get; set; } = new();
    private string filePath = Path.Combine(FileSystem.AppDataDirectory, "recordatorios.json");

    public Command AgregarCommand { get; }
    public Command<Recordatorio> EliminarCommand { get; }

    public RecordatoriosViewModel()
    {
        AgregarCommand = new Command(() => Agregar());
        EliminarCommand = new Command<Recordatorio>((r) => Eliminar(r));
        Cargar();
    }

    public void Agregar()
    {
        var nuevo = new Recordatorio
        {
            Texto = "Nuevo recordatorio",
            FechaHora = TimeSpan.FromHours(12),
            Activo = true
        };
        Recordatorios.Add(nuevo);
        Guardar();
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
}
