using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Mirror.NetworkManager networkManager;

    public InputField ip;
    public InputField username;
    public Button Generate;
    public Camera startCamera;

    public void Connect()
    {
        if (!string.IsNullOrEmpty(ip.text))
        {
            networkManager.networkAddress = ip.text;
            networkManager.StartClient();
        }
        else
        {
            networkManager.StartClient();
        }
        Ender();
    }

    public void Host()
    {
        if (!string.IsNullOrEmpty(ip.text))
        {
            networkManager.networkAddress = ip.text;
            networkManager.StartHost();
        }
        else
        {
            networkManager.StartHost();
        }
        Generate.gameObject.SetActive(true);
        Ender();
    }

    public void Ender()
    {
        startCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Lock()
    {
        Generate.gameObject.SetActive(false);
    }

    public string GetUsername()
    {
        return username.text;
    }
}
