using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BaseCinematicProfile", menuName = "CinematicSystem/New BaseCinematicProfile")]
public partial class BaseCinematicProfile : ScriptableObject
{
    [field: SerializeField]
    public string Title { get; protected set; }

    [field: SerializeField]
    public string Body { get; protected set; }


    [field: SerializeField]
    public List<BaseVoiceClipData> SubtitleDatas { get; protected set; }
}