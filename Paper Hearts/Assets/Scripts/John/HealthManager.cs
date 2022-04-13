using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public GameObject heartPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int HP)
    {
        while(transform.childCount > 0) //Clear all current health
        {
            Destroy(transform.GetChild(0));
        }
        for(int a = 0; a < HP; a++)
        {
            if(a == 0)
            {
                Debug.Log("Heart placed at: (" + transform.position + ")");
                Instantiate(heartPrefab, transform.position, Quaternion.identity, transform);
            }
            else
            {
                Debug.Log("Heart placed at: (" + transform.position + ")");
                Instantiate(heartPrefab, new Vector3(transform.position.x + (1.1f * a), transform.position.y, transform.position.z), Quaternion.identity, transform);
            }
        }
    }

    public void TakeDamage()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        if(transform.childCount == 0)
        {
            //Handle death here. or dont.
        }
    }
}
