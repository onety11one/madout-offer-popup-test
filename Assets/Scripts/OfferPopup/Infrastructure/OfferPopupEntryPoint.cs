using System;
using Zenject;

namespace OfferPopup.Infrastructure
{
    public sealed class OfferPopupEntryPoint : IInitializable, IDisposable
    {
        private readonly OfferPopup.Presentation.OfferPopupPresenter presenter;

        public OfferPopupEntryPoint(OfferPopup.Presentation.OfferPopupPresenter presenter)
        {
            this.presenter = presenter;
        }

        public void Initialize()
        {
            presenter.Show();
        }

        public void Dispose()
        {
            presenter.Dispose();
        }
    }
}
