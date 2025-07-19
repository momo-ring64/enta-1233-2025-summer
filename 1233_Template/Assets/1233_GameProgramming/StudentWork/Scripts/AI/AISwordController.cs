using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISwordController : BaseAIController
{

    [Range(0.001f, 0.1f)] [SerializeField] private float StillThreshold = 0.05f;
   
    [SerializeField] private Rigidbody CharacterRigidBody;
    [SerializeField] private float KnockbackStrength = 5.0f;
 
    private Coroutine MoveCoroutine;
    //will get knockbacked when shot at


    public void GetKnockedBack(Vector3 force)
    {
        StopCoroutine(MoveCoroutine);
        MoveCoroutine = StartCoroutine(ApplyKnockback(force));
    }
    private IEnumerator ApplyKnockback(Vector3 force)
    {
        yield return null;
        Agent.enabled = false;
        CharacterRigidBody.useGravity = true;
        CharacterRigidBody.isKinematic = false;
        CharacterRigidBody.AddForce(force);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => CharacterRigidBody.velocity.magnitude < StillThreshold);
        yield return new WaitForSeconds(0.25f);

        CharacterRigidBody.velocity = Vector3.zero;
        CharacterRigidBody.angularVelocity = Vector3.zero;
        CharacterRigidBody.useGravity = false;
        CharacterRigidBody.isKinematic = true;
        Agent.Warp(transform.position);
        Agent.enabled = true;

        yield return null;





    }


    protected override void ReactToDamage()
    {
        StartCoroutine(ApplyKnockback(new Vector3(KnockbackStrength, KnockbackStrength, KnockbackStrength)));
    }
}
