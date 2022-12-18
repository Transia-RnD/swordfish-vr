using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine.UI;
using GalleryCSharp.Models;
using Swordfish;
using Newtonsoft.Json;


public class FirebaseManager : MonoBehaviour
{
    [Header("Firebase")]
    public FirebaseFirestore db;
    
    private void Awake()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
}
