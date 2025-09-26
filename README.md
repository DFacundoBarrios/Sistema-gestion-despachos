# 📦 Sistema de Gestión de Despachos

## 📖 Descripción

El **Sistema de Gestión de Despachos** es una aplicación web empresarial desarrollada como proyecto académico integrador. El sistema está diseñado para optimizar y automatizar el proceso completo de gestión de pedidos, desde su recepción hasta la entrega final al cliente.

Esta solución permite a las empresas de logística y distribución manejar eficientemente sus operaciones de despacho, proporcionando herramientas para el control de inventarios, seguimiento de órdenes, gestión de rutas de entrega y generación de reportes detallados.

## ✨ Características

### 🚚 **Gestión de Despachos**
- Creación y administración de órdenes de despacho
- Asignación automática de rutas de entrega
- Control de estados de pedidos (Pendiente, En proceso, Enviado, Entregado)
- Seguimiento en tiempo real de entregas
- Gestión de devoluciones y reprogramaciones

### 📦 **Control de Inventarios**
- Gestión de productos y categorías
- Control de stock en tiempo real
- Alertas de inventario bajo
- Historial de movimientos de mercancía

### 👥 **Gestión de Usuarios**
- Sistema de roles y permisos granular
- Perfiles de operadores, supervisores y administradores
- Historial de actividades por usuario
- Dashboard personalizado por rol

### 📊 **Reportes y Analytics**
- Reportes de productividad diaria/mensual
- Estadísticas de entregas exitosas
- Análisis de tiempos de despacho
- Exportación a PDF y Excel
- Gráficos interactivos de rendimiento

### 🔒 **Seguridad y Auditoría**
- Autenticación y autorización segura
- Log de todas las operaciones críticas
- Backup automático de datos
- Encriptación de información sensible

## 🛠️ Tecnologías Utilizadas

### **Backend**
- **ASP.NET Core 6.0** - Framework web principal
- **C# 10** - Lenguaje de programación
- **Entity Framework Core** - ORM para acceso a datos
- **SQL Server** - Sistema de gestión de base de datos

### **Frontend**
- **Razor Pages** - Motor de vistas
- **HTML5** - Estructura y marcado semántico
- **CSS3** - Estilos y diseño responsive
- **Bootstrap 5** - Framework CSS
- **JavaScript** - Interactividad del cliente

### **Herramientas y Librerías**
- **Visual Studio 2022** - IDE de desarrollo
- **SQL Server Management Studio** - Gestión de BD
- **Git/GitHub** - Control de versiones
- **NuGet** - Gestor de paquetes .NET

## 🏗️ Arquitectura del Sistema

El sistema implementa el patrón **Model-View-Controller (MVC)** con una arquitectura en capas:

```
📁 Sistema-gestion-despachos/
├── 🗂️ Controllers/          # Controladores MVC
├── 🗂️ Models/               # Modelos de datos y ViewModels
├── 🗂️ Views/                # Vistas Razor
├── 🗂️ Data/                 # Contexto de Entity Framework
├── 🗂️ Services/             # Lógica de negocio
├── 🗂️ Repositories/         # Patrón Repository
├── 🗂️ DTOs/                 # Data Transfer Objects
├── 🗂️ Utilities/            # Clases utilitarias
└── 🗂️ wwwroot/              # Archivos estáticos
```


## 📋 Requisitos Previos

Antes de instalar, asegúrate de tener:

- **.NET 6.0 SDK** o superior
- **SQL Server 2019** o superior (Express edition es suficiente)
- **Visual Studio 2022** o **Visual Studio Code**
- **SQL Server Management Studio** (opcional pero recomendado)
- **Git** para control de versiones

## 🚀 Instalación

### 1. **Clonar el repositorio**
```bash
git clone https://github.com/DFacundoBarrios/Sistema-gestion-despachos.git
cd Sistema-gestion-despachos
```

### 2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

### 3. **Configurar la base de datos**

#### **Opción A: SQL Server Local**
Edita `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestionDespachosDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### **Opción B: SQL Server Remoto**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tu-servidor;Database=GestionDespachosDB;User Id=tu-usuario;Password=tu-contraseña;TrustServerCertificate=true"
  }
}
```

### 4. **Aplicar migraciones**
```bash
dotnet ef database update
```

### 5. **Sembrar datos iniciales (opcional)**
```bash
dotnet run --seed
```

### 6. **Ejecutar la aplicación**
```bash
dotnet run
```

La aplicación estará disponible en `https://localhost:5001`

## ⚙️ Configuración

### **Configuración de Email**
Para notificaciones por correo, configura en `appsettings.json`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "tu-email@gmail.com",
    "SenderPassword": "tu-app-password",
    "EnableSSL": true
  }
}
```

### **Configuración de Logging**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### **Configuración de Seguridad**
```json
{
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-muy-larga-y-segura",
    "ExpiryInHours": 24,
    "Issuer": "GestionDespachos",
    "Audience": "GestionDespachosUsers"
  }
}
```

### **Flujo de Trabajo Típico**

1. **Login**: Inicia sesión con las credenciales correspondientes
2. **Dashboard**: Visualiza el resumen de operaciones del día
3. **Crear Pedido**: Registra nuevos pedidos de despacho
4. **Asignar Ruta**: Planifica y asigna rutas de entrega
5. **Seguimiento**: Monitorea el estado de los despachos
6. **Reportes**: Genera reportes de productividad



### **Tablas Principales**

#### **Usuarios**
- Gestión de usuarios del sistema
- Roles y permisos
- Información de contacto

#### **Pedidos**
- Órdenes de despacho
- Estados de procesamiento
- Información del cliente

#### **Productos**
- Catálogo de productos
- Control de inventario
- Precios y categorías

#### **Despachos**
- Programación de entregas
- Asignación de recursos
- Seguimiento de estados

## 🔌 API Endpoints

### **Autenticación**
```
POST   /api/auth/login          # Iniciar sesión
POST   /api/auth/logout         # Cerrar sesión
POST   /api/auth/refresh        # Renovar token
```

### **Pedidos**
```
GET    /api/pedidos            # Listar pedidos
POST   /api/pedidos            # Crear pedido
PUT    /api/pedidos/{id}       # Actualizar pedido
DELETE /api/pedidos/{id}       # Eliminar pedido
GET    /api/pedidos/{id}       # Obtener pedido por ID
```

### **Despachos**
```
GET    /api/despachos          # Listar despachos
POST   /api/despachos          # Crear despacho
PUT    /api/despachos/{id}     # Actualizar estado
GET    /api/despachos/ruta     # Optimizar ruta
```

### **Productos**
```
GET    /api/productos          # Listar productos
POST   /api/productos          # Crear producto
PUT    /api/productos/{id}     # Actualizar producto
GET    /api/productos/stock    # Consultar stock
```

### **Reportes**
```
GET    /api/reportes/diario    # Reporte diario
GET    /api/reportes/mensual   # Reporte mensual
GET    /api/reportes/export    # Exportar datos
```

## 👥 Roles y Permisos

### **🔑 Administrador**
- Acceso completo al sistema
- Gestión de usuarios y roles
- Configuración del sistema
- Todos los reportes y estadísticas

### **👨‍💼Usuarios**
- Supervisión de operaciones
- Asignación de despachos
- Reportes operacionales
- Gestión de inventarios


## 👨‍💻 Contacto

**Facundo Barrios** - Desarrollador Full Stack Jr

- 📧 Email: faqbarrios23@gmail.com
- 💼 LinkedIn: [Facundo Barrios](https://www.linkedin.com/in/facundobarrios27/)
- 🐙 GitHub: [DFacundoBarrios](https://github.com/DFacundoBarrios) 


## 🙏

Este proyecto fue desarrollado como **trabajo integrador académico para materia Programacion 3 Backend** en el marco de la formación en desarrollo de software.




