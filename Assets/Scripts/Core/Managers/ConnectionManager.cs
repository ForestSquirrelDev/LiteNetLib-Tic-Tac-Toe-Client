using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using Newtonsoft.Json;
using PoorMansECS.Systems;
using ServerShared.Shared.Network;
using UnityEngine;
using Utils;

namespace Core.Managers {
    public class ConnectionManager : INetEventListener, IUpdateable {
        private readonly NetManager _netManager;
        private readonly IncomingMessagesPipe _incomingMessagesPipe;
        private readonly ConnectionConfig _config;
        
        private NetPeer _server;

        public ConnectionManager(IncomingMessagesPipe incomingMessagesPipe) {
            _netManager = new NetManager(this) {
                AutoRecycle = true,
                IPv6Enabled = false
            };
            _incomingMessagesPipe = incomingMessagesPipe;
            _config = DeserializeConfig($"{Application.dataPath}/Configs/connection_config.json");
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
            _netManager.Connect(_config.Host, _config.Port, _config.ConnectionKey);
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

        private ConnectionConfig DeserializeConfig(string path) {
            string jsonString = File.ReadAllText(path);
            var configRaw = fastJSON.JSON.ToObject<Dictionary<string, object>>(jsonString);
            return new ConnectionConfig(configRaw.GetString("host"), configRaw.GetInt("port"), configRaw.GetString("connection_key"));
        }
    }
}
