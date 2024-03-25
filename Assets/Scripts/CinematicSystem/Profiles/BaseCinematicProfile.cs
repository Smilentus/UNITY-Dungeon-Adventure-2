using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BaseCinematicProfile", menuName = "CinematicSystem/New BaseCinematicProfile")]
public partial class BaseCinematicProfile : ScriptableObject
{
    [TextArea(3, 5)]
    [SerializeField]
    private string m_title;
    public string Title => m_title;


    [TextArea(5, 10)]
    [SerializeField]
    private string m_body;
    public string Body => m_body;


    [field: SerializeField]
    public List<BaseVoiceClipData> SubtitleDatas { get; protected set; }
}