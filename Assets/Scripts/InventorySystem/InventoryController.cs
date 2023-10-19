using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private static InventoryController instance;
    public static InventoryController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryController>(); 
            }

            return instance;
        }
    }



    public event Action onInventoryContainersUpdated;
    public event Action<BaseInventoryContainer> onInventoryContainerOpened;
    public event Action<BaseInventoryContainer> onInventoryContainerClosed;


    private List<BaseInventoryContainer> inventoryContainers = new List<BaseInventoryContainer>();
    /// <summary>
    ///     Все доступные инвентари игрока (сумки, рюкзаки и т.п.)
    /// </summary>
    public List<BaseInventoryContainer> InventoryContainers => inventoryContainers;


    public List<BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer>();


    private BaseMouseItemController mouseItemController;


    public void OnAnyContainerSlotPressed(BaseInventoryContainer _container, BaseInventoryContainerSlot _slot)
    {
        // По факту тут надо обрабатывать любой слот в любом контейнере, что я и делаю
        // Тут должны быть обработки, а пустой ли этот слот, а пустой ли слот в мышке
        // А могу ли я взять этот предмет и т.п.
        // А что если все контейнеры заполнятся, пока я буду перетаскивать предмет?
        // (тут лайфхак сделаю, появится контейнер, который будет по типу земли, якобы ты вещь уронил просто из рук и убрать её некуда было, и таймер запустится ещё, чтобы не абузили в 1 минуту на каждый предмет)

        // В идеале здесь вынести ещё всё это в контроллер, который будет проверять взаимодействие со слотом
        // Типо нажали, зажали или удержали (но я думаю это на сам слот надо выносить)
        // Каждый слот обрабатывает сам себя короче

        // Фактические проверки: (должны отличаться в зависимости от устройств по факту же...)
        // НА ПК:
        // НАЖАТИЕ НА СЛОТ
        // 1) Если слот ПУСТОЙ и мышка ПУСТАЯ - ничего не происходит
        // 2) Если слот ПУСТОЙ и мышка ЗАНЯТА - помещаем из мышки в слот
        // 3) Если слот ЗАНЯТ и мышка ПУСТАЯ - показываем информацию о предмете и возможные действия
        // 4) Если слот ЗАНЯТ и мышка ЗАНЯТА - свапаем предметы с мышки и на слот и забираем то что было в мышку
        //
        // НА МОБИЛКЕ:
        // НАЖАТИЕ НА СЛОТ
        // 1) Если слот ЗАНЯТ и палец ПУСТОЙ - показываем информацию о предмете и возможные действия
        // 2) Если слот ПУСТОЙ и палец ПУСТОЙ - ничего не происходит
        // 3) Если слот ЗАНЯТ и палец ЗАНЯТ - ничего не происходит
        // 4) Если слот ПУСТОЙ и палец ЗАНЯТ - ничего не происходит
        // 
        // УДЕРЖИВАЕМ ПАЛЕЦ НАД СЛОТОМ
        // 1) Если слот ПУСТОЙ и палец ПУСТОЙ - ничего не происходит
        // 2) Если слот ПУСТОЙ и палец ЗАНЯТ - ничего не происходит
        // 3) Если слот ЗАНЯТ и палец ЗАНЯТ - ничего не происходит
        // 4) Если слот ЗАНЯТ и палец ПУСТОЙ - пытаемся забрать предмет в палец, для перемещения
        //
        // ОТПУСКАЕМ ПАЛЕЦ НАД СЛОТОМ 
        // 1) Если слот ПУСТОЙ и палец ЗАНЯТ - помещаем из пальца в слот
        // 2) Если слот ПУСТОЙ и палец ПУСТОЙ - ничего не происходит
        // 3) Если слот ЗАНЯТ и палец ПУСТОЙ - ничего не происходит
        // 4) Если слот ЗАНЯТ и палец ЗАНЯТ - пытаемся вернуть предмет обратно в любой из доступных контейнеров
        // 
        // На любой платформе, в любом случае:
        // Если мы пытались переместить предмет и у нас не получилось
        // То предмет пытается ВЕРНУТЬСЯ в ЛЮБОЙ ДОСТУПНЫЙ контейнер
        // Если же НИ ОДИН КОНТЕЙНЕР НЕ ДОСТУПЕН, то предмет ПОМЕЩАЕТСЯ на ЗЕМЛЮ (динамичный воздушный контейнер)
        // Каждая вещь, помещённая в подобный контейнер имеет собственный таймер (чтобы не обузили, надо лимиты делать)
        // Когда предмет забираешь, таймер не сбрасывается, пока ты его не поместишь обратно в контейнер
        // Если очень часто пытаться поместить предмет с земли в контейнер, он в конечном счёте пропадает (его съедает природное окружение)
        // По истечению таймера предмет навсегда пропадает

    }



    public void AddInventoryContainer(BaseInventoryContainerProfile _baseInventoryContainerProfile)
    {
        inventoryContainers.Add(new BaseInventoryContainer(_baseInventoryContainerProfile));

        onInventoryContainersUpdated?.Invoke();
    }


    public void ToggleContainer(BaseInventoryContainer _container)
    {
        if (OpenedContainers.Contains(_container))
        {
            CloseContainer(_container);
        }
        else
        {
            OpenContainer(_container);
        }
    }


    public void OpenContainer(BaseInventoryContainer _container)
    {
        if (!OpenedContainers.Contains(_container))
        {
            OpenedContainers.Add(_container);

            onInventoryContainerOpened?.Invoke(_container);
        }
    }

    public void CloseContainer(BaseInventoryContainer _container)
    {
        if (OpenedContainers.Contains(_container))
        {
            OpenedContainers.Remove(_container);

            onInventoryContainerClosed?.Invoke(_container);
        }
    }


    [Header("Debug")]
    public BaseInventoryContainerProfile debugProfile;

    [ContextMenu("AddContainer")]
    public void DebugAddContainer()
    {
        AddInventoryContainer(debugProfile);
    }
}
