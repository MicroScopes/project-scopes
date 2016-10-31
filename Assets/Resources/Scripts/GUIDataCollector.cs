/*!
 * @file GUIDataCollector.cs
 * @brief Contains definition of GUIDataCollector class.
 * @author MicroScopes
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief A global namespace for project-scopes.
 *
 * @details Contains all project-scopes related classes.
 */
namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief Collects game related data from GUI.
 *
 * @details In EPIC1 version user has possibitity to setup each player nickname,
 *          color and movement keys. It is also possible to set the initial
 *          values of arena size and all players speed and thickness.
 */
    public class GUIDataCollector : MonoBehaviour
    {
        public static GUIDataCollector instance = null;
        // Player id is located on 6th position in the name string and looks as
        // follows: "Player"+id+"ComponentName".
        private const int PlayerIdPosition = 6;

        // Enabled/Disabled player panels names.
        private const string EnabledPanel = "PlayerEnabledPanel";
        private const string DisabledPanel = "PlayerDisabledPanel";

        // Enabled/Disabled player panels transparency.
        private const float EnabledPanelTransparency = 1.0f;
        private const float DisabledPanelTransparency = 0.25f;

        // Enabled/Disabled player panels prefab objects.
        private GameObject enabledPlayerPrefab;
        private GameObject disabledPlayerPrefab;

        /*!
         * @brief Configurator object. Contains initial information about game setup.
         *
         * @details After the game starts it is updated with the data set by the user.
         *                This attribute is shared between scenes.
         */
        public static Configurator configurator;

        // Contains all possible key codes.
        private Array allKeyCodes;

        // Map of a key codes and corresponding values that will be printed
        // on GUI.
        private Dictionary<string, string> keyCodesMap;

        void Awake ()
        {
            // Implementation of singleton pattern
            if (instance == null)
            {
                instance = this;

                // Object will not be destroyed after scene reload
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);    
            }
        }

        void OnLevelWasLoaded()
        {
            //Call SetupLevel only to main instance of GameManager
        }

        // Reads file to generate map of key codes and corresponding GUI printout.
        // All values in the file are presented in the following format: Key Value
        private void GeneratekeyCodesMap()
        {
            keyCodesMap = new Dictionary<string, string>();
            try
            {
                // Read all lines from 'keycodes.cfg' files and update the map.
                string path = "Assets/Resources/Configs/keycodes.cfg";
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line.Equals(""))
                        {
                            continue;
                        }

                        string[] values = line.Split(' ');
                        keyCodesMap.Add(values[0], values[1]);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // Find objects based on its name and player id.
        private GameObject FindObject(string name, int id)
        {
            return GameObject.Find(name.Insert(PlayerIdPosition, (id).ToString()));
        }

        // Sets panel input field color.
        private void SetPlayerColor(GameObject panel, Color color, bool enabled)
        {
            Color newColor = new Color(color.r, color.g, color.b,
                                       enabled ? EnabledPanelTransparency : DisabledPanelTransparency);
            panel.GetComponentInChildren<InputField>().GetComponent<Image>().color = newColor;

            // If enabled set the color of disabled button text.
            if (enabled)
            {
                panel.GetComponentInChildren<Button>().GetComponentInChildren<Text>().color = newColor;
            }
        }

        // Check if any player is already using given key code.
        private bool IsKeyCodeInUse(int playerId, int key, KeyCode keyCode)
        {
            //for (int i = 0; i < configurator.Players.Length; ++i)
            for (int i = 0; i < configurator.Players.Length; ++i)
            {
                PlayerInitData player = configurator.Players[i];

                // For itself check only if two movement keys are not the same.
                if (i == playerId)
                {
                    int otherKey = (key == 0) ? 1 : 0;
                    if (keyCode == player.MovementKeys[otherKey])
                    {
                        return true;
                    }
                    continue;
                }

                if (player.IsActive && (keyCode == player.MovementKeys[0] || keyCode == player.MovementKeys[1]))
                {
                    return true;
                }
            }

            return false;
        }

        // Reads data from Configruator class and places it on the suitable panels.
        private void SetPanelData(GameObject panel, int id, bool enabled)
        {
            // There is no data to put on disabled panel.
            if (!enabled)
            {
                return;
            }

            // Set initial player data.
            panel.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = configurator.Players[id - 1].Nickname;
            panel.GetComponentInChildren<InputField>().GetComponent<Image>().color = configurator.Players[id - 1].Color;
            FindObject("PlayerDisableButton", id).GetComponentInChildren<Text>().color = configurator.Players[id - 1].Color;

            KeyCode keyLeft = configurator.Players[id - 1].MovementKeys[0];
            if (IsKeyCodeInUse(id - 1, 0, keyLeft))
            {
                    keyLeft = KeyCode.None;
                    configurator.Players[id - 1].MovementKeys[0] = keyLeft;
            }
            FindObject("PlayerKeyLeftButton", id).GetComponentInChildren<Text>().text = keyCodesMap[keyLeft.ToString()];

            KeyCode keyRight = configurator.Players[id - 1].MovementKeys[1];
            if (IsKeyCodeInUse(id - 1, 1, keyRight))
            {
                keyRight = KeyCode.None;
                configurator.Players[id - 1].MovementKeys[1] = keyRight;
            }
            FindObject("PlayerKeyRightButton", id).GetComponentInChildren<Text>().text = keyCodesMap[keyRight.ToString()];
        }

        // Validates input filed data. Converts all added characters to capital letters.
        private char ValidateNickname(string text, int charIndex, char addedChar)
        {
            return addedChar.ToString().ToUpper()[0];
        }

        // Updates configurator with new player nickname.
        private void UpdatePlayerNickname(int id, string nickname)
        {
            if (!nickname.Equals(""))
            {
                configurator.Players[id - 1].Nickname = nickname;
            }
        }

        // Start coroutine that waits for user keyboard or mouse input.
        // Key value stands for left (0) or right (1) keys.
        private void UpdatePlayerMovementKey(int id, int key, Text textField)
        {
            StartCoroutine(GetPlayerMovementKey(id, key, textField));
        }

        // Reads user input and updates both GUI component and configurator.
        private IEnumerator GetPlayerMovementKey(int id, int key, Text textField)
        {
            // Wait until user press any key.
            bool isRunning = true;
            while (isRunning)
            {
                // Decode the key.
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keyCode in allKeyCodes)
                    {
                        // Ignore if key code does not match.
                        if (!Input.GetKeyDown(keyCode))
                        {
                            continue;
                        }

                        // Key code found. Stop searching.
                        if (keyCodesMap.ContainsKey(keyCode.ToString()))
                        {
                            // If key is already in use do not assign it to
                            // the current player.
                            if (IsKeyCodeInUse(id - 1, key, keyCode))
                            {
                                break;
                            }

                            textField.text = keyCodesMap[keyCode.ToString()];
                            configurator.Players[id - 1].MovementKeys[key] = keyCode;

                            break;
                        }
                    }

                    isRunning = false;
                    break;
                }

                yield return null;
            }
        }

        // Depending on the type of the panel add suitable listeners.
        private void AddActionListeners(GameObject panel, int id, bool enabled)
        {
            // Add listeners to enabled panel components.
            if (enabled)
            {
                // Remove player from the game. After pressing disable button
                // enabled panel is removed and disabled is put instead.
                FindObject("PlayerDisableButton", id).GetComponent<Button>().onClick.AddListener(() => DisablePlayer(id));

                // Add nickname input field validation and updates to configurator.
                panel.GetComponentInChildren<InputField>().onValidateInput = ValidateNickname;
                panel.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate { UpdatePlayerNickname(id, panel.GetComponentInChildren<InputField>().text); });

                // Setup player movement keys.
                Button keyLeftButton = FindObject("PlayerKeyLeftButton", id).GetComponent<Button>();
                keyLeftButton.onClick.AddListener(() => UpdatePlayerMovementKey(id, 0, keyLeftButton.GetComponentInChildren<Text>()));
                Button keyRightButton = FindObject("PlayerKeyRightButton", id).GetComponent<Button>();
                keyRightButton.onClick.AddListener(() => UpdatePlayerMovementKey(id, 1, keyRightButton.GetComponentInChildren<Text>()));
            }
            // Add listeners to disabled panel components.
            else
            {
                // Add player to the game. After pressing the button disabled
                // panel is removed and enabled is put instead.
                panel.GetComponent<Button>().onClick.AddListener(() => EnablePlayer(id));
            }
        }

        // Destroys panel and all its components.
        private void DestroyPanel(GameObject root)
        {
            foreach (Transform child in root.transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(root.gameObject);
        }

        // Creates panel and all its components.
        private void CreatePanel(int id, Vector3 position, Color color, bool enabled)
        {
            // Instantiate enabled panel object.
            GameObject panel = Instantiate(enabled ? enabledPlayerPrefab : disabledPlayerPrefab);

            // Set the name of it and all its components. By default newly
            // instatniated object has "(Clone)" at the end of its name. That
            // is why name is splitted by '(' and only first chunk is considered.
            panel.name = panel.name.Insert(PlayerIdPosition, id.ToString()).Split('(')[0];
            foreach (Transform child in panel.transform)
            {
                child.name = child.name.Insert(PlayerIdPosition, id.ToString());
            }

            // Set the panel properties.
            panel.transform.SetParent(GameObject.Find("PlayersSettingsPanel").transform);
            panel.transform.localPosition = position;
            panel.transform.localScale = Vector3.one;
            SetPlayerColor(panel, color, enabled);
            SetPanelData(panel, id, enabled);

            // Depending on what panel is created, set the appropriate actions
            // on its components.
            AddActionListeners(panel, id, enabled);
        }

        // Creates one panel in place of another. This method is used both to create
        // disabled and enables player panels.
        private void LoadPanel(string name, int id, bool enabled)
        {
            // Find the panel on 'i' position and destroy it.
            GameObject panel = GameObject.Find(name.Insert(PlayerIdPosition, id.ToString()));
            DestroyPanel(panel);

            // Create new panel on the disabled panel position.
            CreatePanel(id, panel.transform.localPosition, panel.GetComponentInChildren<InputField>().GetComponent<Image>().color, enabled);
        }

        // Enables new player panel in order to give the possibility to change
        // its settings and join it to the game.
        private void DisablePlayer(int id)
        {
            // Do not disabled panel if only two players left.
            if (configurator.CurrentNoOfPlayers == 2)
            {
                return;
            }

            LoadPanel(EnabledPanel, id, false);

            // Decrease number of players.
            configurator.Players[id - 1].IsActive = false;
            --configurator.CurrentNoOfPlayers;
        }

        // Disabled player panel and remove it from the game.
        private void EnablePlayer(int id)
        {
            LoadPanel(DisabledPanel, id, true);

            // Increase number of players.
            configurator.Players[id - 1].IsActive = true;
            ++configurator.CurrentNoOfPlayers;
        }

        // Updates configurator with new game arena size.
        private void UpdateArenaSize(Slider slider)
        {
            configurator.InitialArenaSize = (int)slider.value;
        }

        // Updates configurator with new players speed.
        private void UpdatePlayersSpeed(Slider slider)
        {
            configurator.InitialPlayersSpeed = (int)slider.value;
        }

        // Updates configurator with new playes size.
        private void UpdatePlayersSize(Slider slider)
        {
            configurator.InitialPlayersSize = (int)slider.value;
        }

        // Loads the scene with game arena and players.
        private void StartGame()
        {
            GameManager.instance.GameConfiguration = configurator;

            for (int i = 0; i < configurator.Players.Length; ++i)
            {
                ProjectScopes.GameManager.instance.players[i].SetupPlayer(configurator.Players[i].Nickname,
                    configurator.Players[i].Color,
                    configurator.Players[i].MovementKeys
                );

                ProjectScopes.GameManager.instance.players[i].IsActive = configurator.Players[i].IsActive;
            }

            ProjectScopes.GameManager.instance.enabled = true;

            GameManager.instance.Restart();
        }

        // This method is called one during scene initialization. It reads
        // configuration data and applies it to the GUI components.
        private void Start()
        {
            if (instance == this)
            {
                // Load prefabs from resources.
                enabledPlayerPrefab = Resources.Load("Prefabs/PlayerEnabledPanel", typeof(GameObject)) as GameObject;
                disabledPlayerPrefab = Resources.Load("Prefabs/PlayerDisabledPanel", typeof(GameObject)) as GameObject;

                // Initialize configurator object.
                configurator = new Configurator();

                // Get all piossible key codes and generate printout map.
                allKeyCodes = Enum.GetValues(typeof(KeyCode));
                GeneratekeyCodesMap();

                // Enable minimum number of players (the MinNoOfPlayer value can be
                // found in the Configurator data class). The initially enabled players
                // will be subsequently inserted on 0 to MinNoOfPlayers positions.
                for (int i = 0; i < Configurator.MinNoOfPlayers; ++i)
                {
                    EnablePlayer(i + 1);
                }

                // Attach "onClick" action to all of the rest disabled panels.
                for (int i = Configurator.MinNoOfPlayers; i < Configurator.MaxNoOfPlayers; ++i)
                {
                    // Create local variable to have different ids.
                    int id = i + 1;
                    FindObject(DisabledPanel, id).GetComponent<Button>().onClick.AddListener(() => EnablePlayer(id));
                }

                // Set the default values of arena size and initial players speed and size.
                Slider arenaSizeSlider = GameObject.Find("ArenaSizePanel").GetComponentInChildren<Slider>();
                arenaSizeSlider.value = configurator.InitialArenaSize;
                arenaSizeSlider.onValueChanged.AddListener(delegate
                    {
                        UpdateArenaSize(arenaSizeSlider);
                    });
                Slider playersSpeedSlider = GameObject.Find("InitialSpeedPanel").GetComponentInChildren<Slider>();
                playersSpeedSlider.value = configurator.InitialPlayersSpeed;
                playersSpeedSlider.onValueChanged.AddListener(delegate
                    {
                        UpdatePlayersSpeed(playersSpeedSlider);
                    });
                Slider playersSizeSlider = GameObject.Find("InitialSizePanel").GetComponentInChildren<Slider>();
                playersSizeSlider.value = configurator.InitialPlayersSize;
                playersSizeSlider.onValueChanged.AddListener(delegate
                    {
                        UpdatePlayersSize(playersSizeSlider);
                    });

                // Add "onClick" action to the START button.
                GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(() => StartGame());
            }
        }

        public void Restart()
        {
            if (instance = this)
            {
                this.enabled = true;
                this.Start();
            }
        }
    }

}