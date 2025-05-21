using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField]
    public Rigidbody rb;

    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

        if (transform.position.y < waveHeight)
        {
            float displaceMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForce(Vector3.up * Physics.gravity.magnitude * displaceMultiplier, ForceMode.Acceleration);
        }
    }
}
