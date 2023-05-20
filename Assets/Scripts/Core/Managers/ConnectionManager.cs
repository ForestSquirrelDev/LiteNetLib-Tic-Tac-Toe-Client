using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using PoorMansECS.Systems;
using UnityEngine;

namespace Core.Managers {
    public class ConnectionManager : INetEventListener, IUpdateable {
        private readonly NetManager _netManager;
    
        private NetPeer _server;

        public ConnectionManager() {
            _netManager = new NetManager(this) {
                AutoRecycle = true,
                IPv6Enabled = false
            };
            _netManager.Start();
        }
    
        public void Update(float delta) {
            _netManager.PollEvents();   
        }

        public void Connect() {
            _netManager.Connect("localhost", 9999, "SomeConnectionKey");
        }

        public void OnPeerConnected(NetPeer peer) {
            _server = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            _server = null;
            Debug.Log("Peer disconnected");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
            Debug.LogError($"OnNetworkError. Endpoint: {endPoint.Address}. Socket error: {socketError}");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {
            Debug.Log($"OnNetworkReceive: {reader.GetString()}");
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) {
            Debug.Log("OnNetworkReceiveUnconnected");
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

        public void OnConnectionRequest(ConnectionRequest request) { }
    }
}
