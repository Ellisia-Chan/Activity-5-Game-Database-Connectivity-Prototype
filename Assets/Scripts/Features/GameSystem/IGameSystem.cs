
using Features.GameSystem;
using UnityEngine;

namespace Core.ServiceLocator {
    /// <summary>
    /// Interface service for the GameSystem
    /// </summary>
	public interface IGameSystem {
		public float RemainingTime { get; }
        public LevelData LevelData { get; }
    }

}