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

        private int gameLevel;

        public bool gameOver = false;

		void Awake ()
		{
            // Implementation of singleton pattern
            if (instance == null)
			{
				instance = this;

                // Object will not be destroyed after scene reload
                DontDestroyOnLoad(gameObject);
			}
			else if (instance != this)
			{
                Destroy(gameObject);    
			}

            gameLevel = 1;
		}

        // Called after enabling game manager
        void Start()
        {
            
        }
		
        //This is called each time a scene is loaded.
        void OnLevelWasLoaded()
        {
            //Call SetupLevel only to main instance of GameManager
            if (instance == this)
            {
                gameOver = CheckIfGameOver();
                if (gameOver)
                {
                    GameOver();
                }
                else
                {
                    Debug.Log("level: " + gameLevel);
                    gameLevel++;
                }
            }
        }


        bool CheckIfGameOver()
        {
            if (gameLevel >= 1)
            {
                return true;
            }

            return false;
        }

        void GameOver()
        {
            gameLevel = 0;
            //Debug.Log("GAME OVER");

            //SceneManager.LoadScene("GUI");
        }
    }

}
