using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Collections;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private TMP_Text usernameText;

    public static event System.Action<ulong> OnPlayerSpawned;
    public NetworkVariable<FixedString64Bytes> Username = new();


    private void Update()
    {
        if (!IsLocalPlayer) return;

        // Skip movement if the input field is focused
        if (UsernameUI.IsTyping) return;

        MovePlayer();
    }

    // Called when the player is spawned on the network (is a public override because it's virtual in NetworkBehaviour)
    public override void OnNetworkSpawn()
    {
        // Listen for username changes
        Username.OnValueChanged += OnUsernameChanged;

        if (!IsLocalPlayer) return;

        OnPlayerSpawned?.Invoke(NetworkManager.Singleton.LocalClientId);

        if (Username.Value.IsEmpty) return;

        // Apply the CURRENT synchronized value immediately.
        // This handles players who were already in the game
        // before this client joined.
        UpdateUsernameText(Username.Value);
    }

    public override void OnNetworkDespawn()
    {
        Username.OnValueChanged -= OnUsernameChanged;
    }

    // Returns the WASD input from the keyboard
    private Vector2 GetWASDInput()
    {
        var input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y++;
        if (Keyboard.current.sKey.isPressed) input.y--;
        if (Keyboard.current.aKey.isPressed) input.x--;
        if (Keyboard.current.dKey.isPressed) input.x++;

        return input;
    }

    // Returns the arrow input from the keyboard
    private Vector2 GetArrowInput()
    {
        var input = Vector2.zero;

        if (Keyboard.current.upArrowKey.isPressed) input.y++;
        if (Keyboard.current.downArrowKey.isPressed) input.y--;
        if (Keyboard.current.leftArrowKey.isPressed) input.x--;
        if (Keyboard.current.rightArrowKey.isPressed) input.x++;

        return input;
    }

    // Sets the movement input from the keyboard based on the player's client ID
    private Vector2 GetMovementInput()
    {
        return OwnerClientId == 0 ? GetWASDInput() : GetArrowInput();
    }

    // Moves the player based on the movement input
    private void MovePlayer()
    {
        var movementInput = GetMovementInput();
        var movement = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    public void ChangeUsername(string newUsername)
    {
        // Only the owner can change the username
        if (!IsOwner) return;

        // Send a request to the server to change the username
        SetUsernameRpc(newUsername);
    }

    [Rpc(SendTo.Server)]
    private void SetUsernameRpc(string newUsername)
    {
        // Basic protection against empty names.
        if (string.IsNullOrWhiteSpace(newUsername)) return;

        // The server changes authoritative state
        Username.Value = newUsername;
    }

    // Called when the username changes
    private void OnUsernameChanged(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        UpdateUsernameText(newValue);
    }

    private void UpdateUsernameText(FixedString64Bytes username)
    {
        usernameText.text = username.ToString();
    }
}
