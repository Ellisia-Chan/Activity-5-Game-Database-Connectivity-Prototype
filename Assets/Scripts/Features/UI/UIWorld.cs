using UnityEngine;
using Core.EventSystem;
using Core.Events.GameSystem;
using Core.Enums.GameSystem;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Features.UI {
    /// <summary>
    /// Handles updating the world UI 
    /// </summary>
	public class UIWorld : MonoBehaviour {
		[Header("Panel")]
        [SerializeField] private GameObject panel;

		[Header("State")]
        [SerializeField] private GameState showState;
		[SerializeField] private GameState hideState;



        // =====================================================================
        //
        //                          Unity Lifecycle
        //
        // =====================================================================
        private void OnEnable() {
            EventBus.Subscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }

        private void OnDisable() {
            EventBus.Unsubscribe<Evt_OnGameStateChanged>(OnGameStateChanged);
        }


        // =====================================================================
        //
        //                          Event Handlers
        //
        // =====================================================================
        private void OnGameStateChanged(Evt_OnGameStateChanged evt) {
            if ((evt.NewState & showState) != 0) {
                HidePanel(true);
            } else if ((evt.NewState & hideState) != 0) {
                HidePanel(false);
            }
        }

        // =====================================================================
        //
        //                          Private Methods
        //
        // =====================================================================
        /// <summary>
        /// Hides the panel
        /// </summary>
        /// <param name="state"></param>
        private void HidePanel(bool state) {
            panel.SetActive(state);
        }
    }
}