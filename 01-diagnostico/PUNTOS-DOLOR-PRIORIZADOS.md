# Puntos de dolor priorizados

## Criterio de priorización

El orden combina tres factores: alcance del cambio, impacto sobre operaciones del negocio y riesgo de regresión. Por eso el primer punto no es simplemente la clase más larga, sino el lugar donde un error puede afectar más datos y más recorridos del sistema.

| Prioridad | Punto de dolor | Hallazgos relacionados | Por qué ocupa esa posición |
|---:|---|---|---|
| 1 | Persistencia e integridad de los datos | H-04, H-06, H-13, H-14, H-15, H-16, H-17 y H-24 | `PersistenciaService` participa en la carga y el guardado de todas las entidades. El formato puede corromperse, algunas cargas cambian o descartan datos y las escrituras no son atómicas. Un error aquí puede afectar todo el inventario y el historial, por eso tiene el mayor alcance. |
| 2 | Concentración de reglas y estado en `Hacienda` | H-02, H-07, H-08, H-18, H-19, H-20 y H-24 | `Hacienda` coordina potreros, reses, ventas, alimentación y vacunación sobre colecciones modificables. Además, varias operaciones quedan a medias cuando falla la persistencia. El impacto es alto, pero está más concentrado en el dominio que el primer punto. |
| 3 | Control de acceso y credenciales | H-09, H-10, H-11 y H-21 | Las rutas de negocio no exigen autorización, las contraseñas se guardan en texto plano y varias operaciones web no están protegidas contra solicitudes externas. Aunque no es la solicitud de cambio elegida para implementar, es un riesgo directo para las operaciones y los datos. |

La prioridad 1 antecede a la 2 porque la persistencia cruza todas las entidades y controla la durabilidad de los datos. `Hacienda` queda segunda porque concentra reglas críticas, pero su alcance está principalmente en el dominio. Seguridad ocupa el tercer lugar por la gravedad de permitir operaciones sin un control efectivo de acceso. La jerarquía de validación sigue siendo un hallazgo real y se corrige en el TO-BE, pero su impacto operativo es menor que el de estos tres puntos.
