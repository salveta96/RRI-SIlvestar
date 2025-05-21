using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloaterBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField]
    public List<GameObject> floaters;

    public float depthBeforeSubmerged = 2f;
    public float displacementAmount = 2f;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        foreach (var floater in floaters)
        {
            Vector3 floaterPos = floater.transform.position;

            float waveHeight = WaveManager.instance.GetWaveHeight(floaterPos.x);

            if (floaterPos.y < waveHeight)
            {
                float displaceMultiplier = Mathf.Clamp01((waveHeight - floaterPos.y) / depthBeforeSubmerged) * displacementAmount;
                _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displaceMultiplier, 0f), floaterPos, ForceMode.Acceleration);
                _rb.AddForce(displaceMultiplier * -_rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                _rb.AddTorque(displaceMultiplier * -_rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }
}
