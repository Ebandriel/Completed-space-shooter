using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    AudioClip _ExplosionEffect;
  

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            UnityEngine.Debug.Log("No Audio source on explosion!");
        }
        else
        {
            audioSource.clip = _ExplosionEffect;
            audioSource.Play();
        }
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
