using UnityEngine;

public static class VectorExtension 
{
    public static Vector3 Rotate(this Vector3 offset, float angel)
    {
        angel *= Mathf.Deg2Rad;
        float xOffset = offset.x * Mathf.Cos(angel) - offset.y * Mathf.Sin(angel);
        float yOffset = offset.x * Mathf.Sin(angel) + offset.y * Mathf.Cos(angel);
        return new Vector3(xOffset, yOffset);
    }
    public static Vector2Int Clamp(this Vector2Int value, Vector2Int min, Vector2Int max)
    {
        var x = Mathf.Clamp(value.x, min.x, max.x);
        var y = Mathf.Clamp(value.y, min.y, max.y);
        return new Vector2Int(x, y);
    }
}
