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
 * @detail Contains all project-scopes related classes.
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
    // Player data
    private string nickname;
    private Color color;
    private float speed;
    private float size;
    private KeyCode[] movementKeys;

    /*!
     * @brief Constructor.
     * 
     * @details Sets the player initial data.
     */
    public Player(string nickname, Color color, float speed, float size, KeyCode[] movementKeys)
    {
        this.nickname = nickname;
        this.color = color;
        this.speed = speed;
        this.size = size;
        this.movementKeys = movementKeys;
    }

    /*!
     * @brief Allows to set and get the color of the player.
     * 
     * @details Player color is decribed by RGB values.
     */
    public Color Color
    {
        set
        {
            color = value;
        }
        get
        {
            return color;
        }
    }

    /*!
     * @brief Allows to set and get the nickname of the player.
     * 
     * @details Player nickname contains only capital letter and is limited
     *          to 9 characters.
     */
    public string Nickname
    {
        set
        {
            nickname = value;
        }
        get
        {
            return nickname;
        }
    }

    /*!
     * @brief Allows to set and get the speed of the player.
     * 
     * @details Player speed is a floating point number and may varry
     *          depending on the game bouns.
     */
    public float Speed
    {
        set
        {
            speed = value;
        }
        get
        {
            return speed;
        }
    }

    /*!
     * @brief Allows to set and get the size of the player.
     * 
     * @details Player size is a floating point number and may varry
     *          depending on the game bonus.
     */
    public float Size
    {
        set
        {
            size = value;
        }
        get
        {
            return size;
        }
    }

    /*!
     * @brief Allows to set and get the movement keys of the player.
     * 
     * @details The movement keys are described by two values: left turn key
     *          and right turn key.
     */
    public KeyCode[] MovementKeys
    {
        set
        {
            movementKeys = value;
        }
        get
        {
            return movementKeys;
        }
    }
}

}