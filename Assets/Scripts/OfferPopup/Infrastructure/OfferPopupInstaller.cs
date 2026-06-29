using OfferPopup.Presentation;
using UnityEngine;
using Zenject;

namespace OfferPopup.Infrastructure
{
    public sealed class OfferPopupInstaller : MonoInstaller
    {
        [SerializeField] private OfferPopupView offerPopupView;
        [SerializeField] private OfferButtonView offerButtonView;
        [SerializeField] private OfferPopupConfig config;

        public override void InstallBindings()
        {
            Container.Bind<OfferPopupView>().FromInstance(offerPopupView).AsSingle();
            Container.Bind<IOfferPopupView>().FromInstance(offerPopupView).AsSingle();
            Container.Bind<OfferButtonView>().FromInstance(offerButtonView).AsSingle();
            Container.Bind<IOfferButtonView>().FromInstance(offerButtonView).AsSingle();

            Container.BindInstance(config.CreateData()).AsSingle();
            Container.Bind<IOfferPopupTimer>().To<OfferPopupTimer>().AsSingle()
                .WithArguments(config.TimerDurationSeconds);
            Container.Bind<OfferPopupPresenter>().AsSingle();
            Container.BindInterfacesTo<OfferPopupEntryPoint>().AsSingle();
        }
    }
}
