using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    public TestRelay relay;
    public LobbyButtons lobbyUI;
    public int buttonClickedInt;
    [Header("UI")]
    public string lobbyName = "MyLobby";
    public string lobbyCode;
    public string playerName = "Player";
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;
    private bool playerIsInLobby;
    public bool isHost;
    private async void Start()
    {
        //await UnityServices.InitializeAsync();

        //AuthenticationService.Instance.SignedIn += () =>
        //{
        //    Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        //};

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //Debug.Log(playerName);
    }
    private async void Authenticate()
    {
        InitializationOptions options = new InitializationOptions();
        options.SetProfile(playerName);

        await UnityServices.InitializeAsync(options);

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
    }
    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }
    private async void HandleLobbyHeartbeat()
    {
        if(hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;

            if(heartbeatTimer < 0)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }
    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;

            if (lobbyUpdateTimer < 0)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;

                if (playerIsInLobby)
                {
                    SetPlayerDataToUI();
                }

                if (joinedLobby.Data["StartGamePressed"].Value != "0")
                {
                    if (!isHost)
                    {
                        relay.JoinRelayPressed(joinedLobby.Data["StartGamePressed"].Value);
                    }

                    joinedLobby = null;
                }
            }
        }
    }
    private async void CreateLobby(string lobbyName)
    {
        try
        {
            int maxPlayers = 4;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = true,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "SnackBar", DataObject.IndexOptions.S1 )},
                    { "StartGamePressed", new DataObject(DataObject.VisibilityOptions.Member, "0")},
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            Debug.Log("Created lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

            Printplayers(hostLobby);
            SetLobbyDataToUI();
            //SetPlayerDataToUI();
            playerIsInLobby = true;
            isHost = true;
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void StartGameCLicked()
    {
        if (isHost)
        {
            try
            {
                relay.CreateRelayPressed();
                
                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "StartGamePressed", new DataObject(DataObject.VisibilityOptions.Member, relay.joinCode) }
                    }
                });

                joinedLobby = lobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    private async void Listlobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                    new QueryFilter(QueryFilter.FieldOptions.S1, "Snackbar", QueryFilter.OpOptions.CONTAINS)
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + lobby.MaxPlayers + " " + lobby.Data["Map"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };

            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            
            Debug.Log("Joined lobby with code" + lobbyCode);

            Printplayers(joinedLobby);
            SetLobbyDataToUI();
            //SetPlayerDataToUI();
            playerIsInLobby = true;
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void QuickJoinlobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
            }
        };
    }
    private void PrintPlayers()
    {
        Printplayers(joinedLobby);
    }
    private void Printplayers(Lobby lobby)
    {
        Debug.Log("players in lobby " + lobby.Name + " " + lobby.Data["Map"].Value);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }
    private async void LobbyUpdateMap(string map)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, map) }
                }
            });
            joinedLobby = hostLobby;

            Printplayers(hostLobby);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                }
            });
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;

            Printplayers(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public void SetPlayerName()
    {
        Authenticate();
    }
    public void CreateLobbyClicked()
    {
        CreateLobby(lobbyName);
    }
    public void JoinLobbyClicked()
    {
        JoinLobbyByCode(lobbyCode);
    }
    public void SetLobbyDataToUI()
    {
        lobbyUI.SetLobbyData(joinedLobby.LobbyCode, joinedLobby.Name);
    }
    public void SetPlayerDataToUI()
    {
        lobbyUI.SetPlayer(joinedLobby.AvailableSlots, joinedLobby);
    }


    public void LeaveClicked()
    {
        if (isHost)
        {
            MigrateLobbyHost();
            LeaveLobby();
        }
        else
        {
            LeaveLobby();
        }
    }
}
