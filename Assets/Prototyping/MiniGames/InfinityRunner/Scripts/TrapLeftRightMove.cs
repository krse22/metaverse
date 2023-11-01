using UnityEngine;

public class TrapLeftRightMove : MonoBehaviour
{

    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private float speed;

    private float direction = 1f;

    private void Start()
    {
        transform.localPosition = new Vector3(Random.Range(min, max), transform.localPosition.y, transform.localPosition.z);
    }

    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + direction * Time.deltaTime * speed, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x < min)
        {
            direction = 1f;
        }
        if (transform.localPosition.x > max) {
            direction = -1f;
        }

    }
}
