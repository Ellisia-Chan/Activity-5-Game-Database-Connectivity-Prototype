using UnityEngine;

namespace Core.ServiceLocator {
	/// <summary>
	/// Interface for ScoreSystem service
	/// </summary>
	public interface IScoreSystem {
		/// <summary>
		/// Returns the final score
		/// </summary>
		/// <returns></returns>
		public int GetFinalScore();
	}
}