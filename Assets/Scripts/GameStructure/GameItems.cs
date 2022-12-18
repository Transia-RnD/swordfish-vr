using UnityEngine;
using GalleryCSharp.Models;
using Firebase.Firestore;

namespace Swordfish
{
    public class GameItems : Singleton<GameItems>
    {
        protected GameItems() { }

        public FirebaseFirestore db = FirebaseFirestore.DefaultInstance;


        // public void ClearState() {
        //     isLoggedIn = false;
        //     didTimeout = false;
        //     userIdToken = "";
        //     userId = "";;
        //     accountId = "";;
        // }
    }
}