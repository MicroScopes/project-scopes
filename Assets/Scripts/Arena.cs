/*!
 * @file Arena.cs
 * @brief Contains Arena class definition.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arena : MonoBehaviour 
{
	private Color[] pixMap;					// Table of texture pixel colors
	private MeshRenderer renderer;			// Arena renderer component
	private Texture2D texture;				// Arena texture

	private int arenaSize;

	private float frameRate;
	private float nextFrame;

	private int initSize = 4;
	private float initSpeed = 2.0f;

	private int noOfPlayers = 1;
	private List<Player> players = new List<Player>();


	// Use this for initialization
	void Start() 
	{		
		frameRate 		= 0.01f;
		arenaSize 		= 800;

		renderer 	= GetComponent<MeshRenderer> ();
		texture 	= new Texture2D (arenaSize,arenaSize);
		pixMap 		= texture.GetPixels ();

		texture.filterMode = FilterMode.Point;//Trilinear;

		// Setting texture to Arena renderer
		renderer.material.mainTexture = texture;

		// Setting Arena background color
		SetArenaBgColor (Color.black);

		for (int i = 0; i < noOfPlayers; i++) {
			players.Add (new Player ());
		}

		foreach (Player player in players) {
			player.SetupPlayer(new Vector2 (Random.Range(20.0f, arenaSize-20), 
											Random.Range(20.0f, arenaSize-20)), 
											Random.Range(0.0f, 2.0f), 
											initSize, 
											initSpeed, 
											Color.red);
		}

		//players [1].ChangeColor (Color.green);
		//players [1].SetControlKeys (KeyCode.Z, KeyCode.X);
	}


	// Update is called once per frame
	void Update () 
	{
		if (Time.time > nextFrame) 
		{
			// Setting next frame delay
			nextFrame = Time.time + frameRate;

			foreach (Player player in players) 
			{
				player.Turn ();
			}
				
			RedrawArena ();
		}

		if(Input.GetKeyDown(KeyCode.G))
		{
			players [0].DoubleSize ();
		}

		if(Input.GetKeyDown(KeyCode.H))
		{
			players [0].ReduceSize ();
		}

		if(Input.GetKeyDown(KeyCode.J))
		{
			players [0].DoubleSpeed ();
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			players [0].ReduceSpeed ();
		}
		// helpful for getting KeyCode of pressed key (to set preferred keys for players in GUI)

		/*foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
				Debug.Log("KeyCode down: " + kcode);
		}*/
	}


	void RedrawArena ()
	{
		DrawPlayers ();

		texture.SetPixels (pixMap);
		texture.Apply (false);
	}


	void DrawPlayers ()
	{
		foreach (Player player in players)
		{
			float playerPosX = player.GetX ();
			float playerPosY = player.GetY ();

			Color playerColor = player.GetColor ();

			int playerSize = player.GetSize ();
			float playerSpeed = player.GetSpeed ();

			float sinDegree = Mathf.Sin (Mathf.PI * player.GetDegree ());
			float cosDegree = Mathf.Cos (Mathf.PI * player.GetDegree ());

			for (int j = 0; j < Mathf.CeilToInt(playerSpeed); j++) 
			{
				if (player.isActive ()) 
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

						for (int k = 1; k <= Mathf.CeilToInt (playerSize * 0.5f) + 3; k++) {
							DrawPixel (posX2 - sinDegree * k / 5.0f, posY2 - cosDegree * k / 5.0f, playerColor, true);
						}

					if ((i % 2 == 0 || i == 1) && DrawPixel (posX2 + 2.8f * sinDegree, posY2 + 2.8f * cosDegree, Color.green, false)) {
							player.Collision ();
						} 

						DrawPixel (posX2, posY2, playerColor, true);
					}
				}
			}

			player.SetX (playerPosX);
			player.SetY (playerPosY);
		}
	}


	/*void DrawPlayers2 ()
	{
		foreach (Player player in players)
		{
			if (player.isActive ()) 
			{
				float playerPosX = player.GetX ();
				float playerPosY = player.GetY ();

				Color playerColor = player.GetColor ();

				int playerSize = player.GetSize ();
				float playerSpeed = player.GetSpeed ();

				float sinDegree = Mathf.Sin (Mathf.PI * player.GetDegree ()) / (playerSize * 0.2f);
				float cosDegree = Mathf.Cos (Mathf.PI * player.GetDegree ()) / (playerSize * 0.2f);

				for (int j = 0; j < Mathf.CeilToInt(playerSpeed * playerSize * 0.1f); j++) 
				{
					playerPosX += sinDegree;
					playerPosY += cosDegree;

					float posX = playerPosX - playerSize * cosDegree * (playerSize * 0.2f);
					float posY = playerPosY + playerSize * sinDegree * (playerSize * 0.2f);

					for (int i = 0; i < 2 * playerSize; i++) {
						float posX2 = posX + i * playerSize * cosDegree * (playerSize * 0.2f) / playerSize;
						float posY2 = posY - i * playerSize * sinDegree * (playerSize * 0.2f) / playerSize;

						DrawPixel (posX2, posY2, playerColor);

						//if (DrawPixel (posX2 + sinDegree, posY2 + cosDegree, playerColor)) {
						//	player.Collision ();
						//}

						//for (int k = 1; k <= Mathf.CeilToInt(playerSize * 0.5f) + 2; k++) 
						{
							//DrawPixel (posX2 - sinDegree*k/5.0f, posY2 - cosDegree*k/5.0f, playerColor);
						}
					}
				}

				player.SetX (playerPosX);
				player.SetY (playerPosY);
			}
		}
	}*/


	bool DrawPixel (float x, float y, Color col, bool collCheck)
	{
		int pos = (Mathf.FloorToInt(y) * arenaSize + Mathf.FloorToInt(x)) % pixMap.Length;

		if (pos < 0) 
		{
			pos = (pixMap.Length + pos) % pixMap.Length;
		}

		if (pixMap [pos] == Color.black || pixMap [pos] == Color.green) 
		{
			if (collCheck) {
				pixMap [pos] = col;
			}
			return false;
		} 
		else 
		{
			return true;
		}
	}


	void SetArenaBgColor (Color col)
	{
		for (int i = 0; i < pixMap.Length; i++) 
		{
			pixMap [i] = col;
		}

		texture.SetPixels (pixMap);
		texture.Apply (false);
	}
}
