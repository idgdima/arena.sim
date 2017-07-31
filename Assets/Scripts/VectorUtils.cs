using UnityEngine;

public static class VectorUtils {
    public static float GetDirectionAngle(Vector3 direcion) {
        return Mathf.Atan2(direcion.y, direcion.x) * Mathf.Rad2Deg;
    }
}