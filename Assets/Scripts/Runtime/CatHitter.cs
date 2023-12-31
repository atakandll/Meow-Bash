﻿using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

namespace Runtime
{
    public class CatHitter : MonoBehaviour
    {
        public GameObject TextScore;
        public GameObject TextScore3D;
        public GameObject[] hitParticles;
        
        private Rigidbody _rigidbody;
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("hittable"))
            {
                Debug.Log("object hitted" + hit.gameObject.name);
                hit.gameObject.tag = "hitted";
                _rigidbody = hit.gameObject.GetComponent<Rigidbody>();
                HittableObjects hittableObjects = hit.gameObject.GetComponent<HittableObjects>();
                HitEffect(_rigidbody,hit);
                Score3DEffect(hittableObjects);
                GetCoins(hittableObjects);
                
            }   
        }
        public void GetCoins(HittableObjects hittableObjects)
        {
            int currentCoins = PlayerPrefs.GetInt("coins", 0);

            if (currentCoins + hittableObjects.coins > 99999999)
            {
                PlayerPrefs.SetInt( "coins", 99999999);
            }
            else
                PlayerPrefs.SetInt("coins", currentCoins + hittableObjects.coins);
        }

        public void HitEffect(Rigidbody _rigidbody, ControllerColliderHit hit)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddExplosionForce(75,transform.position + Vector3.down,15);
            Instantiate(hitParticles[Random.Range(0, hitParticles.Length)], hit.transform.position, Quaternion.identity);
            iTween.PunchScale(TextScore,new Vector3(1.25f,1.25f,1.25f),.3f);
        }

        public void Score3DEffect(HittableObjects hittableObjects)
        {
            int newScore = int.Parse(TextScore.GetComponent<TextMeshProUGUI>().text) + hittableObjects.Points;
            TextScore.GetComponent<TextMeshProUGUI>().text = newScore.ToString();
            TextScore3D.GetComponent<TextMesh>().text = "+" + hittableObjects.Points;
            iTween.PunchScale(TextScore3D,new Vector3(1.25f,1.25f,1.25f),.5f);
            Invoke("Reset",.5f);
        }

        public void Reset()
        {
            TextScore3D.GetComponent<TextMesh>().text = "";
        }
    }
}