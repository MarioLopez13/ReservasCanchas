# BackendReservas

---

##  Tecnologías Usadas

- **ASP.NET Core 7.0**
- **Entity Framework Core**
- **MySql**
- **Swagger/OpenAPI**
- **C# 10**
- **Principios SOLID**
- **Patrones de Diseño: Singleton, Factory**

---

## Estructura del Proyecto

BackendReservas/
├── Controllers/
│ ├── UsuarioController.cs
│ ├── ReservaController.cs
│ ├── HorarioController.cs
│ └── PromocionController.cs
├── Models/
├── Services/
│ └── PromocionService.cs
├── Interfaces/
│ └── IPromocionService.cs
├── Factories/
│ └── BloqueHorarioFactory.cs
├── DTOs/
│ └── BloqueHorarioDTO.cs
├── Data/
│ └── ApplicationDbContext.cs

yaml
Copiar
Editar

---

##  Principios SOLID Aplicados

| Principio | Aplicación |
|-----------|------------|
| SRP (Responsabilidad Única) | `PromocionService` maneja toda la lógica de promociones, separada del controlador |
| DIP (Inversión de Dependencias) | Uso de la interfaz `IPromocionService` para abstraer la lógica del servicio |

---

##  Patrones de Diseño Implementados

| Patrón       | Uso |

| Singleton | `PromocionService` implementa una única instancia con control de concurrencia |
|Factory Method| `BloqueHorarioFactory` encapsula la creación del DTO con lógica clara |

---

##  Endpoints principales

###  Autenticación

- `POST /api/usuario/login` → Login de usuario

###  Usuarios

- `GET /api/usuario` → Listar usuarios
- `POST /api/usuario` → Crear usuario

### 🕓 Horarios

- `GET /api/horario/bloques/cancha/{canchaId}` → Obtener bloques de horario con descuentos calculados automáticamente
- `POST /api/horario/generar/{canchaId}` → Generar horarios de cancha

###  Reservas

- `GET /api/reserva` → Listar reservas
- `POST /api/reserva` → Crear una reserva

###  Promociones

- `GET /api/promocion` → Listar promociones
- `POST /api/promocion` → Crear promoción manual
- `POST /api/promocion/generar-automaticas` → Generar promociones automáticas por sistema

---

## 🛠️ Cómo ejecutar el proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/backend-reservas.git
cd backend-reservas
2. Configurar la conexión a base de datos
Edita el archivo appsettings.json con tu cadena de conexión SQL Server:

json
Copiar
Editar
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ReservasDB;Trusted_Connection=True;"
}
3. Ejecutar migraciones (si es necesario)
bash
Copiar
Editar
dotnet ef database update
4. Correr el proyecto
bash
Copiar
Editar
dotnet run
Luego accede a Swagger en:

bash
Copiar
Editar
http://localhost:{puerto}/swagger
✅ Notas importantes

Todos los cálculos de descuentos y promociones fueron movidos al PromocionService, aplicando el principio de Responsabilidad Única (SRP).

El método GenerarPromocionesAutomaticas() crea descuentos automáticos en horarios de baja demanda para mejorar la ocupación.
