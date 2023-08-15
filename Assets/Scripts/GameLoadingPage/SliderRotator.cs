using UnityEngine;

public class SliderRotator : MonoBehaviour
{

    private void Update()
    {
        transform.Rotate(0f, 0f, 300f * Time.deltaTime);
    }
}
