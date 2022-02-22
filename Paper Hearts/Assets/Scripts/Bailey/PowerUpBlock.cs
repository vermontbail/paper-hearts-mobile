using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBlock : MonoBehaviour
{
    [SerializeField]
    private PowerUp heldPowerUp;
    [SerializeField]
    GameObject createdPowerup;
    // Start is called before the first frame update

    public void CreatePowerUp()
    {
        // use prefab to instantiate powerup
        GameObject pu = Object.Instantiate(createdPowerup, this.transform.position, this.transform.rotation);
        pu.GetComponent<PowerUpScript>().powerUp = heldPowerUp;
    }
}
