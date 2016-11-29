using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

namespace ProjectScopes
{ 
    public class GameManager : MonoBehaviour 
    {
        // Countdown between levels starts with this value.
        private const int Counter = 3;

        // The duration between two counter values.
        private const float Timeout = 1.0f;

        // Determines whether countdown is running.
        private bool countdown = false; 

        private bool roundEndDelay = false; 

        private Arena arena;
        public List<Player> players;
        private Configurator gameConfiguration;

        // Variables used for simple frame rate control
        private float frameRate;
        private float nextFrame;

        private bool pause;

        private bool gameOver;

        // Use this for initialization
        void Start ()
        {
            gameConfiguration = GUIManager.configurator;

            frameRate = 0.01f;
            nextFrame = 0.0f;
            pause = false;

            Screen.SetResolution(gameConfiguration.ArenaSize, gameConfiguration.ArenaSize, false);

            LoadArena();

            // Creates a default set of 6 inactive players
            LoadPlayers();

            StartRound();
        }


        public void LoadArena()
        {
            arena = Resources.Load("Prefabs/Arena", typeof(Arena)) as Arena;

            if (arena)
            {
                Instantiate(arena);
            }
            else
            {
                Debug.LogError("Arena prefab not found");
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


        void StartRound()
        {
            arena.SetupArena(gameConfiguration.ArenaSize);

            ShufflePlayers();

            MovePlayers();
            StartCoroutine(CountDown());
        }
    	
        // Updates frames depending on frameRate 
        void Update () 
        {
            if (Time.time > nextFrame)
            {
                nextFrame = Time.time + frameRate;

                if (!pause && !countdown)
                {
                    MovePlayers();
                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (!countdown)
                {
                    HandlePause();
                }
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


        public void MovePlayers()
        {
            int end = 0;

            foreach (Player player in players) 
            {
                player.Turn();

                if (player.IsActive)
                {
                    end++;
                }
            }

            arena.RedrawArena(this);

            if (!roundEndDelay && end <= 1)
            {
                CheckIfGameOver();
            }
        }


        void CheckIfGameOver()
        {
            if (gameOver)
            {
                this.enabled = false;
                ShowFinalScreen();
            }
            else
            {
                StartCoroutine(EndDelay(2));
            }
        }


        private IEnumerator EndDelay(int sec)
        {
            roundEndDelay = true;
            yield return new WaitForSeconds(sec);

            ResetPlayers();
            roundEndDelay = false;
            StartRound();
        }


        public void AddPoints()
        {
            foreach (Player player in players)
            {
                if (player.IsActive)
                {
                    if (++player.Points >= gameConfiguration.CurrentNoOfPlayers * 5)
                    {
                        gameOver = true;
                    }
                }
            }
        }


        void ResetPlayers()
        {
            foreach (Player player in players) 
            {
                player.Reset();
                player.IsActive = true;
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


        // Starts cunter and runs it between scenes.
        private IEnumerator CountDown()
        {
            countdown = true;

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
            countdown = false;
        }


        // Show screen with final game results.
        private void ShowFinalScreen()
        {
            GameObject finalScreen = GameObject.Find("FinalScreen");
            finalScreen.GetComponent<Canvas>().enabled = true;

            List<Player> sortedPlayers = players.OrderByDescending
                                                 (o => o.Points).ToList();

            int i = 1;
            foreach (Player player in sortedPlayers)
            {
                string nicknameObject = "Player" + i + "NicknameText";
                Text nickname = GameObject.Find(nicknameObject).
                                GetComponent<Text>();
                nickname.color = player.PlayerColor;
                nickname.text = player.Nickname;

                string scoreObject = "Player" + i + "ScoreText";
                Text score = GameObject.Find(scoreObject).
                             GetComponent<Text>();
                score.color = player.PlayerColor;
                score.text = player.Points.ToString();

                ++i;
            }

            Button playeAgain = GameObject.Find("PlayAgainButton").
                                GetComponent<Button>();
            playeAgain.onClick.AddListener(() =>
                                           SceneManager.LoadScene("GUI"));
        }

        void ShufflePlayers()  
        { 
            for (int i = players.Count - 1; i > 0; i--) {
                int r = UnityEngine.Random.Range(0, i + 1);
                Player tmp = players[i];
                players[i] = players[r];
                players[r] = tmp;
            }
        }
    }

}