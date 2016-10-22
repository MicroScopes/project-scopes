/*!
 * @file Configurator.cs
 * @brief Contains definition of Configurator class.
 * author MicroScopes
 */

//==================================================
//               D I R E C T I V E S
//==================================================
using UnityEngine;
using System.Collections.Generic;

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief A global namespace for project-scopes.
 * 
 * @detail Contains all project-scopes related classes.
 */
namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief Stores the game configuration.
 * 
 * @details Contains information about minimum and maximum number of players,
 *          current number of players, initial size of the arena and all players
 *          speed and thickness as well as each player's specific data.
 */
public class Configurator
{
    // Minimum and maximum number of players that can participate the game.
    public const int MinNoOfPlayers = 2;
    public const int MaxNoOfPlayers = 6;

    // Current number of players.
    private int currentNoOfPlayers = 0;

    // Initial size of the arena. User is able to specify whether the size
    // should be small (0), normal (1) or large (2). Default value: normal.
    private int initialArenaSize = 1;
    private int[,] arenaSize = new int[,] { { 300, 300 }, { 600, 600 }, { 900, 900 } };

    // Initial speed of all players. User is able to specify whether the speed
    // should be slow (0), normal (1) or fast (2). Default value: normal.
    private float initialPlayersSpeed = 1;
    private float[] playersSpeed = new float[] { 0.5f, 1.0f, 1.5f };

    // Initial size of all players. User is able to specify whether the size
    // should be thin (0), normal (1) or fat (2). Default value: normal.
    private int initialPlayersSize = 1;
    private float[] playersSize = new float[] { 1.0f, 3.0f, 5.0f };

        public List<PlayerInitData> Players = new List<PlayerInitData>();


        //TESTOWA KONF
        public Configurator()
        {
            currentNoOfPlayers = 2;
            initialArenaSize = 600;
            initialPlayersSize = 6;
            initialPlayersSpeed = 2.0f;

            Players.Add (new PlayerInitData (initialPlayersSize, initialPlayersSpeed, Color.red, KeyCode.LeftArrow, KeyCode.RightArrow));
            Players.Add (new PlayerInitData (initialPlayersSize, initialPlayersSpeed, Color.green, KeyCode.Z, KeyCode.X));
            //Players.Add (new PlayerInitData (5, 2.0f, Color.blue, KeyCode.B, KeyCode.N));
        }
       
    /*!
     * @brief Allows to set and get current number of players.
     * 
     * @details The minimum and maximum number of players are defined by
     *          MinNoOfPlayers and MaxNoOfPlayers constants.
     * @return Number of players that are currently ready to play.
     */
     public int CurrentNoOfPlayers
     {
        set
        {
            currentNoOfPlayers = value;
        }
        get
        {
            return currentNoOfPlayers;
        }
     }

     /*!
      * @brief Allows to set and get the initial size of game arena.
      * 
      * @details The user has possibility to specify whether the size of the
      *          arena should be samll, normal or large.
      * @return Specificator of arena size (0: small, 1: normal, 2: large).
      */
     public int InitialArenaSize
     {
        set
        {
            initialArenaSize = value;
        }
        get
        {
            return initialArenaSize;
        }
     }

     /*!
      * @breif Allows to set and get the initial speed of all players.
      * 
      * @details The user has possibility to specify whether the speed of all
      *          players should be initially slow, normal or fast.
      * @return Specificator of initial players speed (0: slow, 1: normal, 2: fast).
      */
     public float InitialPlayersSpeed
     {
        set
        {
            initialPlayersSpeed = value;
        }
        get
        {
            return initialPlayersSpeed;
        }
     }

     /*!
      * @brief Allows to set and get the initial spize of all players.
      * 
      * @details The user has possibility to specify whether the size of all
      *          players should be initially thin, normal or fat.
      * @return Specificator of initial players size (0: thin, 1: normal, 2: fat).
      */
     public int InitialPlayersSize
     {
        set
        {
            initialPlayersSize = value;
        }
        get
        {
            return initialPlayersSize;
        }
     }
}

}
 
 