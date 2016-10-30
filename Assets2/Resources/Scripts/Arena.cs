/*!
 * @file Arena.cs
 * @brief Contains Arena class definition.
 * @author Marcin
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
 * @detail Contains all project-scopes related classes.
 */
namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief MonoBehavior for Arena prefab
 * 
 * @details Arena class is a conponent script of Arena prefab. It handles setup of main arena texture, 
 *          drawing of player traces and check players collisions. It uses configuration data and
 *          players list directly from GameManager instance
 */

    public class Arena : MonoBehaviour 
    {
        private int arenaSize;

    	private Color[] mainPixelMap;
    	private Texture2D mainTexture;

    	// Initialization
        void Awake() 
        {
            // empty?
    	}


        public void SetupArena()
        {
            arenaSize = GameManager.instance.GameConfiguration.InitialArenaSize;

            // Setup main texture
            mainTexture = new Texture2D (arenaSize,arenaSize);
            mainPixelMap = mainTexture.GetPixels ();

            // Setting Arena background color
            SetArenaBgColor (Color.black);

            // Antialiasing mode
            mainTexture.filterMode = FilterMode.Trilinear;

            // Setting texture to Arena renderer
            MeshRenderer renderer = GetComponent<MeshRenderer> ();
            renderer.sharedMaterial.mainTexture = mainTexture;
        }
            

        public void RedrawArena ()
        {
            DrawPlayers ();

            RefreshTexture();
        }

        // Checks collision and draws each active player on main texture
        void DrawPlayers ()
        {
            foreach (Player player in GameManager.instance.players)
            {
                // Move only active players
                if (player.IsActive)
                {
                    int playerSize = player.PlayerSize;
                    float playerSpeed = player.PlayerSpeed;

                    // Delta of position change ( values between -1 and 1)
                    float deltaX = Mathf.Sin(Mathf.PI * player.PlayerDirection);
                    float deltaY = Mathf.Cos(Mathf.PI * player.PlayerDirection);

                    // Indicates an amount of lines to fill space between next player positions
                    int densityFactor = 3 * Mathf.CeilToInt(playerSpeed + playerSize * 0.2f);

                    // Indicates space in front of line where collision is checked
                    float collisionFactor = playerSize * 0.3f + playerSpeed * 0.2f + 2;

                    // Density of filling lines depending on the player speed and player size
                    for (int j = 0; j <= densityFactor; j++)
                    {
                        // ( left end calculation according to angle )  +  ( next filling line towards movement )
                        float posX = (player.PosX - playerSize * deltaX) + (deltaY * j * 0.33f);
                        float posY = (player.PosY + playerSize * deltaY) + (deltaX * j * 0.33f);

                        // Drawing line from left end
                        for (int i = 1; i < 2 * playerSize; i++)
                        {
                            posX += deltaX;
                            posY -= deltaY;

                            // Draw player only if it is visible
                            if (player.IsVisible())
                            {
                                // Check collision every second pixel horizontally and vertically
                                if (i % 2 == 0 && j % 2 == 1)
                                {
                                    // Check collision in front of the line
                                    if (CheckCollision(posX + collisionFactor * deltaY, posY + collisionFactor * deltaX))
                                    {
                                        player.IsActive = false;
                                    }
                                }

                                DrawPixel(posX, posY, player.PlayerColor);
                            }
                        }
                    }

                    // Update player position
                    player.PosX += deltaY * playerSpeed;
                    player.PosY += deltaX * playerSpeed;

                    // Move player head object
                    player.MoveHead();
                }
            }
        }


        bool CheckCollision(float x, float y)
        {
            int index = PositionToPixMapIndex(x, y);

            if (mainPixelMap[index] != Color.black) 
            {
                return true;
            }

            return false;
        }


    	void DrawPixel (float x, float y, Color col)
    	{
            int index = PositionToPixMapIndex(x, y);

            mainPixelMap[index] = col;
    	}

        // Maps 2D position coordinates to pixel table index
        int PositionToPixMapIndex(float x, float y)
        {
            int pos = (Mathf.FloorToInt (y) * arenaSize + Mathf.FloorToInt (x)) % mainPixelMap.Length;

            if (pos < 0) 
            {
                pos = (mainPixelMap.Length + pos) % mainPixelMap.Length;
            }

            return pos;
        }


    	void SetArenaBgColor (Color col)
    	{
    		for (int i = 0; i < mainPixelMap.Length; i++) 
    		{
    			mainPixelMap [i] = col;
    		}

            RefreshTexture();
    	}


        void RefreshTexture()
        {
            mainTexture.SetPixels (mainPixelMap);
            mainTexture.Apply (false);
        }
    }
}