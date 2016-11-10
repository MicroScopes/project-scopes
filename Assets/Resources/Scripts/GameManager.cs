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
using UnityEngine.SceneManagement;

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
        public List<Player> players;

		private static Configurator gameConfiguration;
        private Level level;

        // Variables used for simple frame rate control
        private float frameRate;
        private float nextFrame;

        private int gameLevel;

		void Awake ()
		{
            gameConfiguration = GUIManager.configurator;
            Screen.SetResolution(gameConfiguration.ArenaSize, gameConfiguration.ArenaSize, false);

             // Implementation of singleton pattern
            if (instance == null)
			{
				instance = this;

                // Component reference to the attached Level script
                level = GetComponent<Level>();

                // Creates a default set of 6 inactive players
                LoadPlayers();

                // Object will not be destroyed after scene reload
                DontDestroyOnLoad(gameObject);
			}
			else if (instance != this)
			{
                Destroy(gameObject);    
			}

            frameRate = 0.01f;
            nextFrame = 0.0f;

            gameLevel = 1;
		}

        // Called after enabling game manager
        void Start()
        {
            if (instance == this)
            {
                this.enabled = true;
                Debug.Log("level: " + gameLevel);
                level.SetupLevel();
            }
        }
		

        //This is called each time a scene is loaded.
        void OnLevelWasLoaded()
        {
            //Call SetupLevel only to main instance of GameManager
            if (instance == this)
            {
                this.enabled = true;
                gameLevel++;

                if (CheckIfGameOver())
                {
                    GameOver();
                }
                else
                {
                    Debug.Log("level: " + gameLevel);
                    level.SetupLevel();
                }
            }
        }

        // Loads Player prefab and Instantiate players set
        private void LoadPlayers()
        {
            Player player = Resources.Load("Prefabs/Player", typeof(Player)) as Player;

            if (player)
            {

                for (int i = 0; i < gameConfiguration.CurrentNoOfPlayers; i++)
                {
                    players.Add(Instantiate(player));

                }

                int j = 0;
                foreach (PlayerInitialData p in gameConfiguration.Players)
                {
                    if (p != null)
                    {
                        KeyCode[] keys = { p.LeftKey, p.RightKey };
                        players[j].SetupPlayer(p.Nickname, p.Color, keys);
                        players[j].IsActive = true;
                        Debug.Log(j);
                        j++;
                    }
                }
            }
            else
            {
                Debug.LogError("Player prefab not found");
            }
        }


        bool CheckIfGameOver()
        {
            if (gameLevel >= 4)
            {
                this.enabled = false;
                //GUIDataCollector.instance.enabled = true;
                return true;
            }

            return false;
        }

        void GameOver()
        {
            gameLevel = 1;
            Debug.Log("GAME OVER");

            foreach (Player player in players)
            {
                player.Reset();
                player.IsActive = false;
            }

            //GUIDataCollector.instance.enabled = true;
            //GUIDataCollector.instance.Restart();
            SceneManager.LoadScene("GUI");
        }

        // Updates frames depending on frameRate 
		void Update () 
        {
            if (Time.time > nextFrame)
            {
                nextFrame = Time.time + frameRate;

                level.MovePlayers();
            }


            // to delete - for test
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
	}

}
