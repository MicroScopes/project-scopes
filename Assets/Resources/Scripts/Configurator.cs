/*!
 * @file Configurator.cs
 * @brief Contains definition of Configurator class.
 * author MicroScopes
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using System.Collections.Generic;

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief A global namespace for project-scopes.
 *
 * @details Contains all project-scopes related classes.
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
    /*!
     * @brief Minimum number of players that can participate the game.
     */
    public const int MinNoOfPlayers = 2;

    /*!
     * @brief Maximum number of players that can participate the game.
     */
    public const int MaxNoOfPlayers = 6;

    // Initial size of the arena. User is able to specify whether the size
    // should be small (0), normal (1) or large (2). Default value: normal.
    private List<int> arenaSize = new List<int> { 600, 800, 1000 };

    // Initial speed of all players. User is able to specify whether the speed
    // should be slow (0), normal (1) or fast (2). Default value: normal.
    private float[] playersSpeed = new float[] { 1.0f, 2.0f, 4.0f };

    // Initial size of all players. User is able to specify whether the size
    // should be thin (0), normal (1) or fat (2). Default value: normal.
    private int[] playersSize = new int[] { 3, 6, 12 };

    // List of all players initial data.
    private List<PlayerInitData> players = new List<PlayerInitData>()
                                           {null, null, null, null, null, null};

    /*!
     * @brief Constructor. Initializes configurator object with initial data.
     *
     * @details Sets the players initial nickname, color, speed, size and movement keys
     *          as well as initial size of the arena and players speed and size.
     */
    public Configurator()
    {
        // Default values of players initial data is 'normal'.
        InitialArenaSize = 1;
        InitialPlayersSpeed = 1;
        InitialPlayersSize = 1;
    }

    /*!
     * @brief Adds a new player on the players list.
     * 
     * @details Creates a new Player object and fills it with initial data.
     * @param id Id of a player. The player will be created on 'id' position
     *        on the list.
     */
    public void AddPlayer(int id)
    {
        ++CurrentNoOfPlayers;
        
        players[id] = new PlayerInitData();
    }

    /*!
     * @brief Removes the player from the players list.
     * 
     * @details Removes a Player object and sets null on its place.
     * @param id Id of a player. The player will be removed from 'id' position
     *        from the list.
     */
    public void RemovePlayer(int id)
    {
        --CurrentNoOfPlayers;

        players[id] = null;
    }

    // Allows to set and get current number of players.
    public int CurrentNoOfPlayers
    {
        set;
        get;
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
        set;
        get;
    }

    /*!
     * @brief Allows to set and get the initial speed of all players.
     *
     * @details The user has possibility to specify whether the speed of all
     *          players should be initially slow, normal or fast.
     * @return Specificator of initial players speed (0: slow, 1: normal, 2: fast).
     */
    public int InitialPlayersSpeed
    {
        set;
        get;
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
        set;
        get;
    }

    /*!
     * @brief Allows to get information of all players.
     *
     * @details GUI can update player specific information depending on
     *          user input.
     */
    public List<PlayerInitData> Players
    {
        get
        {
            return players;
        }
    }

    /*!
     * @brief Allows to get the initial aren size in pixels.
     *
     * @details The return value is based on user choice.
     */
    public int ArenaSize
    {
        get
        {
            return arenaSize[InitialArenaSize];
        }
    }

    /*!
     * @brief Allows to get the initial speed of all players.
     *
     * @details The return value is based on user choice.
     */
    public float PlayersSpeed
    {
        get
        {
            return playersSpeed[InitialPlayersSpeed];
        }
    }

    /*!
     * @brief Allows to get the initial size of all players in game units.
     *
     * @details The return value is based on user choice.
     */
    public int PlayersSize
    {
        get
        {
            return playersSize[InitialPlayersSize];
        }
    }
}

}