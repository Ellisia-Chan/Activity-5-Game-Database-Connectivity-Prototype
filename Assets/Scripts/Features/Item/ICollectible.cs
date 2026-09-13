using UnityEngine;
using Features.Item;

namespace Features.Item {
	public interface ICollectible {
		public ItemDataSO ItemData { get; }

		public void ReturnToSpawner();
	}
}