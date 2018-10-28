using System.Collections;
using UnityEngine;

namespace PHOCUS.UI
{
    public class InteractableIndicator : MonoBehaviour
    {
        public Animator Anim;

        CanvasGroup canvasGroup;

        void Start()
        {
            Anim = GetComponent<Animator>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ToggleIndicator()
        {
            canvasGroup.alpha = 1;
            Anim.SetTrigger("Indicate");
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
        }
    }
}