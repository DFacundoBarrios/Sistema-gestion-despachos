Este proyecto es una aplicación web desarrollada con Razor Pages sobre .NET 8, diseñada para la gestión integral de productos, usuarios, personas, direcciones y pedidos. La arquitectura sigue el patrón MVC adaptado a Razor Pages, facilitando la separación de responsabilidades y la escalabilidad.
Características principales:
•	Gestión de entidades:
•	Productos: CRUD completo para administración de inventario.
•	Usuarios: Registro, edición, eliminación y visualización de usuarios, con roles diferenciados (usuario y administrador).
•	Personas y Direcciones: Relación entre personas y sus direcciones, permitiendo múltiples direcciones por persona.
•	Pedidos y Detalles de Pedido: Creación y seguimiento de pedidos, con detalle de productos asociados.
•	Autenticación y roles:
Implementa autenticación básica y vistas personalizadas para usuarios y administradores, controlando el acceso a funcionalidades sensibles.
•	Interfaz de usuario:
Utiliza Razor Pages para una experiencia fluida y moderna, con vistas organizadas por entidad y validaciones integradas.
•	Persistencia de datos:
Emplea Entity Framework Core para el acceso y gestión de datos, con migraciones para mantener la integridad y evolución del esquema de base de datos.
•	Estructura del proyecto:
•	Controladores: Gestionan la lógica de negocio y las operaciones sobre las entidades.
•	Modelos: Definen la estructura de datos y las relaciones entre entidades.
•	Vistas: Presentan la información y permiten la interacción del usuario.
•	Configuración y despliegue:
Incluye archivos de configuración para distintos entornos (appsettings.json, appsettings.Development.json) y migraciones para la base de datos.
Este proyecto es ideal como base para sistemas de ventas, inventario o gestión de pedidos, y puede ser extendido fácilmente gracias a su estructura modular y uso de tecnologías modernas.
