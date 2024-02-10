using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animation anim;
   
    // Start is called before the first frame update
    void Start()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 
        
        if (anim == null && !anim.isPlaying) { 
            
            anim.Play("Kirby_Jumping");
            }
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        


    }
}
