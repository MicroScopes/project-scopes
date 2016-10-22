using UnityEngine;

namespace ProjectScopes
{
    public class PlayerInitData 
    {
        private Color playerColor;

        private int playerSize;
        private float playerSpeed;

        private KeyCode keyLeft; 
        private KeyCode keyRight;

    	// Use this for initialization
        public PlayerInitData (int size, float speed, Color col, KeyCode keyL, KeyCode keyR) 
        {
            playerSize = size;
            playerSpeed = speed;
            playerColor = col;

            keyLeft = keyL;
            keyRight = keyR;
    	}

        public float PlayerSpeed
        {
            get { return playerSpeed; }
        }

        public int PlayerSize
        {
            get { return playerSize; }
        }

        public Color PlayerColor
        {
            get { return playerColor; }
        }

        public KeyCode KeyLeft
        {
            get { return keyLeft; }
        }

        public KeyCode KeyRight
        {
            get { return keyRight; }
        }
    }
}