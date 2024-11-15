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
}
