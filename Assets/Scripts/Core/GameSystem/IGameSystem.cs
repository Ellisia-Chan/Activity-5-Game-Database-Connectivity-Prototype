
using Core.GameSystem;
using UnityEngine;

namespace Core.ServiceLocator {
	public interface IGameSystem {
		public float ElapsedTime { get; }
        public LevelData LevelData { get; }
    }

}