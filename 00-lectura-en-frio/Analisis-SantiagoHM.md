# Analisis preliminar HACIENDA
## Santiago Hernandez Morantes - 000544853

### De que trata y componentes principales:
Viendo inicialmente el proyecto, parece ser una APP con arquitectura MVC para gestionar o administrar una Hacienda la cual tiene un gran enfoque en la vacunación del ganado, con Vacuna como superclase y como subclases los tipos Bacteriana y Viva, se vacunan animales como el Cebon, el Novillo y el Ternero. Donde Res es la superclase y las demas subclases heredan de ella. Cuenta tambien con clases como Hacienda, que es la principal, Potrero que es lo que guarda las vacas y controla la capacidad. Clase Venta y Usuario.

### Tres lugares de codigo costosos de cambiar

1. Hacienda: Tiene demasiadas responsabilidades, tiene cosas de potreros, reses, alimentación, ventas, inventario, vacunación y eventos. Para cambiar algo es posible que tocara refactorizar muchas otras cosas.
2. PersistenciaService: Esta es otra clase inmensa y trabaja con muchas entidades, por no decir que casi todas. Tambien el sistema que usa para guardar los datos que los separa con "|" me parece que es bastante fragil y facilmente se podria quebrar y volver incompatible
3. Las reglas que dependen del tipo de Res: Hay muchas cosas que no cumplen con el OCP ya que si se quiere agregar otro tipo de Res, o cambiar propiedades tocaria modificar codigo ya escrito, como los condicionales que hay

### Pregunta que le haria al programador
Por que no segregó responsabilidades y metodos en lugar de hacer la clase Hacienda tan compleja y acoplada?
