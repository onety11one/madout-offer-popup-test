using System;
using System.Collections.Generic;
using OfferPopup.Domain;
using UnityEngine;

namespace OfferPopup.Infrastructure
{
    [CreateAssetMenu(menuName = "Offer Popup/Offer Config", fileName = "OfferPopupConfig")]
    public sealed class OfferPopupConfig : ScriptableObject
    {
        [Header("Visuals")]
        [SerializeField] private Sprite offerNameSprite;
        [SerializeField] private Sprite carSprite;
        [SerializeField] private Sprite discountSprite;

        [Header("Price")]
        [SerializeField] private string originalPriceText = "67 999";
        [SerializeField] private string discountedPriceText = "50 000";

        [Header("Timer")]
        [Min(1)]
        [SerializeField] private int timerDurationSeconds = 86400;

        [Header("Rewards")]
        [SerializeField] private List<OfferPopupRewardItem> rewards = new();

        public int TimerDurationSeconds => timerDurationSeconds;

        public OfferPopupData CreateData()
        {
            return new OfferPopupData
            {
                OfferNameSprite = offerNameSprite,
                CarSprite = carSprite,
                DiscountSprite = discountSprite,
                OriginalPriceText = originalPriceText,
                DiscountedPriceText = discountedPriceText,
                TimerText = FormatDuration(TimeSpan.FromSeconds(timerDurationSeconds)),
                Rewards = new List<OfferPopupRewardItem>(rewards)
            };
        }

        private static string FormatDuration(TimeSpan duration)
        {
            return duration.TotalHours >= 1
                ? $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00}"
                : $"{duration.Minutes:00}:{duration.Seconds:00}";
        }
    }
}
