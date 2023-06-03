using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using PoorMansECS.Systems;
using ServerShared.Shared.Network;
using UnityEngine;

namespace Core.Managers {
    public class ConnectionManager : INetEventListener, IUpdateable {
        private readonly NetManager _netManager;
        private readonly IncomingMessagesPipe _incomingMessagesPipe;
        
        private NetPeer _server;

        public ConnectionManager(IncomingMessagesPipe incomingMessagesPipe) {
            _netManager = new NetManager(this) {
                AutoRecycle = true,
                IPv6Enabled = false
            };
            _incomingMessagesPipe = incomingMessagesPipe;
        }

        public void Start() {
            _netManager.Start();
        }
    
        public void Update(float delta) {
            _netManager.PollEvents();   
        }

        public void Dispose() {
            _server?.Disconnect();
        }

        public void Connect() {
            _netManager.Connect("192.168.103.196", 9050, "SomeConnectionKey");
        }

        public void OnPeerConnected(NetPeer peer) {
            _server = peer;
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
            _server = null;
            Debug.Log("Peer disconnected");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) {
            Debug.LogError($"OnNetworkError: {socketError.ToString()}");
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod) {
            Debug.Log($"Network receive: {reader.RawData}");
            _incomingMessagesPipe.ProcessMessage(peer, reader, deliveryMethod);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) { }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency) { }

        public void OnConnectionRequest(ConnectionRequest request) { }
    }
}
