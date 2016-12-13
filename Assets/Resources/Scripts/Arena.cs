/*!
 * @file    Arena.cs
 * @brief   Contains Arena class definition.
 * @author  Marcin
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;

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
 * @brief   Arena is responsible of main play area in game.
 * 
 * @details Arena class is a conponent script of Arena prefab. It handles setup of main arena texture, 
 *          drawing of player traces and check players collisions. It uses configuration data and
 *          players list directly from GameManager instance.
 */

    public class Arena : MonoBehaviour 
    {
        private int arenaSize;

    	private Color[] mainPixelMap;
        private Texture2D mainArenaTexture;

    	
        //void Awake() 
        //{
        //    
    	//}


        public void SetupArena(int arenaSize)
        {
            this.arenaSize = arenaSize;

            // Setup main texture
            mainArenaTexture = new Texture2D (arenaSize,arenaSize);
            mainPixelMap = mainArenaTexture.GetPixels ();

            ClearArena ();

            // Antialiasing mode
            mainArenaTexture.filterMode = FilterMode.Trilinear;
            mainArenaTexture.alphaIsTransparency = true;

            // Setting texture to Arena renderer
            MeshRenderer renderer = GetComponent<MeshRenderer> ();
            renderer.sharedMaterial.mainTexture = mainArenaTexture;
        }

        // MAIN COLLISION DETECTION ALGHORITM
        // Checks collision and draws each active player on main texture
        public void RedrawPlayers (GameManager manager)
        {
            foreach (Player player in manager.players)
            {
                // Move only active players
                if (player.IsActive)
                {
                    int playerSize = player.Size;
                    float playerSpeed = player.Speed;

                    // Delta of position change ( values between -1 and 1)
                    float deltaX = Mathf.Sin(Mathf.PI * player.Direction);
                    float deltaY = Mathf.Cos(Mathf.PI * player.Direction);

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
                                    if (player.IsActive && !player.IsGodMode() && CheckCollision(posX + collisionFactor * deltaY, posY + collisionFactor * deltaX))
                                    {
                                        player.IsActive = false;
                                        manager.AddPoints();
                                    }
                                }

                                DrawPixel(posX, posY, player.Colour);
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

            RefreshTexture();
        }


        bool CheckCollision(float x, float y)
        {
            int pixMapIndex = PositionToPixMapIndex(x, y);

            if (mainPixelMap[pixMapIndex].a != 0f)// != Color.black) 
            {
                return true;
            }

            return false;
        }


    	void DrawPixel (float x, float y, Color col)
    	{
            int pixMapIndex = PositionToPixMapIndex(x, y);

            mainPixelMap[pixMapIndex] = col;
    	}
            
        // Maps 2D position coordinates to pixel table index
        int PositionToPixMapIndex(float x, float y)
        {
            int pixelPosition = (Mathf.FloorToInt (y) * arenaSize + Mathf.FloorToInt (x)) % mainPixelMap.Length;

            if (pixelPosition < 0) 
            {
                pixelPosition = (mainPixelMap.Length + pixelPosition) % mainPixelMap.Length;
            }

            return pixelPosition;
        }


    	void ClearArena ()
    	{
            Color background = new Color(0f, 0f, 0f, 0f);

    		for (int i = 0; i < mainPixelMap.Length; i++) 
    		{
                mainPixelMap[i] = background;
    		}

            RefreshTexture();
    	}


        void RefreshTexture()
        {
            mainArenaTexture.SetPixels (mainPixelMap);
            mainArenaTexture.Apply (false);
        }
    }
}