using IgnitedBox.Tweening;
using IgnitedBox.Tweening.Tweeners;
using IgnitedBox.Tweening.Tweeners.FloatTweeners;
using UnityEngine;
using UnityEngine.UI;

namespace IgnitedBox.UI.Menus.PauseMenu
{
    public abstract class PauseHandler : MonoBehaviour
    {
        private static PauseHandler Instance;

        /// <summary>
        /// Force the handler to pause or unpause.
        /// </summary>
        /// <param name="pause">True to force pause, False to force unpause.</param>
        public static void ForcePause(bool pause)
        {
            if (!Instance || Instance.Paused == pause) return;

            Instance.Paused = pause;
            Instance.UpdatePaused();
        }

        /// <summary>
        /// Enable or Disable the pause controls.
        /// </summary>
        /// <param name="control">Is pausing enabled</param>
        public static void SetControl(bool control)
        {
            Instance.canPause = control;
            if(Instance.pauseButton)
                Instance.pauseButton.gameObject.SetActive(control);

            ForcePause(false);
        }

        //

        public GameObject menuContainer;
        public Button pauseButton;
        public KeyCode pauseKey;

        [Range(0, 2)]
        public float containerFadeTime;

        [Range(0, 2)]
        public float deltaFadeTime;

        protected bool Paused { get; private set; }

        private bool canPause;
        private CanvasGroup containerGroup;
        private float previousTimeScale;
        private TimeScaleTween tween;

        private void Awake()
        {
            if (Instance)
            {
                if (Instance == this) return;
                Destroy(Instance.gameObject);
            }

            Instance = this;

            if (menuContainer && !menuContainer.TryGetComponent(out containerGroup)) 
            {
                containerGroup = menuContainer.gameObject.AddComponent<CanvasGroup>();
                containerGroup.alpha = 0;
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnStart() { }

        private void Update()
        {
            if (canPause && pauseKey != KeyCode.None && Input.GetKeyDown(pauseKey))
                TogglePaused();
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        public void TogglePaused()
        {
            Paused = !Paused;

            UpdatePaused();
        }

        private void UpdatePaused()
        {
            if (Paused) OnPlay();
            else OnPause();

            UpdateContainer();
            UpdateDeltaTime();

            OnPauseUpdated();
        }

        private void UpdateContainer()
        {
            if (menuContainer)
            {
                if (Paused) menuContainer.SetActive(true);
                float target = Paused ? 1 : 0;
                if (containerFadeTime > 0)
                {
                    System.Action callback = Paused ? default(System.Action) : () => menuContainer.SetActive(false);
                    containerGroup.UnscaledTween<CanvasGroup, float, CanvasAlphaTween>(
                        target, containerFadeTime, callback: callback);
                }
                else containerGroup.alpha = target;
            }
        }

        protected virtual void UpdateDeltaTime()
        {
            //If it was just paused, go to 0, else, unpause to last scale
            float targetScale = Paused ? 0 : previousTimeScale;

            //It was just paused so get last scale
            if (Paused) previousTimeScale = GetGameTimeScale();

            if (deltaFadeTime > 0)
            {
                tween = targetScale.UnscaledTween<float, float, TimeScaleTween>
                    (targetScale, deltaFadeTime);
            }
            else Time.timeScale = targetScale;
        }

        protected virtual float GetGameTimeScale()
        {
            //If was re paused before unpause finished
            if (tween != null && tween.State != TweenerBase.TweenState.Finished)
                return tween.Target;

            return Time.timeScale;
        }
        

        protected virtual void OnPlay() { }

        protected virtual void OnPause() { }

        protected virtual void OnPauseUpdated() { }
    }
}
