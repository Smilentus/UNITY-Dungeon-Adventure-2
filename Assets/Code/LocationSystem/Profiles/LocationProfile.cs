using System.Collections.Generic;
using Dimasyechka.Code.GameEventSystem.Profiles;
using Dimasyechka.Code.InventorySystem.BaseItem;
using UnityEngine;

namespace Dimasyechka.Code.LocationSystem.Profiles
{
    [CreateAssetMenu(menuName = "LocationSystem/New LocationProfile", fileName = "LocationProfile")]
    public class LocationProfile : ScriptableObject
    {
        [SerializeField]
        private Sprite m_locationPreviewSprite;
        /// <summary>
        ///     Основное превью локации в виде спрайта
        /// </summary>
        public Sprite LocationPreviewSprite => m_locationPreviewSprite;


        [TextArea(3, 5)]
        [SerializeField]
        private string m_locationTitle;
        /// <summary>
        ///     Наименование локации
        /// </summary>
        public string LocationTitle => m_locationTitle;


        [TextArea(5, 10)]
        [SerializeField]
        private string m_locationDescription;
        /// <summary>
        ///     Подробное описание локации
        /// </summary>
        public string LocationDescription => m_locationDescription;


        [SerializeField]
        private List<BaseGameEventProfile> m_locationEvents = new List<BaseGameEventProfile>();
        /// <summary>
        ///     Список доступных ивентов на локации
        /// </summary>
        public List<BaseGameEventProfile> LocationEvents => m_locationEvents;

        public BaseGameEventProfile GetRandomLocationEvent
            => m_locationEvents.Count > 0
                ? m_locationEvents[Random.Range(0, m_locationEvents.Count)]
                : null;


        [SerializeField]
        private List<BaseItemProfile> m_droppableItems = new List<BaseItemProfile>();
        /// <summary>
        ///     Список предметов, которые могут выпасть на локации
        /// </summary>
        public List<BaseItemProfile> DroppableItems => m_droppableItems;
    }
}
