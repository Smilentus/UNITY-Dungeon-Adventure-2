using Zenject;

namespace Dimasyechka.Code.ZenjectFactories
{
    public class DiContainerFactory
    {
        protected DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
    }
}
