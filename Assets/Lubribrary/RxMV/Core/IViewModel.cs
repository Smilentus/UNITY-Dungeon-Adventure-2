using System;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;

namespace Dimasyechka.Lubribrary.RxMV.Core
{
    public interface IViewModel<T> : IViewModel, IDisposable, IRxLinkable
    {
        public T Model { get; }


        public void SetupModel(T model);
        public void RemoveModel();
    }


    public interface IViewModel { }
}
