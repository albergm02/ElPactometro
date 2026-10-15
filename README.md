<div align="center">

# 🗳️ El Pactómetro

**Aplicación de escritorio para visualizar, comparar y simular procesos electorales en España**

[![.NET](https://img.shields.io/badge/.NET-Framework-512BD4?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-WPF-239120?style=flat&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?style=flat&logo=windows&logoColor=white)](https://www.microsoft.com/windows)
[![Status](https://img.shields.io/badge/Status-Completado-1F7A8C?style=flat)]()

[Descripción](#-sobre-el-proyecto) · [Características](#-características) · [Capturas](#-capturas) · [Cómo ejecutar](#-cómo-ejecutar) · [Arquitectura](#-arquitectura) · [Stack](#-stack-tecnológico)

</div>

---

## 📖 Sobre el proyecto

**El Pactómetro** es una aplicación de escritorio desarrollada en **WPF** y **C#** que permite visualizar y analizar los procesos electorales en España. Ofrece herramientas gráficas para gestionar datos electorales, comparar procesos históricos y **simular formaciones de coalición** mediante un mecanismo de pactómetro.

El objetivo del proyecto es explorar el uso de **interfaces gráficas ricas en WPF**, el **patrón MVC** aplicado a aplicaciones de escritorio y el manejo de **eventos y binding bidireccional** con `INotifyPropertyChanged`.

---

## ✨ Características

- 📊 **Visualización de resultados electorales** mediante gráficos de barras dinámicos.
- ➕ **Gestión manual** de procesos electorales y partidos políticos.
- ⏳ **Modo comparativo**: compara gráficamente resultados de varias elecciones a lo largo del tiempo.
- 🤝 **Modo pactómetro**: selecciona partidos y comprueba si suman mayoría absoluta.
- 🪟 **Interfaz multi-ventana**: ventanas interconectadas que se comunican entre sí.
- 📂 **Carga de datos desde XML**: procesos predefinidos leídos desde un archivo de solo lectura.
- 🎨 **Estilos personalizables**: sistema de reskin de colores y recursos visuales.


## 🚀 Cómo ejecutar

### Requisitos

- **Windows 10/11**
- **.NET Framework 4.7.2 o superior** (o .NET 6+ si el proyecto se ha migrado)
- **Visual Studio 2022** (para desarrollo) o **Visual Studio Community** (gratuito)

### Opción 1 — Abrir el proyecto

1. Clona el repositorio:
   ```bash
   git clone https://github.com/albergm02/ElPactometro.git
   ```
2. Abre el archivo `.sln` con **Visual Studio**.
3. Restaura los paquetes NuGet si es necesario (`Click derecho en la solución → Restore NuGet Packages`).
4. Compila y ejecuta con `F5`.

### Opción 2 — Build ejecutable

Si tienes una build compilada:

1. Ejecuta `ElPactometro.exe` desde la carpeta `bin/Release/`.
2. La aplicación leerá el archivo `datos.xml` incluido.

---

## 🧭 Estructura de la aplicación

### Ventanas principales

| Ventana | Función |
| :--- | :--- |
| **Gráficos** | Muestra los gráficos de barras de los resultados electorales del proceso seleccionado. |
| **Tablas** | Lista todos los procesos electorales y los partidos asociados. Actúa como controlador principal. |

### Ventanas adicionales

| Ventana | Función |
| :--- | :--- |
| **Añadir partido** | Permite añadir manualmente un nuevo partido político a un proceso. |
| **Añadir proceso** | Permite crear manualmente un nuevo proceso electoral. |
| **Modo comparativo** | Compara gráficamente varios procesos electorales a la vez. |
| **Modo pactómetro** | Permite seleccionar partidos y simular coaliciones con mayoría absoluta. |

---

## 🏗️ Arquitectura

El proyecto sigue el patrón **Modelo-Vista-Controlador (MVC)** aplicado a WPF:

- **Modelos**: `Proceso` y `Partido` representan los datos.
- **Vistas**: archivos `.xaml` que definen la interfaz.
- **Controlador**: la ventana `Tablas` actúa como mediadora entre la entrada del usuario y la actualización del gráfico.

### Componentes principales

#### `Proceso` — modelo de un proceso electoral

Representa una convocatoria electoral con:
- Tipo (generales / autonómicas).
- Fecha.
- Lista de partidos participantes.
- Escaños totales y escaños necesarios para la mayoría absoluta.
- Soporte de importación desde XML y actualización dinámica de escaños.

#### `Partido` — modelo de un partido político

Representa un partido político con:
- Nombre y escaños obtenidos.
- Barra gráfica asociada con tooltip, color y etiqueta.
- Actualización reactiva ante cambios de datos o de UI.

### Manejo de eventos

- Los eventos siguen la convención `OnXxxx`.
- Se usa `INotifyPropertyChanged` para actualizar la UI automáticamente cuando cambian los datos.
- Los eventos disparan interacciones entre ventanas (por ejemplo, añadir un proceso actualiza los gráficos en tiempo real).

### Ejemplos de interacción

- **Añadir un proceso** → los gráficos se actualizan en tiempo real.
- **Seleccionar partidos en modo pactómetro** → las métricas de coalición se recalculan dinámicamente.
- **Modo comparativo** → filtra procesos y actualiza el gráfico comparativo.

---

## 📈 Tipos de gráficos

| Tipo | Descripción |
| :--- | :--- |
| **Gráfico principal** | Muestra los resultados de un único proceso electoral seleccionado. |
| **Gráfico comparativo** | Muestra varios procesos en paralelo, uno junto a otro. |
| **Gráfico pactómetro** | Simula coaliciones y muestra si alcanzan la mayoría absoluta (176 escaños). |

El renderizado se hace mediante `StackPanel` y barras generadas dinámicamente a partir de los datos de cada partido.

---

## 🛠️ Stack tecnológico

| Categoría | Tecnología |
| :--- | :--- |
| **Framework** | WPF (Windows Presentation Foundation) |
| **Lenguaje** | C# |
| **UI** | XAML |
| **Persistencia** | XML (solo lectura) |
| **Arquitectura** | MVC · Binding bidireccional con `PropertyChanged` |
| **IDE** | Visual Studio 2022 |
| **Control de versiones** | Git · GitHub |

---

## 📂 Estructura del proyecto

```
ElPactometro/
├── ElPactometro/
│   ├── Modelos/
│   │   ├── Proceso.cs
│   │   └── Partido.cs
│   ├── Vistas/
│   │   ├── MainWindow.xaml
│   │   ├── Tablas.xaml
│   │   ├── AddParty.xaml
│   │   ├── AddProcess.xaml
│   │   ├── ComparativeMode.xaml
│   │   └── PactometerMode.xaml
│   ├── Recursos/
│   │   ├── Iconos/
│   │   └── Estilos/
│   └── datos.xml
├── docs/
│   └── screenshots/
└── README.md
```

## 📚 Referencias

- [WPF Scroll Controls — Microsoft Docs](https://learn.microsoft.com/es-es/windows/apps/design/controls/scroll-controls)
- [WPF XAML Events](https://wpf-tutorial.com/es/7/xaml/eventos-en-xaml/)
- [StackPanel Class Documentation](https://learn.microsoft.com/es-es/dotnet/api/system.windows.controls.stackpanel)
- Iconos de [Flaticon](https://www.flaticon.es/)

---

## 👤 Autor

**Alberto García Martín**

- 🌐 Portfolio: [albertogm.dev](https://albertogm.dev)
- 💼 LinkedIn: [in/albertogarciamartin](https://www.linkedin.com/in/albertogarciamartin/)
- 🐙 GitHub: [@albergm02](https://github.com/albergm02)
- 📧 Email: alberto.g.m.0214@gmail.com

---

<div align="center">

**⭐ Si este proyecto te resulta interesante, considera darle una estrella.**

</div>
