using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OfferPopup.Domain;
using UnityEngine;
using Zenject;

namespace OfferPopup.Presentation
{
    public sealed class OfferPopupPresenter : IDisposable
    {
        private readonly IOfferPopupView view;
        private readonly IOfferButtonView offerButtonView;
        private readonly IOfferButtonView buyButtonView;
        private readonly ICarVfxView carVfx;
        private readonly IOfferPopupTimer timer;
        private readonly OfferPopupData data;
        private readonly CancellationTokenSource destroyCts = new();
        private bool timerRunning;

        public OfferPopupPresenter(
            IOfferPopupView view,
            [Inject(Id = "OfferButton")] IOfferButtonView offerButtonView,
            [Inject(Id = "BuyButton")] IOfferButtonView buyButtonView,
            ICarVfxView carVfx,
            IOfferPopupTimer timer,
            OfferPopupData data)
        {
            this.view = view;
            this.offerButtonView = offerButtonView;
            this.buyButtonView = buyButtonView;
            this.carVfx = carVfx;
            this.timer = timer;
            this.data = data;

            this.offerButtonView.Clicked += HandleOpenClicked;
            this.view.BuyClicked += HandleBuyClicked;
            this.view.CloseClicked += HandleCloseClicked;
        }

        public void Show()
        {
            view.Render(data);
            view.SetRewards(data.Rewards);
            view.SetVisible(false);
            
            offerButtonView.SetIdleAnimationActive(true);
            
            buyButtonView?.SetIdleAnimationActive(false);
        }

        public void Dispose()
        {
            offerButtonView.Clicked -= HandleOpenClicked;
            view.BuyClicked -= HandleBuyClicked;
            view.CloseClicked -= HandleCloseClicked;
            destroyCts.Cancel();
            destroyCts.Dispose();
        }

        private async UniTaskVoid RunTimerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                view.SetTimer(timer.GetFormattedRemainingTime());
                await timer.TickAsync(token);
            }
        }

        private void HandleOpenClicked()
        {
            view.SetVisible(true);
            
            offerButtonView.SetIdleAnimationActive(false);
            
            buyButtonView?.SetIdleAnimationActive(true);

            if (timerRunning)
            {
                return;
            }

            timerRunning = true;
            RunTimerAsync(destroyCts.Token).Forget();
        }

        private void HandleBuyClicked()
        {
            buyButtonView?.SetBuyAnimationInactive(true);
            carVfx?.PlayVfx();
            Debug.Log("ВОСХИТИТЕЛЬНО!");
        }

        private void HandleCloseClicked()
        {
            view.SetVisible(false);
            
            offerButtonView.SetIdleAnimationActive(true);
            
            buyButtonView?.SetIdleAnimationActive(false);
        }
    }
}