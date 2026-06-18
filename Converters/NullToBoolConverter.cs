using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ReporteBaches.app.Converters;

/// <summary>
/// Implementa 'IValueConverter'. Convierte un objeto o ruta de imagen (que puede ser nula)
/// en un valor lógico (true/false) para mostrar u ocultar elementos en pantalla.
/// </summary>
public class NullToBoolConverter : IValueConverter
{
    /// <summary>
    /// De C# a la pantalla (XAML).
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool tieneValor = value != null; // Retorna true si el objeto tiene datos (no es nulo)

        // Si en XAML escribimos ConverterParameter=invert, invertimos el resultado lógico
        if (parameter?.ToString() == "invert")
        {
            return !tieneValor;
        }

        return tieneValor;
    }

    /// <summary>
    /// De la pantalla (XAML) de vuelta al C# (No lo necesitamos, por eso lanza excepción).
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Lanza error si se intenta hacer la conversión inversa
    }
}