# 🗳️ El Pactómetro

**El Pactómetro** is a WPF desktop application for visualizing and analyzing electoral processes in Spain. It provides graphical tools to manage electoral data, compare historical processes, and simulate coalition formations using a pactometer-like mechanism.

Developed by **Alberto García Martín**.

---

## 📊 Features

- Visual interface for displaying election results using bar charts.
- Manual creation and modification of electoral processes and political parties.
- Comparative mode to graphically compare election results across time.
- Pactometer mode to simulate coalition governments and evaluate majority thresholds.
- Responsive GUI with multiple interconnected windows.
- Data loaded from a read-only XML file with predefined processes.

---

## 🧭 Application Structure

### Main Windows

- **Graphics Window:** Shows bar charts of election results for selected processes.
- **Tables Window:** Lists all electoral processes and associated parties.

### Additional Windows

- **AddParty Window:** Manually add a new political party to a process.
- **AddProcess Window:** Manually add a new electoral process.
- **Comparative Mode:** Compare multiple electoral processes graphically.
- **Pactometer Mode:** Select parties to simulate majority coalition formations.

---

## 🧱 Core Components

### `Proceso` (Process)
Represents an election event:
- Type (general/regional)
- Date
- List of participating parties
- Total and required seats for majority
- XML import support and dynamic seat updates

### `Partido` (Party)
Represents a political party:
- Name and seats obtained
- Associated graphical bar with tooltip, color, and label
- Reactive updates when data or UI changes

---

## 🎨 UI and Styling

- Sidebar with icons for minimizing, maximizing, or closing windows.
- Fully reskinnable color styles.
- Icon images sourced from [Flaticon](https://www.flaticon.es/).

---

## 🔄 Event Handling & Architecture

Follows a **Model-View-Controller (MVC)** approach:

- Events are named with `OnXxxx` convention and trigger interactions between windows.
- `Tablas` (Tables window) acts as the controller, mediating changes between user input and the visual chart output.
- Use of `PropertyChanged` for dynamic UI updates.

### Examples of Event Interactions

- Adding a process updates the charts in real time.
- Selecting parties in Pactometer mode dynamically updates coalition metrics.
- Comparative mode filters processes and updates chart displays.

---

## 📈 Graph Types

- **Main Chart:** Displays results for one selected electoral process.
- **Comparative Chart:** Displays side-by-side comparisons of multiple processes.
- **Pactometer Chart:** Displays simulated majority coalitions.

Rendering is handled via `StackPanel` and dynamically generated bars using party data.

---

## 📂 Technologies Used

- WPF (Windows Presentation Foundation)
- XAML for UI layout
- C# for application logic and event handling
- XML for data storage (read-only)

---


## 📚 References

- [WPF Scroll Controls - Microsoft Docs](https://learn.microsoft.com/es-es/windows/apps/design/controls/scroll-controls?tabs=scrollview)
- [WPF XAML Events](https://wpf-tutorial.com/es/7/xaml/eventos-en-xaml/)
- [StackPanel Class Documentation](https://learn.microsoft.com/es-es/dotnet/api/system.windows.controls.stackpanel?view=windowsdesktop-8.0)
- Several tutorial videos and code samples on WPF (see full list in PDF report)

---
