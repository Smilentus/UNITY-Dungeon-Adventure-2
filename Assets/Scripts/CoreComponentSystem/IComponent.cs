using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponent
{
    public ICore attachedCore { get; set; }

    public void InjectComponent(ICore core);
}
