using System;
using System.Collections.Generic;
using UnityEngine;

namespace OfferPopup.Domain
{
    [Serializable]
    public sealed class OfferPopupData
    {
        public Sprite OfferNameSprite;
        public Sprite CarSprite;
        public Sprite DiscountSprite;
        public string OriginalPriceText;
        public string DiscountedPriceText;
        public string TimerText;
        public List<OfferPopupRewardItem> Rewards = new();
    }
}
