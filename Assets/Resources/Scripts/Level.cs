using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // List

namespace ProjectScopes
{ 
    public class Level : MonoBehaviour 
    {
        private Arena arena;

        // Use this for initialization
    	void Awake () 
        {
            
    	}
    	
    	// Update is called once per frame
    	/*void Update () {
    	
    	}*/

        public void SetupLevel(Configurator gameConf)
        {
            arena = Resources.Load("Prefabs/Arena", typeof(Arena)) as Arena;

            if (arena)
            {
                Instantiate(arena);
                arena.SetupArena(gameConf.InitialArenaSize);
            }
            else
            {
                Debug.LogError("Arena prefab not found");
            }
        }

        public void MovePlayers(List<Player> players)
        {
            foreach (Player player in players) 
            {
                player.Turn ();
            }

            arena.RedrawArena(players);
        }
    }

}