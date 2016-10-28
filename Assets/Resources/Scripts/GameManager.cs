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
 * @brief Main manager of the game
 * 
 * @details GameManager class is based on singleton pattern and contains players list
 *          and initial game configuration. It is set by default to disable until it gets
 *          initial configuration and players data from GUI.
 */

	public class GameManager : MonoBehaviour 
	{
		public static GameManager instance = null;

		private static Configurator gameConfiguration;
        private Level level;
        public List<Player> players;

        // Variables used for simple frame rate control
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

            // Sets GameManager to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);

            // Component reference to the attached Level script
            level = GetComponent<Level>();

            frameRate = 0.01f;
            nextFrame = 0.0f;
		}

        // Called after enabling game manager
        void Start()
        {
            StartLevel();
        }
		

        //This is called each time a scene is loaded.
        void OnLevelWasLoaded()
        {
            //Call StartLevel to initialize next level.
            StartLevel();
        }

        private void StartLevel()
        {
            this.enabled = true;
                
            LoadPlayers();

            level.SetupLevel(GameConfiguration.InitialArenaSize);
        }

        private void LoadPlayers()
        {
            players = new List<Player>();

            Player player = Resources.Load("Prefabs/Player", typeof(Player)) as Player;

            if (player)
            {
                foreach (PlayerInitData playerData in gameConfiguration.Players)
                {
                    players.Add(Instantiate(player));
                    players[players.Count - 1].SetupPlayer(playerData, gameConfiguration.InitialArenaSize);
                }
            }
            else
            {
                Debug.LogError("Player prefab not found");
            }
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

                if (!this.enabled)
                {
                    this.enabled = true;
                }
			}
			get
			{
				return gameConfiguration;
			}
		}
	}

}
