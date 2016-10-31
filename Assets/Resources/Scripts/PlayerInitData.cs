using UnityEngine;
using System.Collections;

namespace ProjectScopes
{
  
    public class PlayerInitData
    {
        public PlayerInitData(string nickname, Color color, float speed, float size, KeyCode[] movementKeys, bool isActive)
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
