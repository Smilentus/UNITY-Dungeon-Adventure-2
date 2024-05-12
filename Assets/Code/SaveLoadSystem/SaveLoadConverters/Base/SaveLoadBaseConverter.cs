using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Interfaces;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base
{
    public abstract class SaveLoadBaseConverter<T> : MonoBehaviour, ISaveLoadConverter
    {
        private SaveLoadSystemController _saveLoadSystemController;

        [Inject]
        public void Construct(SaveLoadSystemController saveLoadSystemController)
        {
            _saveLoadSystemController = saveLoadSystemController;
        }


        public virtual void Awake()
        {
            _saveLoadSystemController.AddSaveLoadConverter(this);

            SetDefaultData();
        }


        object ISaveLoadConverter.GetConverterData(string saveFileName) => GetConverterData(saveFileName);

        public abstract T GetConverterData(string saveFileName);
        public abstract void ParseGeneralSaveData(GeneralSaveData generalSaveData);

        public abstract void SetDefaultData();

        public T ExtractDataType(GeneralSaveData generalSaveData)
        {
            foreach (object savedObject in generalSaveData.SavedObjects)
            {
                if (savedObject.GetType().Equals(typeof(T)))
                {
                    return (T)savedObject;
                }
            }

            return default(T);
        }
    }
}