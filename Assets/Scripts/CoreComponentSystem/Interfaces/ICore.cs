using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICore
{
    public List<IComponent> components { get; set; }
    public void BuildWithComponents();
}
