using System.Collections.Generic;

/// <summary>
///     То самое хранилище, предметы в котором хранятся в бесконечном стаке, но при этом пропадают через какое-то время
/// </summary>
public class DynamicTimelyInventoryContainer : BaseInventoryContainer
{
    public DynamicTimelyInventoryContainer(BaseInventoryContainerProfile _profile) : base(_profile)
    {
        inventoryContainerProfile = _profile;

        m_inventorySlots = new BaseInventoryContainerSlot[_profile.ContainerCapacity];
    }
}