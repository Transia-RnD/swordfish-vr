using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlaying : MonoBehaviour
{
    public AudioSource Telephone;

    public void PlayTelephone() => Telephone.Play();
}