using UnityEngine;
using Unity.Netcode;

public class ConnectionManager : MonoBehaviour
{
    // Assigned to the Host button in the UI
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
    }

    // Assigned to the Join button in the UI
    public void Join()
    {
        NetworkManager.Singleton.StartClient();
    }
}
