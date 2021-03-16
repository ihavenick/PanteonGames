using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    //dönme hızı
    [SerializeField] private float rotationSpeed = 80f;
    //sağa mı dönsün diye tanımlanmış bool
    [SerializeField] private bool rotateRight = true;
    
    void Update()
    {
        //bug olmasın diye yerel değişken tanımla
        float finalRotationSpeed = rotationSpeed;
        
        //Sağ dönmesi gerekiyorsa - ile çarp
        if (rotateRight)
            finalRotationSpeed = -rotationSpeed;
            
        //Döndür
        transform.Rotate(0f, 0f, finalRotationSpeed * Time.deltaTime);
    }
    
    //oyuncuya degerse
    void OnCollisionEnter(Collision cllsn)
    {
        //Oyuncuyu tag inden bul
        //GameObject character = GameObject.FindGameObjectWithTag("Player");

        if (cllsn.collider.tag == "Player")
        {
            //birlikte hareket etmek icin karakterin transformuna parrent ekle
            cllsn.collider.transform.SetParent(this.transform);
        }
       
    }
    //artık degmiyorsa 
    void OnCollisionExit(Collision other)
    {
        GameObject character = GameObject.FindGameObjectWithTag("Player");
        //ayrılırsa parenttan cıkar ve rotasyonu sıfırla
        character.transform.parent = null;
        character.transform.rotation = Quaternion.Euler(0,0,0);;
    }
    
}
