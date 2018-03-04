using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour
{
    private ParticleSystem ps;
    public bool moduleEnabled;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var emission = ps.emission;
        emission.enabled = moduleEnabled;
    }

    void OnGUI()
    {
        moduleEnabled = GUI.Toggle(new Rect(25, 45, 100, 30), moduleEnabled, "Enabled");
    }
}
