using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : Character, IDamageable
{
    private static GameObject localInstance;
    [SerializeField] private TextMeshPro playerNameText;

    public int shootCounter;
    private Rigidbody rb;
    [SerializeField] private float speed;

    public static GameObject LocalInstance => localInstance;

    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(SetName), RpcTarget.AllBuffered, GameData.playerName);
            localInstance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
    }

    [PunRPC]
    private void SetName(string playerName)
    {
        playerNameText.text = playerName;
    }

    private void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        Move();
        Shoot();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, vertical * speed);

        if (horizontal != 0 || vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
            obj.GetComponent<Bullet>().SetUp(transform.forward, photonView.ViewID);
            
        }
    }

    public override void TakeDamage(float amount)
    {
        vida -= amount;
        if (vida <= 0 && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void ReceiveDamage(float damage)
    {
        if (photonView.IsMine)
        {
            TakeDamage(damage);
        }
    }
}
