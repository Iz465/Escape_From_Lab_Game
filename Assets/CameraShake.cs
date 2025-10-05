using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake(float x, float y)
    {
        float positiveX = x + 0.1f;
        float negativeX = x - 0.1f;

        float positiveY = y + 0.1f;
        float negativeY = y - 0.1f;

    }
}
