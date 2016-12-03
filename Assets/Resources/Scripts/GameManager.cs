/*!
 * @file    GameManager.cs
 * @brief   Contains GameManager class definition.
 * @author  Marcin
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief   A global namespace for project-scopes.
 * @detail  Contains all project-scopes related classes.
 */

namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief   GameManager handles main gameplay flow.
 * 
 * @details GameManager handles all gameplay rules and special keys actions. It holds players and arena objects. 
 *          It calculates points after each round and decides when game is over and which player wins.
 *          It is also responsible for managing of pause and final screens.         
 */

    public class GameManager : MonoBehaviour 
    {
        // Countdown between levels starts with this value.
        private const int Counter = 4;

        // Current counter value.
        private int counter = Counter;

        // The duration between two counter values.
        private const float Timeout = 1.0f;

        // Determines whether countdown is running.
        private bool countdownEnabled = false; 

        // Determines whether next round start is delaying.
        private bool roundEndDelay = false; 

        private bool pauseEnabled = false;
        private bool gameOver = false;

        private Configurator gameConfiguration;
        private Arena arena;

        public List<Player> players;

        // Variables used for simple frame rate control
        private float frameRate = 0.01f;
        private float nextFrame = 0.0f;

        private int winningScore;


        // Use this for initialization
        void Start ()
        {
            gameConfiguration = GUIManager.configurator;
            winningScore = gameConfiguration.CurrentNoOfPlayers * 5;

            Screen.SetResolution(gameConfiguration.ArenaSize, gameConfiguration.ArenaSize, false);

            LoadArena();
            LoadPlayers();

            StartRound();
        }

        // Loads Arena prefab
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
                SetPlayersSet(player);
            }
            else
            {
                Debug.LogError("Player prefab not found");
            }
        }


        private void SetPlayersSet(Player player)
        {
            for (int i = 0; i < gameConfiguration.CurrentNoOfPlayers; i++)
            {
                players.Add(Instantiate(player));
            }

            int playerDataInd = 0;
            foreach (PlayerInitialData playerData in gameConfiguration.PlayersData)
            {
                if (playerData != null)
                {
                    KeyCode[] keys = { playerData.LeftKey, playerData.RightKey };
                    players[playerDataInd].SetupPlayer(playerData.Nickname, playerData.Color, keys);
                    players[playerDataInd].IsActive = true;
                    playerDataInd++;
                }
            }
        }

        void StartRound()
        {
            arena.SetupArena(gameConfiguration.ArenaSize);

            ResetPlayers();

            // Shuffles players to randomize winner in case of head to head collision
            ShufflePlayers();

            // Move players to initial positions before round start
            MovePlayers();

            ResetCounter();
            StartCoroutine("CountDown");
        }
    	
        
        void Update () 
        {
            // Updates frames depending on frameRate 
            if (Time.time > nextFrame)
            {
                nextFrame = Time.time + frameRate;

                if (!countdownEnabled && !pauseEnabled)
                {
                    MovePlayers();
                }
            }

            HandleSpecialKeys();
        }


        public void MovePlayers()
        {
            int activePlayers = 0;

            foreach (Player player in players) 
            {
                player.Move();

                if (player.IsActive)
                {
                    activePlayers++;
                }
            }

            arena.RedrawPlayers(this);

            if (!roundEndDelay && activePlayers <= 1)
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
                StartCoroutine(RoundEndDelay(2));
            }
        }


        private IEnumerator RoundEndDelay(int seconds)
        {
            roundEndDelay = true;
            yield return new WaitForSeconds(seconds);

            roundEndDelay = false;
            StartRound();
        }


        public void AddPoints()
        {
            foreach (Player player in players)
            {
                if (player.IsActive)
                {
                    if (++player.Points >= winningScore)
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
            pauseEnabled = !pauseEnabled;

            GameObject pausePanel = GameObject.Find("PausePanel");

            if (pausePanel)
            {
                pausePanel.transform.GetComponent<Canvas>().enabled = pauseEnabled;
            }
            else
            {
                Debug.LogError("no PausePanel object");
            }
        }

        // Resets couroutine counter.
        private void ResetCounter()
        {
            counter = 4;
        }


        // Starts cunter and runs it between scenes.
        private IEnumerator CountDown()
        {
            if (counter > 0)
            {
                countdownEnabled = true;

                GameObject countdownPanel = GameObject.Find("CountdownPanel");
                countdownPanel.transform.GetComponent<Canvas>().enabled = true;

                while (counter > 1)
                {
                    countdownPanel.GetComponentInChildren<Text>().text = (counter - 1).ToString();
                    yield return new WaitForSeconds(Timeout);

                    --counter;
                }

                countdownPanel.GetComponentInChildren<Text>().text = "GO!";
                yield return new WaitForSeconds(Timeout / 4.0f);

                --counter;

                countdownPanel.transform.GetComponent<Canvas>().enabled = false;
                countdownEnabled = false;
            }
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
                nickname.color = player.Colour;
                nickname.text = player.Nickname;

                string scoreObject = "Player" + i + "ScoreText";
                Text score = GameObject.Find(scoreObject).
                             GetComponent<Text>();
                score.color = player.Colour;
                score.text = player.Points.ToString();

                ++i;
            }

            Button playAgain = GameObject.Find("PlayAgainButton").
                                GetComponent<Button>();
            playAgain.onClick.AddListener(() =>
                                SceneManager.LoadScene("GUI"));

            Button exit = GameObject.Find("ExitButton").
                          GetComponent<Button>();
            exit.onClick.AddListener(() => QuitGame());
        }

        // Shows the panel with exit game question.
        private void ShowExitGamePanel()
        {
            GameObject exitGamePanel = GameObject.Find("ExitGamePanel");
            bool enabled = exitGamePanel.transform.GetComponent<Canvas>().enabled;
            exitGamePanel.transform.GetComponent<Canvas>().enabled = !enabled;
            // Show panel.
            if (!enabled)
            {
                pauseEnabled = true;
                StopCoroutine("CountDown");

                Button yesButton = GameObject.Find("YesButton").
                                   GetComponent<Button>();
                yesButton.onClick.AddListener(() =>
                                              SceneManager.LoadScene("GUI"));

                Button noButton = GameObject.Find("NoButton").
                                   GetComponent<Button>();
                noButton.onClick.AddListener(() => 
                                             HideExitGamePanel(exitGamePanel));
            }
            // Continue counting if needed.
            else
            {
                StartCoroutine("CountDown");
                pauseEnabled = false;
            }
        }

        // Hides the exit game panel.
        private void HideExitGamePanel(GameObject panel)
        {
            panel.transform.GetComponent<Canvas>().enabled = false;
            pauseEnabled = false;
            StartCoroutine("CountDown");
        }

        void ShufflePlayers()  
        { 
            for (int i = players.Count - 1; i > 0; i--) 
            {
                int r = UnityEngine.Random.Range(0, i + 1);
                Player tmp = players[i];
                players[i] = players[r];
                players[r] = tmp;
            }
        }

        // Quit the game.
        private void QuitGame()
        {
            // Uncomment if you want to test this functionality in Unity editor.
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }


        private void HandleSpecialKeys()
        {
            // SPACE KEY - pause
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (!roundEndDelay && !countdownEnabled)
                {
                    HandlePause();
                }
            }

            // ESC - "Do you wan to exit?" screen.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowExitGamePanel();
            }


            // to delete - for test
            if(Input.GetKeyDown(KeyCode.G))
            {
                foreach (Player player in players) 
                {
                    player.DoubleSize ();
                }
            }

            if(Input.GetKeyDown(KeyCode.H))
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
    }

}