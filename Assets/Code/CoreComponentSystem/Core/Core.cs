using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Code.CoreComponentSystem.Interfaces;
using UnityEngine;

namespace Dimasyechka.Code.CoreComponentSystem.Core
{
    public class Core : MonoBehaviour, ICore
    {
        public List<IComponent> components { get; set; }

        private void Awake()
        {
            BuildWithComponents();
        }

        public virtual void BuildWithComponents()
        {
            components = GetComponentsInChildren<IComponent>().ToList();

            for (int i = 0; i < components.Count; i++)
            {
                components[i].InjectComponent(this);
            }
        }
    }
}
