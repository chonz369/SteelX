using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public abstract class PhotonNetworkTransport : INetworkTransport, IConnectionCallbacks, IOnEventCallback, IInRoomCallbacks, IMatchmakingCallbacks
{
    public PhotonNetworkTransport() {
        PhotonNetwork.AddCallbackTarget(this);

        PhotonNetwork.SendRate = NetworkConfig.PhotonSendRate.IntValue;
        PhotonNetwork.SerializationRate = NetworkConfig.PhotonSerializeRate.IntValue;
    }

    //client wait server to send a connect event to him, so client won't send anything if server isn't joined
    public void Connect() {
        if (!PhotonNetwork.IsConnected) {//this is for starting game directly , not through lobby->room
            PhotonNetwork.ConnectUsingSettings();
        }else if (!PhotonNetwork.InRoom) {
            OnConnectedToMaster();
        }else {
            transportEvents.Enqueue(new TransportEvent(TransportEvent.Type.Connect, PhotonNetwork.LocalPlayer.ActorNumber, null));
        }
    }

    public void Disconnect() {
        transportEvents.Enqueue(new TransportEvent(TransportEvent.Type.Disconnect, PhotonNetwork.LocalPlayer.ActorNumber, null));

        Shutdown();
    }
    
    public bool NextEvent(ref TransportEvent e) {
        if (transportEvents.Count > 0) {
            e = transportEvents.Dequeue();
            return true;
        }
        return false;
    }

    public virtual string GetConnectionDescription(int connectionId) {
        if(PhotonNetwork.IsConnected || PhotonNetwork.InRoom) {
            return "Not connected";
        } else {
            return $"Region : {PhotonNetwork.CloudRegion}, Room name : {PhotonNetwork.CurrentRoom.Name}, " +
                $"ConnectionId : {PhotonNetwork.LocalPlayer.ActorNumber}, IsMasterClient : {PhotonNetwork.IsMasterClient}";
        }
    }

    public abstract void SendData(int connectionId, TransportEvent.Type type, byte[] data);

    public virtual void Shutdown() {
        PhotonNetwork.RemoveCallbackTarget(this);

        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public virtual void Update() {
    }

    //Photon interface
    //========================================================
    public void OnConnected() {
    }

    public virtual void OnConnectedToMaster() {
        GameDebug.Log("Connected to master");

        //this is for starting game directly , not through lobby->room
        if (!PhotonNetwork.InRoom) {
            PhotonNetwork.JoinOrCreateRoom(NetworkConfig.TestRoomName.Value, new RoomOptions(), TypedLobby.Default);
        }
    }

    public virtual void OnDisconnected(DisconnectCause cause) {
        GameDebug.Log("Disconnected due to : " + cause.ToString());

        Disconnect();
    }

    public abstract void OnEvent(EventData photonEvent);

    public virtual void OnRegionListReceived(RegionHandler regionHandler) {
    }

    public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data) {
    }

    public virtual void OnCustomAuthenticationFailed(string debugMessage) {
    }

    public virtual void OnPlayerEnteredRoom(Player newPlayer) {
    }

    public virtual void OnPlayerLeftRoom(Player otherPlayer) {
    }

    public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
    }

    public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
    }

    public virtual void OnMasterClientSwitched(Player newMasterClient) {
    }

    public virtual void OnFriendListUpdate(List<FriendInfo> friendList) {
    }

    public virtual void OnCreatedRoom() {
    }

    public virtual void OnCreateRoomFailed(short returnCode, string message) {
    }

    public virtual void OnJoinedRoom() {
        GameDebug.Log("Joined room : " + PhotonNetwork.CurrentRoom.Name);
    }

    public virtual void OnJoinRoomFailed(short returnCode, string message) {
    }

    public virtual void OnJoinRandomFailed(short returnCode, string message) {
    }

    public virtual void OnLeftRoom() {
    }

    //========================================================

    protected Queue<TransportEvent> transportEvents = new Queue<TransportEvent>(256);
}

public class ClientPhotonNetworkTransport : PhotonNetworkTransport
{
    public override void OnEvent(EventData photonEvent) {
        TransportEvent transportEvent = new TransportEvent();
        if (Enum.IsDefined(typeof(TransportEvent.Type), (TransportEvent.Type)photonEvent.Code)) {
            transportEvent.type = (TransportEvent.Type)photonEvent.Code;
            transportEvent.ConnectionId = PhotonNetwork.LocalPlayer.ActorNumber;
        } else {//we may also receive some photon internal events
            return;
        }

        if((TransportEvent.Type)photonEvent.Code == TransportEvent.Type.Data) {
            transportEvent.Data = (byte[])photonEvent.CustomData;
        }

        transportEvents.Enqueue(transportEvent);
    }

    public override void SendData(int connectionId, TransportEvent.Type type, byte[] data) {
        RaiseEventOptions options = new RaiseEventOptions() {
            Receivers = ReceiverGroup.MasterClient
        };
        PhotonNetwork.RaiseEvent((byte)type, data, options, SendOptions.SendUnreliable);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        transportEvents.Enqueue(new TransportEvent(TransportEvent.Type.Connect, PhotonNetwork.LocalPlayer.ActorNumber, null));
    }
}

public class ServerPhotonNetworkTransport : PhotonNetworkTransport, IMatchmakingCallbacks
{
    public override void OnEvent(EventData photonEvent) {
        TransportEvent transportEvent = new TransportEvent();
        if (Enum.IsDefined(typeof(TransportEvent.Type), (TransportEvent.Type)photonEvent.Code)) {
            transportEvent.type = (TransportEvent.Type)photonEvent.Code;
            transportEvent.ConnectionId = photonEvent.Sender;
        } else {//we may also receive some photon internal events
            return;
        }

        if ((TransportEvent.Type)photonEvent.Code == TransportEvent.Type.Data) {
            transportEvent.Data = (byte[])photonEvent.CustomData;
        }

        transportEvents.Enqueue(transportEvent);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();

        if (!PhotonNetwork.IsMasterClient) {
            GameDebug.Log("Request to be the master client");
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        //trigger connect event foreach player
        foreach(var player in PhotonNetwork.PlayerList) {
            transportEvents.Enqueue(new TransportEvent(TransportEvent.Type.Connect, player.ActorNumber, null));
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        GameDebug.Assert(newMasterClient == PhotonNetwork.LocalPlayer);
        GameDebug.Log("Server is the master client now");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);

        transportEvents.Enqueue(new TransportEvent(TransportEvent.Type.Connect, newPlayer.ActorNumber, null));
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        base.OnPlayerLeftRoom(otherPlayer);

        transportEvents.Enqueue(new TransportEvent(TransportEvent.Type.Disconnect, otherPlayer.ActorNumber, null));
    }

    public override void SendData(int connectionId, TransportEvent.Type type, byte[] data) {
        RaiseEventOptions options = new RaiseEventOptions() {
            TargetActors = new int[] { connectionId }
        };
        PhotonNetwork.RaiseEvent((byte)type, data, options, SendOptions.SendUnreliable);
    }
}

