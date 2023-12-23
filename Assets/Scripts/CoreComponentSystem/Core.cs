using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour, ICore
{
    public List<IComponent> components { get; set; }

    private void Start()
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
