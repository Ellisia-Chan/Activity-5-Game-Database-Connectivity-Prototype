

namespace Core.Events.ScoreSystem {
    public readonly struct Evt_OnScoreUpdate {
        public readonly int Score;

        public Evt_OnScoreUpdate(int score) {
            Score = score;
        }
    }
}