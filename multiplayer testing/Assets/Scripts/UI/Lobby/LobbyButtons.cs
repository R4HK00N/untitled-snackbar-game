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
    public string[] playerNames;

    public void SetLobbyName()
    {
        lobbyManager.lobbyName = LobbyNameInput.text;
    }
    public void SetLobbyCode()
    {
        LobbyCodeInput.text = lobbyManager.lobbyCode;
    }
    public void CreateLobbyButton(int buttenClickedIndex)
    {
        lobbyManager.ButtonClicked(0);
    }
    public void JoinLobbyByCode()
    {
        lobbyManager.ButtonClicked(1);
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
                break;
            case 1:
                playerTemplate[2].SetActive(true);
                break;
            case 0:
                playerTemplate[3].SetActive(true);
                break;
        }
    }
}
