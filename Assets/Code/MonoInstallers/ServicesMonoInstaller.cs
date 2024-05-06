using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.MonoInstallers
{
    public class ServicesMonoInstaller : MonoInstaller
    {
        [Header("Instances")]
        [SerializeField]
        private GameController _gameController;


        public override void InstallBindings()
        {
            
        }
    }
}
