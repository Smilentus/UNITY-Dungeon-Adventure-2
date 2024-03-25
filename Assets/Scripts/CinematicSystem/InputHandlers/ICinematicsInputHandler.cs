public interface ICinematicsInputHandler
{
    public event System.Action<bool> onToggled;
    public event System.Action onForceSkip;


    public void Toggle(bool status);
}