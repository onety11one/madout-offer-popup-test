using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace OfferPopup.Presentation
{
    public sealed class OfferButtonView : MonoBehaviour, IOfferButtonView
    {
        [SerializeField] private Button button;
        [SerializeField] private OfferButtonAnimator animator;

        public event Action Clicked;

        private readonly CompositeDisposable subscriptions = new();

        private void Awake()
        {
            BindUi();
        }

        private void OnDestroy()
        {
            subscriptions.Dispose();
            animator?.Stop();
        }

        public void SetBuyAnimationInactive(bool active)
        {
            if (active)
            {
                animator?.PlayButtonGlow();
            }
        }

        public void SetIdleAnimationActive(bool active)
        {
            if (active)
            {
                animator?.Play();
            }
            else
            {
                animator?.Stop();
            }
        }

        private void BindUi()
        {
            subscriptions.Clear();

            if (button == null)
            {
                return;
            }

            button.OnClickAsObservable()
                .Subscribe(_ => Clicked?.Invoke())
                .AddTo(subscriptions);
        }
    }
}
