using UnityEngine;
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

	public Player ()
	{
		playerPos = new Vector2(0.0f, 0.0f);

		playerSize 		= 4;
		playerSpeed 	= 1.0f;
		playerDegree 	= 0.0f;	

		playerColor = Color.white;

		keyLeft = KeyCode.LeftArrow;
		keyRight = KeyCode.RightArrow;
	}

	/*public Player (Vector2 startPos, float startDeg, int size, int speed, Color col)
	{
		playerPos = startPos;

		playerSize 		= size;
		playerSpeed 	= speed;
		playerDegree 	= startDeg;	

		playerColor = col;
	}*/

	public void SetupPlayer(Vector2 startPos, float startDeg, int size, float speed, Color col)
	{
		playerPos = startPos;

		playerSize 		= size;
		playerSpeed 	= speed;
		playerDegree 	= startDeg;	

		playerColor = col;

		keyLeft = KeyCode.LeftArrow;
		keyRight = KeyCode.RightArrow;
	}

	public void Turn ()
	{
		if (Input.GetKey (keyLeft)) 
		{
			playerDegree -= 0.02f + playerSpeed * 0.002f;
		}
		if (Input.GetKey (keyRight)) 
		{
			playerDegree += 0.02f + playerSpeed * 0.002f;
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
		} else if (playerSize > 1 && playerSize <= 4) {
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

	public void Collision()
	{
		active = false;
		Debug.Log ("Collided");
	}
}
