using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // List

namespace ProjectScopes
{
    
    public class Level : MonoBehaviour 
    {
        private GameObject arena;
        private List<Player> players;

        private int noOfPlayers;

        // Use this for initialization
    	void Start () 
        {
            players = new List<Player>();
            noOfPlayers = 0;
    	}
    	
    	// Update is called once per frame
    	/*void Update () {
    	
    	}*/

        public void SetupLevel(Configurator gameConf)
        {
            //noOfPlayers = gameConf.CurrentNoOfPlayers;

            arena = Resources.Load("Prefabs/Arena", typeof(GameObject)) as GameObject;

            if (arena)
            {
                Instantiate(arena);
            }
            else
            {
                Debug.LogError("Arena prefab not found");
            }
        }

        public void MovePlayers()
        {

        }
    }

}