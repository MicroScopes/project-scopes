using UnityEngine;
using System.Collections;

namespace ProjectScopes
{
    
    public class Bonus : MonoBehaviour 
    {
        int effectType;
        GameManager manager;
        private Texture bonusTexture;

        int bonusTime = 4;

    	// Use this for initialization
    	void Start () 
        {
            effectType = Random.Range(0, 6);
            manager = gameObject.transform.GetComponentInParent<GameManager>();

            string texPath = "Textures/" + effectType;

            bonusTexture = (Texture)Resources.Load(texPath);

            MeshRenderer renderer = GetComponent<MeshRenderer> ();
            renderer.material.mainTexture = bonusTexture;
    	}
    	
    	// Update is called once per frame
    	void Update () 
        {
    	
    	}

        public void SetPosition(float arenaSize)
        {
            gameObject.transform.position = new Vector3(Random.Range(1f, arenaSize - 1f),
                                                        Random.Range(1f, arenaSize - 1f), 1f);
        }

        void OnTriggerEnter(Collider obj) 
        {
            if (obj.tag == "Player")
            {
                StartCoroutine(ApplyEffect(obj.GetComponent<Player>()));

                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

        private IEnumerator ApplyEffect(Player player)
        {
            switch (effectType)
            {
                case 0:
                    player.ReduceSize();
                    break;
                case 1:
                    player.IncreaseSpeed();
                    break;
                case 2:
                    player.ReduceSpeed();
                    break;
                case 3:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.DoubleSize();
                        }
                    }
                    break;
                case 4:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.IncreaseSpeed();
                        }
                    }
                    break;
                case 5:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.ReduceSpeed();
                        }
                    }
                    break;
                /*case 6:
                    player.BigTurns = true;
                    break;
                case 7:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.BigTurns = true;
                        }
                    }
                    break;*/
                case 6:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.TurnsDirection = -1;
                        }
                    }
                    break;
            }

            yield return new WaitForSeconds(bonusTime);

            switch (effectType)
            {
                case 0:
                    player.DoubleSize();
                    break;
                case 1:
                    player.ReduceSpeed();
                    break;
                case 2:
                    player.IncreaseSpeed();
                    break;
                case 3:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.ReduceSize();
                        }
                    }
                    break;
                case 4:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.ReduceSpeed();
                        }
                    }
                    break;
                case 5:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.IncreaseSpeed();
                        }
                    }
                    break;
                /*case 6:
                    player.BigTurns = false;
                    break;
                case 7:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.BigTurns = false;
                        }
                    }
                    break;*/
                case 6:
                    foreach (Player play in manager.players)
                    {
                        if (play.Nickname != player.Nickname && play.IsActive)
                        {
                            play.TurnsDirection = 1;
                        }
                    }
                    break;
            }

            Destroy(gameObject);
        }
    }
}
