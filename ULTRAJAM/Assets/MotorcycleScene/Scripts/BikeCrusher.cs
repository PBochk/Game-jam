using UnityEngine;

public class BikeCrusher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Dummy d = other.GetComponent<Dummy>();
        if (d != null)
        {
            d.Crush();
        }
    }
}