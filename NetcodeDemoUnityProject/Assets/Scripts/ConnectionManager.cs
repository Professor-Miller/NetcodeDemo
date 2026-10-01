using UnityEngine;
using Unity.Netcode.Transports.UTP;
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
        LANConnect();
        NetworkManager.Singleton.StartClient();
    }

    private void LANConnect()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("192.168.50.1",7777);
    }
}
