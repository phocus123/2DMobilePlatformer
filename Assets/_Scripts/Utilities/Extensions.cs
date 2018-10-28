using UnityEngine;

namespace PHOCUS.Utilities
{
    public static class Extensions 
    {
        public static CanvasGroup Toggle(this CanvasGroup source)
        {
            source.alpha = source.alpha == 0 ? 1 : 0;
            source.blocksRaycasts = source.blocksRaycasts == true ? false : true;

            return source;
        }
    }
}