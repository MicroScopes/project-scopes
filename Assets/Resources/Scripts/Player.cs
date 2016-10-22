using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectScopes
{
    public class Player : MonoBehaviour 
    {
        private Vector2 playerPos;
        private Color playerColor;

        private int playerSize;
        private float playerSpeed;

        private float playerDirection;

        private KeyCode keyLeft; 
        private KeyCode keyRight;

        private int arenaSize;

        private bool active;
        private bool visible;

        private int holeDelay;
        private int holeTimerDelay;
        private int holeTimer;
        private int holeSize;

        private GameObject borderHead;
        private List<GameObject> borderHeads;

        private float arenaScalar;

    	// Use this for initialization
        void Awake () 
        {
            holeDelay = 0;
            holeTimerDelay = 0;
            holeTimer = 1000;
            holeSize = 50;

            active = true;
            visible = true;

            borderHeads = new List<GameObject>();

            /*playerPos = new Vector2(0.0f, 0.0f);

            playerSize      = 4;
            playerSpeed     = 1.0f;
            playerDirection = 0.0f;

            playerColor = Color.white;

            keyLeft = KeyCode.LeftArrow;
            keyRight = KeyCode.RightArrow;*/

            borderHead = Resources.Load("Prefabs/PlayerHead", typeof(GameObject)) as GameObject;

            if (borderHead)
            {
                for (int i = 0; i < 3; i++)
                {
                    borderHeads.Add(Instantiate(borderHead));
                }
            }
            else
            {
                Debug.LogError("PlayerHead prefab not found");
            }
    	}
    	
    	// Update is called once per frame
    	/*void Update () {
    	
    	}*/

        /*public void SetupPlayer(Vector2 startPos, float startDeg, int size, float speed, Color col)
        {
            playerPos = startPos;

            playerSize      = size;
            playerSpeed     = speed;
            playerDirection = startDeg; 

            playerColor = col;

            keyLeft = KeyCode.LeftArrow;
            keyRight = KeyCode.RightArrow;
        }*/
            
        public void SetupPlayer(PlayerInitData playerData, int arenaInitSize)
        {
            arenaSize = arenaInitSize;
            arenaScalar = 6.0f / (float)arenaSize;

            playerPos = new Vector2(Random.Range(20.0f, arenaSize - 20),
                                    Random.Range(20.0f, arenaSize - 20));

            playerSize = playerData.PlayerSize;
            playerSpeed = playerData.PlayerSpeed;
            playerDirection = Random.Range(0.0f, 2.0f); 

            playerColor = playerData.PlayerColor;

            keyLeft = playerData.KeyLeft;
            keyRight = playerData.KeyRight;

            MeshRenderer renderer;

            this.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            renderer = this.GetComponent<MeshRenderer> ();
            renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);

            foreach (GameObject head in borderHeads)
            {
                head.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
                renderer = head.GetComponent<MeshRenderer> ();
                renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
            }
        }

        public void Turn ()
        {
            if (active) 
            {
                int hSize = Mathf.CeilToInt((holeSize / playerSpeed) * playerSize);

                if (Input.GetKey (keyLeft)) 
                {
                    playerDirection -= 0.02f + playerSpeed * 0.002f;
                }
                if (Input.GetKey (keyRight)) 
                {
                    playerDirection += 0.02f + playerSpeed * 0.002f;
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

                float headSize = playerSize * 0.02f;

                this.transform.localScale = new Vector3 (headSize, headSize, headSize);

                foreach (GameObject head in borderHeads)
                {
                    head.transform.localScale = new Vector3 (headSize, headSize, headSize);
                }

                MoveHead();
            }
        }

        public void MoveHead()
        {
            int corner = 0;

            this.transform.position = new Vector3(playerPos.x*arenaScalar, playerPos.y*arenaScalar, 0.0f);

            foreach (GameObject head in borderHeads)
            {
                head.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            }

            if (playerPos.y < playerSize) {
                corner = 1;
                borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y + arenaSize) * arenaScalar, 0.0f);
            } if (playerPos.y > arenaSize - playerSize) {
                corner += 2;
                borderHeads[0].transform.position = new Vector3 (playerPos.x * arenaScalar, (playerPos.y - arenaSize) * arenaScalar, 0.0f);
            } if (playerPos.x < playerSize) {
                corner += 4;
                borderHeads[0].transform.position = new Vector3 ((playerPos.x + arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
            } if (playerPos.x > arenaSize - playerSize) {
                corner += 8;
                borderHeads[0].transform.position = new Vector3 ((playerPos.x - arenaSize) * arenaScalar, playerPos.y * arenaScalar, 0.0f);
            }

            switch (corner) 
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
            get { return playerPos.x; }
            set 
            { 
                //playerPos.x = value;

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
            get { return playerPos.y; }
            set 
            { 
                //playerPos.y = value;

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

        public float PlayerSpeed
        {
            get { return playerSpeed; }
        }

        public int PlayerSize
        {
            get { return playerSize; }
        }

        public float PlayerDirection
        {
            get { return playerDirection; }
        }

        public void IncreaseSpeed ()
        {
            if (playerSpeed <= 8) {
                playerSpeed *= 2;
            }
        }

        public void ReduceSpeed ()
        {
            if (playerSpeed > 0.25f) {
                playerSpeed /= 2.0f;
            }
        }

        public Color PlayerColor
        {
            get { return playerColor; }
            //set 
            //{ 
            //    playerColor = value;
            //}
        }

        public void DoubleSize()
        {
            if (playerSize < 5) {
                playerSize += 1;
            }
            else if (playerSize >= 5 && playerSize <= 20) {
                playerSize += 5;
            }
        }

        public void ReduceSize()
        {
            if (playerSize > 5) {
                playerSize -= 5;
            } else if (playerSize > 2 && playerSize <= 5) {
                playerSize -= 1;
            }
        }

        /*public void SetControlKeys(KeyCode keyL, KeyCode keyR)
        {
            keyLeft = keyL;
            keyRight = keyR;
        }*/

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public bool IsVisible()
        {
            return visible;
        }
    }
}
