using System.Collections.Generic;

namespace Dimasyechka.Code.CoreComponentSystem.Interfaces
{
    public interface ICore
    {
        public List<IComponent> components { get; set; }
        public void BuildWithComponents();
    }
}
