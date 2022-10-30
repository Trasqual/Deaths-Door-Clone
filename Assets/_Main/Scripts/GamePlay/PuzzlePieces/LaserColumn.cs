using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserColumn : MonoBehaviour
{
    private Rotator _rotator;
    private List<LaserCaster> _laserCasters = new List<LaserCaster>();

    private void Awake()
    {
        _rotator = GetComponent<Rotator>();
        _laserCasters = GetComponentsInChildren<LaserCaster>().ToList();
    }

    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        _rotator.enabled = true;
        foreach (var laserCaster in _laserCasters)
        {
            laserCaster.Activate();
        }
    }

    public void DeActivate()
    {
        _rotator.enabled = false;
        foreach (var laserCaster in _laserCasters)
        {
            laserCaster.DeActivate();
        }
    }
}
