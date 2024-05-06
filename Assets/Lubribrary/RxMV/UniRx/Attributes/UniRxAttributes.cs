using System;

namespace Dimasyechka.Lubribrary.RxMV.UniRx.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RxAdaptablePropertyAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RxAdaptableCollectionAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RxAdaptableCommandAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.Method)]
    public class RxAdaptableMethodAttribute : Attribute { }
}
