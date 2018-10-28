using UnityEngine;

namespace PHOCUS.Environment
{
    public enum PathState
    {
        Enabled,
        Disabled
    }

    public class Path : MonoBehaviour
    {
        public SpriteRenderer enabledSprite;
        public SpriteRenderer disabledSprite;
        public BoxCollider2D boxCollider;
        public bool IsEnabled;
        public PathState State;

        const float enabledAlpha = 255f;
        const float disabledAlpha = 80f;

        void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>(true);

            if (enabledSprite == null)
                enabledSprite = renderers[0];

            if (disabledSprite == null)
                disabledSprite = renderers[1];
        }

        void Update()
        {
            if (!IsEnabled && State != PathState.Disabled)
            {
                State = PathState.Disabled;
                ToggleTiles();

            }
            else if (IsEnabled && State != PathState.Enabled)
            {
                State = PathState.Enabled;
                ToggleTiles();
            }
        }

        void ToggleTiles()
        {
            boxCollider.enabled = boxCollider.enabled == false ? true : false;
            enabledSprite.gameObject.SetActive(!enabledSprite.gameObject.activeSelf);
            disabledSprite.gameObject.SetActive(!disabledSprite.gameObject.activeSelf);
        }
    }
}