using System;
using System.Collections.Generic;

namespace Core.Managers {
    public class MessagesPipe {
        private readonly Dictionary<MessageType, List<IMessageReceiver>> _receivers;
        private readonly ConnectionManager _connectionManager;

        public MessagesPipe(ConnectionManager connectionManager) {
            _receivers = new Dictionary<MessageType, List<IMessageReceiver>>();
            foreach (var messageType in Enum.GetValues(typeof(MessageType))) {
                _receivers[(MessageType)messageType] = new List<IMessageReceiver>();
            }

            _connectionManager = connectionManager;
        }

        public void Register(MessageType type, IMessageReceiver receiver) {
            _receivers[type].Add(receiver);
        }
    }
}
