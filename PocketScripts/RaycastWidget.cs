using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// SPΔWN™ 🕹️ [EXPERIMENTAL] 📗 http://spawncampgames.github.io/docs
// License: CC BY-NC-ND 4.0 (Creative Commons Attribution-NonCommercial-NoDerivatives)

namespace SPWN.POCKET
{
    /// <summary>
    /// Raycast Debug and Visualization Tool [POCKET]
    /// <para>Visualizes raycasts and hit points in Scene or Game view.</para>
    /// <para>Customize direction, color, and distance from the Inspector.</para>
    /// <para>Toggle Gizmos for Editor-only or Runtime display.</para>
    /// <para>Press 'Return' to print hit info to the Console.</para>
    /// <para>[POCKET EDITION] v7.22.25</para>
    /// </summary>
    public class RaycastWidget : MonoBehaviour
    {
        public enum Direction
        {
            Forward, 
            Backward,
            Left,
            Right,
            Up,
            Down,
            Custom
        }

        [Header("Inputs")]
        [Tooltip("GameObject used as the raycast origin (default: this GameObject)")]
        public GameObject target;

        [Tooltip("Raycast direction relative to target's local space (ignored if 'Custom' is selected)")]
        public Direction direction = Direction.Forward;

        [Tooltip("Custom local direction used if 'Custom' is selected in Direction enum (should be normalized)")]
        public Vector3 customDirection = Vector3.zero;

        [Tooltip("Use target's local space for direction (true) or world space (false)")]
        public bool useLocalSpace = true;

        [Tooltip("Key to press for printing current raycast hit info to the Console")]
        public KeyCode logKey = KeyCode.Return;

        [Header("Graphics")]
        [Tooltip("Color of the ray when no hit occurs")]
        public Color rayColor = Color.green;

        [Tooltip("Color of the ray when a hit occurs")]
        public Color rayHitColor = Color.red;

        [Tooltip("Maximum distance for the raycast")]
        public float rayDistance = 10f;

        [Tooltip("Layers to include in raycast")]
        public LayerMask layers = ~0;

        [Tooltip("Toggle drawing of gizmos for raycast visualization")]
        public bool drawGizmos = true;

        [Tooltip("Draw gizmos only in Editor and not in Play mode")]
        public bool gizmosInEditorOnly = true;

        [Header("Trigger Detection")]
        [Tooltip("Include trigger colliders in raycast (true) or ignore them (false)")]
        public bool detectTriggers = true;

        [Header("Outputs (ReadOnly)")]
        [SerializeField, Tooltip("Whether the raycast hit an object")]
        private bool hitSuccess;

        [SerializeField, Tooltip("The GameObject that was hit by the raycast")]
        private GameObject hitObject;

        private RaycastHit hitInfo;

        Vector3 GetDirection()
        {
            Vector3 dir = direction switch
            {
                Direction.Forward => Vector3.forward,
                Direction.Backward => Vector3.back,
                Direction.Left => Vector3.left,
                Direction.Right => Vector3.right,
                Direction.Up => Vector3.up,
                Direction.Down => Vector3.down,
                Direction.Custom => customDirection.normalized,
                _ => Vector3.forward
            };

            return useLocalSpace && target != null ? target.transform.TransformDirection(dir) : dir;
        }

        void Awake()
        {
            if (target == null)
                target = gameObject;
        }

        void Update()
        {
            Vector3 dir = GetDirection();
            QueryTriggerInteraction triggerOption = detectTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;

            if (Physics.Raycast(transform.position, dir, out hitInfo, rayDistance, layers, triggerOption))
            {
                hitObject = hitInfo.collider.gameObject;
                hitSuccess = true;
            }
            else
            {
                hitObject = null;
                hitSuccess = false;
            }

            if (Input.GetKeyDown(logKey))
            {
                Debug.Log($"[RaycastWidget] Raycast Success: {hitSuccess}");
                if (hitSuccess)
                {
                    Debug.Log($"Hit Object: {hitObject.name}");
                    Debug.Log($"Hit Point: {hitInfo.point}");
                    Debug.Log($"Hit Normal: {hitInfo.normal}");
                    Debug.Log($"Hit Distance: {hitInfo.distance}");
                }
            }
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (gizmosInEditorOnly && !UnityEditor.EditorApplication.isPlaying)
            {
                DrawGizmo();
            }
#endif
            if (!gizmosInEditorOnly && Application.isPlaying)
            {
                DrawGizmo();
            }
        }

        void DrawGizmo()
        {
            if (!drawGizmos) return;

            Vector3 dir = GetDirection();

            Gizmos.color = hitSuccess ? rayHitColor : rayColor;
            Gizmos.DrawRay(transform.position, dir * rayDistance);

#if UNITY_EDITOR
            if (hitSuccess)
            {
                Handles.color = Gizmos.color;
                Handles.DrawWireDisc(hitInfo.point, hitInfo.normal, 0.1f);
            }
#endif
        }
    }
}
