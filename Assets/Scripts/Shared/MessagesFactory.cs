using System;
using System.Collections.Generic;

public static class MessagesFactory {
    private static Dictionary<MessageType, Func<IMessage>> _messagesFunctors = new();
    
    static MessagesFactory() {
        
    }

    public static IMessage Build(MessageType type) {
        return _messagesFunctors[type].Invoke();
    }
}
