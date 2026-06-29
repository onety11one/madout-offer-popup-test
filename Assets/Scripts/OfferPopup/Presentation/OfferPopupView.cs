using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using OfferPopup.Domain;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace OfferPopup.Presentation
{
    public sealed class OfferPopupView : MonoBehaviour, IOfferPopupView
    {
        [Header("Root")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Images")]
        [SerializeField] private Image offerNameImage;
        [SerializeField] private Image carImage;
        [SerializeField] private Image discountImage;

        [Header("Text")]
        [SerializeField] private TMP_Text originalPriceText;
        [SerializeField] private TMP_Text discountedPriceText;
        [SerializeField] private TMP_Text timerText;

        [Header("Buttons")]
        [SerializeField] private Button buyButton;
        [SerializeField] private Button closeButton;

        [Header("Rewards")]
        [SerializeField] private Transform rewardsRoot;

        [Header("Element Animators")]
        [SerializeField] private PopupElementAnimator carAnimator;
        [SerializeField] private PopupElementAnimator headerAnimator;
        [SerializeField] private PopupElementAnimator timerAnimator;
        [SerializeField] private PopupElementAnimator rewardsAnimator;
        [SerializeField] private PopupElementAnimator purchaseButtonAnimator;
        [SerializeField] private PopupElementAnimator closeButtonAnimator;
        [SerializeField] private PopupElementAnimator carTextAnimator;
        
        [SerializeField] private float delayBetweenAnimations;
        
        private Sequence currentAnimation;

        public event Action BuyClicked;
        public event Action CloseClicked;

        private readonly CompositeDisposable subscriptions = new();
        private bool isVisible = false;
        private bool isInitialized = false;

        private void Awake()
        {
            BindUi();
        }

        private void Start()
        {
            InitializeElements();
        }

        private void OnDestroy()
        {
            subscriptions.Dispose();
            currentAnimation?.Kill();
        }

        private void InitializeElements()
        {
            if (isInitialized) return;
            isInitialized = true;
            
            ResetAllElementsToAnimationStart();
            SetAllElementsActive(false);
            
            gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void Render(OfferPopupData data)
        {
            if (offerNameImage != null && data.OfferNameSprite != null) 
                offerNameImage.sprite = data.OfferNameSprite;
            if (carImage != null && data.CarSprite != null) 
                carImage.sprite = data.CarSprite;
            if (discountImage != null && data.DiscountSprite != null) 
                discountImage.sprite = data.DiscountSprite;
            if (originalPriceText != null) 
                originalPriceText.text = data.OriginalPriceText;
            if (discountedPriceText != null) 
                discountedPriceText.text = data.DiscountedPriceText;
            if (timerText != null) 
                timerText.text = data.TimerText;
        }

        public void SetRewards(IReadOnlyList<OfferPopupRewardItem> rewards)
        {
            
        }

        public void SetVisible(bool visible)
        {
            if (isVisible == visible) return;
            isVisible = visible;
            
            currentAnimation?.Kill();

            if (visible)
            {
                ShowWithAnimation();
            }
            else
            {
                HideWithAnimation();
            }
        }
        
        public void SetBuyButtonAnimationActive(bool active)
        {
            var buyButtonView = buyButton?.GetComponent<IOfferButtonView>();
            if (buyButtonView != null)
            {
                buyButtonView.SetIdleAnimationActive(active);
            }
        }

        private void ShowWithAnimation()
        {
            ResetAllElementsToAnimationStart();
            
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            
            SetAllElementsActive(true);
            
            currentAnimation = DOTween.Sequence()
                .SetLink(gameObject);
            
            if (carAnimator != null)
            {
                currentAnimation.AppendCallback(() => carAnimator.PlayShowAnimation());
                currentAnimation.AppendInterval(0.2f);
            }
            
            if (headerAnimator != null)
            {
                currentAnimation.AppendInterval(delayBetweenAnimations);
                currentAnimation.AppendCallback(() => headerAnimator.PlayShowAnimation());
            }
            
            if (timerAnimator != null)
            {
                currentAnimation.AppendInterval(delayBetweenAnimations);
                currentAnimation.AppendCallback(() => timerAnimator.PlayShowAnimation());
            }
            
            if (rewardsAnimator != null)
            {
                currentAnimation.AppendInterval(delayBetweenAnimations);
                currentAnimation.AppendCallback(() => rewardsAnimator.PlayShowAnimation());
            }
            
            if (carTextAnimator != null)
            {
                currentAnimation.AppendInterval(delayBetweenAnimations * 0.5f);
                currentAnimation.AppendCallback(() => carTextAnimator.PlayShowAnimation());
            }
            
            if (purchaseButtonAnimator != null)
            {
                currentAnimation.AppendInterval(delayBetweenAnimations);
                currentAnimation.AppendCallback(() => purchaseButtonAnimator.PlayShowAnimation());
            }
            
            if (closeButtonAnimator != null)
            {
                currentAnimation.AppendInterval(delayBetweenAnimations * 0.5f);
                currentAnimation.AppendCallback(() => closeButtonAnimator.PlayShowAnimation());
            }
        }

        private void HideWithAnimation()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            currentAnimation = DOTween.Sequence()
                .SetLink(gameObject);
            
            
            if (closeButtonAnimator != null)
            {
                currentAnimation.AppendCallback(() => closeButtonAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }
            
            if (purchaseButtonAnimator != null)
            {
                currentAnimation.AppendCallback(() => purchaseButtonAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }
            
            if (carTextAnimator != null)
            {
                currentAnimation.AppendCallback(() => carTextAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }
            
            if (rewardsAnimator != null)
            {
                currentAnimation.AppendCallback(() => rewardsAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }

            if (timerAnimator != null)
            {
                currentAnimation.AppendCallback(() => timerAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }

            if (headerAnimator != null)
            {
                currentAnimation.AppendCallback(() => headerAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }

            if (carAnimator != null)
            {
                currentAnimation.AppendCallback(() => carAnimator.PlayHideAnimation());
                currentAnimation.AppendInterval(delayBetweenAnimations);
            }
            
            currentAnimation.Append(canvasGroup.DOFade(0f, 0.2f));
            currentAnimation.OnComplete(() => 
            {
                gameObject.SetActive(false);
                SetAllElementsActive(false);
            });
        }

        private void ResetAllElementsToAnimationStart()
        {
            if (carAnimator != null) carAnimator.ResetToAnimationStartPosition();
            if (headerAnimator != null) headerAnimator.ResetToAnimationStartPosition();
            if (timerAnimator != null) timerAnimator.ResetToAnimationStartPosition();
            if (rewardsAnimator != null) rewardsAnimator.ResetToAnimationStartPosition();
            if (purchaseButtonAnimator != null) purchaseButtonAnimator.ResetToAnimationStartPosition();
            if (closeButtonAnimator != null) closeButtonAnimator.ResetToAnimationStartPosition();
            if (carTextAnimator != null) carTextAnimator.ResetToAnimationStartPosition();
        }
        
        private void SetAllElementsActive(bool active)
        {
            if (carAnimator != null) carAnimator.gameObject.SetActive(active);
            if (headerAnimator != null) headerAnimator.gameObject.SetActive(active);
            if (timerAnimator != null) timerAnimator.gameObject.SetActive(active);
            if (rewardsAnimator != null) rewardsAnimator.gameObject.SetActive(active);
            if (purchaseButtonAnimator != null) purchaseButtonAnimator.gameObject.SetActive(active);
            if (closeButtonAnimator != null) closeButtonAnimator.gameObject.SetActive(active);
            if (carTextAnimator != null) carTextAnimator.gameObject.SetActive(active);
        }
        

        public void SetTimer(string value)
        {
            if (timerText != null)
            {
                timerText.text = value;
            }
        }

        private void BindUi()
        {
            subscriptions.Clear();

            if (buyButton != null)
            {
                buyButton.OnClickAsObservable()
                    .Subscribe(_ => BuyClicked?.Invoke())
                    .AddTo(subscriptions);
            }

            if (closeButton != null)
            {
                closeButton.OnClickAsObservable()
                    .Subscribe(_ => CloseClicked?.Invoke())
                    .AddTo(subscriptions);
            }
        }
    }
}