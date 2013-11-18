using UnityEngine;
using System.Collections;
using System; // For List class;

static class ItemInventory {
	
	public static int             InventorySize   = 50; // Need to modify PlayerHUD.DisplayBoxItemList() to modify this
	public static InventorySlot[] InventoryList   = new InventorySlot[InventorySize];
	private static Weapon		  _EquippedWeapon = null;
	private static GameObject     _Player         = GameObject.FindGameObjectWithTag("Player");
	private static GameObject     _GO_InstantiatedWeapon;
	
	// Structure used to represent an inventory slot
	public struct InventorySlot
    {
		public int itemId;
		public Weapon slotWeapon;
        public int  slotCount;
		public bool isSlotFull;
    };	
	
	//Read only EquippedWeapon
	public static Weapon EquippedWeapon
	{
		get {return _EquippedWeapon; }
	}
	
	//Initialize the InventoryList
	public static void IniItemInventory()
	{
		for(int i = 0; i < InventorySize; i++)
		{
			InventoryList[i].isSlotFull = false;
			InventoryList[i].slotWeapon = null;
			InventoryList[i].slotCount = 0;
		}
	}
	
	// Return the first empty slot in the inventory
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
	
	// Equip weapon to use on Left click
	public static void EquipWeapon(Weapon _WeaponToEquip)
	{
		if(_WeaponToEquip != null)
		{
			if(ItemInventory.EquippedWeapon != null)
			{
				ItemInventory.UnequipWeapon();
			}
			_EquippedWeapon = _WeaponToEquip;
			InstatiateWeaponInHand(_WeaponToEquip.ItemPrefab);
		}
	}
	
	public static void EquipWeapon(int _idWeapon, int _level, float _curExp)
	{
		Weapon _newWeapon = CreateWeapon(_idWeapon, _level, _curExp);
		EquipWeapon (_newWeapon);
	}
	
	// Unequip weapon and destroy the object
	public static void UnequipWeapon()
	{
		int _emptySlot;
		_emptySlot = FindFirstEmptySlot();
		
		InventoryList[_emptySlot].slotWeapon = ItemInventory.EquippedWeapon;
		InventoryList[_emptySlot].slotCount = 1;
		InventoryList[_emptySlot].isSlotFull = true;
		
		_EquippedWeapon = null;
		GameObject.Destroy (_GO_InstantiatedWeapon);
	}
	
	// Instantiate a weapon in front of the player as a child of it
	private static void InstatiateWeaponInHand(GameObject _GO_ToInstantiate)
	{
		if(_GO_ToInstantiate != null)
		{
			GameObject _ItemToInstantiate = _GO_ToInstantiate;
			
			_GO_InstantiatedWeapon = GameObject.Instantiate(_ItemToInstantiate,Vector3.zero, Quaternion.identity) as GameObject ;
			_GO_InstantiatedWeapon.transform.parent = Camera.mainCamera.transform;
			
			_GO_InstantiatedWeapon.transform.localPosition = new Vector3(0.5f,-0.40f,1.2f);
			_GO_InstantiatedWeapon.transform.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 1.0f, 0.0f));
			
			_GO_InstantiatedWeapon.tag = "EquippedItem";
		}
	}
	
	// Add an item to the inventory
	public static void AddItem(Weapon _WeaponToAdd)
	{
		int _emptySlot;
		
		_emptySlot = FindFirstEmptySlot();
		ItemInventory.InventoryList[_emptySlot].itemId  = _WeaponToAdd.IdItem;
		ItemInventory.InventoryList[_emptySlot].slotWeapon   = _WeaponToAdd;
		ItemInventory.InventoryList[_emptySlot].slotCount  = 1;
		ItemInventory.InventoryList[_emptySlot].isSlotFull = true;
	}
	
	public static void AddItem(int _idWeapon, int _level, float _curExp)
	{
		Weapon _newWeapon = CreateWeapon(_idWeapon, _level, _curExp);
		AddItem (_newWeapon);
	}
	
	public static Weapon CreateWeapon(int _idWeapon, int _level, float _curExp)
	{
		Weapon _weaponCreated   =  new Weapon();
		_weaponCreated.IdWeapon = _idWeapon;
		_weaponCreated.Level    = _level;
		_weaponCreated.CurExp   = _curExp;
		_weaponCreated.InitializeItem();
		_weaponCreated.UpdateStats();
		
		return _weaponCreated;
	}
	
	//Remove the item at the inputed position
	static public void RemoveItemFromInventory(int _slotNbr)
	{
		ItemInventory.InventoryList[_slotNbr].slotWeapon = null;
		ItemInventory.InventoryList[_slotNbr].slotCount = 0;
		ItemInventory.InventoryList[_slotNbr].isSlotFull = false;
	}
}
