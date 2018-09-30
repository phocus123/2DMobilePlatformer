using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.UI
{
    public class InteractableText : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            StartCoroutine(FadeAlertText());
        }

        IEnumerator FadeAlertText()
        {
            float progress = 0f;

            yield return new WaitForSeconds(1.5f);

            while (progress < 1)
            {
                progress += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, progress);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}