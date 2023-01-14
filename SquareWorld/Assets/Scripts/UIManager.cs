using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Mirror.NetworkManager networkManager;

    public InputField inputField;
    public Button Generate;
    public Camera startCamera;

    public void Connect()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            networkManager.networkAddress = inputField.text;
            networkManager.StartClient();
        }
        else
        {
            networkManager.StartClient();
        }
        startCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Host()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            networkManager.networkAddress = inputField.text;
            networkManager.StartHost();
        }
        else
        {
            networkManager.StartHost();
        }
        Generate.gameObject.SetActive(true);
        startCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Lock()
    {
        Generate.gameObject.SetActive(false);
    }
}
