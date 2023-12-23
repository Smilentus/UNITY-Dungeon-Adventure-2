public interface IBattleActionInteraction
{
    public string InteractionTitle { get; }

    public BaseBattleInteractionView ActionProfileViewPrefab { get; }
    public SerializableMonoScript<IBattleActionExecuter> ActionExecuter { get; }
}