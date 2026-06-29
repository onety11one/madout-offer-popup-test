using System;
using System.Collections.Generic;
using System.Linq;
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

        public event Action BuyClicked;
        public event Action CloseClicked;

        private readonly CompositeDisposable subscriptions = new();

        private void Awake()
        {
            BindUi();
        }

        private void OnDestroy()
        {
            subscriptions.Dispose();
        }

        public void Render(OfferPopupData data)
        {
            if (offerNameImage != null && data.OfferNameSprite != null) offerNameImage.sprite = data.OfferNameSprite;
            if (carImage != null && data.CarSprite != null) carImage.sprite = data.CarSprite;
            if (discountImage != null && data.DiscountSprite != null) discountImage.sprite = data.DiscountSprite;
            if (originalPriceText != null) originalPriceText.text = data.OriginalPriceText;
            if (discountedPriceText != null) discountedPriceText.text = data.DiscountedPriceText;
            if (timerText != null) timerText.text = data.TimerText;
        }

        public void SetRewards(IReadOnlyList<OfferPopupRewardItem> rewards)
        {
            if (rewardsRoot == null)
            {
                return;
            }

            var children = rewardsRoot.Cast<Transform>().ToArray();
            for (var i = 0; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(i < rewards.Count);
            }
        }

        public void SetVisible(bool visible)
        {
            if (canvasGroup == null)
            {
                gameObject.SetActive(visible);
                return;
            }

            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
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
