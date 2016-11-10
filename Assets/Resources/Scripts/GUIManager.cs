/*!
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
    public static Configurator configurator = new Configurator();

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

        // Set initial value and add action listeners to sliders and start
        // button components.
        InitializeArenaSizeSlider();
        InitializePlayersSettingsSliders();
        InitializeStartButton();
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
        GUIHelper.SetInitialPlayerData(panel, configurator.Players[id - 1]);

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

    // Loads PlayerEnabledPanel in place of PlayerDisabledPanel.
    private void EnablePlayer(int id)
    {
        // Increase number of players.
        configurator.AddPlayer(id - 1);

        // Fill player data with the initial values.
        string[] data = GUIHelper.PlayerInitialData[id - 1].Split(';');
        configurator.Players[id - 1].Nickname = data[0];
        configurator.Players[id - 1].Color = new Color(float.Parse(data[1]),
                                                       float.Parse(data[2]),
                                                       float.Parse(data[3]),
                                                       1.0f);
        configurator.Players[id - 1].LeftKey = (KeyCode)Enum.Parse
                                               (typeof(KeyCode), data[4]);
        configurator.Players[id - 1].RightKey = (KeyCode)Enum.Parse
                                                (typeof(KeyCode), data[5]);

        // Display panel on the GUI.
        LoadPanel(enabledPanel, disabledPanel, id);
    }

    // Gets user input and updates player movement key.
    private IEnumerator GetPlayerMovementKey(int id, Button keyButton)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                // Decode the key.
                KeyCode keyCode = GUIHelper.KeyPressed();

                // Check if key is supported and noone is using it already.
                if (GUIHelper.IsKeySupported(keyCode) &&
                    !GUIHelper.IsKeyInUsed(keyCode, configurator.Players))
                {
                    // Update GUI with key output format.
                    GUIHelper.UpdateKeyButtonText(keyButton, keyCode);

                    // Determine what key was updated by checking parent name.
                    if (keyButton.name.Contains("Left"))
                    {
                        configurator.Players[id - 1].LeftKey = keyCode;
                    }
                    else
                    {
                        configurator.Players[id - 1].RightKey = keyCode;
                    }

                    break;
                }
            }

            yield return null;
        }
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

    // Adds action listener to the start button.
    private void InitializeStartButton()
    {
        Button start = GUIHelper.Find<Button>(GUIHelper.Find("StartButton"));
        start.onClick.AddListener(() => StartGame());
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
            nickname = configurator.Players[id - 1].Nickname;
        }
        configurator.Players[id - 1].Nickname = nickname;
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
}

}