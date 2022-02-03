using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Plugins.SwarmUi {
    [RequireComponent(typeof(CanvasGroup))]
    public class UiFlipAnimator : MonoBehaviour {
        [SerializeField] private float _animationTimeMin = 0.7f;
        [SerializeField] private float _animationTimeMax = 2f;

        [SerializeField] private float _numberOfRotations = 5;
        [SerializeField] private Easings.Functions _easing = Easings.Functions.CubicEaseInOut;

        private float _animationTime;
        private float _currentLerpTime;
        private Quaternion _sourceOrientation;
        private float _sourceAngle;
        private Vector3 _sourceAxis;
        private float _targetAngle;
        private Vector3 _targetAxis;
        private Vector3 _startPos;
        private Vector3 _endPos;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private float _endAlpha;
        private float _startAlpha;
        private bool _isAnimating;
        private Vector3 _initalStart;
        private bool _initPosSet;
        private SwarmUiController _swarmUiController;
        private bool _isVisible;
        private float _animationTimeOverride;
        private Vector3 _initalEnd;

        private void Awake() {
            _swarmUiController = transform.parent.GetComponent<SwarmUiController>();
            Assert.IsNotNull(_swarmUiController);
            _swarmUiController.OnShow += OnShow;
            _swarmUiController.OnHide += OnHide;
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start() {
            _sourceOrientation = transform.localRotation;
            _sourceOrientation.ToAngleAxis(out _sourceAngle, out _sourceAxis);

            _targetAxis = _sourceAxis;

            _canvasGroup.alpha = _startAlpha = 0f;

            _currentLerpTime = float.MaxValue;
            _isAnimating = false;
            _animationTimeOverride = -1;
            InteractionsAllowed(false);
        }

        private void OnShow() {
            if (!_initPosSet) {
                _initPosSet = true;
                Vector3 localPosition = _rectTransform.localPosition;
                _initalStart = localPosition + Vector3.forward * 1000f;
                _initalEnd = localPosition;
            }

            _startPos = _initalStart;
            _endPos = _initalEnd;
            _startAlpha = 0f;
            _endAlpha = 1f;
            _isVisible = true;
            Animate();
        }

        private void OnHide() {
            Vector3 tempStartPos = _startPos;
            _startPos = _endPos;
            _endPos = tempStartPos;
            _startAlpha = 1f;
            _endAlpha = 0f;
            _isVisible = false;
            Animate();
        }

        public void Hide() {
            _animationTimeOverride = .5f;
            _swarmUiController.Hide();
        }

        private void Animate() {
            float startTime = 0f;
            if (_animationTimeOverride > 0) {
                startTime = _animationTimeOverride;
                _animationTimeOverride = -1;
            }
            _animationTime = Random.Range(_animationTimeMin, _animationTimeMax);
            
            _targetAngle = _numberOfRotations * 360f + _sourceAngle;
            _currentLerpTime = 0f;
            StartCoroutine(PostponeStart(startTime));
        }

        private IEnumerator PostponeStart(float time) {
            yield return new WaitForSecondsRealtime(time);
            _isAnimating = true;
        }

        void Update() {
            if (!_isAnimating) return;

            bool finishedVisibility = false;
            _currentLerpTime += Time.deltaTime;
            if (_currentLerpTime > _animationTime) {
                _isAnimating = false;
                finishedVisibility = _isVisible;
                _currentLerpTime = _animationTime;
            }

            float progress = _currentLerpTime / _animationTime;

            progress = Easings.Interpolate(progress, _easing);

            float currentAngle = Mathf.Lerp(_sourceAngle, _targetAngle, progress);
            Vector3 currentAxis = Vector3.Slerp(_sourceAxis, _targetAxis, progress);
            _canvasGroup.alpha = Mathf.Lerp(_startAlpha, _endAlpha, progress);

            _rectTransform.localPosition = Vector3.Lerp(_startPos, _endPos, progress);

            transform.localRotation = Quaternion.AngleAxis(currentAngle, currentAxis);

            InteractionsAllowed(finishedVisibility);
        }

        private void InteractionsAllowed(bool enable) {
            _canvasGroup.interactable = enable;
            _canvasGroup.blocksRaycasts = enable;
        }
    }
}