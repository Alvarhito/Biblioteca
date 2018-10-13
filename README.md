# Biblioteca
Por

-Álvaro José Ortega Vargas - T00046501

-Charles Acevedo           - T000

-Hugo chaves :v            - T000


El programa consiste en un pequeño sistema de sugerencias de libros en donde se actualiza y se muestran diferentes sugerencias, o se actualiza individualmente, usando programación reactiva.
Se utilizó:
- Unity3D (Motor gráfico para videojuegos)
- UniRx (Plugin para la programación reactiva)
- SQlite (Plugin para manejar la base de datos)
- Biblioteca.db (Bases de datos local)
- C# (Lenguaje de programación)

Base de datos:  Se creó una base de datos local con información correspondiente a los libros organizando esa información en dos tablas, Libros y Autores.

Libros: Información de publicación del libro.

![Alt text](https://github.com/Alvarhito/Biblioteca/blob/master/ImagenesReadme/Libro.png "Libros")
 
Donde en el campo autor es una llave foránea de la tabla Autores

Autores: Con nombres de los autores.

![Alt text](https://github.com/Alvarhito/Biblioteca/blob/master/ImagenesReadme/Autores.png "Autores")

Manejando la base de datos:
```php
void sugerir(){  	
     conn = "URI=file:" + Application.dataPath + path;
     dbconn = new SqliteConnection (conn);       
     dbconn.Open ();    
     IDbCommand dbcmd = dbconn.CreateCommand ();      
     string comando="select * from Libros";       
     dbcmd.CommandText = comando;       
     IDataReader lector = dbcmd.ExecuteReader ();       
     float posiciony = posicionInicial;        
     int num = 0;        
     while (lector.Read() && num<5) {            
          InfoLibros.Add (guardar (lector));       
     }  	 
// (continuando código…)
Si quieres ver el código completo entra a Biblioteca/Assets/Scripts/Sugerencias.cs 
}
```
  
Como vemos en el código, nos conectamos a la base de datos con “SqliteConnection”, y hacemos una consulta, buscando la información de todos los libros.

#Interfaz de usuario: Esto es lo que se muestra al usuario, en los cuales hay botones (señalados en rojo), y son en los que aplicaremos la programación reactiva, y los utilizaremos como observables, que serán observado por los observadores, que en este caso son los objetos de libro (señalados en amarillo).

![Alt text](https://github.com/Alvarhito/Biblioteca/blob/master/ImagenesReadme/Vista.png "vista")
 
Aplicándola de la siguiente manera:
```php
using UniRx; 
.
.
.
void Start () {     
     bactualiza.OnClickAsObservable ().Subscribe (_ => {        
         actualizarTodos();       
     });       
     sugerir ();   
  } 
// (continuando código…)
Si quieres ver el código completo entra a Biblioteca/Assets/Scripts/Sugerencias.cs 
```

La variable bactualiza es la referencia al botón de actualizar. Con la librería de unity3D para la programación reactiva (UniRx), añadimos el botón el clic del actualizar como observable, y utilizo el operador “Subscribe”, para subscribir el observable a una función que actualizará los 5 libros sugeridos.




El anterior código solo era para el botón se actualizar. El siguiente código muestra algo parecido pero ahora para las “x”s de cada libro, para actualizarlo individualmente:
```php
void Start () {
     .
     .
     .        
      eliminar.OnClickAsObservable ().Subscribe (_ => {           
           elimina ();        
      });   
 } 
// (continuando código…)
Si quieres ver el código completo entra a Biblioteca/Assets/Scripts/Libro.cs 
```


En la clase “Libro”, hacemos cada botón de eliminar como observable, y se subscribe a una función que actualiza solo el libro en que fue clicleado.

