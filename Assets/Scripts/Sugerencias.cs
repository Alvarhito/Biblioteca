using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;
using System;

using UnityEngine.UI;

using UniRx;

public class Sugerencias : MonoBehaviour {
	public GameObject canvas;
	public GameObject paraLibros;
	public Button bactualiza;

	string path="/Db/Biblioteca.db";
	public float posicionInicial=-19;
	public float saltos=-61;

	string conn;
	IDbConnection dbconn;

	struct infoLibro{
		public int id;
		public string nombre;
		public string autor;
		public string anio;
	}

	List<GameObject> ObjetoLibros = new List<GameObject> ();
	List<infoLibro> InfoLibros = new List<infoLibro>();

	List<int> auxnum = new List<int> ();
	// Use this for initialization
	void Start () {
		bactualiza.OnClickAsObservable ().Subscribe (_ => {
			actualizarTodos();
		});
		sugerir ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	int repetido(int aux){
		bool repetido = true;
		while (repetido && auxnum.Count>0) {
			for (int j = 0; j < auxnum.Count; j++) {
				if (aux == auxnum [j]) {
					aux = UnityEngine.Random.Range (0, InfoLibros.Count);
					repetido = true;
					break;
				} else {
					repetido = false;
				}
			}
		}

		return aux;
	}
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
			//agregaLibro(lector,posiciony);
			//posiciony -= saltos;
			//num += 1;
		}
		for(int i=0;i<5;i++){
			int aux = UnityEngine.Random.Range (0, InfoLibros.Count);

			aux = repetido (aux);

			auxnum.Add (aux);
			agregaLibro (InfoLibros [aux], posiciony);
			posiciony -= saltos;
		}

		lector.Close ();
		lector = null;

		dbcmd.Dispose ();
		dbcmd = null;

		dbconn.Close ();
		dbconn=null;

	}
	infoLibro guardar (IDataReader informacion){
		string conn = "URI=file:" + Application.dataPath + path;
		IDbConnection dbconn = (IDbConnection)new SqliteConnection (conn);
		dbconn.Open ();


		IDbCommand dbcmd = dbconn.CreateCommand ();
		string comando="select * from Autores where IdAutor="+informacion.GetInt32(2);
		dbcmd.CommandText = comando;
		IDataReader lector = dbcmd.ExecuteReader ();
		string NombreAutor="";
		while (lector.Read ()) {
			NombreAutor = lector.GetString (1);
		}

		infoLibro aux = new infoLibro ();
		aux.id = informacion.GetInt32 (0);
		aux.nombre = informacion.GetString (1);
		if (informacion.GetInt32 (3) > 21)
			aux.anio = informacion.GetInt32 (3).ToString ();
		else {
			aux.anio = "Siglo "+informacion.GetInt32 (3);
		}
		aux.autor = NombreAutor;
		return aux;
	}

	void agregaLibro(infoLibro informacion,float posiciony){
		//Debug.Log (informacion.GetInt32(0));

		Vector2 posicion = new Vector2 (0f, posiciony);
		GameObject objeto = Instantiate (paraLibros, posicion, Quaternion.identity);
		objeto.transform.SetParent (canvas.transform);

		modificarDatos (objeto, informacion);

		ObjetoLibros.Add (objeto);
		ObjetoLibros [ObjetoLibros.Count - 1].GetComponent<Libro> ().indexado = ObjetoLibros.Count - 1;
	}

	void actualizarTodos(){
		for (int i = 0; i < ObjetoLibros.Count; i++) {
			int aux;
			if (auxnum.Count < 15) {
				aux = repetido (UnityEngine.Random.Range (0, InfoLibros.Count));
			}else{
				aux = UnityEngine.Random.Range (0, InfoLibros.Count);
				auxnum = new List<int> ();
			}

			modificarDatos (ObjetoLibros [i], InfoLibros [aux]);

			auxnum.Add (aux);
		}
	}
	public void actualizarUno(int index){
		int aux;
		if (auxnum.Count < 15) {
			aux = repetido (UnityEngine.Random.Range (0, InfoLibros.Count));
		} else {
			auxnum = new List<int> ();
			for (int i = 0; i < ObjetoLibros.Count; i++) {
				auxnum.Add (ObjetoLibros [i].GetComponent<Libro> ().id - 1);
			}
			aux = repetido (UnityEngine.Random.Range (0, InfoLibros.Count));

		}

		modificarDatos (ObjetoLibros [index], InfoLibros [aux]);

		auxnum.Add (aux);
	}

	void modificarDatos(GameObject a,infoLibro b){
		
		a.GetComponent<Libro> ().id = b.id;
		a.GetComponent<Libro> ().nombre.text = b.nombre;
		a.GetComponent<Libro> ().autor.text = b.autor;

		a.GetComponent<Libro> ().anio.text = b.anio;

		Sprite spr = Resources.Load<Sprite> ("Imagenes Libros/Libro (" + b.id + ")");
		a.GetComponent<Libro> ().portada.sprite = spr;


	}
}
