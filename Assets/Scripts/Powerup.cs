using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3;

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, 0);
        //if at bottom destroy object
        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }

    }
    //on collision with player passes object type then destroys object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Powerup(this.gameObject.tag);
                Destroy(this.gameObject);
            }
            
        }
    }


}
