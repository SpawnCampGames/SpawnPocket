using UnityEngine;

// SPΔWN™ 🕹️ [EXPERIMENTAL] 📗 http://spawncampgames.github.io/docs
// License: CC BY-NC-ND 4.0 (Creative Commons Attribution-NonCommercial-NoDerivatives)

namespace SPWN.POCKET
{
    /// <summary>
    /// Detects 2D trigger and collision enter/exit events [POCKET]
    /// <para>Supports filtering by tag, component, or any collider.</para>
    /// <para>UseTrigger = true for triggers, false for collisions.</para>
    /// <para>RequireKey = true requires pressing a key to interact.</para>
    /// <para>Customize interaction key and layer mask from the Inspector.</para>
    /// <para>[POCKET EDITION] v7.23.25</para>
    /// </summary>
    public class ContactDetector2D : MonoBehaviour
    {
        public enum FilterType { Tag, Component, Any }

        [Header("Settings")]
        public bool useTrigger = true;
        public bool requireKey = false;
        public KeyCode interactionKey = KeyCode.E;
        public FilterType filterType = FilterType.Any;
        public string filterTag = "Player";
        public string filterComponentName = "Collider2D";
        public LayerMask layerMask = ~0;

        [Header("State (Read Only)")]
        public bool IsContacting { get; private set; } = false;

        private GameObject contactingObject = null;

        bool IsValidContact(GameObject other)
        {
            if (((1 << other.layer) & layerMask) == 0)
                return false;

            switch (filterType)
            {
                case FilterType.Tag:
                    return other.CompareTag(filterTag);
                case FilterType.Component:
                    return other.GetComponent(filterComponentName) != null;
                case FilterType.Any:
                default:
                    return true;
            }
        }

        bool CheckInteraction()
        {
            return !requireKey || Input.GetKeyDown(interactionKey);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!useTrigger) return;
            if (!IsValidContact(other.gameObject)) return;

            IsContacting = true;
            contactingObject = other.gameObject;

            if (CheckInteraction())
                InteractionEnter(contactingObject);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!useTrigger) return;
            if (contactingObject == other.gameObject)
            {
                InteractionExit(contactingObject);
                IsContacting = false;
                contactingObject = null;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (useTrigger) return;
            if (!IsValidContact(collision.gameObject)) return;

            IsContacting = true;
            contactingObject = collision.gameObject;

            if (CheckInteraction())
                InteractionEnter(contactingObject);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (useTrigger) return;
            if (contactingObject == collision.gameObject)
            {
                InteractionExit(contactingObject);
                IsContacting = false;
                contactingObject = null;
            }
        }

        // Override these by copying the script and editing here
        public void InteractionEnter(GameObject obj)
        {
            Debug.Log($"[ContactDetector2D] Interaction Enter with {obj.name}");
        }

        public void InteractionExit(GameObject obj)
        {
            Debug.Log($"[ContactDetector2D] Interaction Exit with {obj.name}");
        }
    }
}
