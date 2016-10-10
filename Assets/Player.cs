using UnityEngine;
using System.Collections;

public class Player// : MonoBehaviour 
{
	private Vector2 playerPos;
	private Color playerColor;

	private int playerSize;
	private int playerSpeed;

	private float playerDegree;


	public Player (Vector2 startPos, float startDeg, int size, int speed, Color col)
	{
		playerPos = startPos;

		playerSize 		= size;
		playerSpeed 	= speed;
		playerDegree 	= startDeg;	

		playerColor = col;
	}

	public void Turn (float turn)
	{
		playerDegree += 0.02f * turn;
	}

	public float GetX ()
	{
		return playerPos.x;
	}

	public float GetY ()
	{
		return playerPos.y;
	}

	public int GetSpeed ()
	{
		return playerSpeed;
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
}
