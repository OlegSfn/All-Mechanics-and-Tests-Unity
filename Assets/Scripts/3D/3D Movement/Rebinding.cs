using UnityEngine;
using System.Collections;
using TMPro;

public class Rebinding : MonoBehaviour
{
    // Panels
    [SerializeField] private GameObject inputPanel;
    private Transform menuPanel;

    // Reference to InputManager 
    private InputManager playerInp;

    // Handling button change
    private bool waitingForKey = false;
    private KeyCode newKey;
    private Event keyEvent;

    // Key text
    private TextMeshProUGUI buttonText;


    void Start()
    {
        //Assign menuPanel to the Panel object in our Canvas
        //Make sure it's not active when the game starts
        inputPanel.SetActive(false);

        // Assigning values
        menuPanel = inputPanel.transform.GetChild(0);
        int lastChild = menuPanel.GetChild(0).childCount-1;
        waitingForKey = false;
        playerInp = InputManager.playerInp;

        // Matching text values in rebinding menu with our InputManager
        for (int i = 0; i < menuPanel.childCount; i++)
        {
            Transform layoutGroup = menuPanel.GetChild(i);
            if (layoutGroup.name == "Forward")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.forward.ToString();
            else if (layoutGroup.name == "Backward")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.backward.ToString();
            else if (layoutGroup.name == "Left")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.left.ToString();
            else if (layoutGroup.name == "Right")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.right.ToString();
            else if (layoutGroup.name == "Sprint")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.sprint.ToString();
            else if (layoutGroup.name == "Jump")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.jump.ToString();
            else if (layoutGroup.name == "Crouch")
                layoutGroup.GetChild(lastChild).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerInp.crouch.ToString();
        }
    }

	void Update()
    {
        //Escape key will open or close the panel
        if (Input.GetKeyDown(KeyCode.Escape) && !inputPanel.gameObject.activeSelf)
		{
            inputPanel.gameObject.SetActive(true);

            // Making cursor visible
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            // Freezing time and movements
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && inputPanel.gameObject.activeSelf)
		{
            inputPanel.gameObject.SetActive(false);
            
            // Making cursor unvisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Unfreezing time and movements
            Time.timeScale = 1f;
        }
    }



    void OnGUI()
    {
        /*keyEvent dictates what key our user presses
         * bt using Event.current to detect the current
         * event
         */
        keyEvent = Event.current;

        //Executes if a button gets pressed and
        //the user presses a key
        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
            waitingForKey = false;
        }
    }

    /*Buttons cannot call on Coroutines via OnClick().
     * Instead, we have it call StartAssignment, which will
     * call a coroutine in this script instead, only if we
     * are not already waiting for a key to be pressed.
     */

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    //Assigns buttonText to the text component of
    //the button that was pressed

    public void SendText(TextMeshProUGUI text)
    {
        buttonText = text;
    }

    /*AssignKey takes a keyName as a parameter. The
     * keyName is checked in a switch statement. Each
     * case assigns the command that keyName represents
     * to the new key that the user presses, which is grabbed
     * in the OnGUI() function, above.
     */

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;
        while (!keyEvent.isKey)
            yield return null;

        switch (keyName)
        {
            case "forward":
                playerInp.forward = newKey; //Set forward to new keycode
                buttonText.text = newKey.ToString(); //Set button text to new key
                PlayerPrefs.SetString("forwardKey", newKey.ToString()); //save new key to PlayerPrefs
                break;

            case "backward":
                playerInp.backward = newKey; //set backward to new keycode
                buttonText.text = newKey.ToString(); //set button text to new key
                PlayerPrefs.SetString("backwardKey", newKey.ToString()); //save new key to PlayerPrefs
                break;

            case "left":
                playerInp.left = newKey; //set left to new keycode
                buttonText.text = newKey.ToString(); //set button text to new key
                PlayerPrefs.SetString("leftKey", newKey.ToString()); //save new key to playerprefs
                break;

            case "right":
                playerInp.right = newKey; //set right to new keycode
                buttonText.text = newKey.ToString(); //set button text to new key
                PlayerPrefs.SetString("rightKey", newKey.ToString()); //save new key to playerprefs
                break;

            case "sprint":
                playerInp.sprint = newKey; //set crouch to new keycode
                buttonText.text = newKey.ToString(); //set button text to new key
                PlayerPrefs.SetString("sprintKey", newKey.ToString()); //save new key to playerprefs
                break;

            case "jump":
                playerInp.jump = newKey; //set jump to new keycode
                buttonText.text = newKey.ToString(); //set button text to new key
                PlayerPrefs.SetString("jumpKey", newKey.ToString()); //save new key to playerprefs
                break;

            case "crouch":
                playerInp.crouch = newKey; //set crouch to new keycode
                buttonText.text = newKey.ToString(); //set button text to new key
                PlayerPrefs.SetString("crouchKey", newKey.ToString()); //save new key to playerprefs
                break;
        }
        yield return null;
    }
}
