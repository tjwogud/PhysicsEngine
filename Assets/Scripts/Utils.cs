using UnityEngine;

public static class Utils
{
    public static float GetAngle(this Vector2 vector)
    {
        if (vector.y == 0)
            return vector.x >= 0 ? 0 : Mathf.PI;
        if (vector.x == 0)
            return vector.y > 0 ? Mathf.PI / 2 : Mathf.PI * 3 / 2;
        return Mathf.Atan(vector.y / vector.x); 
    }
    public static Vector2 Polar(float magnitude, float angle) => new(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
    public static float Cross(this Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;
}