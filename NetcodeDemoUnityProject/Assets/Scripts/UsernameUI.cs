using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.EventSystems;

public class UsernameUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;

    public static bool IsTyping { get; private set; }

    void Update()
    {
        // Check if the input field is focused
        IsTyping = usernameInputField.isFocused;
    }

    // Called when the username input field is updated
    public void UpdateUsername()
    {
        if (NetworkManager.Singleton == null)
        {
            ResetInputField();
            return;
        }

        // Get the player object and player controller
        var playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
        if (playerObject == null)
        {
            ResetInputField();
            return;
        }

        var playerController = playerObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            ResetInputField();
            return;
        }

        // Update the username on the player controller
        playerController.ChangeUsername(usernameInputField.text);
        ResetInputField();
    }

    // Resets the input field to its default state
    private void ResetInputField()
    {
        usernameInputField.text = "";
        usernameInputField.DeactivateInputField();
        EventSystem.current.SetSelectedGameObject(null);
    }
}
