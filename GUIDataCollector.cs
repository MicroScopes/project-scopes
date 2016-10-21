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

//==================================================
//                 N A M E S P A C E
//==================================================

/*!
 * @brief A global namespace for project-scopes.
 * 
 * @detail Contains all project-scopes related classes.
 */
namespace ProjectScopes
{

//==================================================
//                    C L A S S
//==================================================

/*!
 * @brief Collects game related data from GUI.
 * 
 * @details In EPIC1 version user has possibitity to setup each player color,
 *          nickname and movement keys. It is also possible to set the initial
 *          values of arena size and all players speed and thickness.
 */
public class GUIDataCollector: MonoBehaviour
{
    // Player id is located on 6th position in the name strin and looks as
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

    // Configurator object. Contains initial information about game setup.
    // After the game starts it is updated with the data ste by the user.
    private Configurator configurator;

    // Find objects based on its name and player id.
    private GameObject FindObject(string name, int id)
    {
        return GameObject.Find(name.Insert(PlayerIdPosition, (id).ToString()));
    }

    // Sets panel input field color.
    private void SetPlayerColor(GameObject panel, Color color, bool enabled)
    {
        Color newColor = new Color(color.r, color.g, color.b, (enabled) ? EnabledPanelTransparency : DisabledPanelTransparency);
        panel.GetComponentInChildren<InputField>().GetComponent<Image>().color = newColor;
        // If enabled set the color of disabled button text.
        if (enabled)
        {
            panel.GetComponentInChildren<Button>().GetComponentInChildren<Text>().color = newColor;
        }
    }

    // Reads data from Configruator class and places it on the suitable panels.
    private void SetPanelData(GameObject panel, int id, bool enabled)
    {
        // There is no data to put on disabled panel.
        if (!enabled)
        {
            return;
        }

        // TO BE REMOVED temporary nickname.
        panel.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = "PLAYER " + id.ToString();
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
        GameObject panel = Instantiate((enabled) ? enabledPlayerPrefab : disabledPlayerPrefab);

        // Set the name of it and all its components. By default newly
        // instatniated object has "(Clone)" at the end of its name. That
        // is why name is splitted by '(' and only first chunk is considered.
        panel.name = panel.name.Insert(PlayerIdPosition, id.ToString()).Split('(')[0];
        foreach (Transform child in panel.transform)
        {
            child.name = child.name.Insert(PlayerIdPosition, id.ToString());
        }

        // Set the parent of the panel.
        panel.transform.SetParent(GameObject.Find("PlayersSettingsPanel").transform);

        // Set the panel position.
        panel.transform.localPosition = position;

        // Set the panel scale.
        panel.transform.localScale = Vector3.one;

        // Set the player panel color.
        SetPlayerColor(panel, color, enabled);

        // Reads configuration data and puts it on panel.
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
        --configurator.CurrentNoOfPlayers;
    }

    // Disabled player panel and remove it from the game.
    private void EnablePlayer(int id)
    {
        LoadPanel(DisabledPanel, id, true);

        // Increase number of players.
        ++configurator.CurrentNoOfPlayers;
    }

    // This method is called one during scene initialization. It reads
    // configuration data and applies it to the GUI components.
    private void Start()
    {
        // Load prefabs from resources.
        enabledPlayerPrefab = Resources.Load("Prefabs/PlayerEnabledPanel", typeof(GameObject)) as GameObject;
        disabledPlayerPrefab = Resources.Load("Prefabs/PlayerDisabledPanel", typeof(GameObject)) as GameObject;

        // Initialize configurator object.
        configurator = new Configurator();

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
        GameObject.Find("ArenaSizePanel").GetComponentInChildren<Slider>().value = configurator.InitialArenaSize;
        GameObject.Find("InitialSpeedPanel").GetComponentInChildren<Slider>().value = configurator.InitialPlayersSpeed;
        GameObject.Find("InitialSizePanel").GetComponentInChildren<Slider>().value = configurator.InitialPlayersSize;

        // Add "onClick" action to the START button.
    }
}

}