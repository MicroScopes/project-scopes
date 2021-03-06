﻿/*!
 * @file GUIManager.cs
 * @brief Contains definition of GUIManager class.
 * @author MicroScopes
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

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
 * @brief Collects user data from the Graphical User Interface.
 *
 * @details In EPIC1 user has possibitity to setup each player nickname,
 *          color and movement keys. It is also possible to set the initial
 *          values of arena size and all players speed and thickness.
 */
public class GUIManager: MonoBehaviour
{
    // Screen properties.
    private const bool Fullscreen = false;
    private const int ScreenWidth = 650;
    private const int ScreenHeight = 475;

    // Disabled/Enabled player panels prefab objects.
    private GameObject disabledPanel;
    private GameObject enabledPanel;

    /*!
     * @brief Current game configuration.
     *
     * @details The configurator object is used by GameManager to read the
     *          initial game configuration.
     */
    public static Configurator configurator;

    // Add all necessary action listeners to panel components.
    private void AddPlayerActionListeners(GameObject root, int id)
    {
        // In case of disabled panel make possibility do enable player.
        if (root.name.Contains("Disabled"))
        {
            Button enable = GUIHelper.Find<Button>(root);
            enable.onClick.AddListener(() => EnablePlayer(id));
        }
        else
        {
            // Set player nickname.
            InputField nickname = GUIHelper.FindChild<InputField>(root);
            nickname.onValidateInput = GUIHelper.ValidateNickname;
            Text placeholder = GUIHelper.Find<Text>(nickname.placeholder.gameObject);
            nickname.onEndEdit.AddListener
                     (delegate{SetPlayerNickname(id, nickname.text.Equals("") ?
                               placeholder.text : nickname.text);});

            // Add possibility to disable panel.
            Button disable = GUIHelper.FindChild<Button>(root, "Disable");
            disable.onClick.AddListener(() => DisablePlayer(id));

            // Set player movement keys.
            Button leftKey = GUIHelper.FindChild<Button>(root, "Left");
            leftKey.onClick.AddListener(() => SetPlayerMovementKey(id,
                                                                   leftKey));

            Button rightKey = GUIHelper.FindChild<Button>(root, "Right");
            rightKey.onClick.AddListener(() => SetPlayerMovementKey(id,
                                                                    rightKey));
        }
    }

    // Initializes all necessery data and reads initial game and setup GUI
    // initial configuration.
    private void Awake()
    {
        // Set initial screen size.
        Screen.SetResolution(ScreenWidth, ScreenHeight, Fullscreen);

        // Load player panel prefabs.
        disabledPanel = GUIHelper.Load("Prefabs/PlayerDisabledPanel");
        enabledPanel = GUIHelper.Load("Prefabs/PlayerEnabledPanel");

        // The game is started for the first time. Create initial configuration.
        if (configurator == null)
        {
            // Initialize configurator with the default data.
            configurator = new Configurator();

            // Enable minimum number of players. The MinNoOfPlayer value can be
            // found in the Configurator data class. The initially enabled players
            // will be subsequently inserted on 0 to MinNoOfPlayers positions.
            int i = 0;
            while (i < Configurator.MinNoOfPlayers)
            {
                EnablePlayer(++i);
            }

            // Attach action listeners to all of the rest disabled panels.
            while (i < Configurator.MaxNoOfPlayers)
            {
                int id = (i++) + 1;
                AddPlayerActionListeners(GUIHelper.Find(disabledPanel.name, id),
                                         id);
            }
        }
        // Restore the configuration from the previous game.
        else
        {
            int i = 0;
            foreach (PlayerInitialData player in configurator.PlayersData)
            {
                // Check which players has been enabled and load their panels.
                if (player != null)
                {
                    EnableExistingPlayer(++i);
                }
                // Add the listeners for the rest of them.
                else
                {
                    int id = ++i;
                    AddPlayerActionListeners(GUIHelper.Find(disabledPanel.name, id),
                                             id);
                }
            }
        }

        // Set initial value and add action listeners to sliders and start
        // button components.
        InitializeArenaSizeSlider();
        InitializePlayersSettingsSliders();
        InitializeButtons();
    }

    // Creates the given component and all its children based on the pattern
    // object.
    private void CreatePanel(GameObject root, GameObject pattern, int id)
    {
        // Instatntiate the component.
        GameObject panel = Instantiate(root);

        // Setup its properties.
        panel.transform.SetParent(pattern.transform.parent);
        panel.transform.localPosition = pattern.transform.localPosition;
        panel.transform.localScale = pattern.transform.localScale;
        GUIHelper.RenamePlayerComponent(panel, id);
        GUIHelper.SetPlayerComponentColor(panel, pattern);
        GUIHelper.SetInitialPlayerData(panel, configurator.PlayersData[id - 1]);

        // Add action listeners to the panel components.
        AddPlayerActionListeners(panel, id);
    }

    
    // Destroys the given component and all its children.
    private void DestroyPanel(GameObject root)
    {
        foreach (Transform child in root.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(root.gameObject);
    }

    // Loads PlayerDisabledPanel in place of PlayerEnabledPanel.
    private void DisablePlayer(int id)
    {
        // Do not disable panel if there is only minimum number of players
        // assigned to the game.
        if (configurator.CurrentNoOfPlayers <= Configurator.MinNoOfPlayers)
        {
            return;
        }

        // Decrease number of players.
        configurator.RemovePlayer(id - 1);

        LoadPanel(disabledPanel, enabledPanel, id);
    }

    // Loads the PlayerEnabledPanel of the player that has been enabled in the
    // previous game.
    private void EnableExistingPlayer(int id)
    {
        // Display panel on the GUI.
        LoadPanel(enabledPanel, disabledPanel, id);
    }

    // Loads PlayerEnabledPanel in place of PlayerDisabledPanel.
    private void EnablePlayer(int id)
    {
        // Increase number of players.
        configurator.AddPlayer(id - 1);

        // Fill player data with the initial values.
        PlayerInitialData player = GUIHelper.PlayerInitialData[id - 1];
        configurator.PlayersData[id - 1].Nickname = player.Nickname;
        configurator.PlayersData[id - 1].Color = player.Color;
        configurator.PlayersData[id - 1].LeftKey = KeyCode.None;
        if (GUIHelper.IsKeySupported(player.LeftKey) &&
            !GUIHelper.IsKeyInUsed(player.LeftKey, configurator.PlayersData))
        {
            configurator.PlayersData[id - 1].LeftKey = player.LeftKey;
        }
        configurator.PlayersData[id - 1].RightKey = KeyCode.None;
        if (GUIHelper.IsKeySupported(player.RightKey)  &&
            !GUIHelper.IsKeyInUsed(player.RightKey, configurator.PlayersData))
        {
            configurator.PlayersData[id - 1].RightKey = player.RightKey;
        }

        // Display panel on the GUI.
        LoadPanel(enabledPanel, disabledPanel, id);

        UpdateStartButtonStatus();
    }

    // Gets user input and updates player movement key.
    private IEnumerator GetPlayerMovementKey(int id, Button keyButton)
    {
        Button keyLeft = GUIHelper.Find<Button>
                         (GUIHelper.Find("Player" + id + "KeyLeftButton"));
        Button keyRight = GUIHelper.Find<Button>
                          (GUIHelper.Find("Player" + id + "KeyRightButton"));
        while (true)
        {
            // Disable Start button while waiting for user input.
            Button start = GUIHelper.Find<Button>
                                     (GUIHelper.Find("StartButton"));
            GUIHelper.SetButtonEnabled(start, false);
            GUIHelper.SetButtonEnabled(keyButton, false);

            // Disable MovementKeys button  while waiting for user input.
            keyLeft.interactable = false;
            keyRight.interactable = false;

            if (Input.anyKeyDown)
            {
                // Decode the key.
                KeyCode keyCode = GUIHelper.KeyPressed();

                // Check if key is supported and noone is using it already.
                if (GUIHelper.IsKeySupported(keyCode) &&
                    !GUIHelper.IsKeyInUsed(keyCode, configurator.PlayersData))
                {
                    // Update GUI with key output format.
                    GUIHelper.UpdateKeyButtonText(keyButton, keyCode);

                    // Determine what key was updated by checking parent name.
                    if (keyButton.name.Contains("Left"))
                    {
                        configurator.PlayersData[id - 1].LeftKey = keyCode;
                        GUIHelper.PlayerInitialData[id - 1].LeftKey = keyCode;
                    }
                    else
                    {
                        configurator.PlayersData[id - 1].RightKey = keyCode;
                        GUIHelper.PlayerInitialData[id - 1].RightKey = keyCode;
                    }
                }

                break;
            }

            yield return null;
        }

        // Delay a little change of button status.
        yield return new WaitForSeconds(0.10f);
        UpdateStartButtonStatus();
        GUIHelper.SetButtonEnabled(keyButton, true);
        keyLeft.interactable = true;
        keyRight.interactable = true;
    }
    
    // Sets the initial value and adds action listener to the arena size
    // slider.
    private void InitializeArenaSizeSlider()
    {
        Slider slider = GUIHelper.FindChild<Slider>
                        (GUIHelper.Find("ArenaSizePanel"));
        slider.value = configurator.InitialArenaSize;
        slider.onValueChanged.AddListener
                              (delegate{ SetInitialArenaSize((int)slider.value);});

    }

    // Adds action listener to the start button.
    private void InitializeButtons()
    {
        Button start = GUIHelper.Find<Button>(GUIHelper.Find("StartButton"));
        start.onClick.AddListener(() => StartGame());

        Button exit = GUIHelper.Find<Button>(GUIHelper.Find("ExitButton"));
        exit.onClick.AddListener(() => QuitGame());
    }

    // Sets the initial value and adds action listener to the initial players
    // speed and size sliders.
    private void InitializePlayersSettingsSliders()
    {
        Slider speed = GUIHelper.FindChild<Slider>
                       (GUIHelper.Find("InitialSpeedPanel"));
        speed.value = configurator.InitialPlayerSpeed;
        speed.onValueChanged.AddListener
                             (delegate{SetInitialPlayerSpeed((int)speed.value);});

        Slider size = GUIHelper.FindChild<Slider>
                      (GUIHelper.Find("InitialSizePanel"));
        size.value = configurator.InitialPlayerSize;
        size.onValueChanged.AddListener
                            (delegate{SetInitialPlayerSize((int)size.value);});
    }

    // Loads one panel in place of another.
    private void LoadPanel(GameObject dst, GameObject src, int id)
    {
        // Find PlayerEnabledPanel od 'id' position.
        GameObject panel = GUIHelper.Find(src.name, id);

        // Destroy it.
        DestroyPanel(panel);

        // Create PlayerDisabledPanel on its place.
        CreatePanel(dst, panel, id);
    }
    
    // Udpates configurator with the new initial arena size.
    private void SetInitialArenaSize(int size)
    {
        configurator.InitialArenaSize = size;
    }

    // Starts coorutine that waits for user input.
    private void SetPlayerMovementKey(int id, Button keyButton)
    {
        StartCoroutine(GetPlayerMovementKey(id, keyButton));
    }

    // Updates game configuration with player nickname.
    private void SetPlayerNickname(int id, string nickname)
    {
        if (nickname.Equals(""))
        {
            nickname = configurator.PlayersData[id - 1].Nickname;
        }
        configurator.PlayersData[id - 1].Nickname = nickname;
        GUIHelper.PlayerInitialData[id - 1].Nickname = nickname;
    }

    // Updates configurator with the new initial player size.
    private void SetInitialPlayerSize(int size)
    {
        configurator.InitialPlayerSize = size;
    }

    // Updates configurator with the new initial player speed.
    private void SetInitialPlayerSpeed(int speed)
    {
        configurator.InitialPlayerSpeed = speed;
    }

    // Starts the game.
    private void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    // Updates status of StartButton.
    private void UpdateStartButtonStatus()
    {
        // Get StartButton.
        Button start = GUIHelper.Find<Button>(GUIHelper.Find("StartButton"));

        // Check if all players have movement keys set.
        foreach (PlayerInitialData player in configurator.PlayersData)
        {
            if (player != null)
            {
                if (player.LeftKey == KeyCode.None ||
                    player.RightKey == KeyCode.None)
                {
                    GUIHelper.SetButtonEnabled(start, false);
                    return;
                }
            }
        }
        GUIHelper.SetButtonEnabled(start, true);
    }

    // Quit the game.
    private void QuitGame()
    {
        // Uncomment if you want to test this functionality in Unity editor.
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}

}