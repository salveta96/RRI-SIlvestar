using Unity.Mathematics;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public float amplitude = 1f;
    public float waveLenght = 2f;
    public float speed = 1f;
    public float offset = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Wavemanager instance already exists, destroying this");
            Destroy(this);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        offset += speed * Time.deltaTime;
    }

    public float GetWaveHeight(float x)
    {
        return amplitude * math.sin(x / waveLenght + offset);
    }
}
