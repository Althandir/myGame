using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageSettings 
{
	[SerializeField] StorageLevelEnum _level;
	[SerializeField] StorageLevelEnum _maxLevel;
	[SerializeField] int _currentMaxAmount;
	[SerializeField] int _actualAmount;
	[SerializeField] List<StorageSlot> _StorageSlots;

	static int increaseAmount = 2000;

	public int CurrentMaxAmount { get => _currentMaxAmount;}
	public int ActualAmount { get => _actualAmount; }
	public int Level { get => (int)_level; }
	
	private StorageLevelEnum MaxLevel { get => _maxLevel; }

	public StorageSettings(List<StorageSlot> storageSlots)
	{
		// Set max Values
		_currentMaxAmount = increaseAmount;
		_maxLevel = StorageLevelEnum.Level3;

		// Set actual Values
		_level = StorageLevelEnum.Level0;
		_actualAmount = 0;

		// Set Reference to StorageSlots
		this._StorageSlots = storageSlots;
	}

	public void UpdateActualAmount()
	{
		int value = 0;
		foreach (StorageSlot slot in _StorageSlots)
		{
			value += slot.Amount;
		}
		_actualAmount = value;
	}

	/// <summary>
	/// Function to increase Level and maxAmount of Storage
	/// </summary>
	public void UpgradeStorageLevel()
	{
		// MaxLevel is not reached
		if (_level != _maxLevel)
		{
			// increase Level by 1
			_level += 1;
			// increase size of Storage
			_currentMaxAmount += increaseAmount;
		}
	}

	enum StorageLevelEnum
	{
		Level0, Level1, Level2, Level3
	}


	
}
