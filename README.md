# LogisticsOrders

## Descripción
Gestión de órdenes de despacho con cálculo de distancia y costo, arquitectura limpia, Razor Pages y SQL Server.

## Reqisitos técnicos de la prueba
- Arquitectura limpia, separación de capas, principios SOLID.
- Cálculo de distancia con Haversine.
- Validaciones y manejo de errores.
- Pruebas unitarias y de integración.

# ✅ Checklist de Validación según Requisitos

## 1. Arquitectura y Organización
- [x] El proyecto sigue una arquitectura limpia/en capas/hexagonal.
- [x] El código está organizado en capas: Dominio, Aplicación, Infraestructura, Presentación.
- [x] Se usan interfaces para la abstracción de dependencias.
- [x] Se aplican principios SOLID y patrones de diseño (Repositorio, Servicio, Handler, etc.).
- [x] El repositorio Git está bien estructurado y documentado.

## 2. Funcionalidad de Negocio
- [x] Se puede crear una orden con cliente, producto, cantidad, origen y destino.
- [x] El sistema calcula automáticamente la distancia (Haversine) entre origen y destino.
- [x] El costo se calcula correctamente según los intervalos de distancia.
- [x] No se permite crear órdenes con distancias <1 km o >1000 km.
- [x] La orden se almacena correctamente en la base de datos.
- [x] Se puede consultar el detalle de una orden (cliente, producto, coordenadas, distancia, costo).
- [x] Se pueden consultar órdenes filtradas por cliente.
- [x] El reporte por cliente muestra la cantidad de órdenes por intervalo y permite descarga en Excel.

## 3. Validaciones y Seguridad
- [x] Se valida que el producto sea válido y no existan órdenes duplicadas.
- [x] Se valida la dirección de origen usando integración con API externa.
- [x] Se muestran notificaciones y mensajes de confirmación/validación en la UI.
- [x] Se registra auditoría de creación, edición y eliminación de órdenes.
- [x] Los endpoints/páginas sensibles están protegidos por roles.
- [x] El acceso denegado muestra un mensaje adecuado.

## 4. Pruebas
- [x] Existen pruebas unitarias para lógica de negocio, validaciones y servicios.
- [x] Existen pruebas de integración para endpoints principales y reportes.
- [x] Existen pruebas de integración para endpoints protegidos por roles.
- [x] Existen pruebas de integración para descargas de archivos (Excel).
- [x] Todas las pruebas pasan correctamente (`dotnet test`).

## 5. Base de Datos y Configuración
- [x] El script de inicialización de base de datos está disponible (`scripts/init-db.sql`).
- [x] La cadena de conexión está correctamente configurada en `appsettings.json`.
- [x] Se pueden ejecutar migraciones y levantar la base de datos sin errores.

## 6. Frontend y Experiencia de Usuario
- [x] Los formularios usan validación del lado del cliente y del servidor.
- [x] La UI es coherente y fácil de usar.
- [x] El dashboard muestra estadísticas y gráficos con datos reales.
- [x] Los reportes y listados son claros y exportables.

## 7. Despliegue y Documentación
- [x] El archivo de perfil de publicación (`PruebaTecnica10.PublishProfile`) está presente y configurado.
- [x] El README.md incluye instrucciones claras para levantar el proyecto, ejecutar la base de datos, correr pruebas y publicar en Azure.
- [x] El despliegue en Azure funciona correctamente y la aplicación es accesible en la URL indicada.

## 🚀 Ejemplos de uso

### Crear una orden
1. Accede a `/Orders/Create`.
2. Completa el formulario con cliente, producto, cantidad, origen (lat/lng) y destino (lat/lng).
3. Haz clic en "Crear orden".
4. Si la orden es válida, se mostrará un mensaje de éxito y la orden será almacenada.

### Consultar órdenes por cliente
1. Accede a `/Orders/ByClient?ClientName=NombreCliente`.
2. Visualiza el listado de órdenes filtradas por cliente.

### Descargar reporte en Excel
1. Accede a `/Reports/ByClient?ClientName=NombreCliente`.
2. Haz clic en "Descargar Excel" para obtener el reporte de órdenes por cliente.

---

## ☁️ Despliegue en Azure y conexión

### 1. Configuración de la base de datos Azure SQL

- **Servidor:** `dbpruebatecnica.database.windows.net`
- **Base de datos:** `Manuel`
- **Usuario:** `mlopez`
- **Contraseña:** `Manuel123`

Asegúrate de ejecutar el script de inicialización (`scripts/init-db.sql`) en la base de datos antes de publicar la aplicación.

### 2. Configuración de la cadena de conexión

En `appsettings.json`:
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=tcp:dbpruebatecnica.database.windows.net,1433;Initial Catalog=Manuel;Persist Security Info=False;User ID=mlopez;Password=Manuel123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

## 🚀 Instrucciones de Publicación y Ejecución 
### 3. Publicación en Azure

1. Abre la solución en Visual Studio.
2. Haz clic derecho sobre el proyecto `LogisticsOrders.API` y selecciona **Publicar**.
3. Elige **Importar perfil** y selecciona el archivo `PruebaTecnica10.PublishProfile` de la raíz del proyecto.
4. Sigue los pasos del asistente para publicar en Azure App Service.
5. Verifica que la aplicación esté disponible en:  
   [https://pruebatecnica10.azurewebsites.net](https://pruebatecnica10.azurewebsites.net)

---

## 🏃‍♂️ Ejecución del proyecto localmente

1. Ejecuta el script de base de datos:  
   - Abre SQL Server Management Studio o Azure Data Studio.
   - Ejecuta `scripts/init-db.sql` en la base de datos `Manuel`.

2. Configura la cadena de conexión en `appsettings.json` como se muestra arriba.

3. Aplica las migraciones (si es necesario):
   - Abre una terminal en la raíz del proyecto.
   - Ejecuta el siguiente comando para aplicar las migraciones:  
   ```bash
   dotnet ef database update --project LogisticsOrders.Infrastructure --startup-project LogisticsOrders.API
   ```
4. Ejecuta la aplicación:
   - Abre una terminal en la raíz del proyecto.
   - Ejecuta el siguiente comando para iniciar la aplicación:  
   ```bash
   dotnet run --project LogisticsOrders.API
   ```
5. Accede a la aplicación en [http://localhost:5000](http://localhost:5000) o el puerto configurado.
---
## 🧪 Ejecución de pruebas

### Pruebas unitarias
- Abre una terminal en la raíz del proyecto.
- Ejecuta el siguiente comando para correr las pruebas unitarias:  
```bash
dotnet test .\LogisticsOrders.UnitTests\LogisticsOrders.UnitTests.csproj
```
### Pruebas de integración
- Abre una terminal en la raíz del proyecto.
- Ejecuta el siguiente comando para correr las pruebas de integración:  
```bash
dotnet test .\LogisticsOrders.IntegrationTests\LogisticsOrders.IntegrationTests.csproj
```

- Las pruebas de integración simulan peticiones reales a los endpoints, incluyendo endpoints protegidos y descargas de archivos Excel.
- Asegúrate de que la base de datos de pruebas esté configurada correctamente o usa la configuración InMemory para pruebas.

---