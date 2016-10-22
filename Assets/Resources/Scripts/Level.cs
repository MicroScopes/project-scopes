using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // List

namespace ProjectScopes
{ 
    public class Level : MonoBehaviour 
    {
        private Arena arena;
        //private List<Player> players;

        private int noOfPlayers;

        // Use this for initialization
    	void Awake () 
        {
            //players = new List<Player>();
            //noOfPlayers = 0;
    	}
    	
    	// Update is called once per frame
    	/*void Update () {
    	
    	}*/

        public void SetupLevel(Configurator gameConf)
        {
            //noOfPlayers = gameConf.Players.Count;
            /*Player player = Resources.Load("Prefabs/Player", typeof(Player)) as Player;

            if (player)
            {
                foreach (PlayerInitData playerData in gameConf.Players)
                {
                    player.SetupPlayer(playerData, gameConf.InitialArenaSize);
                    players.Add(Instantiate(player));
                    //players[players.Count - 1].SetupPlayer(playerData, gameConf.InitialArenaSize);
                }
            }
            else
            {
                Debug.LogError("Player prefab not found");
            }

            arena = Resources.Load("Prefabs/Arena", typeof(Arena)) as Arena;

            if (arena)
            {
                arena.SetupArena(players, gameConf.InitialArenaSize);
                Instantiate(arena);
            }
            else
            {
                Debug.LogError("Arena prefab not found");
            }*/
        }

        public void MovePlayers()
        {
            foreach (Player player in GameManager.instance.players) 
            {
                player.Turn ();
            }

            GameManager.instance.arena.RedrawArena();
        }
    }

}