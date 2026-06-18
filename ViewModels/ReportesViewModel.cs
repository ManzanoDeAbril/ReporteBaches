using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Requerido para usar ObservableCollection
using System.IO; // Requerido para manipulación de archivos y carpetas (Path, Stream, File)
using System.Threading.Tasks;
using System.Diagnostics; // Permite usar 'Debug.WriteLine' para enviar mensajes a la consola de depuración
using System.Linq; // Permite buscar elementos en colecciones (FirstOrDefault)
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReporteBaches.app.Models;

namespace ReporteBaches.app.ViewModels;

public partial class ReportesViewModel : ObservableObject
{
    // 'ObservableCollection' es una lista especial de C# que le avisa al control List/CollectionView
    // de la pantalla XAML que se agregaron o eliminaron elementos para actualizar la vista de inmediato.
    public static ObservableCollection<ReporteModel> ReportesRealizados { get; } = new();

    [ObservableProperty]
    public partial string? Descripcion { get; set; }

    [ObservableProperty]
    public partial string? RutaImagen { get; set; }

    [ObservableProperty]
    public partial string CoordenadasTexto { get; set; } = "Ubicación GPS: No obtenida";

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    // Campos privados para almacenar latitud y longitud numéricas
    private double _latitud;
    private double _longitud;

    /// <summary>
    /// Abre la cámara del dispositivo para capturar la foto del bache.
    /// </summary>
    [RelayCommand]
    private async Task TomarFotoAsync()
    {
        try
        {
            // 'MediaPicker.Default.IsCaptureSupported' verifica si el celular o PC tiene una cámara física activa
            if (MediaPicker.Default.IsCaptureSupported)
            {
                // 'CapturePhotoAsync()' dispara la cámara nativa del sistema operativo (Android/Windows)
                // y retorna un objeto 'FileResult' que representa la foto tomada temporalmente.
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // 'FileSystem.CacheDirectory' es la carpeta segura de almacenamiento temporal de la app.
                    // 'Path.Combine' junta la ruta del directorio con el nombre de archivo de forma segura.
                    string rutaLocal = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    // Copiamos la foto del buffer temporal a nuestro archivo permanente local
                    using Stream sourceStream = await photo.OpenReadAsync(); // Abre flujo de lectura
                    using FileStream localFileStream = File.OpenWrite(rutaLocal); // Abre flujo de escritura local
                    await sourceStream.CopyToAsync(localFileStream); // Copia el archivo en segundo plano

                    // Actualiza la propiedad observable para que se dibuje la imagen en la pantalla
                    RutaImagen = rutaLocal;
                }
            }
            else
            {
                // Si estamos en un emulador o PC sin webcam, abrimos el selector de galería para pruebas
                await Shell.Current.DisplayAlertAsync("Cámara no disponible", "Abriendo galería de fotos...", "Aceptar");

                // 'PickPhotosAsync()' abre la galería del dispositivo para seleccionar archivos
                var photos = await MediaPicker.Default.PickPhotosAsync();

                // '.FirstOrDefault()' toma la primera foto seleccionada de la lista (o null si canceló)
                var photo = photos?.FirstOrDefault();
                if (photo != null)
                {
                    RutaImagen = photo.FullPath; // Asigna la ruta de la foto seleccionada
                }
            }
        }
        catch (Exception ex)
        {
            // Registra el error en la consola de Visual Studio Insiders para el programador
            Debug.WriteLine($"Error de cámara: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Cámara", "Permiso de cámara no concedido o acción cancelada.", "Aceptar");
        }
    }

    /// <summary>
    /// Consulta el sensor GPS para guardar la ubicación exacta del bache.
    /// </summary>
    [RelayCommand]
    private async Task ObtenerUbicacionAsync()
    {
        try
        {
            IsBusy = true;
            CoordenadasTexto = "Obteniendo datos del GPS...";

            // 'GeolocationAccuracy.Medium' establece una precisión intermedia (suficiente y ahorra batería)
            // 'TimeSpan.FromSeconds(10)' limita la consulta a máximo 10 segundos de espera
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            // Llama a la API GPS de MAUI en segundo plano
            Location? location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
            {
                // Guarda las coordenadas decimales
                _latitud = location.Latitude;
                _longitud = location.Longitude;

                // ':F6' formatea el número decimal a exactamente 6 dígitos después de la coma
                CoordenadasTexto = $"Lat: {_latitud:F6}, Lon: {_longitud:F6}";
            }
            else
            {
                CoordenadasTexto = "Ubicación GPS apagada o sin señal.";
            }
        }
        catch (PermissionException)
        {
            // Este bloque 'catch' específico se dispara si el usuario no autorizó a la app a usar el GPS
            await Shell.Current.DisplayAlertAsync("Permisos", "Activa el GPS y permite que la app acceda a tu ubicación.", "Aceptar");
        }
        catch (Exception ex)
        {
            // Atrapa cualquier otro error genérico de lectura del sensor
            CoordenadasTexto = "Error al intentar usar el GPS.";
            Debug.WriteLine($"Error GPS: {ex.Message}");
        }
        finally
        {
            IsBusy = false; // Desactiva el ActivityIndicator
        }
    }

    /// <summary>
    /// Valida el reporte, lo añade a la lista y le otorga +50 puntos de ciudadano al usuario.
    /// </summary>
    [RelayCommand]
    private async Task EnviarReporteAsync()
    {
        // Si no se ha capturado una foto, se cancela el proceso
        if (string.IsNullOrEmpty(RutaImagen))
        {
            await Shell.Current.DisplayAlertAsync("Foto Faltante", "Toma una foto del bache para continuar.", "Aceptar");
            return;
        }

        // Si no se ha consultado el GPS, se cancela el proceso
        if (_latitud == 0 && _longitud == 0)
        {
            await Shell.Current.DisplayAlertAsync("Ubicación Faltante", "Obtén la ubicación GPS antes de enviar.", "Aceptar");
            return;
        }

        try
        {
            IsBusy = true;
            await Task.Delay(1500); // Simulamos la subida de datos a la base de datos (1.5 segundos)

            // Creamos una nueva instancia de ReporteModel con los datos del formulario
            var nuevoReporte = new ReporteModel
            {
                // Si la descripción está vacía, le asignamos un texto genérico por defecto
                Descripcion = string.IsNullOrWhiteSpace(Descripcion) ? "Daño vial reportado" : Descripcion,
                Latitud = _latitud,
                Longitud = _longitud,
                RutaImagenLocal = RutaImagen
            };

            // '.Insert(0, nuevoReporte)' añade el reporte al principio de la lista (en el índice 0)
            // para que los reportes nuevos aparezcan arriba en la pantalla de historial.
            ReportesRealizados.Insert(0, nuevoReporte);

            // Sumamos los puntos al usuario logueado en la sesión global
            if (App.UsuarioActual != null)
            {
                App.UsuarioActual.PuntosAcumulados += 50; // Suma 50 puntos
            }

            await Shell.Current.DisplayAlertAsync("¡Gracias!", "Reporte recibido correctamente. Ganaste +50 puntos.", "Aceptar");

            // Limpiamos los campos del formulario para permitir ingresar otro reporte nuevo
            Descripcion = string.Empty;
            RutaImagen = null;
            CoordenadasTexto = "Ubicación GPS: No obtenida";
            _latitud = 0;
            _longitud = 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo subir: {ex.Message}", "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
}