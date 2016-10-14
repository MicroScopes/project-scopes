﻿using UnityEngine;
using System.Collections;

public class Player// : MonoBehaviour 
{
	private Vector2 playerPos;
	private Color playerColor;

	private int playerSize;
	private float playerSpeed;

	private float playerDegree;

	private KeyCode keyLeft; 
	private KeyCode keyRight;

	private int direction = 0;

	private bool active = true;
	private bool visible = true;

	private int holeDelay = 0;
	private int holeTimerDelay = 0;
	private int holeTimer = 1000;
	private int holeSize = 50;

	private GameObject playerHead;
	private GameObject borderHead;

	public Player (GameObject head, GameObject bHead)
	{
		playerPos = new Vector2(0.0f, 0.0f);

		playerSize 		= 4;
		playerSpeed 	= 1.0f;
		playerDegree 	= 0.0f;	

		playerColor = Color.white;

		keyLeft = KeyCode.LeftArrow;
		keyRight = KeyCode.RightArrow;

		playerHead = head;
		borderHead = bHead;
	}

	public void SetupPlayer(Vector2 startPos, float startDeg, int size, float speed, Color col)
	{
		playerPos = startPos;

		playerSize 		= size;
		playerSpeed 	= speed;
		playerDegree 	= startDeg;	

		playerColor = col;

		keyLeft = KeyCode.LeftArrow;
		keyRight = KeyCode.RightArrow;

		borderHead.transform.position = new Vector3(-playerSize, -playerSize, 0.0f);
	}

	public void Turn ()
	{
		if (active) {

			int hSize = Mathf.CeilToInt((holeSize / playerSpeed) * playerSize);

			if (Input.GetKey (keyLeft)) {
				playerDegree -= 0.02f + playerSpeed * 0.002f;
			}
			if (Input.GetKey (keyRight)) {
				playerDegree += 0.02f + playerSpeed * 0.002f;
			}

			if (holeDelay < holeTimer) {
				holeDelay+=10;
			} else {
				visible = false;
				if (holeTimerDelay < hSize) {
					holeTimerDelay+=10;
				} else {
					visible = true;
					holeDelay = 0;
					holeTimerDelay = 0;
				}
			}

			//playerHead.transform.position = new Vector3(playerPos.x*0.01f, playerPos.y*0.01f, 0.0f);
			playerHead.transform.localScale = new Vector3 (playerSize*0.02f, playerSize*0.02f, playerSize*0.02f);
			borderHead.transform.localScale = new Vector3 (playerSize*0.02f, playerSize*0.02f, playerSize*0.02f);
			//MoveHead();
		}
	}

	public void MoveHead(float x, float y, int arenaSize)
	{
		playerHead.transform.position = new Vector3(x*0.01f, y*0.01f, 0.0f);

		if (x < 0 + playerSize) {
			borderHead.transform.position = new Vector3 ((x + arenaSize) * 0.01f, y * 0.01f, 0.0f);
		}

		if (x > arenaSize - playerSize) {
			borderHead.transform.position = new Vector3 ((x - arenaSize) * 0.01f, y * 0.01f, 0.0f);
		}

		if (y < 0 + playerSize) {
			borderHead.transform.position = new Vector3 (x * 0.01f, (y + arenaSize) * 0.01f, 0.0f);
		}

		if (y > arenaSize - playerSize) {
			borderHead.transform.position = new Vector3 (x * 0.01f, (y - arenaSize) * 0.01f, 0.0f);
		}
	}

	public float GetX ()
	{
		return playerPos.x;
	}

	public float GetY ()
	{
		return playerPos.y;
	}

	public float GetSpeed ()
	{
		return playerSpeed;
	}

	public void DoubleSpeed ()
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

	public int GetSize ()
	{
		return playerSize;
	}

	public float GetDegree()
	{
		return playerDegree;
	}

	public void SetX (float x)
	{
		playerPos.x = x;
	}

	public void SetY (float y)
	{
		playerPos.y = y;
	}

	public Color GetColor()
	{
		return playerColor;
	}

	public void ChangeColor(Color col)
	{
		playerColor = col;
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

	public bool isActive()
	{
		return active;
	}

	public bool isVisible()
	{
		return visible;
	}

	public void Collision()
	{
		active = false;
	}
}
