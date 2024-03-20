public interface IGameEventsExecuter
{
    public System.Type ProfileType { get; }
    public void TryExecuteGameEvent(BaseGameEventProfile _profile);
}