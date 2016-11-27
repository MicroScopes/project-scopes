using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectScopes
{ 
    public class Level : MonoBehaviour 
    {
        // Countdown between levels starts with this value.
        private const int Counter = 3;

        // The duration between two counter values.
        private const float Timeout = 1.0f;

        private Arena arena;
        public List<Player> players;
        private Configurator gameConfiguration;

        // Variables used for simple frame rate control
        private float frameRate;
        private float nextFrame;

        private bool pause;

        // Use this for initialization
        void Awake ()
        {
            gameConfiguration = GUIManager.configurator;

            Screen.SetResolution(gameConfiguration.ArenaSize, gameConfiguration.ArenaSize, false);

            // Creates a default set of 6 inactive players
            LoadPlayers();

            frameRate = 0.01f;
            nextFrame = 0.0f;

            pause = false;
        }

        //This is called each time a scene is loaded.
        void OnLevelWasLoaded()
        {
            this.enabled = true;
            SetupLevel();

            MovePlayers();
            StartCoroutine(CountDown());
        }
    	
        // Updates frames depending on frameRate 
        void Update () 
        {
            if (Time.time > nextFrame)
            {
                nextFrame = Time.time + frameRate;

                if (!pause)
                {
                    MovePlayers();
                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                HandlePause();
            }

            // to delete - for test
            if(Input.GetKeyDown(KeyCode.G))
            {
                foreach (Player player in players) 
                {
                    player.DoubleSize ();
                }
            }

            if(Input.GetKeyDown(KeyCode.H))//
            {
                foreach (Player player in players) 
                {
                    player.ReduceSize ();
                }
            }

            if(Input.GetKeyDown(KeyCode.J))
            {
                foreach (Player player in players) 
                {
                    player.IncreaseSpeed ();
                }
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                foreach (Player player in players) 
                {
                    player.ReduceSpeed ();
                }
            }
        }

        // Loads Player prefab and Instantiate players set
        private void LoadPlayers()
        {
            Player player = Resources.Load("Prefabs/Player", typeof(Player)) as Player;

            if (player)
            {

                for (int i = 0; i < gameConfiguration.CurrentNoOfPlayers; i++)
                {
                    players.Add(Instantiate(player));
                }

                int j = 0;
                foreach (PlayerInitialData p in gameConfiguration.Players)
                {
                    if (p != null)
                    {
                        KeyCode[] keys = { p.LeftKey, p.RightKey };
                        players[j].SetupPlayer(p.Nickname, p.Color, keys);
                        players[j].IsActive = true;
                        j++;
                    }
                }
            }
            else
            {
                Debug.LogError("Player prefab not found");
            }
        }

        public void SetupLevel()
        {
            arena = Resources.Load("Prefabs/Arena", typeof(Arena)) as Arena;

            if (arena)
            {
                Instantiate(arena);
                arena.SetupArena(gameConfiguration.ArenaSize);
            }
            else
            {
                Debug.LogError("Arena prefab not found");
            }
        }

        public void MovePlayers()
        {
            bool end = true;

            foreach (Player player in players) 
            {
                if (player != null)
                {
                    player.Turn();

                    if (player.IsActive)
                    {
                        end = false;
                    }
                }
            }

            arena.RedrawArena(this);

            if (end)
            {
                this.enabled = false;
                Invoke("Restart", 1.5f);
            }
        }


        void HandlePause()
        {
            pause = !pause;

            GameObject pausePane = GameObject.Find("PausePanel");

            if (pausePane)
            {
                pausePane.transform.GetComponent<Canvas>().enabled = pause;
            }
            else
            {
                Debug.LogError("no PausePanel object");
            }
        }

        //Restart reloads the scene when called.
        private void Restart ()
        {
            //Load Main scene
            if (!GameManager.instance.gameOver)
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                ShowFinalScreen();
            }
        }

        // Starts cunter and runs it between scenes.
        private IEnumerator CountDown()
        {
            pause = true;

            GameObject countdownPanel = GameObject.Find("CountdownPanel");
            countdownPanel.transform.GetComponent<Canvas>().enabled = true;

            int value = Counter;
            while (value > 0)
            {
                countdownPanel.GetComponentInChildren<Text>().text = value.ToString();
                yield return new WaitForSeconds(Timeout);

                --value;
            }

            countdownPanel.GetComponentInChildren<Text>().text = "GO!";
            yield return new WaitForSeconds(Timeout / 4.0f);

            countdownPanel.transform.GetComponent<Canvas>().enabled = false;
            pause = false;
        }

        // Show screen with final game results.
        private void ShowFinalScreen()
        {
            GameObject finalScreen = GameObject.Find("FinalScreen");
            finalScreen.GetComponent<Canvas>().enabled = true;

            Configurator configurator = GUIManager.configurator;
            GameObject board = GameObject.Find("Board");

            List<KeyValuePair<string, int>> playerScores = PlayerScores();

            int i = 1;
            foreach (Player player in players)
            {
                string nicknameObject = "Player" + i + "NicknameText";
                Text nickname = GameObject.Find(nicknameObject).
                                GetComponent<Text>();
                nickname.color = player.PlayerColor;
                nickname.text = playerScores[i - 1].Key;

                string scoreObject = "Player" + i + "ScoreText";
                Text score = GameObject.Find(scoreObject).
                             GetComponent<Text>();
                score.color = player.PlayerColor;
                score.text = playerScores[i - 1].Value.ToString();

                ++i;
            }

            Button playeAgain = GameObject.Find("PlayAgainButton").
                                GetComponent<Button>();
            playeAgain.onClick.AddListener(() =>
                                           SceneManager.LoadScene("GUI"));
        }

        // Gets the player nicknames and scores and sorts them.
        private List<KeyValuePair<string, int>> PlayerScores()
        {
            // TOBEREMOVED
            System.Random rnd = new System.Random();
            List<KeyValuePair<string, int>> playerScores =
                                                new List<KeyValuePair<string, int>>();
            foreach (Player player in players)
            {
                // TODO Replace rnd.Next() with player.Score
                playerScores.Add(new KeyValuePair<string, int>
                                 (player.Nickname, rnd.Next(0, 60)));
            }
            playerScores.Sort((a, b) => a.Value.CompareTo(b.Value));
            playerScores.Reverse();

            return playerScores;
        }
    }

}