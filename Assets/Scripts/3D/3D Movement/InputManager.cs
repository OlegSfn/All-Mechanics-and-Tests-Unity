using UnityEngine;

public class InputManager : MonoBehaviour
{

    //Used for singleton
    public static InputManager playerInp = null;

    //Create Keycodes that will be associated with each of our commands.
    //These can be accessed by any other script in our game
    [SerializeField] public KeyCode forward { get; set; }
    public KeyCode backward { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }

    public KeyCode sprint { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode crouch { get; set; }



    void Awake()
    {
        //Singleton pattern
        if (playerInp == null)
        {
            DontDestroyOnLoad(gameObject);
            playerInp = this;
        }
        else if (playerInp != this)
        {
            Destroy(gameObject);
        }

        /*Assign each keycode when the game starts.
         * Loads data from PlayerPrefs so if a user quits the game,
         * their bindings are loaded next time. Default values
         * are assigned to each Keycode via the second parameter
         * of the GetString() function
         */
        // Movement's Keys
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", "W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));

        // Special movement's keys
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("sprintKey", "LeftShift"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("crouchKey", "LeftControl"));
    }
}