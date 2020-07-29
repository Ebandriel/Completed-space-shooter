using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private bool _isEnemyLaser = false;
    // Update is called once per frame
    void Update()
    {
        if(_isEnemyLaser)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime, 0);
        if (transform.position.y >= 10f)
        {
            //check if object has parent and if it does destroy parent
            if (this.transform.parent == null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }

        }
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, 0);
        if (transform.position.y <= -10f)
        {
            //check if object has parent and if it does destroy parent
            if (this.transform.parent == null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }

        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if(player!=null)
            {
                player.Damage();
            }

        }
    }
}
