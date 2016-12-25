using UnityEngine;

public class Ball : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            ScoreManager.instance.Score += 1;
        }
    }
}
