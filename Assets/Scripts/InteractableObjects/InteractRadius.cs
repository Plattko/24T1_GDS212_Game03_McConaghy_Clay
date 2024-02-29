using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plattko
{
    public class InteractRadius : MonoBehaviour
    {
        protected float currentAlpha = 0f;
        protected float fadeTime = 0.3f;

        protected Coroutine ButtonFade;

        public virtual void Interact()
        {

        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                GameObject interactButton = playerController.interactButton;
                Image image = interactButton.GetComponent<Image>();

                if (ButtonFade != null)
                {
                    StopCoroutine(ButtonFade);
                }

                ButtonFade = StartCoroutine(FadeInInteractButton(image, playerController));
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                GameObject interactButton = playerController.interactButton;
                Image image = interactButton.GetComponent<Image>();

                if (ButtonFade != null)
                {
                    StopCoroutine(ButtonFade);
                }
                ButtonFade = StartCoroutine(FadeOutInteractButton(image, playerController));
            }
        }

        protected virtual IEnumerator FadeInInteractButton(Image image, PlayerController playerController)
        {
            Debug.Log("Started fade in coroutine.");
            float remainingAlpha = 1f - currentAlpha;
            float adjustedFadeTime = fadeTime * remainingAlpha;
            
            while (currentAlpha < 1f)
            {
                currentAlpha += Time.deltaTime / adjustedFadeTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
                yield return null;
            }
            playerController.isInteractButtonVisible = true;
        }

        protected virtual IEnumerator FadeOutInteractButton(Image image, PlayerController playerController)
        {
            Debug.Log("Started fade out coroutine.");
            playerController.isInteractButtonVisible = false;
            float adjustedFadeTime = fadeTime * currentAlpha;

            while (currentAlpha > 0f)
            {
                currentAlpha -= Time.deltaTime / adjustedFadeTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
                yield return null;
            }
        }
    }
}
