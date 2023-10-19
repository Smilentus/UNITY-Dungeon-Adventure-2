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
    ///     ��� ��������� ��������� ������ (�����, ������� � �.�.)
    /// </summary>
    public List<BaseInventoryContainer> InventoryContainers => inventoryContainers;


    public List<BaseInventoryContainer> OpenedContainers = new List<BaseInventoryContainer>();


    private BaseMouseItemController mouseItemController;


    public void OnAnyContainerSlotPressed(BaseInventoryContainer _container, BaseInventoryContainerSlot _slot)
    {
        // �� ����� ��� ���� ������������ ����� ���� � ����� ����������, ��� � � �����
        // ��� ������ ���� ���������, � ������ �� ���� ����, � ������ �� ���� � �����
        // � ���� �� � ����� ���� ������� � �.�.
        // � ��� ���� ��� ���������� ����������, ���� � ���� ������������� �������?
        // (��� ������� ������, �������� ���������, ������� ����� �� ���� �����, ����� �� ���� ������ ������ �� ��� � ������ � ������ ����, � ������ ���������� ���, ����� �� ������� � 1 ������ �� ������ �������)

        // � ������ ����� ������� ��� �� ��� � ����������, ������� ����� ��������� �������������� �� ������
        // ���� ������, ������ ��� �������� (�� � ����� ��� �� ��� ���� ���� ��������)
        // ������ ���� ������������ ��� ���� ������

        // ����������� ��������: (������ ���������� � ����������� �� ��������� �� ����� ��...)
        // �� ��:
        // ������� �� ����
        // 1) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 2) ���� ���� ������ � ����� ������ - �������� �� ����� � ����
        // 3) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
        // 4) ���� ���� ����� � ����� ������ - ������� �������� � ����� � �� ���� � �������� �� ��� ���� � �����
        //
        // �� �������:
        // ������� �� ����
        // 1) ���� ���� ����� � ����� ������ - ���������� ���������� � �������� � ��������� ��������
        // 2) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 3) ���� ���� ����� � ����� ����� - ������ �� ����������
        // 4) ���� ���� ������ � ����� ����� - ������ �� ����������
        // 
        // ���������� ����� ��� ������
        // 1) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 2) ���� ���� ������ � ����� ����� - ������ �� ����������
        // 3) ���� ���� ����� � ����� ����� - ������ �� ����������
        // 4) ���� ���� ����� � ����� ������ - �������� ������� ������� � �����, ��� �����������
        //
        // ��������� ����� ��� ������ 
        // 1) ���� ���� ������ � ����� ����� - �������� �� ������ � ����
        // 2) ���� ���� ������ � ����� ������ - ������ �� ����������
        // 3) ���� ���� ����� � ����� ������ - ������ �� ����������
        // 4) ���� ���� ����� � ����� ����� - �������� ������� ������� ������� � ����� �� ��������� �����������
        // 
        // �� ����� ���������, � ����� ������:
        // ���� �� �������� ����������� ������� � � ��� �� ����������
        // �� ������� �������� ��������� � ����� ��������� ���������
        // ���� �� �� ���� ��������� �� ��������, �� ������� ���������� �� ����� (���������� ��������� ���������)
        // ������ ����, ���������� � �������� ��������� ����� ����������� ������ (����� �� �������, ���� ������ ������)
        // ����� ������� ���������, ������ �� ������������, ���� �� ��� �� ��������� ������� � ���������
        // ���� ����� ����� �������� ��������� ������� � ����� � ���������, �� � �������� ����� ��������� (��� ������� ��������� ���������)
        // �� ��������� ������� ������� �������� ���������

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
