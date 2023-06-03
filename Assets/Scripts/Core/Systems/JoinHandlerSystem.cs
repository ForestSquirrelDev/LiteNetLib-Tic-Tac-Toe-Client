using Core.Components;
using Core.Entities;
using Core.Systems.Events;
using LiteNetLib;
using PoorMansECS.Systems;
using Server.Shared.Network;
using ServerShared.Shared.Network;
using UnityEngine;

namespace Core.Systems {
    public class JoinHandlerSystem : SystemBase, INetMessageListener {
        private IncomingMessagesPipe _incomingMessagesPipe;
        private OutgoingMessagesPipe _outgoingMessagesPipe;
        
        public JoinHandlerSystem(SystemsContext context) : base(context) { }

        public void InjectDependencies(IncomingMessagesPipe incomingMessagesPipe, OutgoingMessagesPipe outgoingMessagesPipe) {
            _incomingMessagesPipe = incomingMessagesPipe;
            _outgoingMessagesPipe = outgoingMessagesPipe;
        }

        protected override void OnStart() {
            _incomingMessagesPipe.Register(MessageType.ConnectionEstablishedMessage, this);
        }

        protected override void OnUpdate(float delta) { }

        protected override void OnStop() { }
        
        public void ReceiveMessage(MessageWrapper messageWrapper) {
            if (messageWrapper.Message is not ConnectionEstablishedMessage)
                return;
            
            _context.World.Entities.GetFirst<Room>().SetComponent(new RoomServerComponent(messageWrapper.AssociatedPeer));
            RequestAndProcessJoin(_outgoingMessagesPipe, messageWrapper.AssociatedPeer, _context);
        }
        
        private async void RequestAndProcessJoin(OutgoingMessagesPipe outgoingPacketsPipe, NetPeer server, SystemsContext context) {
            var joinRequest = new JoinRequestMessage();
            var (success, response) = 
                await outgoingPacketsPipe.SendAndWaitForResponse(server, joinRequest, MessageType.AcceptJoinMessage, 2);
            if (!success) {
                Debug.LogError("Failed to join the room");
                return;
            }
            var responseMessage = (AcceptJoinMessage)response.Message;
            context.World.Entities.GetFirst<Player>().SetComponent(new GameSideComponent((GameSide)responseMessage.GameSide));
            context.EventBus.SendEvent(new JoinedRoomEvent());
        }
    }
}