using System;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.ReflectionCollectors.Base
{
    public abstract class ReflectiveCollector : IDisposable
    {
        protected object _reflectionObject;


        public void SetReflectionObject(object reflectionObject)
        {
            _reflectionObject = reflectionObject;
        }


        public abstract void CollectReflections();
        public abstract void Dispose();
    }
}
