using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoke : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GameObject.Find("MouseSqueakEffectPlayer").GetComponent<AudioSource>();
    }

    public void OnMousePoked()
    {
        source.Play();
    }
}
