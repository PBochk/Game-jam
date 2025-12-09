using UnityEngine;
using UnityEngine.UI;

namespace Shooter.Gameplay
{
    public class EnemyIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform indicatorTransform;
        [SerializeField] private Image indicatorImage;
        [SerializeField] private Transform targetEnemy;

        private Camera mainCamera;
        private Canvas parentCanvas;
        private float margin = 50f;

        void Awake()
        {
            if (indicatorTransform is null)
                indicatorTransform = GetComponent<RectTransform>();

            if (indicatorImage is null)
                indicatorImage = GetComponent<Image>();

            mainCamera = Camera.main;
            parentCanvas = GetComponentInParent<Canvas>();
        }

        public void SetTarget(Transform enemy)
        {
            targetEnemy = enemy;
        }

        public void SetMargin(float newMargin)
        {
            margin = newMargin;
        }

        public void Update()
        {
            if (targetEnemy == null)
            {
                gameObject.SetActive(false);
                return;
            }
            if (Camera.main == null)
                return;

            mainCamera = Camera.main;

            UpdateIndicatorPosition();
        }

        private void UpdateIndicatorPosition()
        {
            var screenPoint = mainCamera.WorldToViewportPoint(targetEnemy.position);

            if (screenPoint.z < 0)
            {
                screenPoint.x = 1 - screenPoint.x;
                screenPoint.y = 1 - screenPoint.y;
                screenPoint.z = Mathf.Abs(screenPoint.z);
            }

            bool isOnScreen = screenPoint.x >= 0 && screenPoint.x <= 1 &&
                             screenPoint.y >= 0 && screenPoint.y <= 1 && screenPoint.z > 0;

            if (isOnScreen)
            {
                indicatorImage.enabled = false;
                return;
            }

            indicatorImage.enabled = true;

            screenPoint.x = Mathf.Clamp01(screenPoint.x);
            screenPoint.y = Mathf.Clamp01(screenPoint.y);

            var screenPos = new Vector2(
                screenPoint.x * Screen.width,
                screenPoint.y * Screen.height
            );

            screenPos.x = Mathf.Clamp(screenPos.x, margin, Screen.width - margin);
            screenPos.y = Mathf.Clamp(screenPos.y, margin, Screen.height - margin);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)parentCanvas.transform,
                screenPos,
                parentCanvas.worldCamera,
                out Vector2 canvasPos
            );

            indicatorTransform.anchoredPosition = canvasPos;

            var dir = (targetEnemy.position - mainCamera.transform.position).normalized;
            dir.z = 0;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            indicatorTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}