using UnityEngine;
using System.Collections;

public class AnimEventCall : MonoBehaviour {

    public delegate void _OnAttacking();
    public _OnAttacking onAttacking; 

    public void OnAnimAttack()
    { 
        if (onAttacking != null)
        { 
	        onAttacking(); 
        }
    } 
    
}
