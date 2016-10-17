using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
	private Vector2 playerPos;
	private Color playerColor;

	private int playerSize;
	private float playerSpeed;

	private float playerDirection;

	private KeyCode keyLeft; 
	private KeyCode keyRight;

	private bool active;
	private bool visible;

	private int holeDelay;
	private int holeTimerDelay;
	private int holeTimer;
	private int holeSize;

	private GameObject playerHead;
	private GameObject borderHead1;
	private GameObject borderHead2;
	private GameObject borderHead3;

	public Player (GameObject head, GameObject bHead1, GameObject bHead2, GameObject bHead3)
	{
		playerPos = new Vector2(0.0f, 0.0f);

		playerSize 		= 4;
		playerSpeed 	= 1.0f;
		playerDirection = 0.0f;

		holeDelay = 0;
		holeTimerDelay = 0;
		holeTimer = 1000;
		holeSize = 50;

		active = true;
		visible = true;

		playerColor = Color.white;

		keyLeft = KeyCode.LeftArrow;
		keyRight = KeyCode.RightArrow;

		playerHead = head;
		borderHead1 = bHead1;
		borderHead2 = bHead2;
		borderHead3 = bHead3;
	}

	public void SetupPlayer(Vector2 startPos, float startDeg, int size, float speed, Color col)
	{
		playerPos = startPos;

		playerSize 		= size;
		playerSpeed 	= speed;
		playerDirection = startDeg;	

		playerColor = col;

		keyLeft = KeyCode.LeftArrow;
		keyRight = KeyCode.RightArrow;

		borderHead1.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);
		borderHead2.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);
		borderHead3.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);

		MeshRenderer renderer = playerHead.GetComponent<MeshRenderer> ();
		renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);

		renderer = borderHead1.GetComponent<MeshRenderer> ();
		renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
		renderer = borderHead2.GetComponent<MeshRenderer> ();
		renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
		renderer = borderHead3.GetComponent<MeshRenderer> ();
		renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
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

			playerHead.transform.localScale = new Vector3 (headSize, headSize, headSize);

			borderHead1.transform.localScale = new Vector3 (headSize, headSize, headSize);
			borderHead2.transform.localScale = new Vector3 (headSize, headSize, headSize);
			borderHead3.transform.localScale = new Vector3 (headSize, headSize, headSize);
		}
	}

	public void MoveHead(float x, float y, int arenaSize)
	{
		int corner = 0;

		playerHead.transform.position = new Vector3(x*0.01f, y*0.01f, 0.0f);

		borderHead1.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);
		borderHead2.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);
		borderHead3.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);

		if (y < playerSize) {
			corner = 1;
			borderHead1.transform.position = new Vector3 (x * 0.01f, (y + arenaSize) * 0.01f, 0.0f);
		} if (y > arenaSize - playerSize) {
			corner += 2;
			borderHead1.transform.position = new Vector3 (x * 0.01f, (y - arenaSize) * 0.01f, 0.0f);
		} if (x < playerSize) {
			corner += 4;
			borderHead1.transform.position = new Vector3 ((x + arenaSize) * 0.01f, y * 0.01f, 0.0f);
		} if (x > arenaSize - playerSize) {
			corner += 8;
			borderHead1.transform.position = new Vector3 ((x - arenaSize) * 0.01f, y * 0.01f, 0.0f);
		}

		switch (corner) {
		case 5:
			borderHead1.transform.position = new Vector3 (x * 0.01f, (y + arenaSize) * 0.01f, 0.0f);
			borderHead2.transform.position = new Vector3 ((x + arenaSize) * 0.01f, (y + arenaSize) * 0.01f, 0.0f);
			borderHead3.transform.position = new Vector3 ((x + arenaSize) * 0.01f, y * 0.01f, 0.0f);
			break;
		case 6:
			borderHead1.transform.position = new Vector3 (x * 0.01f, (y - arenaSize) * 0.01f, 0.0f);
			borderHead2.transform.position = new Vector3 ((x + arenaSize) * 0.01f, (y - arenaSize) * 0.01f, 0.0f);
			borderHead3.transform.position = new Vector3 ((x + arenaSize) * 0.01f, y * 0.01f, 0.0f);
			break;
		case 9:
			borderHead1.transform.position = new Vector3 (x * 0.01f, (y + arenaSize) * 0.01f, 0.0f);
			borderHead2.transform.position = new Vector3 ((x - arenaSize) * 0.01f, (y + arenaSize) * 0.01f, 0.0f);
			borderHead3.transform.position = new Vector3 ((x - arenaSize) * 0.01f, y * 0.01f, 0.0f);
			break;			
		case 10:
			borderHead1.transform.position = new Vector3 (x * 0.01f, (y - arenaSize) * 0.01f, 0.0f);
			borderHead2.transform.position = new Vector3 ((x - arenaSize) * 0.01f, (y - arenaSize) * 0.01f, 0.0f);
			borderHead3.transform.position = new Vector3 ((x - arenaSize) * 0.01f, y * 0.01f, 0.0f);
			break;
		}
	}

	public float PosX
	{
		get { return playerPos.x; }
		set { playerPos.x = value; }
	}

	public float PosY
	{
		get { return playerPos.y; }
		set { playerPos.y = value; }
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
		set 
		{ 
			playerColor = value;

			MeshRenderer renderer = playerHead.GetComponent<MeshRenderer> ();
			renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);

			renderer = borderHead1.GetComponent<MeshRenderer> ();
			renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
			renderer = borderHead2.GetComponent<MeshRenderer> ();
			renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
			renderer = borderHead3.GetComponent<MeshRenderer> ();
			renderer.material.color = playerColor + new Color(0.5f, 0.5f, 0.5f);
		}
	}

	public void DoubleSize()
	{
		if (playerSize < 4) {
			playerSize += 1;
		}
		else if (playerSize >= 4 && playerSize <= 20) {
			playerSize += 4;
		}
	}

	public void ReduceSize()
	{
		if (playerSize > 4) {
			playerSize -= 4;
		} else if (playerSize > 2 && playerSize <= 4) {
			playerSize -= 1;
		}
	}

	public void SetControlKeys(KeyCode keyL, KeyCode keyR)
	{
		keyLeft = keyL;
		keyRight = keyR;
	}

	public bool IsActive
	{
		get { return active; }
		set { active = value; }
	}

	public bool isVisible()
	{
		return visible;
	}
}
