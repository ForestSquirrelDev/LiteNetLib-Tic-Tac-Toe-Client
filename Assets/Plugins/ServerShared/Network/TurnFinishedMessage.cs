using LiteNetLib.Utils;

namespace ServerShared.Shared.Network {
    public struct TurnFinishedMessage : IMessage {
        public MessageType Type => MessageType.TurnFinished;
        public int X { get; private set; }
        public int Y { get; private set; }
        public byte GameSide { get; private set; }
        
        public void Serialize(NetDataWriter writer) {
            writer.Put(X);
            writer.Put(Y);
            writer.Put(GameSide);
        }

        public void Deserialize(NetDataReader reader) {
            X = reader.GetInt();
            Y = reader.GetInt();
            GameSide = reader.GetByte();
        }
    }
}
