using Core.Managers;

namespace Core.Systems {
    public class SystemsContext {
        public PoorMansECS.Entities.Entities Entities { get; }
        public MessagesPipe MessagesPipe { get; }

        public SystemsContext(PoorMansECS.Entities.Entities entities, MessagesPipe messagesPipe) {
            Entities = entities;
            MessagesPipe = messagesPipe;
        }
    }
}
