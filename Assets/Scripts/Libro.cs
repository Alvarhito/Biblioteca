using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class Libro : MonoBehaviour {
	public int indexado;
	public int id;
	public Image portada;
	public Text nombre;
	public Text autor;
	public Text anio;
	public Button eliminar;

	public GameObject controlador;


	// Use this for initialization
	void Start () {
		controlador = GameObject.Find ("Controlador");
		//eliminar.onClick.AddListener (elimina);
		eliminar.OnClickAsObservable ().Subscribe (_ => {
			elimina ();
		});


	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (elimi.Value);
	}
	void elimina(){
		controlador.GetComponent<Sugerencias> ().actualizarUno (indexado);
	}
}
