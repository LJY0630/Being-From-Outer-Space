using UnityEngine;

namespace PixelPlay.OffScreenIndicator
{
    public class OffScreenIndicatorCore
    {
        /// <summary>
        /// Gets the position of the target mapped to screen cordinates.
        /// </summary>
        /// <param name="mainCamera">Refrence to the main camera</param>
        /// <param name="targetPosition">Target position</param>
        /// <returns></returns>
        public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            return screenPosition;
        }

        /// <summary>
        /// Gets if the target is within the view frustrum.
        /// </summary>
        /// <param name="screenPosition">Position of the target mapped to screen cordinates</param>
        /// <returns></returns>
        public static bool IsTargetVisible(Vector3 screenPosition, RectTransform rect)
        {
            bool isTargetVisible = screenPosition.z >= 0 && screenPosition.x >= 0 && screenPosition.x <= 256.1 && screenPosition.y >= 0 && screenPosition.y <= 256.1;
            return isTargetVisible;
        }

        /// <summary>
        /// Gets the screen position and angle for the arrow indicator. 
        /// </summary>
        /// <param name="screenPosition">Position of the target mapped to screen cordinates</param>
        /// <param name="angle">Angle of the arrow</param>
        /// <param name="screenCentre">The screen  centre</param>
        /// <param name="screenBounds">The screen bounds</param>
        public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds, RectTransform panelRect, Vector3 where)
        {
            screenPosition -= screenCentre;

            // When the targets are behind the camera their projections on the screen (WorldToScreenPoint) are inverted,
            // so just invert them.
            if (screenPosition.z < 0)
            {
                screenPosition *= -1;
            }

            screenPosition.x *= panelRect.localScale.x;
            screenPosition.y *= panelRect.localScale.y;

            // Angle between the x-axis (bottom of screen) and a vector starting at zero (bottom-left corner of screen) and terminating at screenPosition.
            angle = Mathf.Atan2(screenPosition.y, screenPosition.x);

            // Calculate the maximum x and y position within the panel's boundaries
            float maxX = panelRect.rect.width * panelRect.localScale.x / 2f;
            float maxY = panelRect.rect.height * panelRect.localScale.y / 2f;

            // Adjust the x and y positions to fit within the panel's boundaries
            screenPosition.x = Mathf.Clamp(screenPosition.x, -maxX, maxX);
            screenPosition.y = Mathf.Clamp(screenPosition.y, -maxY, maxY);
            screenPosition.z = 0.0f;

            // Bring the ScreenPosition back to its original reference.
            screenPosition += where;
        }
    }
}
