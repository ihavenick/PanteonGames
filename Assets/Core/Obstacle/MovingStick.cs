using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStick : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        //2sn de bir tekrarı tetikle
        InvokeRepeating("MoveStick", 2.0f, 2f);
    }
    
    //Cubuğu hareket ettir
    void MoveStick()
    {
        //anımator null gelmişmi bir doğrulama
        if (anim != null)
            //Base Layer daki HalfDonut stateini oynat
        anim.Play("Base Layer.HalfDonut", 0, 0);
    }
}
