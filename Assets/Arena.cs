using UnityEngine;
using System.Collections;
using ProjectScopes;

public class Arena : MonoBehaviour 
{
	private Color[] pixMap;					// Table of texture pixel colors
	private MeshRenderer renderer;			// Arena renderer component
	private Texture2D texture;				// Arena texture

	private int arenaSize;

	private float frameRate;
	private float nextFrame;

	private int initSize = 5;
	private int initSpeed = 2;

	private Player[] players = new Player[2];

    private Configurator configurator = GUIDataCollector.configurator;

	// Use this for initialization
	void Start() 
	{
        Debug.Log(configurator.CurrentNoOfPlayers);

		frameRate 		= 0.01f;
		arenaSize 		= 600;

		renderer 	= GetComponent<MeshRenderer> ();
		texture 	= new Texture2D (arenaSize,arenaSize);
		pixMap 		= texture.GetPixels ();

		texture.filterMode = FilterMode.Trilinear;

		// Setting texture to Arena renderer
		renderer.material.mainTexture = texture;

		// Setting Arena background color
		SetArenaBgColor (Color.black);

		players[0] = new Player (new Vector2 (10.0f, 0.0f), 0.33f, initSize, initSpeed, Color.red);
		players[1] = new Player (new Vector2 (300.0f, 100.0f), 0.0f, initSize, initSpeed, Color.green);
	}


	// Update is called once per frame
	void Update () 
	{
		if (Time.time > nextFrame) 
		{
			// Setting next frame delay
			nextFrame = Time.time + frameRate;

			/*foreach (Player player in players) 
			{
				player.Turn (Input.GetAxis ("Horizontal"));
			}*/

			players[0].Turn (Input.GetAxis ("Horizontal"));
			players[1].Turn (Input.GetAxis ("Vertical"));

			RedrawArena ();
		}
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

			float sinDegree = Mathf.Sin (Mathf.PI * player.GetDegree());
			float cosDegree = Mathf.Cos (Mathf.PI * player.GetDegree());

			int playerSize = player.GetSize ();

			for (int j = 0; j < player.GetSpeed(); j++) 
			{
				playerPosX += sinDegree;
				playerPosY += cosDegree;

				float posX = playerPosX - playerSize * cosDegree;
				float posY = playerPosY + playerSize * sinDegree;

				for (int i = 0; i < 2 * playerSize; i++) 
				{
					float posX2 = posX + i * playerSize * cosDegree / playerSize;
					float posY2 = posY - i * playerSize * sinDegree / playerSize;

					DrawPixel (posX2, posY2, playerColor);

					DrawPixel (posX2-sinDegree / 4, posY2-cosDegree / 4, playerColor);
					DrawPixel (posX2+sinDegree / 4, posY2+cosDegree / 4, playerColor);

					DrawPixel (posX2-sinDegree / 2, posY2-cosDegree / 2, playerColor);
					DrawPixel (posX2+sinDegree / 2, posY2+cosDegree / 2, playerColor);

					DrawPixel (posX2-sinDegree, posY2-cosDegree, playerColor);
					DrawPixel (posX2+sinDegree, posY2+cosDegree, playerColor);
				}
			}

			player.SetX (playerPosX);
			player.SetY (playerPosY);
		}
	}


	void DrawPixel (float x, float y, Color col)
	{
		int pos = (Mathf.FloorToInt(y) * arenaSize + Mathf.FloorToInt(x)) % pixMap.Length;

		if (pos < 0) 
		{
			pos = (pixMap.Length + pos) % pixMap.Length;
		}

		pixMap[pos] = col;
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
