using UnityEngine;

namespace Core.Enums.SceneSystem {
	/// <summary>
	/// All the scenes in the game.
	/// Enum must match the name of the scene in the build settings
	/// </summary>
	public enum SceneID {
		None,
		MainMenuScene,
        LoadingScene,
		GameScene,
	}
}