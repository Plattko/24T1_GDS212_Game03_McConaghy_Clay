using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Plattko
{
    public class MountainRange : MonoBehaviour
    {
        [SerializeField] Transform mountainRange;
        [SerializeField] TextMeshProUGUI gameFinishedText;
        Vector3 targetPosition = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        float smoothTime = 2f;

        private bool hasPlayerReachedEnd = false;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                mountainRange.position = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
                targetPosition = new Vector3(transform.position.x, transform.position.y + 6f, transform.position.z);
                hasPlayerReachedEnd = true;
                StartCoroutine(FadeInText());
            }
        }

        private void Update()
        {
            if (hasPlayerReachedEnd)
            {
                mountainRange.position = Vector3.SmoothDamp(mountainRange.position, targetPosition, ref velocity, smoothTime);
            }
        }

        private IEnumerator FadeInText()
        {
            Debug.Log("Fade in text started.");
            Color startColour = gameFinishedText.color;
            float targetAlpha = 1f;
            Color targetColour = new Color(startColour.r, startColour.g, startColour.b, targetAlpha);
            float fadeTime = 2f;
            float timeElapsed = 0f;

            while (timeElapsed < fadeTime)
            {
                gameFinishedText.color = Color.Lerp(startColour, targetColour, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            gameFinishedText.color = targetColour;
            Debug.Log("Fade in text complete.");
        }
    }
}
