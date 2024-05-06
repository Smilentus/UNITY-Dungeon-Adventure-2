namespace Dimasyechka.Code.CoreComponentSystem.Interfaces
{
    public interface IComponent
    {
        public ICore attachedCore { get; set; }

        public void InjectComponent(ICore core);
    }
}
