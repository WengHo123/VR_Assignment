using UnityEngine;

public class FollowSkinnedMeshCollider : MonoBehaviour
{
    public Transform targetBone; // The bone the collider should follow
    private Vector3 localPositionOffset;
    private Quaternion localRotationOffset;

    void Start()
    {
        if (targetBone == null)
        {
            Debug.LogError("Target bone is not assigned.");
            return;
        }

        // Calculate initial local position and rotation offsets
        localPositionOffset = transform.localPosition;
        localRotationOffset = transform.localRotation;
    }

    void Update()
    {
        if (targetBone == null) return;

        // Update position and rotation to follow the target bone
        transform.position = targetBone.TransformPoint(localPositionOffset);
        transform.rotation = targetBone.rotation * localRotationOffset;
    }
}
