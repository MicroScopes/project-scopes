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
        //private string playerNickname;

        private float playerDirection;

        //private KeyCode keyLeft; 
        //private KeyCode keyRight;

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
        private float angleScalar;

    	// Use this for initialization
        void Awake () 
        {
            holeDelay = 0;
            holeTimerDelay = 0;
            holeTimer = 1000;
            holeSize = 50;

            active = false;
            visible = false;

            borderHeads = new List<GameObject>();
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

            // Sets Player object to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
    	}
    	
    	// Update is called once per frame
    	/*void Update () {
    	
    	}*/
            
        public void SetupPlayer(string nickname, Color color, KeyCode[] movementKeys)
        {
            arenaSize = GameManager.instance.GameConfiguration.ArenaSize;
            arenaScalar = 6.0f / arenaSize;
            angleScalar = 0.015f;

            playerPos = new Vector2(Random.Range(20.0f, arenaSize - 20),
                                    Random.Range(20.0f, arenaSize - 20));

            playerSize = GameManager.instance.GameConfiguration.PlayerSize;
            playerSpeed = GameManager.instance.GameConfiguration.PlayerSpeed;
            playerDirection = Random.Range(0.0f, 2.0f); 

            playerColor = color;
            //playerNickname = nickname;
            Nickname = nickname;

            //keyLeft = movementKeys[0];
            //keyRight = movementKeys[1];

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

            playerSize = GameManager.instance.GameConfiguration.PlayerSize;
            playerSpeed = GameManager.instance.GameConfiguration.PlayerSpeed;
            playerDirection = Random.Range(0.0f, 2.0f);

            float headSize = 2 * arenaScalar * playerSize;

            this.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            this.transform.localScale = new Vector3 (headSize, headSize, headSize);

            foreach (GameObject head in borderHeads)
            {
                head.transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
            }

            holeDelay = 0;
            holeTimerDelay = 0;
            visible = false;
            active = true;
        }


        public void Turn ()
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
        }

        public void DoubleSize()
        {
            if (playerSize < 6) {
                playerSize += 1;
            }
            else if (playerSize >= 6 && playerSize <= 24) {
                playerSize += 6;
            }
        }

        public void ReduceSize()
        {
            if (playerSize > 6) {
                playerSize -= 6;
            } else if (playerSize > 2 && playerSize <= 6) {
                playerSize -= 1;
            }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
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
    }
}
