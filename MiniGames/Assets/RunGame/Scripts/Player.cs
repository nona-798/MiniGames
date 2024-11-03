using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState { Run = 0, Jump, Hit }
public class Player : MonoBehaviour
{
    [Header("KeyCode")]
    [SerializeField]
    private KeyCode keyCodeJump;

    [Header("Status")]
    [SerializeField]
    private float jumpPower;
    private bool isGround;
    private bool isDouble;

    private float blinkTime = 2.0f;

    private Rigidbody2D rigid;
    private PlayerAnimatorController anim;

    
    
    // ��ֹ� ��ġ ( Ʈ���� �浹 �̺�Ʈ )
    
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimatorController>();
        isGround = true;
        isDouble = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(keyCodeJump) && isGround)
        {
            GetJump();
        }
        if (Input.GetKeyDown(keyCodeJump) && !isDouble && !isGround)
        {
            GetdoubleJump();
        }
    }

    private void GetJump()
    {
        // ���� ( ���� �Ŀ� )
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    private void GetdoubleJump()
    {
        isDouble = true;
        // ���� ( ���� �Ŀ� )
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private IEnumerator AfterHit()
    {
        blinkTime += Time.deltaTime;
        Color temp = new Color(1, 1, 1, 1);
        this.gameObject.GetComponentInChildren<SpriteRenderer>().color = temp;
        if (blinkTime < 1.0f)
        {
            temp.a -= 0.1f;
        }
        else if (blinkTime <2.0f)
        {
            temp.a += 0.1f;
        }
        yield return new WaitForSeconds(2.0f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        rigid.simulated = false;
        // �ִϸ��̼�
        anim.PlayAnimation(PlayerState.Hit);
        // ����
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        // ���� ( ���� �浹 �̺�Ʈ )
        isGround = true;
        isDouble = false;
        // �ִϸ��̼�
        anim.PlayAnimation(PlayerState.Run);
        // ����
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        // ���� ( ���� �浹 �̺�Ʈ )
        isGround = false;
        // �ִϸ��̼�
        anim.PlayAnimation(PlayerState.Jump);
        // ����
    }
}
