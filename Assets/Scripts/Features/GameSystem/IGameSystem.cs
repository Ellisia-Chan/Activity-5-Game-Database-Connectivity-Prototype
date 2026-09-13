
using Features.GameSystem;
using UnityEngine;

namespace Core.ServiceLocator {
	public interface IGameSystem {
		public float RemainingTime { get; }
        public LevelData LevelData { get; }
    }

}