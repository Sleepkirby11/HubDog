using UnityEngine;

public class Folder : MonoBehaviour
{
    public GameObject explorer;
    public GameObject disableObject;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(disableObject != null)
                disableObject.SetActive(false);
            explorer.SetActive(true);
        }
    }
}
