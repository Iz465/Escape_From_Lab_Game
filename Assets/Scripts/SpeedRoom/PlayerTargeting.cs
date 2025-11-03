using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    Transform plr;
    public Transform muzzle;
    public GameObject bullet;
    [SerializeField] float shootSpeed, timeBetweenBullet;
    float lastShot;
    float health = 100;

    [SerializeField] RectTransform healthBar;

    void Start()
    {
        healthBar = transform.Find("Canvas").Find("GreenHealth").GetComponent<RectTransform>();
        StartCoroutine(WaitForPlayer());
    }
    
    IEnumerator WaitForPlayer()
    {
        while (!GameObject.FindGameObjectWithTag("Player"))
        {
            yield return null;
        }
        if (GameObject.Find("Camera"))
        {
            Destroy(GameObject.Find("Camera"));
        }

        plr = GameObject.FindGameObjectWithTag("Player").transform;

        plr.position = transform.position;
    }

    void ShootPlayer()
    {
        Vector3 plrDirection = new Vector3(plr.position.x, transform.position.y, plr.position.z);
        transform.LookAt(plrDirection);

        if (lastShot > Time.time) return; //stop bullet spray
        lastShot = Time.time + timeBetweenBullet;
        GameObject newBullet = Instantiate(bullet);
        Transform bull = newBullet.transform;

        bull.position = muzzle.position;
        bull.LookAt(plr.position);

        bull.GetComponent<Rigidbody>().linearVelocity = bull.forward * shootSpeed;
    }

    public void TakeDamage()
    {
        health -= 10;
        healthBar.localScale = new Vector3(health / 100,1,1);

        if (health <= 0)
            Destroy(gameObject);
    }

    void Update()
    {
        if (plr == null) return;
        ShootPlayer();
    }
}
