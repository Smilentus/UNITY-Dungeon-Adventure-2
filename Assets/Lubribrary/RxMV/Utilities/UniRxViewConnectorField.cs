using System;

namespace Dimasyechka.Lubribrary.RxMV.Utilities
{
    [Serializable]
    public class UniRxViewConnectorField
    {
        public bool AutoSyncValues;

        public UniRxReflectionField PresenterReactiveField = new UniRxReflectionField();

        public UniRxReflectionField ViewReactiveField = new UniRxReflectionField();
    }
}
