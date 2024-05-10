using UnityEngine;
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

    public class DiContainerCreationFactory<T> : DiContainerFactory
        where T : Component
    {
        public T InstantiateForComponent(GameObject originalPrefab, Transform spawnParent)
        {
            T instantiatedComponent = _diContainer.InstantiatePrefabForComponent<T>(originalPrefab, spawnParent);
            return instantiatedComponent;
        }
    }
}
