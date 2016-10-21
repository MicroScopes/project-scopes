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
			DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached Level script
            level = GetComponent<Level>();

			StartLevel();
		}
		
		// Update is called once per frame
		void Update () 
        {
            level.MovePlayers();
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
