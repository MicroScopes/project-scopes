/*!
 * @file Player.cs
 * @brief Contains definition of Player class.
 * @author MicroScopes
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
 * @brief Contains player specific data.
 *
 * @details Stores information about player nickname, color, speed, size and
 *          movement keys.
 */
public class Player
{
    /*!
     * @brief Constructor.
     *
     * @details Sets the player initial data.
     */
    public Player(string nickname, Color color, float speed, float size, KeyCode[] movementKeys, bool isActive)
    {
        Nickname = nickname;
        Color = color;
        Speed = speed;
        Size = size;
        MovementKeys = movementKeys;
        IsActive = isActive;
    }

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
     * @brief Allows to set and get the speed of the player.
     *
     * @details Player speed is a floating point number and may varry
     *          depending on the game bouns.
     */
    public float Speed
    {
        set;
        get;
    }

    /*!
     * @brief Allows to set and get the size of the player.
     *
     * @details Player size is a floating point number and may varry
     *          depending on the game bonus.
     */
    public float Size
    {
        set;
        get;
    }

    /*!
     * @brief Allows to set and get the movement keys of the player.
     *
     * @details The movement keys are described by two values: left turn key
     *          and right turn key.
     */
    public KeyCode[] MovementKeys
    {
        set;
        get;
    }

    /*!
     * @brief Allows to set and get the player presence in the game.
     *
     * @details By default all players are not present. Initially two players
     *          are activated as it is the required minimum. User can then
     *          manipulate between two and six players active.
     */
    public bool IsActive
    {
        set;
        get;
    }
}

}