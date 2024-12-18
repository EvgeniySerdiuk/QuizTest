using UnityEngine;

namespace Project.Scripts.Extensions
{
    public static class Extension
    {
        public static void Show(this CanvasGroup image, float alpha = 1)
        {
            image.alpha = alpha;
            image.blocksRaycasts = true;
            image.interactable = true;
        }

        public static void Hide(this CanvasGroup image, float alpha = 0)
        {
            image.alpha = alpha;

            if (alpha == 0)
            {
                image.blocksRaycasts = false;
                image.interactable = false;
            }
        }
    }
}