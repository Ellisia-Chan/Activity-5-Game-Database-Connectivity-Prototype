
using Core.Enums.SceneSystem;

namespace Core.Events.SceneSystem {
    public struct Evt_OnSceneLoadStarted {
        public SceneID TargetSceneID;

        public Evt_OnSceneLoadStarted(SceneID targetSceneID) {
            TargetSceneID = targetSceneID;
        }
    }

    public struct Evt_OnSceneLoadProgress {
        public float Progress;

        public Evt_OnSceneLoadProgress(float progress) {
            Progress = progress;
        }
    }

    public struct Evt_OnSceneLoadCompleted {
        public SceneID TargetSceneID;

        public Evt_OnSceneLoadCompleted(SceneID targetSceneID) {
            TargetSceneID = targetSceneID;
        }
    }
}
