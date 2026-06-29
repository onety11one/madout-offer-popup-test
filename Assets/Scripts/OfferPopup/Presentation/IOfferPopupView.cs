using System;
using System.Collections.Generic;
using OfferPopup.Domain;

namespace OfferPopup.Presentation
{
    public interface IOfferPopupView
    {
        event Action OpenClicked;
        event Action BuyClicked;
        event Action CloseClicked;

        void Render(OfferPopupData data);
        void SetRewards(IReadOnlyList<OfferPopupRewardItem> rewards);
        void SetVisible(bool visible);
        void SetTimer(string value);
    }
}
