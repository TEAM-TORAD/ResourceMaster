using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;


public class SyncScript : MonoBehaviourPun, IPunObservable
{
    // Sync variables
    public Vector3 objectPosition;
    public Quaternion objectRotation;
    public Vector3 objectScale;
    public float animatorVelocity;
    public bool animatorGrounded;


    private Animator animator;

    public float lerpSpeed = 3.0f;
    public bool online = true;

    private void Start()
    {
        if (GetComponentInChildren<Animator>() != null) animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (online)
        {
            if (!photonView.IsMine) UpdateTransform();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.transform.position);
            stream.SendNext(gameObject.transform.rotation);
            stream.SendNext(gameObject.transform.localScale);
            stream.SendNext(animator.GetFloat("Velocity"));
            stream.SendNext(animator.GetBool("isGrounded"));
        }
        else if (stream.IsReading)
        {
            //Same order as above in stream.IsWriting!
            objectPosition = (Vector3)stream.ReceiveNext();
            objectRotation = (Quaternion)stream.ReceiveNext();
            objectScale = (Vector3)stream.ReceiveNext();
            animatorVelocity = (float)stream.ReceiveNext();
            animatorGrounded = (bool)stream.ReceiveNext();
        }
    }
    private void UpdateTransform()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, objectPosition, lerpSpeed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, objectRotation, lerpSpeed * Time.deltaTime);
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, objectScale, lerpSpeed * Time.deltaTime);
        animator.SetFloat("Velocity", animatorVelocity);
        animator.SetBool("isGrounded", animatorGrounded);
    }
    private void UpdateAnimator()
    {
        //animator.SetFloat("Move", )
    }
}
