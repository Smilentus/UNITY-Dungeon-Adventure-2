namespace Dimasyechka.Code.GlobalWindows.Interfaces
{
    public interface IGlobalWindow
    {
        public IGlobalWindowData GlobalWindowData { get; }

        public bool IsShown { get; }

        public void SetWindowData(IGlobalWindowData globalWindowData);

        public void Show(IGlobalWindowData globalWindowData);
        public void Show();
        public void Hide();
    }

    public interface IGlobalWindowData 
    {
        public string GlobalWindowTitle { get; set; }
    }
}