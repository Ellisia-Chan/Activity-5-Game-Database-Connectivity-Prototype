using UnityEngine;
using Features.Item;

namespace Features.Item {
	/// <summary>
	/// Interface for items that can be collected.
	/// </summary>
	public interface ICollectible {
		public ItemDataSO ItemData { get; }

		public void ReturnToSpawner();
	}
}