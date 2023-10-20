using System.Collections.Generic;

/// <summary>
///     �� ����� ���������, �������� � ������� �������� � ����������� �����, �� ��� ���� ��������� ����� �����-�� �����
/// </summary>
public class DynamicTimelyInventoryContainer : BaseInventoryContainer
{
    public DynamicTimelyInventoryContainer(BaseInventoryContainerProfile _profile) : base(_profile)
    {
        inventoryContainerProfile = _profile;

        m_inventorySlots = new BaseInventoryContainerSlot[_profile.ContainerCapacity];
    }
}