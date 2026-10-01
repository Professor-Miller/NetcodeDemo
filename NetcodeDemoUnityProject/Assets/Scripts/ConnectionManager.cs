using UnityEngine;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;

public class ConnectionManager : MonoBehaviour
{
    // Assigned to the Host button in the UI
    public void Host()
    {
        LANConnectServer();
        NetworkManager.Singleton.StartHost();
    }

    // Assigned to the Join button in the UI
    public void Join()
    {
        //LANConnectClient();
        NetworkManager.Singleton.StartClient();
    }

    private void LANConnectClient()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("192.168.50.1", 7777);
    }

    private void LANConnectServer()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("0.0.0.0", 7777);
    }
}
