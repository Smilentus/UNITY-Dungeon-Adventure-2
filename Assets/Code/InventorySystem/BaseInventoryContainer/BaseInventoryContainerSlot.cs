namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    [System.Serializable]
    public class BaseInventoryContainerSlot
    {
        protected BaseItem.BaseItem _slotItem;
        public BaseItem.BaseItem SlotItem { get => _slotItem; }


        protected int _currentStack;
        public int CurrentStack { get => _currentStack; set => _currentStack = value; }
    
        public int MaximumStack
        {
            get
            {
                if (IsSlotEmpty)
                {
                    return 0;
                }
                else
                {
                    return _slotItem.BaseItemProfile.MaximumStack;
                }
            }
        }

        public int DeltaStack
        {
            get
            {
                if (IsSlotEmpty)
                {
                    return 0;
                }
                else
                {
                    return MaximumStack - CurrentStack;
                }
            }
        }

     
        public bool IsSlotEmpty => _slotItem == null || _slotItem.BaseItemProfile == null;


        public BaseInventoryContainerSlot()
        {
            _slotItem = null;
            _currentStack = 0;
        }


        public void SetItem(BaseItem.BaseItem _item)
        {
            _slotItem = _item;
        }

        public void SetItem(BaseItem.BaseItem _item, int _stack)
        {
            _slotItem = _item;
            _currentStack = _stack;
        }

        /// <summary>
        ///     Добавляем определённое количество предметов в стак слота
        /// </summary>
        /// <param name="_stack">
        ///     Количество предметов, которое надо поместить уже к имеющемуся предмету
        /// </param>
        /// <returns>
        ///     Количество непоместившихся в слот предметов 
        /// </returns>
        public int AddItemStack(int _stack)
        {
            if (_slotItem != null)
            {
                if (_stack <= DeltaStack)
                {
                    _currentStack += _stack;
                    return 0;
                }    
                else
                {
                    int unplacedStack = _stack - DeltaStack;

                    _currentStack = MaximumStack;

                    return unplacedStack;
                }
            }
            else
            {
                return _stack;
            }
        }

        public void ClearSlot()
        {
            _slotItem = null;
            _currentStack = 0;
        }
    }
}
