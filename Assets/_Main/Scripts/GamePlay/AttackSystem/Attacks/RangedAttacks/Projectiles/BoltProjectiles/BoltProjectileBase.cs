using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
	public class BoltProjectileBase : ProjectileBase
	{
        [SerializeField] protected float projectileSpeed = 5f;
        public Collider Col => GetComponent<Collider>();
    }
}
