using UnityEngine;
using System.Collections;
using System; // For List class;

static class ItemInventory {
	
	public static int             InventorySize = 50;
	public static InventorySlot[] InventoryList = new InventorySlot[InventorySize];
	private static Item			  _EquippedItem = GameObject.FindGameObjectWithTag("PlayerMaster").GetComponent<Inventory>().ItemList[(int)ItemName.RockSword];
	private static GameManager    _GameManager  = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	private static GameObject      _Player      = GameObject.FindGameObjectWithTag("Player");
	
	public struct InventorySlot
    {
        public Item slotItem;
        public int  slotCount;
		public bool isSlotFull;
    };	
	
	public static int FindFirstEmptySlot()
	{
		int _emptySlotNbr = -1;
		for(int i = 0; i < InventoryList.Length; i++)
		{
			if(InventoryList[i].isSlotFull == false)
			{
				_emptySlotNbr = i;
				i = InventoryList.Length + 1;
			}
		}
		return _emptySlotNbr;
	}
	
	public static Item EquippedItem
	{
		get {return _EquippedItem; }
	}
	
	public static void EquipItem(Item _ItemToEquip)
	{
		_EquippedItem = _ItemToEquip;
		InstatiateItem();
	}
	
	private static void InstatiateItem()
	{
		GameObject _InstantiatedItem;
		GameObject _ItemToInstantiate = _GameManager.GetComponent<PrefabManager>().Prefab_Item_RockSword;
		
		_InstantiatedItem = GameObject.Instantiate(_ItemToInstantiate,Vector3.zero, Quaternion.identity) as GameObject ;
		_InstantiatedItem.transform.parent = _Player.transform;
		
		_InstantiatedItem.transform.localPosition = new Vector3(0.5f,0.30f,1.2f);
		_InstantiatedItem.tag = "EquippedItem";
	}
	
	public static void UnequipItem()
	{
		_EquippedItem = null;
	}
	
	public static void AddItem(Item _ItemToAdd)
	{
		int _emptySlot;
		
		_emptySlot = FindFirstEmptySlot();
		ItemInventory.InventoryList[_emptySlot].slotItem   = _ItemToAdd;
		ItemInventory.InventoryList[_emptySlot].slotCount  = 1;
		ItemInventory.InventoryList[_emptySlot].isSlotFull = true;
	}
}
