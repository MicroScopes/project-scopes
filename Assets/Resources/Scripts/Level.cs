using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public void SetupLevel()
        {
            arena = Resources.Load("Prefabs/Arena", typeof(Arena)) as Arena;

            if (arena)
            {
                Instantiate(arena);
                arena.SetupArena();
            }
            else
            {
                Debug.LogError("Arena prefab not found");
            }

            ResetPlayers();
        }

        public void MovePlayers()
        {
            bool end = true;

            foreach (Player player in GameManager.instance.players) 
            {
                player.Turn ();

                if (player.IsActive)
                {
                    end = false;
                }
            }

            arena.RedrawArena();

            if (end)
            {
                Invoke("Restart", 1.5f);
                GameManager.instance.enabled = false;
            }
        }


        void ResetPlayers()
        {
            foreach (Player player in GameManager.instance.players)
            {
                player.Reset();
            }
        }

        //Restart reloads the scene when called.
        private void Restart ()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game.
            Application.LoadLevel (Application.loadedLevel);
        }
    }

}