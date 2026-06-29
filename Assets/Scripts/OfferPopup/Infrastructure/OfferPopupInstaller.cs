using OfferPopup.Presentation;
using UnityEngine;
using Zenject;

namespace OfferPopup.Infrastructure
{
    public sealed class OfferPopupInstaller : MonoInstaller
    {
        private static readonly object Offerbutton = "OfferButton";
        private static readonly object BuyButton = "BuyButton";

        [SerializeField] private OfferPopupView offerPopupView;
        [SerializeField] private OfferButtonView offerButtonView;
        [SerializeField] private OfferButtonView buyButtonView;
        [SerializeField] private OfferPopupConfig config;

        public override void InstallBindings()
        {
            Container.Bind<OfferPopupView>().FromInstance(offerPopupView).AsSingle();
            Container.Bind<IOfferPopupView>().FromInstance(offerPopupView).AsSingle();
            Container.Bind<IOfferButtonView>().WithId(Offerbutton).FromInstance(offerButtonView).AsCached();
            Container.Bind<IOfferButtonView>().WithId(BuyButton).FromInstance(buyButtonView).AsCached();
            Container.BindInstance(config.CreateData()).AsSingle();
            Container.Bind<IOfferPopupTimer>().To<OfferPopupTimer>().AsSingle().WithArguments(config.TimerDurationSeconds);
            Container.Bind<OfferPopupPresenter>().AsSingle();
            Container.BindInterfacesTo<OfferPopupEntryPoint>().AsSingle();
        }
    }
}