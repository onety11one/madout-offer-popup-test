using System;
using DG.Tweening;
using UnityEngine;

namespace OfferPopup.Presentation
{
    [Serializable]
    public class PopupElementAnimation
    {
        public enum AnimationType
        {
            SlideFromLeft,
            SlideFromRight,
            SlideFromTop,
            SlideFromBottom,
            ScalePunch,
            FadeIn,
            RotateIn
        }

        public AnimationType Type;
        public float Duration = 0.5f;
        public float Delay = 0f;
        public Ease Ease = Ease.OutBack;
        public float Strength = 1f;
        public bool ResetOnStart = true;
    }

    public sealed class PopupElementAnimator : MonoBehaviour
    {
        [Header("Element References")]
        [SerializeField] private RectTransform targetRect;
        [SerializeField] private CanvasGroup targetCanvasGroup;
        
        [Header("Animation Settings")]
        [SerializeField] private PopupElementAnimation showAnimation;
        [SerializeField] private PopupElementAnimation hideAnimation;
        
        private Vector3 originalPosition;
        private Vector3 originalScale;
        private float originalAlpha;
        private Tween currentAnimation;

        private void Awake()
        {
            if (targetRect == null) targetRect = GetComponent<RectTransform>();
            if (targetCanvasGroup == null) targetCanvasGroup = GetComponent<CanvasGroup>();
            
            originalPosition = targetRect.anchoredPosition;
            originalScale = targetRect.localScale;
            originalAlpha = targetCanvasGroup != null ? targetCanvasGroup.alpha : 1f;
        }

        private void OnDestroy()
        {
            currentAnimation?.Kill();
        }

        public void PlayShowAnimation()
        {
            PlayAnimation(showAnimation, true);
        }

        public void PlayHideAnimation()
        {
            PlayAnimation(hideAnimation, false);
        }

        private void PlayAnimation(PopupElementAnimation anim, bool isShow)
        {
            currentAnimation?.Kill();

            if (anim == null || targetRect == null)
            {
                SetDefaultState(isShow);
                return;
            }

            if (anim.ResetOnStart)
            {
                ResetToInitialState();
            }

            var sequence = DOTween.Sequence();
            sequence.SetDelay(anim.Delay);

            switch (anim.Type)
            {
                case PopupElementAnimation.AnimationType.SlideFromLeft:
                    AnimateSlideFromLeft(sequence, anim, isShow);
                    break;
                case PopupElementAnimation.AnimationType.SlideFromRight:
                    AnimateSlideFromRight(sequence, anim, isShow);
                    break;
                case PopupElementAnimation.AnimationType.SlideFromTop:
                    AnimateSlideFromTop(sequence, anim, isShow);
                    break;
                case PopupElementAnimation.AnimationType.SlideFromBottom:
                    AnimateSlideFromBottom(sequence, anim, isShow);
                    break;
                case PopupElementAnimation.AnimationType.ScalePunch:
                    AnimateScalePunch(sequence, anim, isShow);
                    break;
                case PopupElementAnimation.AnimationType.FadeIn:
                    AnimateFade(sequence, anim, isShow);
                    break;
                case PopupElementAnimation.AnimationType.RotateIn:
                    AnimateRotateIn(sequence, anim, isShow);
                    break;
            }

            sequence.SetLink(gameObject);
            currentAnimation = sequence;
        }

        private void AnimateSlideFromLeft(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            var screenWidth = Screen.width;
            Vector2 from = isShow ? new Vector2(-screenWidth, originalPosition.y) : originalPosition;
            Vector2 to = isShow ? originalPosition : new Vector2(-screenWidth, originalPosition.y);
            
            if (isShow)
            {
                targetRect.anchoredPosition = from;
            }
            
            sequence.Append(targetRect.DOAnchorPos(to, anim.Duration).SetEase(anim.Ease));
        }

        private void AnimateSlideFromRight(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            var screenWidth = Screen.width;
            Vector2 from = isShow ? new Vector2(screenWidth, originalPosition.y) : originalPosition;
            Vector2 to = isShow ? originalPosition : new Vector2(screenWidth, originalPosition.y);
            
            if (isShow)
            {
                targetRect.anchoredPosition = from;
            }
            
            sequence.Append(targetRect.DOAnchorPos(to, anim.Duration).SetEase(anim.Ease));
        }

        private void AnimateSlideFromTop(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            var screenHeight = Screen.height;
            Vector2 from = isShow ? new Vector2(originalPosition.x, screenHeight) : originalPosition;
            Vector2 to = isShow ? originalPosition : new Vector2(originalPosition.x, screenHeight);
            
            if (isShow)
            {
                targetRect.anchoredPosition = from;
            }
            
            sequence.Append(targetRect.DOAnchorPos(to, anim.Duration).SetEase(anim.Ease));
        }

        private void AnimateSlideFromBottom(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            var screenHeight = Screen.height;
            Vector2 from = isShow ? new Vector2(originalPosition.x, -screenHeight) : originalPosition;
            Vector2 to = isShow ? originalPosition : new Vector2(originalPosition.x, -screenHeight);
            
            if (isShow)
            {
                targetRect.anchoredPosition = from;
            }
            
            sequence.Append(targetRect.DOAnchorPos(to, anim.Duration).SetEase(anim.Ease));
        }

        private void AnimateScalePunch(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            if (isShow)
            {
                targetRect.localScale = Vector3.zero;
                sequence.Append(targetRect.DOScale(originalScale * anim.Strength, anim.Duration)
                    .SetEase(anim.Ease));
                sequence.Append(targetRect.DOScale(originalScale, anim.Duration * 0.3f)
                    .SetEase(Ease.InOutSine));
            }
            else
            {
                sequence.Append(targetRect.DOScale(Vector3.zero, anim.Duration)
                    .SetEase(anim.Ease));
            }
        }

        private void AnimateFade(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            if (targetCanvasGroup == null) return;
            
            var from = isShow ? 0f : originalAlpha;
            var to = isShow ? originalAlpha : 0f;
            
            if (isShow)
            {
                targetCanvasGroup.alpha = from;
            }
            
            sequence.Append(targetCanvasGroup.DOFade(to, anim.Duration).SetEase(anim.Ease));
        }

        private void AnimateRotateIn(Sequence sequence, PopupElementAnimation anim, bool isShow)
        {
            if (isShow)
            {
                targetRect.rotation = Quaternion.Euler(0, 0, -180f);
                targetRect.localScale = Vector3.zero;
                
                sequence.Join(targetRect.DORotate(Vector3.zero, anim.Duration).SetEase(anim.Ease));
                sequence.Join(targetRect.DOScale(originalScale, anim.Duration).SetEase(anim.Ease));
            }
            else
            {
                sequence.Append(targetRect.DORotate(new Vector3(0, 0, 180f), anim.Duration)
                    .SetEase(anim.Ease));
                sequence.Join(targetRect.DOScale(Vector3.zero, anim.Duration)
                    .SetEase(anim.Ease));
            }
        }

        private void SetDefaultState(bool isShow)
        {
            if (isShow)
            {
                targetRect.anchoredPosition = originalPosition;
                targetRect.localScale = originalScale;
                if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void ResetToInitialState()
        {
            targetRect.anchoredPosition = originalPosition;
            targetRect.localScale = originalScale;
            if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
            targetRect.rotation = Quaternion.identity;
        }

        
        public void ResetToStartPosition()
        {
            currentAnimation?.Kill();
            currentAnimation = null;
            
            if (targetRect == null) return;
            
            targetRect.anchoredPosition = originalPosition;
            targetRect.localScale = originalScale;
            targetRect.rotation = Quaternion.identity;
            
            if (targetCanvasGroup != null)
            {
                targetCanvasGroup.alpha = originalAlpha;
            }
        }
        
        public void ResetToAnimationStartPosition()
        {
            currentAnimation?.Kill();
            currentAnimation = null;
            
            if (targetRect == null) return;
            
            if (showAnimation == null)
            {
                ResetToStartPosition();
                return;
            }
            
            switch (showAnimation.Type)
            {
                case PopupElementAnimation.AnimationType.SlideFromLeft:
                    targetRect.anchoredPosition = new Vector2(-Screen.width, originalPosition.y);
                    targetRect.localScale = originalScale;
                    targetRect.rotation = Quaternion.identity;
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
                    break;
                    
                case PopupElementAnimation.AnimationType.SlideFromRight:
                    targetRect.anchoredPosition = new Vector2(Screen.width, originalPosition.y);
                    targetRect.localScale = originalScale;
                    targetRect.rotation = Quaternion.identity;
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
                    break;
                    
                case PopupElementAnimation.AnimationType.SlideFromTop:
                    targetRect.anchoredPosition = new Vector2(originalPosition.x, Screen.height);
                    targetRect.localScale = originalScale;
                    targetRect.rotation = Quaternion.identity;
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
                    break;
                    
                case PopupElementAnimation.AnimationType.SlideFromBottom:
                    targetRect.anchoredPosition = new Vector2(originalPosition.x, -Screen.height);
                    targetRect.localScale = originalScale;
                    targetRect.rotation = Quaternion.identity;
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
                    break;
                    
                case PopupElementAnimation.AnimationType.ScalePunch:
                    targetRect.anchoredPosition = originalPosition;
                    targetRect.localScale = Vector3.zero;
                    targetRect.rotation = Quaternion.identity;
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
                    break;
                    
                case PopupElementAnimation.AnimationType.FadeIn:
                    targetRect.anchoredPosition = originalPosition;
                    targetRect.localScale = originalScale;
                    targetRect.rotation = Quaternion.identity;
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = 0f;
                    break;
                    
                case PopupElementAnimation.AnimationType.RotateIn:
                    targetRect.anchoredPosition = originalPosition;
                    targetRect.localScale = Vector3.zero;
                    targetRect.rotation = Quaternion.Euler(0, 0, -180f);
                    if (targetCanvasGroup != null) targetCanvasGroup.alpha = originalAlpha;
                    break;
                    
                default:
                    ResetToStartPosition();
                    break;
            }
        }
    }
}