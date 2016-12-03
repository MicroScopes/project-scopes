/*!
 * @file GUIHelper.cs
 * @brief Contains definition of GUIHelper class.
 * @author MicroScopes
 */

//==================================================
//               D I R E C T I V E S
//==================================================

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;

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
 * @brief This class provides methods for managing GUI elements.
 *
 * @details It helps to keep GUIManager class readable and easy to maintain.
 */
public class GUIHelper
{
    // Position of element 'id' in the name string.
    private const int IdPosition = 6;
    
    /*!
     * @brief Contains list of all possible key codes.
     *
     * @details It is used to determine the user input when it comes to the
     *          selection of movement keys.
     */
    public static readonly Array KeyCodes = Enum.GetValues(typeof(KeyCode));

    /*!
     * @brief Map of key codes supported by the game and corresponding key
     *        output format.
     *
     * @details It is used to print the simpified version of the key codes
     *          on GUI components.
     */
    public static readonly Dictionary<string, string> KeyMap = KeyCodesMap();

    /*!
     * @brief Contains the initial player settings.
     *
     * @details The settings are read from 'defaults.cfg' file.
     */
    public static readonly List<PlayerInitialData> PlayerInitialData = 
                                    InitialPlayerConfiguration();

    // Reads initial players configuration from 'defaults.cfg' file.
    private static List<PlayerInitialData> InitialPlayerConfiguration()
    {
        string path = "Assets/Resources/Configs/defaults.cfg";
        string[] data;
        using (StreamReader streamReader = new StreamReader(path))
        {
            data = streamReader.ReadToEnd().Split('\n');
        }

        List<PlayerInitialData> initialData = new List<PlayerInitialData>();
        foreach (string row in data)
        {
            string[] items = row.Split(',');
            PlayerInitialData player = new PlayerInitialData();
            player.Nickname = items[0];
            player.Color = new Color(float.Parse(items[1]),
                                     float.Parse(items[2]),
                                     float.Parse(items[3]),
                                     1.0f);
            player.LeftKey = (KeyCode)Enum.Parse(typeof(KeyCode), items[4]);
            player.RightKey = (KeyCode)Enum.Parse(typeof(KeyCode), items[5]);

            initialData.Add(player);
        }

        return initialData;
    }

    // Reads list of all key codes and the corresponding key output format from
    // the 'keycodes.cfg' file.
    private static Dictionary<string, string> KeyCodesMap()
    {
        // Read the file and generate tha list based on its content.
        string path = "Assets/Resources/Configs/keycodes.cfg";
        string[] lines = null;
        using (StreamReader streamReader = new StreamReader(path))
        {
            lines = streamReader.ReadToEnd().Split('\n');
        }

        return lines.ToDictionary(s => s.Split(' ')[0], s => s.Split(' ')[1]);
    }

    /*!
     * @brief Finds component of a given name and returns it as GameObject
     *        type.
     *
     * @details If the id is given the name of the element is generated
     *          based on the id value.
     * @param name Name of the requested element.
     * @param id Id of the element. Some of the GUI components have similar
     *           names. The difference between them is the id located on
     *           the 'IdPosition' position in the name string.
     * @return The requested element as GameObject type.
     */
    public static GameObject Find(string name, int id = -1)
    {
        name = (id == -1) ? name : name.Insert(IdPosition, id.ToString());
        return GameObject.Find(name);
    }

    /*!
     * @brief Finds the subcomponent of the given element.
     *
     * @details The type of the subcomponent is determined by the generic
     *          method parameter.
     * @param component The component of which subcomponent is to be found.
     * @return The subcomponent of a given type.
     */
    public static T Find<T>(GameObject component)
    {
        return component.GetComponent<T>();
    }
    
    /*!
     * @brief Finds the child component of the given component.
     *
     * @details The type of the child component is determined by the generic
     *          method parameter.
     * @param component The component of which child component is to be found.
     * @return The child component of a given type.
     */
    public static T FindChild<T>(GameObject component)
    {
        return component.GetComponentInChildren<T>();
    }

    /*!
     * @brief Finds the child component of the given component based on the
     *        child component name.
     *
     * @details The method finds the child component based of the given name.
     *          Its usefull when the component has many child components of the
     *          same type. The type of the child component is determined by the
     *          generic method parameter.
     * @param component The component of which child component is to be found.
     * @param pattern The part of the child component name.
     * @return The child component of the given type.
     */
    public static T FindChild<T>(GameObject component, string pattern)
    {
        T[] components = component.GetComponentsInChildren<T>();
        return components.First(s => s.ToString().Contains(pattern));
    }

    /*!
     * @brief Checks is key code is already in use.
     *
     * @details Searches through the all players and looks at theirs movement
     *          keys.
     * @param keyCode Key code to be verified.
     * @param players List of all players.
     * @return True or false depending on verification status.
     */
    public static bool IsKeyInUsed(KeyCode keyCode,
                                   List<PlayerInitialData> players)
    {
        foreach (PlayerInitialData player in players)
        {
            if (player != null &&
               (keyCode == player.LeftKey || keyCode == player.RightKey))
            {
                return true;
            }
        }

        return false;
    }

    /*!
     * @brief Checks if the given key code is supported by the game.
     *
     * @details Looks for the key code string representation in the 'KeyMap'.
     * @param keyCode Key code to be verified.
     * @return True or false depending on verification status.
     */
    public static bool IsKeySupported(KeyCode keyCode)
    {
        return KeyMap.ContainsKey(keyCode.ToString());
    }

    /*!
     * @brief Checks what key was pressed by the user.
     *
     * @details Based on the 'KeyMap' the code of the pressed key is taken.
     * @return KeyCode representation of the pressed key.
     */ 
    public static KeyCode KeyPressed()
    {
        KeyCode key = 0;
        foreach (KeyCode keyCode in KeyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                key = keyCode;
                break;
            }
        }

        return key;
    }

    /*!
     * @brief Loads an object of the given name into GameObject component.
     *
     * @details All project related data is located into Resources directory.
     * @param name Name of the object to be load. Full path has to be provided.
     * @return GameObject representation of the search facility.
     */
    public static GameObject Load(string path)
    {
        return Resources.Load(path, typeof(GameObject)) as GameObject;
    }

    /*!
     * @brief Renames component and all its children.
     *
     * @details The component and its subcomponents name is based on its
     *          initial name and id. The '(Clone)' is removed from the final
     *          component name. The component name must start with 'Player'.
     * @param root The component that is to be renamed.
     * @param id Id of the component.
     */
    public static void RenamePlayerComponent(GameObject root, int id)
    {
        root.name = root.name.Split('(')[0].Insert(IdPosition, id.ToString());
        foreach (Transform child in root.transform)
        {
            if (!child.name.Contains("Player"))
            {
                continue;
            }
            child.name = child.name.Insert(IdPosition, id.ToString());
        }
    }

    /*!
     * @bried Sets button interactable and color.
     *
     * @details Depending on button state sets its properties.
     * @param button Button to be modified.
     * @param enabled Button state.
     */
    public static void SetButtonEnabled(Button button, bool enabled)
    {
        Color buttonColor = Find<Image>(button.gameObject).color;
        buttonColor.a = enabled ? 1.0f : 0.25f;
        button.interactable = enabled;
        Find<Image>(button.gameObject).color = buttonColor;

        Color textColor = FindChild<Text>(button.gameObject).color;
        textColor.a = enabled ? 1.0f : 0.25f;
        FindChild<Text>(button.gameObject).color = textColor;
    }

    /*!
     * @brief Updates GUI components with the initial player data.
     *
     * @details The data here is the actual data that is stored in the
     *          PlayerInitialData class.
     * @param panel The panel on which the data is to be displayed.
     * @param data The data to be displayed.
     */
    public static void SetInitialPlayerData(GameObject panel, PlayerInitialData player)
    {
        // Display data on PlayerEnabeldPanel only.
        if (panel.name.Contains("Enabled"))
        {
            // Nickname.
            InputField nickname = FindChild<InputField>(panel);
            nickname.text = player.Nickname;
            nickname.placeholder.GetComponent<Text>().text = player.Nickname;

            // Color.
            Image color = Find<Image>(FindChild<InputField>(panel).gameObject);
            color.color = player.Color;

            // Left turn key.
            Text leftKey = FindChild<Text>(FindChild<Button>(panel, "Left")
                                           .gameObject);
            leftKey.text = KeyMap[player.LeftKey.ToString()];

            // Right turn key.
            Text rightKey = FindChild<Text>(FindChild<Button>(panel, "Right")
                                           .gameObject);
            rightKey.text = KeyMap[player.RightKey.ToString()];
        }
    }

    /*!
     * @brief Sets the color of the given component.
     *
     * @details The component color is based on source component color.
     *          If the PlayerEnabledPanel is to be created the color is opaque,
     *          otherwise the color has 75% transparency.
     * @param dst Destination element which is given with color.
     * @param src Source element from which the color value is taken.
     */
    public static void SetPlayerComponentColor(GameObject dst, GameObject src)
    {
        // Find the source element color and set its transparency depending on
        // component type.
        Color color = Find<Image>(FindChild<InputField>(src).gameObject).color;
        color.a = Find<Image>(dst).color.a;

        // Set the color of the input field.
        InputField inputField = FindChild<InputField>(dst);
        Find<Image>(inputField.gameObject).color = color;

        // In case of enabled panel set the color of the disable button text.
        Button disableButton = FindChild<Button>(dst, "Disable");
        if (disableButton)
        {
            FindChild<Text>(disableButton.gameObject).color = color;
        }
    }

    /*!
     * @brief Updates key button text with the key code output format.
     *
     * @detail The output format is retrieved from 'KeyMap'.
     * @param keyButton Key button to be updated.
     * @param keyCode Key code on which output format is based.
     */
    public static void UpdateKeyButtonText(Button keyButton, KeyCode keyCode)
    {
        Text keyText = FindChild<Text>(keyButton.gameObject);
        keyText.text = KeyMap[keyCode.ToString()];
    }

    /*!
     * @brief Validates user text input.
     *
     * @details Converts all added characters to capital letters.
     * @param text Inserted text.
     * @param index Index on characted in the text.
     * @param addedChar Newly added characted.
     * @return Inserted character converted to upper case.
     */
    public static char ValidateNickname(string text, int index, char addedChar)
    {
        // Return addedChar converted to uppercase.
        if (char.IsWhiteSpace(addedChar))
        {
            return char.MinValue;
        }
        return char.ToUpper(addedChar);
    }
}

}