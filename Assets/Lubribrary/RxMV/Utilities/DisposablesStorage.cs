using System;
using System.Collections.Generic;

namespace Dimasyechka.Lubribrary.RxMV.Utilities
{
    public sealed class DisposablesStorage : IDisposablesStorage
    {
        private List<IDisposable> _disposables = new List<IDisposable>();


        public void Dispose()
        {
            ClearDisposables();
            _disposables = null;
        }


        public void ClearDisposables()
        {
            if (_disposables != null)
            {
                for (int i = 0; i < _disposables.Count; i++)
                {
                    _disposables[i].Dispose();
                }
            }

            _disposables = new List<IDisposable>();
        }


        public void AddToDisposables(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void AddToDisposables(List<IDisposable> disposables)
        {
            _disposables.AddRange(disposables);
        }


        public void RemoveDisposables(IDisposable disposable)
        {
            disposable.Dispose();
            _disposables.Remove(disposable);
        }

        public void RemoveDisposables(List<IDisposable> disposables)
        {
            foreach (IDisposable disposable in disposables)
            {
                disposable.Dispose();
                _disposables.Remove(disposable);
            }
        }
    }
}
