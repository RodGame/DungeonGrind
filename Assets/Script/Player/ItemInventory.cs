using UnityEngine;
using System.Collections;
using System; // For List class;

static class ItemInventory {
	
	public static int             InventorySize = 50;
	public static InventorySlot[] InventoryList = new InventorySlot[InventorySize];
	private static Item			  _EquippedItem = null;
	private static GameManager    _GameManager  = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameManager>();
	private static GameObject     _Player       = GameObject.FindGameObjectWithTag("Player");
	private static GameObject     _GO_InstantiatedItem;
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
		InstatiateItemInHand(_ItemToEquip.ItemPrefab);
	}
	
	private static void InstatiateItemInHand(GameObject _GO_ToInstantiate)
	{
		if(_GO_ToInstantiate != null)
		{
			GameObject _ItemToInstantiate = _GO_ToInstantiate;
			
			_GO_InstantiatedItem = GameObject.Instantiate(_ItemToInstantiate,Vector3.zero, Quaternion.identity) as GameObject ;
			_GO_InstantiatedItem.transform.parent = _Player.transform;
			
			_GO_InstantiatedItem.transform.localPosition = new Vector3(0.5f,0.30f,1.2f);
			_GO_InstantiatedItem.tag = "EquippedItem";
		}
	}
	
	public static void UnequipItem()
	{
		_EquippedItem = null;
		GameObject.Destroy (_GO_InstantiatedItem);
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
