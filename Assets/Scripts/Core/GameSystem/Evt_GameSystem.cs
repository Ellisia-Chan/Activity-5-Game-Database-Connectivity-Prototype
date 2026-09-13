using Core.Enums;

namespace Core.Events.GameSystem{
    /// <summary>
    /// The event fired when the game state changes
    /// </summary>
    public readonly struct Evt_OnGameStateChanged { 
        public readonly GameState NewState;

        // Constructor
        public Evt_OnGameStateChanged(GameState newState) {
            NewState = newState;
        }
    }
}