using System;
using UnityEngine;

namespace Plugins.SwarmUi {
    public class SwarmUiController : MonoBehaviour {
        public event Action OnShow;
        public event Action OnHide;

        private bool _isVisible;

        public void Show() {
            _isVisible = true;
            OnShow?.Invoke();
        }

        public void Hide() {
            _isVisible = false;
            OnHide?.Invoke();
        }

        public void ToggleVisibility() {
            if (_isVisible) {
                Hide();
            } else {
                Show();
            }
        }
    }
}