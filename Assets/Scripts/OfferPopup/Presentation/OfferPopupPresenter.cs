using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using OfferPopup.Domain;

namespace OfferPopup.Presentation
{
    public sealed class OfferPopupPresenter : IDisposable
    {
        private readonly IOfferPopupView view;
        private readonly IOfferPopupTimer timer;
        private readonly OfferPopupData data;
        private readonly CancellationTokenSource destroyCts = new();
        private bool timerRunning;

        public OfferPopupPresenter(IOfferPopupView view, IOfferPopupTimer timer, OfferPopupData data)
        {
            this.view = view;
            this.timer = timer;
            this.data = data;

            this.view.OpenClicked += HandleOpenClicked;
            this.view.BuyClicked += HandleBuyClicked;
            this.view.CloseClicked += HandleCloseClicked;
        }

        public void Show()
        {
            view.Render(data);
            view.SetRewards(data.Rewards);
            view.SetVisible(false);
        }

        public void Dispose()
        {
            view.OpenClicked -= HandleOpenClicked;
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
            if (timerRunning)
            {
                return;
            }

            timerRunning = true;
            RunTimerAsync(destroyCts.Token).Forget();
        }

        private void HandleBuyClicked()
        {
        }

        private void HandleCloseClicked()
        {
            view.SetVisible(false);
        }
    }
}
