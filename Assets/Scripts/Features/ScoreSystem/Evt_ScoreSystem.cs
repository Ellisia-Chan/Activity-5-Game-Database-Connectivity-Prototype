

namespace Core.Events.ScoreSystem {
    /// <summary>
    /// Event raised when the score is updated
    /// </summary>
    public readonly struct Evt_OnScoreUpdate {
        public readonly int Score;

        public Evt_OnScoreUpdate(int score) {
            Score = score;
        }
    }
}