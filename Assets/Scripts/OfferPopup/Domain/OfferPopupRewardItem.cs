using System;
using UnityEngine;

namespace OfferPopup.Domain
{
    [Serializable]
    public sealed class OfferPopupRewardItem
    {
        public string Id;
        public string Title;
        public int Amount;
        public bool IsMainReward;
        public Sprite Icon;
    }
}
