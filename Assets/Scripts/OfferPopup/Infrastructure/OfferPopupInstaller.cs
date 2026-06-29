using OfferPopup.Presentation;
using UnityEngine;
using Zenject;

namespace OfferPopup.Infrastructure
{
    public sealed class OfferPopupInstaller : MonoInstaller
    {
        [SerializeField] private OfferPopupView view;
        [SerializeField] private OfferPopupConfig config;

        public override void InstallBindings()
        {
            Container.Bind<OfferPopupView>().FromInstance(view).AsSingle();
            Container.Bind<IOfferPopupView>().FromInstance(view).AsSingle();

            Container.BindInstance(config.CreateData()).AsSingle();
            Container.Bind<IOfferPopupTimer>().To<OfferPopupTimer>().AsSingle()
                .WithArguments(config.TimerDurationSeconds);
            Container.Bind<OfferPopupPresenter>().AsSingle();
            Container.BindInterfacesTo<OfferPopupEntryPoint>().AsSingle();
        }
    }
}
