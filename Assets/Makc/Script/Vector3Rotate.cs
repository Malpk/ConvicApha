using UnityEngine;

public static class Vector3Rotate 
{
    public static Vector3 Rotate(this Vector3 offset, float angel)
    {
        angel *= Mathf.Deg2Rad;
        float xOffset = offset.x * Mathf.Cos(angel) - offset.y * Mathf.Sin(angel);
        float yOffset = offset.x * Mathf.Sin(angel) + offset.y * Mathf.Cos(angel);
        return new Vector3(xOffset, yOffset);
    }
}
