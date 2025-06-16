# Prueba Técnica - Tri-E-Shop
## Notas de Desarrollo
### Microservicio de Productos (Products.API)
El código de este servicio está implementado para funcionar como un proxy hacia una API externa.
**Incidencia de Entorno Local:**
Durante las pruebas en mi entorno de red local, la llamada saliente a la API externa (`https://api.platform.sh`) es bloqueada y devuelve un error `403 Forbidden`.
Pruebas de diagnóstico con datos locales confirman que la API interna, sus controladores y endpoints funcionan correctamente con un código `200 OK`. 
Se concluye que el bloqueo se debe a una configuración de red local (firewall/proxy) y no a un error en el código.

![image](https://github.com/user-attachments/assets/7de93ba7-7cc9-4eed-89e0-64d9201bc1da)
