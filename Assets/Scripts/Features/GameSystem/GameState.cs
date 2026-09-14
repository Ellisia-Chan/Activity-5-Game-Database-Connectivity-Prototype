namespace Core.Enums.GameSystem {
    /// <summary>
    /// The game state of the game
    /// </summary>
    [System.Flags]
    public enum GameState {
        None = 0,
        Waiting = 1 << 0,
        Playing = 1 << 1,
        Paused = 1 << 2,
        Over = 1 << 3
    }
}
