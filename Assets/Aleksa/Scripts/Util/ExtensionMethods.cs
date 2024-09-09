using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Converts an angle in degrees to a normalized flight direction vector.
    /// </summary>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns>A normalized Vector2 representing the flight direction.</returns>
    public static Vector2 ToFlightDirection(this float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
    
    /// <summary>
    /// Gets the angle of a Transform's rotation around the Z-axis in degrees.
    /// </summary>
    /// <param name="transform">The Transform whose rotation angle is to be calculated.</param>
    /// <returns>The angle of the Transform's rotation around the Z-axis in degrees.</returns>
    public static float GetAngle(this Transform transform)
    {
        return transform.eulerAngles.z;
    }
}
