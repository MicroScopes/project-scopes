using UnityEngine;
using System.Collections;


// One idea how to pass conf from Configurator to GameManager

public class Loader : MonoBehaviour {

	// Use this for initialization

    void Awake ()
    {

        ProjectScopes.Configurator conf = new ProjectScopes.Configurator();

        // 1. First - attach game configuration to GameManager and update it after any live change
        ProjectScopes.GameManager.instance.GameConfiguration = conf;

        // 2. Second - setup all 6 players with default values and update it after any live change
        ProjectScopes.GameManager.instance.players[0].SetupPlayer("testowy1", Color.red, new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow });
        ProjectScopes.GameManager.instance.players[1].SetupPlayer("testowy2", Color.green, new KeyCode[] { KeyCode.Z, KeyCode.X });

        // 3. Third - if player is enabled in GUI set IsActive to true
        ProjectScopes.GameManager.instance.players[0].IsActive = true;
        ProjectScopes.GameManager.instance.players[1].IsActive = true;

        // 4. Fourth - when start button pushed, disable this and enable GameManager
        ProjectScopes.GameManager.instance.enabled = true;
        this.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
