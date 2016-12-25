using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject bulletPrefab;
    public GameObject tankTurrent;
    public Slider powerSlider;
    public float fireRate;
    public float startFirePower;
    float power;
    float rate;
    bool isPressed = true;
    Camera cam;

    void Awake()
    {
        instance = this;
        cam = Camera.main;
    }

    void Start()
    {
        Cursor.visible = false;
        power = startFirePower;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0f)
        {
            Vector3 angle = transform.eulerAngles;
            angle += new Vector3(0f, h, 0f);
            angle.y = Mathf.Clamp(angle.y, 0f, 60f);
            transform.eulerAngles = angle;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isPressed = false;
            GameObject bullet = Instantiate(bulletPrefab) as GameObject;
            bullet.transform.position = tankTurrent.transform.position + (Vector3.up / 2.3f);
            bullet.transform.rotation = tankTurrent.transform.rotation;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * power);
            power = startFirePower;
            rate = 0f;
            Destroy(bullet, 10f);
        }

        if (Input.GetButton("Fire1"))
        {
            powerSlider.value = (power - 1000f);
            power += 30f;
            rate += Time.deltaTime;
            print(power);
        }
      
        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = true;
        }
    }

}
