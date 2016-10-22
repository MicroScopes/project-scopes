/*!
 * @file GameManager.cs
 * @brief Contains definition of GameManager class.
 * @author Marcin
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief A global namespace for project-scopes.
 * @detail Contains all project-scopes related classes.
 */
namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief
 * 
 * @details
 *
 *
 */

	public class GameManager : MonoBehaviour 
	{
		public static GameManager instance = null;

		private Configurator gameConfiguration;
        private Level level;
        public List<Player> players;
        public Arena arena;

        private float frameRate;
        private float nextFrame;

		void Awake ()
		{
			// Implementation of singleton pattern
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);    
			}

			//Sets this to not be destroyed when reloading scene
			//DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached Level script
            level = GetComponent<Level>();

            // to remove
            TempTestConfiguration();


            frameRate = 0.01f;
            nextFrame = 0.0f;

            players = new List<Player>();

            Setup();

			StartLevel();
		}
		

        //This is called each time a scene is loaded.
        void OnLevelWasLoaded(int index)
        {
            //Call StartLevel to initialize next level.
            StartLevel();
        }

        private void StartLevel ()
        {
            level.SetupLevel(gameConfiguration);
        }

		// Update is called once per frame
		void Update () 
        {
            if (Time.time > nextFrame)
            {
                nextFrame = Time.time + frameRate;

                level.MovePlayers();
            }

            if(Input.GetKeyDown(KeyCode.G))
            {
                foreach (Player player in players) 
                {
                    player.DoubleSize ();
                }
            }

            if(Input.GetKeyDown(KeyCode.H))//
            {
                foreach (Player player in players) 
                {
                    player.ReduceSize ();
                }
            }

            if(Input.GetKeyDown(KeyCode.J))
            {
                foreach (Player player in players) 
                {
                    player.IncreaseSpeed ();
                }
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                foreach (Player player in players) 
                {
                    player.ReduceSpeed ();
                }
            }
		}

		public Configurator GameConfiguration
		{
			set
			{
				gameConfiguration = value;
			}
			get
			{
				return gameConfiguration;
			}
		}


        private void Setup()
        {
            Player player = Resources.Load("Prefabs/Player", typeof(Player)) as Player;

            if (player)
            {
                foreach (PlayerInitData playerData in gameConfiguration.Players)
                {
                    //player.SetupPlayer(playerData, gameConfiguration.InitialArenaSize);
                    players.Add(Instantiate(player));
                    players[players.Count - 1].SetupPlayer(playerData, gameConfiguration.InitialArenaSize);
                }
            }
            else
            {
                Debug.LogError("Player prefab not found");
            }

            arena = Resources.Load("Prefabs/Arena", typeof(Arena)) as Arena;

            if (arena)
            {
                //arena.SetupArena(gameConfiguration.InitialArenaSize);
                Instantiate(arena);
                arena.SetupArena(gameConfiguration.InitialArenaSize);
            }
            else
            {
                Debug.LogError("Arena prefab not found");
            }
        }

        // to remove
        private void TempTestConfiguration()
        {
            gameConfiguration = new Configurator();

            /*gameConfiguration.CurrentNoOfPlayers = 2;
            gameConfiguration.InitialArenaSize = 600;
            gameConfiguration.InitialPlayersSize = 5;
            gameConfiguration.InitialPlayersSpeed = 2.0f;*/
        }
	}

}
