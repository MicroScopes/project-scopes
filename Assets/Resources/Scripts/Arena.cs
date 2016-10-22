/*!
 * @file Arena.cs
 * @brief Contains Arena class definition.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectScopes
{
    public class Arena : MonoBehaviour 
    {
    	public GameObject head;

    	private Color[] mianPixMap;				// Table of texture pixel colors
    	private Texture2D mainTexture;			// Arena texture

    	private int arenaSize;

    	// Use this for initialization
        void Awake() 
        {
            
    	}

        public void RedrawArena ()
    	{
            DrawPlayers (GameManager.instance.players);

    		mainTexture.SetPixels (mianPixMap);
    		mainTexture.Apply (false);
    	}


        public void SetupArena(int arenaInitSize)
        {
            arenaSize = arenaInitSize;

            mainTexture = new Texture2D (arenaSize,arenaSize);
            mianPixMap = mainTexture.GetPixels ();

            mainTexture.filterMode = FilterMode.Trilinear;

            // Setting texture to Arena renderer
            MeshRenderer renderer = GetComponent<MeshRenderer> ();
            renderer.sharedMaterial.mainTexture = mainTexture;

            // Setting Arena background color
            SetArenaBgColor (Color.black);
        }


        void DrawPlayers (List<Player> players)
    	{
            foreach (Player player in players)
    		{
    			float playerPosX = player.PosX;
    			float playerPosY = player.PosY;

    			Color playerColor = player.PlayerColor;

    			int playerSize = player.PlayerSize;
    			float playerSpeed = player.PlayerSpeed;

    			float sinDegree = Mathf.Sin (Mathf.PI * player.PlayerDirection);
    			float cosDegree = Mathf.Cos (Mathf.PI * player.PlayerDirection);

    			for (int j = 0; j < Mathf.CeilToInt(playerSpeed); j++) 
    			{
    				if (player.IsActive) 
    				{
    					if (playerSpeed < 1) {
    						playerPosX += sinDegree * playerSpeed;
    						playerPosY += cosDegree * playerSpeed;
    					} else {
    						playerPosX += sinDegree;
    						playerPosY += cosDegree;
    					}

    					float posX = playerPosX - playerSize * cosDegree;
    					float posY = playerPosY + playerSize * sinDegree;

    					for (int i = 1; i < 2 * playerSize; i++)
    					{
    						float posX2 = posX + i * playerSize * cosDegree / playerSize;
    						float posY2 = posY - i * playerSize * sinDegree / playerSize;

    						for (int k = 1; k <= Mathf.CeilToInt (playerSize * 0.7f) + 3; k++) {
    							DrawPixel (mianPixMap, posX2 - sinDegree * k * 0.2f, posY2 - cosDegree * k * 0.2f, playerColor, player.IsVisible(), true);
    						}
    							
    						if ((i % 2 == 0 || i == 1) && DrawPixel (mianPixMap, posX2 + (playerSize * 0.5f + 1) * sinDegree, posY2 + (playerSize * 0.3f + 1) * cosDegree, Color.green, player.IsVisible(), false)) {
    							player.IsActive = false;
    						} 

    						DrawPixel (mianPixMap, posX2, posY2, playerColor, player.IsVisible(), true);
    					}
    				}
    			}

    			player.PosX = playerPosX;
    			player.PosY = playerPosY;

                player.MoveHead();
    		}
    	}


    	bool DrawPixel (Color[] map, float x, float y, Color col, bool visible, bool collCheck)
    	{
    		int pos = (Mathf.FloorToInt (y) * arenaSize + Mathf.FloorToInt (x)) % map.Length;

    		if (pos < 0) 
    		{
    			pos = (map.Length + pos) % map.Length;
    		}

    		if (visible) 
    		{
    			if (map [pos] == Color.black) 
    			{
    				if (collCheck) {
    					map [pos] = col;
    				}
    				return false;
    			} 
    			else 
    			{
    				return true;
    			}
    		}

    		return false;
    	}


    	void SetArenaBgColor (Color col)
    	{
    		for (int i = 0; i < mianPixMap.Length; i++) 
    		{
    			mianPixMap [i] = col;
    		}

    		mainTexture.SetPixels (mianPixMap);
    		mainTexture.Apply (false);
    	}
    }
}