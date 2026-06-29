using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace OfferPopup.Presentation
{
    public sealed class OfferButtonAnimator : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] private RectTransform buttonRoot;
        [SerializeField] private RectTransform shine;
        [SerializeField] private Graphic shineGraphic;
        [SerializeField] private Graphic glow;

        [Header("Scale")]
        [SerializeField] private float idleScale = 1f;
        [SerializeField] private float punchScale = 1.08f;
        [SerializeField] private float breatheDuration = 0.9f;

        [Header("Rotation")]
        [SerializeField] private float tiltAngle = 2.5f;
        [SerializeField] private float tiltDuration = 0.55f;

        [Header("Glow")]
        [SerializeField] private float glowMinAlpha = 0.35f;
        [SerializeField] private float glowMaxAlpha = 1f;
        [SerializeField] private float glowDuration = 0.7f;

        [Header("Shine")]
        [SerializeField] private float shineTravel = 560f;
        [SerializeField] private float shineDuration = 0.75f;
        [SerializeField] private float shineDelay = 1.15f;

        private Tweener scaleTween;
        private Tweener rotationTween;
        private Tweener glowTween;
        private Sequence shineSequence;
        private Vector3 initialScale;
        private Vector3 initialRotation;
        private Vector2 initialShinePosition;
        private Color initialShineColor;
        private Color initialGlowColor;

        private void Awake()
        {
            buttonRoot ??= transform as RectTransform;
            initialScale = buttonRoot.localScale;
            initialRotation = buttonRoot.localEulerAngles;

            if (shine != null)
            {
                initialShinePosition = shine.anchoredPosition;
            }

            if (shineGraphic != null)
            {
                initialShineColor = shineGraphic.color;
            }

            if (glow != null)
            {
                initialGlowColor = glow.color;
            }
        }

        private void OnDestroy()
        {
            Stop();
        }

        public void Play()
        {
            Stop();

            gameObject.SetActive(true);
            buttonRoot.localScale = initialScale * idleScale;
            buttonRoot.localEulerAngles = initialRotation;

            scaleTween = buttonRoot.DOScale(initialScale * punchScale, breatheDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject)
                .SetUpdate(true);

            rotationTween = buttonRoot.DOLocalRotate(initialRotation + new Vector3(0f, 0f, tiltAngle), tiltDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject)
                .SetUpdate(true);

            if (glow != null)
            {
                var color = initialGlowColor;
                color.a = glowMinAlpha;
                glow.color = color;

                glowTween = DOTween.To(
                        () => glow.color.a,
                        alpha =>
                        {
                            var glowColor = glow.color;
                            glowColor.a = alpha;
                            glow.color = glowColor;
                        },
                        glowMaxAlpha,
                        glowDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(gameObject)
                    .SetUpdate(true);
            }

            if (shine != null)
            {
                shineSequence = CreateShineSequence()
                    .SetLink(gameObject)
                    .SetUpdate(true);
            }
        }

        public void Stop()
        {
            scaleTween?.Kill();
            rotationTween?.Kill();
            glowTween?.Kill();
            shineSequence?.Kill();
            scaleTween = null;
            rotationTween = null;
            glowTween = null;
            shineSequence = null;

            if (buttonRoot == null)
            {
                return;
            }

            buttonRoot.DOKill();
            buttonRoot.localScale = initialScale == Vector3.zero ? Vector3.one : initialScale;
            buttonRoot.localEulerAngles = initialRotation;

            if (shine != null)
            {
                shine.DOKill();
                shine.anchoredPosition = initialShinePosition;
            }

            if (shineGraphic != null)
            {
                shineGraphic.DOKill();
                shineGraphic.color = initialShineColor;
            }

            if (glow != null)
            {
                glow.DOKill();
                glow.color = initialGlowColor;
            }
        }

        private Sequence CreateShineSequence()
        {
            var from = initialShinePosition + Vector2.left * shineTravel * 0.5f;
            var to = initialShinePosition + Vector2.right * shineTravel * 0.5f;

            shine.anchoredPosition = from;
            SetShineAlpha(0f);

            return DOTween.Sequence()
                .AppendInterval(shineDelay)
                .AppendCallback(() =>
                {
                    shine.anchoredPosition = from;
                    SetShineAlpha(0f);
                })
                .Append(DOTween.To(
                        () => shine.anchoredPosition,
                        value => shine.anchoredPosition = value,
                        to,
                        shineDuration)
                    .SetEase(Ease.InOutQuad))
                .Join(DOTween.Sequence()
                    .Append(DOTween.To(() => GetShineAlpha(), SetShineAlpha, 1f, shineDuration * 0.5f)
                        .SetEase(Ease.InOutSine))
                    .Append(DOTween.To(() => GetShineAlpha(), SetShineAlpha, 0f, shineDuration * 0.5f)
                        .SetEase(Ease.InOutSine)))
                .AppendCallback(() => shine.anchoredPosition = from)
                .SetLoops(-1);
        }

        private float GetShineAlpha()
        {
            return shineGraphic != null ? shineGraphic.color.a : 0f;
        }

        private void SetShineAlpha(float alpha)
        {
            if (shineGraphic == null)
            {
                return;
            }

            var color = shineGraphic.color;
            color.a = alpha;
            shineGraphic.color = color;
        }
    }
}
