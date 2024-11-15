using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyButtons : MonoBehaviour
{
    public TestLobby lobbyManager;
    public TMP_InputField LobbyNameInput;
    public TMP_InputField LobbyCodeInput;
    public string lobbyName;
    public string lobbyCode;
    [Header("Created lobby UI")]
    public TMP_Text joinLobbyCodeText;
    public TMP_Text LobbyNameText;
    public GameObject[] playerTemplate;
    public TMP_Text[] playerNameText;
    public List<string> playerNames = new List<string>();

    public void SetLobbyName()
    {
        lobbyManager.lobbyName = LobbyNameInput.text;
    }
    public void SetLobbyCode()
    {
        lobbyManager.lobbyCode = LobbyCodeInput.text;
    }
    public void CreateLobbyButton(int buttenClickedIndex)
    {
        lobbyManager.CreateLobbyClicked();
    }
    public void JoinLobbyByCode()
    {
        lobbyManager.JoinLobbyClicked();
    }

    public void SetLobbyData(string code, string lobbyName)
    {
        joinLobbyCodeText.text = code;
        LobbyNameText.text = lobbyName;
    }
    public void SetPlayer(int amountOfPlayers)
    {
        Debug.Log(amountOfPlayers);
        switch (amountOfPlayers)
        {
            case 3:
                playerTemplate[0].SetActive(true);
                playerNameText[0].text = playerNames[0];
                break;
            case 2:
                playerTemplate[1].SetActive(true);
                playerNameText[1].text = playerNames[1];
                break;
            case 1:
                playerTemplate[2].SetActive(true);
                playerNameText[2].text = playerNames[2];
                break;
            case 0:
                playerTemplate[3].SetActive(true);
                playerNameText[3].text = playerNames[3];
                break;
        }
    }
}
