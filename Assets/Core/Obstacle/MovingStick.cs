using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStick : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private string AnimName ="HalfDonut";
    [SerializeField] private float Time = 2f;
    [SerializeField] private float repeatRate = 2f;
    void Start()
    {
        anim = GetComponent<Animator>();
        //2sn de bir tekrarı tetikle
        InvokeRepeating("Move", Time, repeatRate);
    }
    
    //Cubuğu hareket ettir
    void Move()
    {
        //anımator null gelmişmi bir doğrulama
        if (anim != null)
            //Base Layer daki Animasyon stateini oynat
        anim.Play("Base Layer." + AnimName, 0, 0);
    }
}
