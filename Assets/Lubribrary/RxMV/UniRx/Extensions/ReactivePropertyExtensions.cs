using System;
using System.Collections.Generic;
using UniRx;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Extensions
{
    public static class ReactivePropertyExtensions
    {
        public static List<IDisposable> SubscribeToEachOther<T>(this ReactiveProperty<T> thisProperty, ReactiveProperty<T> targetProperty)
        {
            IDisposable thisPropertyDisposable = thisProperty.Subscribe(x => { targetProperty.Value = x; });
            IDisposable targetPropertyDisposable = targetProperty.Subscribe(x => { thisProperty.Value = x; });

            return new List<IDisposable>() { thisPropertyDisposable, targetPropertyDisposable };
        }
    }
}
