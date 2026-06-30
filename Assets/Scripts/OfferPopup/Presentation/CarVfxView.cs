using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace OfferPopup.Presentation
{
    public class CarVfxView : MonoBehaviour, ICarVfxView
    {
        [SerializeField] private Image carLightsLeftImage;
        [SerializeField] private Image carLightsRightImage;
        [SerializeField] private AudioSource carAudioSource;
        [SerializeField] private ParticleSystem carParticles;
        [SerializeField] float glowDuration = 0.5f;
        
        public void PlayVfx()
        {
            if (carLightsLeftImage != null && carLightsRightImage != null)
            {
                DOTween.Sequence()
                    .Join(carLightsLeftImage.DOFade(1, 1 * glowDuration).SetEase(Ease.OutSine))
                    .Join(carLightsRightImage.DOFade(1, 1 * glowDuration).SetEase(Ease.OutSine))
                    .Append(carLightsLeftImage.DOFade(0, 1 * glowDuration).SetEase(Ease.InSine))
                    .Join(carLightsRightImage.DOFade(0, 1 * glowDuration).SetEase(Ease.InSine))
                    .SetLink(gameObject)
                    .SetUpdate(true);
            }
            
            PlayEngineSound();
            PlayParticles();
        }

        public void PlayEngineSound()
        {
            if (carAudioSource != null) carAudioSource.Play();
        }
        
        private void PlayParticles()
        {
            if (carParticles != null) carParticles.Play();
        }
    }
}