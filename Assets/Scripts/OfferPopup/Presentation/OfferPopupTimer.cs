using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace OfferPopup.Presentation
{
    public interface IOfferPopupTimer
    {
        string GetFormattedRemainingTime();
        UniTask TickAsync(CancellationToken token);
    }

    public sealed class OfferPopupTimer : IOfferPopupTimer
    {
        private readonly DateTime expiresAtUtc;

        public OfferPopupTimer(int durationSeconds)
        {
            expiresAtUtc = DateTime.UtcNow.AddSeconds(durationSeconds);
        }

        public string GetFormattedRemainingTime()
        {
            var remaining = expiresAtUtc - DateTime.UtcNow;
            if (remaining < TimeSpan.Zero)
            {
                remaining = TimeSpan.Zero;
            }

            return remaining.TotalHours >= 1
                ? $"{(int)remaining.TotalHours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}"
                : $"{remaining.Minutes:00}:{remaining.Seconds:00}";
        }

        public async UniTask TickAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
        }
    }
}
