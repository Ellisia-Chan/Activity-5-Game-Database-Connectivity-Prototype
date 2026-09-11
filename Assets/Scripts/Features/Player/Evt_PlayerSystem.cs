using UnityEngine;

namespace Core.Events.PlayerSystem {
    public readonly struct Evt_OnItemCollected {
        public readonly int ItemScore;

        public Evt_OnItemCollected(int itemScore) {
            ItemScore = itemScore;
        }
    }
}
