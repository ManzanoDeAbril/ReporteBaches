# 🛣️ VíaLimpia — ReporteBaches

Aplicación móvil multiplataforma desarrollada con **.NET MAUI** que permite a ciudadanos reportar baches y daños viales usando su smartphone. El sistema incluye captura de foto, ubicación GPS automática y un sistema de puntos de recompensa para incentivar la participación ciudadana.

---

## 📱 Capturas de pantalla

> *(Agrega aquí screenshots de la app corriendo en Android o iOS)*

---

## ✨ Funcionalidades

- **Login de usuario** — Acceso con correo y contraseña, sesión estática en memoria.
- **Reportar bache** — Captura de foto con la cámara del dispositivo y obtención de coordenadas GPS en tiempo real.
- **Historial de reportes** — Lista de todos los reportes enviados en la sesión, con foto, fecha y estado.
- **Perfil ciudadano** — Visualización de puntos acumulados y rango de ciudadano (sistema de recompensas).
- **Sistema de puntos** — Cada reporte enviado suma +50 puntos y desbloquea rangos (Ciudadano Iniciante 🥉, Guardián de las Vías 🥈, Héroe del Pavimento 🥇).

---

## 🏗️ Arquitectura

El proyecto sigue el patrón **MVVM (Model-View-ViewModel)** con inyección de dependencias nativa de .NET MAUI.

```
ReporteBaches/
├── Models/
│   ├── ReporteModel.cs       # Datos de un reporte vial (Id, coordenadas, foto, estado)
│   └── UsuarioModel.cs       # Datos del usuario y cálculo de rango por puntos
├── ViewModels/
│   ├── LoginViewModel.cs     # Lógica de autenticación y navegación
│   ├── ReportesViewModel.cs  # Lógica de cámara, GPS y envío de reportes
│   └── PerfilViewModel.cs    # Carga de datos de sesión y cierre de sesión
├── Views/
│   ├── LoginPage.xaml        # Pantalla de inicio de sesión
│   ├── ReportarPage.xaml     # Formulario de reporte con foto y GPS
│   ├── HistorialPage.xaml    # Lista de reportes enviados
│   └── PerfilPage.xaml       # Perfil y puntos del ciudadano
├── Converters/
│   └── NullToBoolConverter.cs  # Convierte nulo a bool para mostrar/ocultar imagen
├── AppShell.xaml             # Navegación Shell con tabs y rutas
└── MauiProgram.cs            # Configuración de la app e inyección de dependencias
```

---

## 🧰 Stack tecnológico

| Tecnología | Versión | Uso |
|---|---|---|
| .NET MAUI | net10.0 | Framework multiplataforma (Android, iOS, macOS, Windows) |
| CommunityToolkit.Mvvm | 8.4.2 | ObservableObject, RelayCommand, ObservableProperty |
| C# | 13+ | Lenguaje principal (nullable enabled, partial classes) |
| XAML | .NET MAUI | Diseño de interfaces declarativas |

---

## 📦 Requisitos previos

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.8+) con el workload **.NET Multi-platform App UI**  
  ó  
  [Visual Studio Code](https://code.visualstudio.com/) + extensión C# Dev Kit + MAUI
- .NET SDK 10.0 o superior
- Android SDK (para emulador o dispositivo físico Android)
- Xcode 15+ (solo para compilar iOS/macOS en Mac)

---

## 🚀 Instalación y ejecución

1. Clona el repositorio:
   ```bash
   git clone https://github.com/ManzanoDeAbril/ReporteBaches.git
   cd ReporteBaches
   ```

2. Restaura las dependencias:
   ```bash
   dotnet restore
   ```

3. Ejecuta en Android (emulador o dispositivo):
   ```bash
   dotnet build -t:Run -f net10.0-android
   ```

4. O abre `ReporteBaches.app.slnx` en Visual Studio y presiona **F5** para ejecutar.

---

## 📋 Permisos requeridos (Android)

La app solicita los siguientes permisos en tiempo de ejecución:

- `CAMERA` — Para capturar la foto del bache.
- `ACCESS_FINE_LOCATION` / `ACCESS_COARSE_LOCATION` — Para obtener las coordenadas GPS.
- `READ_MEDIA_IMAGES` — Para acceder a la galería si la cámara no está disponible.

Estos permisos están declarados en `Platforms/Android/AndroidManifest.xml`.

---

## 🎮 Flujo de la aplicación

```
LoginPage
    └─► (credenciales válidas)
            └─► MainTabs
                    ├── ReportarPage   → Foto + GPS + Descripción → Enviar (+50 pts)
                    ├── HistorialPage  → Lista de reportes enviados en sesión
                    └── PerfilPage     → Nombre, email, puntos, rango → Cerrar sesión
```

---

## 🏆 Sistema de rangos

| Puntos acumulados | Rango |
|---|---|
| 0 – 99 | Ciudadano Iniciante 🥉 |
| 100 – 299 | Guardián de las Vías 🥈 |
| 300+ | Héroe del Pavimento 🥇 |

---

## ⚠️ Estado del proyecto

> Este proyecto es un prototipo funcional con **datos en memoria** (sin backend ni base de datos persistente). Los reportes y la sesión se pierden al cerrar la app.

Posibles mejoras futuras:
- Integración con una API REST (Node.js / ASP.NET Core)
- Persistencia local con SQLite
- Mapa interactivo con marcadores de baches reportados
- Panel de administración municipal
- Notificaciones push sobre el estado del reporte

---

## 👩‍💻 Autora

Desarrollado por **Matias Ojeda**, **Juan Pablo Saavedra** y **Mauricio Lizana** 
GitHub: [@ManzanoDeAbril](https://github.com/ManzanoDeAbril)

---

## 📄 Licencia

Este proyecto está bajo la licencia MIT. 
