/*!
 * @file    Player.cs
 * @brief   Contains Player class definition.
 * @author  Marcin
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief   A global namespace for project-scopes.
 * @detail  Contains all project-scopes related classes.
 */

namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief   Player handles all player specific attributes and methods.
 * 
 * @details Player class holds all player main settings like size, speed, nickname and color. 
 *          It handles movement keys specific for each player and calculates the angle in which the player is moving.
 *          It measures the time, when player becomes invisible for a moment and manage of head object behavior.                  
 */

    public class Player : MonoBehaviour 
    {
        private Vector2 playerPos;
        private Color playerColor;

        private int playerSize;
        private float playerSpeed;

        private float playerDirection;

        private bool active = false;
        private bool visible = false;

        private int holeDelay = 0;
        private int holeTimerDelay = 0;
        private int holeTimer = 1000;
        private int holeSize = 50;

        private GameObject borderHead;
        private List<GameObject> borderHeads;

        private int arenaSize;
        private float arenaScalar;
        private float angleScalar = 0.015f;

        private Configurator gameConfiguration;

    	// Use this for initialization
        void Awake () 
        {
            gameConfiguration = GUIManager.configurator;
            borderHeads = new List<GameObject>();

            arenaSize = gameConfiguration.ArenaSize;
            arenaScalar = 6.0f / arenaSize;

            LoadHead();
    	}


        private void LoadHead()
        {
            borderHead = Resources.Load("Prefabs/PlayerHead", typeof(GameObject)) as GameObject;

            if (borderHead)
            {
                for (int i = 0; i < 3; i++)
                {
                    borderHeads.Add(Instantiate(borderHead));
                    borderHeads[borderHeads.Count - 1].transform.SetParent(this.transform);
                }
            }
            else
            {
                Debug.LogError("PlayerHead prefab not found");
            }
        }
            

        public void SetupPlayer(string nickname, Color color, KeyCode[] movementKeys)
        {
            playerPos = new Vector2(Random.Range(20.0f, arenaSize - 20),
                                    Random.Range(20.0f, arenaSize - 20));

            playerSize = gameConfiguration.PlayerSize;
            playerSpeed = gameConfiguration.PlayerSpeed;
            playerDirection = Random.Range(0.0f, 2.0f); 

            playerColor = color;
            Nickname = nickname;
            Points = 0;

            MovementKeys = movementKeys;

            MeshRenderer renderer;

            this.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            renderer = this.GetComponent<MeshRenderer> ();
            renderer.material.color = playerColor + new Color(0.2f, 0.2f, 0.2f);

            foreach (GameObject head in borderHeads)
            {
                head.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
                renderer = head.GetComponent<MeshRenderer> ();
                renderer.material.color = playerColor + new Color(0.2f, 0.2f, 0.2f);
            }
        }


        public void Reset()
        {
            playerPos = new Vector2(Random.Range(20.0f, arenaSize - 20),
                                    Random.Range(20.0f, arenaSize - 20));
            
            playerSize = gameConfiguration.PlayerSize;
            playerSpeed = gameConfiguration.PlayerSpeed;
            playerDirection = Random.Range(0.0f, 2.0f);

            this.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);

            foreach (GameObject head in borderHeads)
            {
                head.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            }

            holeDelay = 0;
            holeTimerDelay = 0;
            visible = false;
        }


        public void Move()
        {
            if (active) 
            {
                int hSize = Mathf.CeilToInt((holeSize / playerSpeed) * playerSize);

                if (Input.GetKey (MovementKeys[0])) 
                {
                    playerDirection += angleScalar + (playerSpeed - 1.0f) * 0.004f;
                }
                if (Input.GetKey (MovementKeys[1])) 
                {
                    playerDirection -= angleScalar + (playerSpeed - 1.0f) * 0.004f;
                }

                if (holeDelay < holeTimer)
                {
                    holeDelay+=10;
                }
                else
                {
                    visible = false;
                    if (holeTimerDelay < hSize)
                    {
                        holeTimerDelay+=10;
                    }
                    else
                    {
                        visible = true;
                        holeDelay = 0;
                        holeTimerDelay = 0;
                    }
                }

                float headSize = 2 * arenaScalar * playerSize;

                this.transform.localScale = new Vector3 (headSize, headSize, headSize);
            }
        }


        public void MoveHead()
        {
            int cornerIndex = 0;

            this.transform.position = new Vector3(playerPos.x*arenaScalar, playerPos.y*arenaScalar, 0.0f);

            foreach (GameObject head in borderHeads)
            {
                head.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            }

            if (playerPos.y < playerSize) 
            {
                cornerIndex += 1;
                borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y + arenaSize) * arenaScalar, 0.0f);
            } 
            if (playerPos.y > arenaSize - playerSize) 
            {
                cornerIndex += 2;
                borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y - arenaSize) * arenaScalar, 0.0f);
            } 
            if (playerPos.x < playerSize) 
            {
                cornerIndex += 4;
                borderHeads[0].transform.position = new Vector3 ((playerPos.x + arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
            } 
            if (playerPos.x > arenaSize - playerSize) 
            {
                cornerIndex += 8;
                borderHeads[0].transform.position = new Vector3 ((playerPos.x - arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
            }

            switch (cornerIndex) 
            {
                case 5:
                    borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y + arenaSize) * arenaScalar, 0.0f);
                    borderHeads[1].transform.position = new Vector3 ((playerPos.x + arenaSize) * arenaScalar, (playerPos.y + arenaSize) * arenaScalar, 0.0f);
                    borderHeads[2].transform.position = new Vector3 ((playerPos.x + arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
                    break;
                case 6:
                    borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y - arenaSize) * arenaScalar, 0.0f);
                    borderHeads[1].transform.position = new Vector3 ((playerPos.x + arenaSize) * arenaScalar, (playerPos.y - arenaSize) * arenaScalar, 0.0f);
                    borderHeads[2].transform.position = new Vector3 ((playerPos.x + arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
                    break;
                case 9:
                    borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y + arenaSize) * arenaScalar, 0.0f);
                    borderHeads[1].transform.position = new Vector3 ((playerPos.x - arenaSize) * arenaScalar, (playerPos.y + arenaSize) * arenaScalar, 0.0f);
                    borderHeads[2].transform.position = new Vector3 ((playerPos.x - arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
                    break;          
                case 10:
                    borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y - arenaSize) * arenaScalar, 0.0f);
                    borderHeads[1].transform.position = new Vector3 ((playerPos.x - arenaSize) * arenaScalar, (playerPos.y - arenaSize) * arenaScalar, 0.0f);
                    borderHeads[2].transform.position = new Vector3 ((playerPos.x - arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
                    break;
            }    
        }


        public float PosX
        {
            get 
            {
                return playerPos.x;
            }
            set 
            { 
                if (value < 0)
                {
                    playerPos.x = (arenaSize + value) % arenaSize;
                }
                else
                {
                    playerPos.x = value % arenaSize;
                }
            }
        }

        public float PosY
        {
            get 
            {
                return playerPos.y;
            }
            set 
            {
                if (value < 0)
                {
                    playerPos.y = (arenaSize + value) % arenaSize;
                }
                else
                {
                    playerPos.y = value % arenaSize;
                }
            }
        }

        public float Speed
        {
            get 
            {
                return playerSpeed;
            }
        }

        public int Size
        {
            get 
            {
                return playerSize;
            }
        }

        public float Direction
        {
            get 
            {
                return playerDirection;
            }
        }

        public void IncreaseSpeed ()
        {
            if (playerSpeed <= 8)
            {
                playerSpeed *= 2;
            }
        }

        public void ReduceSpeed ()
        {
            if (playerSpeed > 0.25f)
            {
                playerSpeed /= 2.0f;
            }
        }

        public Color Colour
        {
            get
            {
                return playerColor;
            }
        }

        public void DoubleSize()
        {
            if (playerSize < 6)
            {
                playerSize += 1;
            }
            else if (playerSize >= 6 && playerSize <= 24)
            {
                playerSize += 6;
            }
        }

        public void ReduceSize()
        {
            if (playerSize > 6)
            {
                playerSize -= 6;
            } 
            else if (playerSize > 2 && playerSize <= 6)
            {
                playerSize -= 1;
            }
        }

        public bool IsActive
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public bool IsVisible()
        {
            return visible;
        }

        public KeyCode[] MovementKeys
        {
            set;
            get;
        }

        public string Nickname
        {
            set;
            get;
        }

        public int Points
        {
            set;
            get;
        }
    }
}
