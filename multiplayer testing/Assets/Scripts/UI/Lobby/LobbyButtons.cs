using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyButtons : MonoBehaviour
{
    public TestLobby lobbyManager;
    public TMP_InputField playerNameInput;
    public TMP_InputField LobbyNameInput;
    public TMP_InputField LobbyCodeInput;
    [Header("Created lobby UI")]
    public GameObject startGameButton;
    public TMP_Text joinLobbyCodeText;
    public TMP_Text LobbyNameText;
    public GameObject[] playerTemplate;
    public TMP_Text[] playerNameText;
    public List<string> playerNames = new List<string>();

    public void SetPlayerName()
    {
        lobbyManager.playerName = playerNameInput.text;
    }
    public void SetLobbyName()
    {
        lobbyManager.lobbyName = LobbyNameInput.text;
    }
    public void SetLobbyCode()
    {
        lobbyManager.lobbyCode = LobbyCodeInput.text;
    }
    public void SetPlayerButton()
    {
        lobbyManager.playerName = playerNameInput.text;
        lobbyManager.SetPlayerName();
    }
    public void CreateLobbyButton()
    {
        lobbyManager.lobbyName = LobbyNameInput.text;
        lobbyManager.CreateLobbyClicked();

        startGameButton.SetActive(true);
    }
    public void JoinLobbyByCode()
    {
        lobbyManager.lobbyName = LobbyNameInput.text;
        lobbyManager.JoinLobbyClicked();

        startGameButton.SetActive(false);
    }

    public void SetLobbyData(string code, string lobbyName)
    {
        joinLobbyCodeText.text = code;
        LobbyNameText.text = lobbyName;
    }

    public void LeaveLobbyButton()
    {
        lobbyManager.LeaveClicked();
    }
    public void SetPlayer(int amountOfPlayers, Lobby lobby)
    {
        switch (amountOfPlayers)
        {
            case 3:
                playerTemplate[0].SetActive(true);
                playerNameText[0].text = lobby.Players[0].Data["PlayerName"].Value;

                playerTemplate[1].SetActive(false);
                playerTemplate[2].SetActive(false);
                playerTemplate[3].SetActive(false);
                break;
            case 2:
                playerTemplate[0].SetActive(true);
                playerNameText[0].text = lobby.Players[0].Data["PlayerName"].Value;

                playerTemplate[1].SetActive(true);
                playerNameText[1].text = lobby.Players[1].Data["PlayerName"].Value;

                playerTemplate[2].SetActive(false);
                playerTemplate[3].SetActive(false);
                break;
            case 1:
                playerTemplate[0].SetActive(true);
                playerNameText[0].text = lobby.Players[0].Data["PlayerName"].Value;

                playerTemplate[1].SetActive(true);
                playerNameText[1].text = lobby.Players[1].Data["PlayerName"].Value;

                playerTemplate[2].SetActive(true);
                playerNameText[2].text = lobby.Players[2].Data["PlayerName"].Value;

                playerTemplate[3].SetActive(false);
                break;
            case 0:
                playerTemplate[0].SetActive(true);
                playerNameText[0].text = lobby.Players[0].Data["PlayerName"].Value;

                playerTemplate[1].SetActive(true);
                playerNameText[1].text = lobby.Players[1].Data["PlayerName"].Value;

                playerTemplate[2].SetActive(true);
                playerNameText[2].text = lobby.Players[2].Data["PlayerName"].Value;

                playerTemplate[3].SetActive(true);
                playerNameText[3].text = lobby.Players[3].Data["PlayerName"].Value;
                break;
        }
    }
}
