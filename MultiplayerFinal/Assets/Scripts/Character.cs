using UnityEngine;
using Photon.Pun;


public abstract class Character : MonoBehaviourPun
{
    public float vida;
    public abstract void TakeDamage(float amount);
}

public interface IDamageable
{
    void ReceiveDamage(float damage);
}