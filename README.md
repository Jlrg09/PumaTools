# PumaTools - Instalador de Ofimática y Optimizador de Windows

PumaTools es una utilidad de administración de sistemas Windows y despliegue de software desarrollada para entornos profesionales de soporte técnico IT. Esta aplicación Windows Forms automatiza tareas comunes de administración de sistemas, instalación de software y procedimientos de optimización de Windows. [1](#0-0) 

## Características Principales

- **Gestión de Usuarios**: Restauración y reparación de perfiles de usuario de Windows
- **Optimización del Sistema**: Ajustes de rendimiento para Windows 10 y Windows 11
- **Despliegue de Software**: Instalación automatizada de suites de oficina y herramientas de productividad
- **Limpieza del Sistema**: Eliminación de archivos temporales y limpieza del registro
- **Personalización de UI**: Modificación de la posición de la barra de tareas en Windows 11

## Requisitos del Sistema

- **Sistema Operativo**: Windows (Windows 10/11)
- **Privilegios**: Se requieren permisos de administrador para todas las operaciones
- **Runtime**: .NET 9.0 Windows runtime environment
- **Arquitectura**: Windows x64

## Instalación y Preparación

Antes de ejecutar el programa, debe preparar la siguiente estructura de carpetas: [2](#0-1) 

```
Resources/
└── Instalables/
    ├── office365.exe
    ├── chrome.exe
    ├── firefox.exe
    ├── brave.exe
    ├── adobe.exe
    ├── pdfpro.exe
    ├── vscode.exe
    └── Office2019/
        ├── setup.exe
        ├── configuracion.xml
        └── [archivos adicionales del ODT]
```

Coloque los instaladores con los nombres exactos especificados. Para Office 2019, debe copiar la carpeta completa con los archivos del Office Deployment Tool.

## Uso

1. Ejecute `PumaTool.exe` como Administrador
2. Desde el menú principal podrá acceder a cada función: [3](#0-2) 
   - Restaurar usuario
   - Optimizar Windows 10 y 11
   - Instalar Ofimática
   - Cambiar posición del botón de inicio (Windows 11)
   - Limpiar temporales

## Sistema de Instalación de Software

El sistema de instalación soporta los siguientes paquetes de software: [4](#0-3) 

| Software | Ejecutable |
|----------|------------|
| Office 365 | office365.exe |
| Office 2019 | setup.exe + configuracion.xml |
| Chrome | chrome.exe |
| Firefox | firefox.exe |
| Brave | brave.exe |
| Adobe | adobe.exe |
| PDF Pro | pdfpro.exe |
| Visual Studio Code | vscode.exe |

El sistema ofrece dos modos de instalación:
- **Secuencial**: Espera a que cada instalación se complete antes de continuar
- **Por lotes**: Inicia todas las instalaciones simultáneamente

## Arquitectura de la Aplicación

PumaTools sigue una arquitectura de Windows Forms con múltiples formularios especializados: [5](#0-4) [6](#0-5) 

- `FormBienvenida`: Pantalla de bienvenida inicial
- `FormPrincipal`: Panel principal con acceso a todas las funciones
- `FormInstaladorOfimatica`: Instalador de software
- `FormSeleccionarUsuario`: Diálogo de selección de usuario
- `FormPantallaCarga`: Pantalla de progreso de operaciones

## Licencia y Derechos de Autor

PumaTool © José Romero 2025. Todos los derechos reservados. [7](#0-6) 

Este software ha sido desarrollado para uso exclusivo en entornos profesionales de soporte técnico, reparación o administración de sistemas. Queda estrictamente prohibida su venta, redistribución o uso con fines comerciales sin autorización expresa del autor.

## Soporte Técnico

Desarrollado por José Romero - Grupo TIC UNIVERSIDAD DEL MAGDALENA [8](#0-7) 

Contacto: Joseromerolg@unimagdalena.edu.co

## Notes
El README se basa principalmente en el archivo LEEME.txt existente y la documentación de la wiki del proyecto. La aplicación está construida con .NET 9.0 y utiliza Windows Forms para la interfaz de usuario. El sistema de instalación de software implementa lógica especial para Office 2019 que utiliza el Office Deployment Tool de Microsoft con archivos de configuración XML.

Wiki pages you might want to explore:
- [Overview (Jlrg09/PumaTools)](/wiki/Jlrg09/PumaTools#1)
- [User Interface Forms (Jlrg09/PumaTools)](/wiki/Jlrg09/PumaTools#3.2)
- [Software Installation System (Jlrg09/PumaTools)](/wiki/Jlrg09/PumaTools#4.1)

### Citations

**File:** LEEME.txt (L1-14)
```text
PumaTool - Instalador de Ofimática y Optimizador de Windows

===========================================
DESCRIPCIÓN GENERAL
===========================================

Este programa permite:

- Restaurar usuarios de Windows.
- Optimizar Windows 10 y 11 para mejorar rendimiento.
- Instalar paquetes de ofimática y herramientas de productividad.
- Personalizar la posición de la barra de tareas de Windows 11.
- Realizar limpieza de archivos temporales.

```

**File:** LEEME.txt (L19-50)
```text
Antes de ejecutar el programa, debe preparar la siguiente estructura de carpetas:

- Dentro del directorio donde se encuentra el ejecutable, debe crear la siguiente ruta:

  Resources\Instalables\

- Allí debe colocar los instaladores de los programas con los siguientes nombres exactos:

  - Office 365  →  office365.exe
  - Office 2019 → (Carpeta completa, ver abajo)
  - Chrome      →  chrome.exe
  - Firefox     →  firefox.exe
  - Brave       →  brave.exe
  - Adobe       →  adobe.exe
  - PDF Pro     →  pdfpro.exe
  - Visual Studio Code → vscode.exe

IMPORTANTE:

- Para Office 2019 debe copiar dentro de:
  Resources\Instalables\Office2019\

- Dentro de esa carpeta deben estar:
  - setup.exe
  - configuracion.xml
  - (Todos los archivos adicionales requeridos por el instalador de Office 2019)

- La estructura debe quedar así:
  
  Resources\Instalables\Office2019\setup.exe
  Resources\Instalables\Office2019\configuracion.xml
  Resources\Instalables\Office2019\otros_archivos...
```

**File:** LEEME.txt (L56-63)
```text
- Ejecute el archivo PumaTool.exe como Administrador.
- Desde el menú principal podrá acceder a cada una de las funciones:
  - Restaurar usuario
  - Optimizar Windows 10 y 11
  - Instalar Ofimática
  - Cambiar posición del botón de inicio (Windows 11)
  - Limpiar temporales
  - Solucionar problemas de instalacion de office (Proximamente)
```

**File:** LEEME.txt (L73-85)
```text
TÉRMINOS DE USO Y DERECHOS DE AUTOR
===========================================

PumaTool © José Romero 2025.  
Todos los derechos reservados.

Este software ha sido desarrollado para uso exclusivo en entornos profesionales de soporte técnico, reparación o administración de sistemas.  

Queda estrictamente prohibida su venta, redistribución o uso con fines comerciales o de lucro sin autorización expresa del autor.

El autor no se hace responsable por daños, pérdidas de información, o mal uso del software por parte de terceros.

Cualquier modificación, adaptación o distribución debe contar con el permiso explícito del autor.
```

**File:** LEEME.txt (L91-94)
```text
PumaTool desarrollado por:
José Romero - Grupo TIC UNIVERSIDAD DEL MAGDALENA
Contacto: Joseromerolg@unimagdalena.edu.co

```

**File:** FormInstaladorOfimatica.cs (L70-77)
```csharp
            programasList.Items.Add("Office 365");
            programasList.Items.Add("Office 2019");
            programasList.Items.Add("Chrome");
            programasList.Items.Add("Firefox");
            programasList.Items.Add("Brave");
            programasList.Items.Add("Adobe");
            programasList.Items.Add("PDF Pro");
            programasList.Items.Add("Visual Studio Code");
```

**File:** FormPrincipal.cs (L47-62)
```csharp
        private void btnRestaurarUsuario_Click(object sender, EventArgs e)
        {
            using (var seleccionar = new FormSeleccionarUsuario())
            {
                if (seleccionar.ShowDialog() == DialogResult.OK)
                {
                    using (var carga = new FormPantallaCarga())
                    {
                        carga.Show();
                        Application.DoEvents(); // Muestra la pantalla antes de iniciar
                        RestaurarUsuario(seleccionar.UsuarioSeleccionado, carga);
                        carga.Close();
                    }
                }
            }
        }
```
