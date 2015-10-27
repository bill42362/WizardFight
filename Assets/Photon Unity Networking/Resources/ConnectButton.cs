using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectButton : Photon.PunBehaviour
{
    public void OnClicked()
    {
        if ( !PhotonNetwork.connected )
            PhotonNetwork.ConnectUsingSettings("0.001");
        else
        {
            Debug.Log("Connection Duplicated!!!");
        }
    }
    public override void OnConnectedToPhoton()
    {
        base.OnConnectedToPhoton();
        Button button = this.GetComponent<Button>();
        button.interactable = false;
        Debug.Log("Connection Success!!!");
    }

}
