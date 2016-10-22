using UnityEngine;
using System.Collections;


// One idea how to pass conf from Configurator to GameManager

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        ProjectScopes.Configurator conf = new ProjectScopes.Configurator();

        ProjectScopes.GameManager.instance.GameConfiguration = conf;

        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
