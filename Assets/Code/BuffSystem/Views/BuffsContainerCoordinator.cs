using Dimasyechka.Code.BuffSystem.Containers;
using UnityEngine;

namespace Dimasyechka.Code.BuffSystem.Views
{
    public class BuffsContainerCoordinator : MonoBehaviour
    {
        [SerializeField]
        private BuffsContainer _buffContainer;

        [SerializeField]
        private BuffsContainerViewModel _buffsContainerViewModel;


        private void Awake()
        {
            _buffsContainerViewModel.SetupModel(_buffContainer);
        }
    }
}
