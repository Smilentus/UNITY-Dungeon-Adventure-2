using Dimasyechka.Code.GameEventSystem.Profiles;

namespace Dimasyechka.Code.GameEventSystem.Interfaces
{
    public interface IGameEventsExecuter
    {
        public System.Type ProfileType { get; }
        public void TryExecuteGameEvent(BaseGameEventProfile _profile);
    }
}