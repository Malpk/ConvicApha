using UnityEngine;

static class Extensions
{
    public static void LookAt2D(this Transform me, Vector3 target, Vector3? eye = null)
    {
        float signedAngle = Vector2.SignedAngle(eye ?? me.up, target - me.position);

        if (Mathf.Abs(signedAngle) >= 1e-3f)
        {
            var angles = me.eulerAngles;
            angles.z += signedAngle;
            me.eulerAngles = angles;
        }
    }
    public static void LookAt2D(this Transform me, Transform target, Vector3? eye = null)
    {
        me.LookAt2D(target.position, eye);
    }
}
