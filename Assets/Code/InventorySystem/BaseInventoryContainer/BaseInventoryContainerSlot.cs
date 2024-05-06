namespace Dimasyechka.Code.InventorySystem.BaseInventoryContainer
{
    [System.Serializable]
    public class BaseInventoryContainerSlot
    {
        protected BaseItem.BaseItem slotItem;
        public BaseItem.BaseItem SlotItem { get => slotItem; }


        protected int currentStack;
        public int CurrentStack { get => currentStack; set => currentStack = value; }
    
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
                    return slotItem.BaseItemProfile.MaximumStack;
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

     
        public bool IsSlotEmpty => slotItem == null || slotItem.BaseItemProfile == null;


        public BaseInventoryContainerSlot()
        {
            slotItem = null;
            currentStack = 0;
        }


        public void SetItem(BaseItem.BaseItem _item)
        {
            slotItem = _item;
        }

        public void SetItem(BaseItem.BaseItem _item, int _stack)
        {
            slotItem = _item;
            currentStack = _stack;
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
            if (slotItem != null)
            {
                if (_stack <= DeltaStack)
                {
                    currentStack += _stack;
                    return 0;
                }    
                else
                {
                    int unplacedStack = _stack - DeltaStack;

                    currentStack = MaximumStack;

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
            slotItem = null;
            currentStack = 0;
        }
    }
}
