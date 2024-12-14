using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    private int ownerId;
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float damageAmount = 20f;
    private Vector3 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction, int ownerId)
    {
        this.direction = direction;
        this.ownerId = ownerId;
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.photonView.ViewID != ownerId)
            {
                player.photonView.RPC("ReceiveDamage", RpcTarget.AllBuffered, damageAmount);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}