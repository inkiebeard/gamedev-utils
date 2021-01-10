using UnityEngine;

public class simpleShoot : MonoBehaviour {
    public Transform bullet;
    public float speed = 1000;
    public Transform firePoint;
    public float lifetime = 3f;

    void Start() {
        firePoint = firePoint ? firePoint : GameObject.Find("PlayerFirePoint").transform;
    }
    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Transform bl = Instantiate(bullet, firePoint.position, firePoint.rotation);
            bl.GetComponent<Rigidbody>().AddForce(bl.forward * speed);
            Destroy(bl.gameObject,lifetime);
        }
    }
}