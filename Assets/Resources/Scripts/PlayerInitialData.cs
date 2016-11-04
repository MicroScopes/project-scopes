/*!
 * @file PlayerInitialData.cs
 * @brief Contains definition of PlayerInitialData class.
 * author MicroScopes
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;

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
 * @brief Stores the initial values of a player.
 *
 * @details These values are then sent from GUI to GameManager in order to
 *          create the players on the Arena.
 */
public class PlayerInitialData
{
    /*!
     * @brief Allows to set and get the color of the player.
     *
     * @details Player color is decribed by RGB values.
     */
    public Color Color
    {
        set;
        get;
    }

    /*!
     * @brief Allowst to set and get the player left turn key.
     *
     * @details The key value is individual for each player.
     */
    public KeyCode LeftKey
    {
        set;
        get;
    }

    /*!
     * @brief Allows to set and get the nickname of the player.
     *
     * @details Player nickname contains only capital letter and is limited
     *          to 9 characters.
     */
    public string Nickname
    {
        set;
        get;
    }

    /*!
     * @brief Allowst to set and get the player right turn key.
     *
     * @details The key value is individual for each player.
     */
    public KeyCode RightKey
    {
        set;
        get;
    }
}

}