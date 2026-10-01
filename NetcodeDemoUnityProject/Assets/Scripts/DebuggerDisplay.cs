using UnityEngine;
using TMPro;

public class DebuggerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    void OnEnable() => PlayerController.OnPlayerSpawned += DisplayPlayerSpawned;
    void OnDisable() => PlayerController.OnPlayerSpawned -= DisplayPlayerSpawned;

    private void DisplayPlayerSpawned(ulong clientId)
    {
        text.text = $"Player spawned! ClientID: {clientId}\nPlayer controls: {DisplayPlayerControls(clientId)}";
    }

    private string DisplayPlayerControls(ulong clientId)
    {
        if (clientId == 0)
        {
            return $"WASD to move";
        }

        else
        {
            return $"Arrow keys to move";
        }
    }
}
