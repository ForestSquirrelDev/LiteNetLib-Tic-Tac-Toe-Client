namespace Core.Managers {
    public readonly struct ConnectionConfig {
        public string Host { get; }
        public int Port { get; }
        public string ConnectionKey { get; }

        public ConnectionConfig(string host, int port, string connectionKey) {
            Host = host;
            Port = port;
            ConnectionKey = connectionKey;
        }
    }
}
