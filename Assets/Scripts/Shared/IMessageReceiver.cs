public interface IMessageReceiver {
    public void Receive(MessageType type, IMessage message);
}
