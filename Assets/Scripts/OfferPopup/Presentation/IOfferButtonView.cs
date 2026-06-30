using System;

namespace OfferPopup.Presentation
{
    public interface IOfferButtonView
    {
        event Action Clicked;

        void SetIdleAnimationActive(bool active);
        void SetBuyAnimationInactive(bool active);
    }
}
