using UnityEngine;

public struct Polar {
    public float Distance;
    public float Angle;

    public static Polar FromCartesian(float x, float y) {
        return new Polar {
            Distance = Mathf.Sqrt(x * x + y * y),
            Angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg
        };
    }

    public Polar(float distance, float angle) {
        Distance = distance;
        Angle = angle;
    }

    public float X {
        get { return Mathf.Cos(Angle * Mathf.Deg2Rad) * Distance; }
    }

    public float Y {
        get { return Mathf.Sin(Angle * Mathf.Deg2Rad) * Distance; }
    }

}