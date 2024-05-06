using System;
using System.Collections.Generic;

namespace Dimasyechka.Lubribrary.RxMV.Utilities
{
    public interface IDisposablesStorage : IDisposable
    {
        void ClearDisposables();

        void AddToDisposables(IDisposable disposable);
        void AddToDisposables(List<IDisposable> disposables);

        void RemoveDisposables(IDisposable disposable);
        void RemoveDisposables(List<IDisposable> disposables);
    }
}
