using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterAction : MonoBehaviour
{
	public bool ground;

	static int hashSpeed = Animator.StringToHash("Speed");
	static int hashFallSpeed = Animator.StringToHash("FallSpeed");
	static int hashGroundDistance = Animator.StringToHash("GroundDistance");
	static int hashIsCrouch = Animator.StringToHash("IsCrouch");

	static int hashDamage = Animator.StringToHash("Damage");
	static int hashDamageTrigger = Animator.StringToHash("DamageTriger");
	public int i = 0;
	public bool downCk;

	[SerializeField] private float characterHeightOffset = 0.2f;
	[SerializeField] LayerMask groundMask;

	[SerializeField, HideInInspector] Animator animator;
	[SerializeField, HideInInspector] SpriteRenderer spriteRenderer;
	[SerializeField, HideInInspector] Rigidbody2D rig2d;
	BoxCollider2D BColl2D;
	public float life;
	public float maxLife;

	public bool downmin = false;
	public int hp = 4;

	void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		rig2d = GetComponent<Rigidbody2D>();
		BColl2D = GetComponent<BoxCollider2D>();
		downCk = true;
	}

	void Update()
	{
		//float axis = Input.GetAxis ("Horizontal");
		//bool isDown = Input.GetAxisRaw ("Vertical") < 0;

		Vector2 velocity = rig2d.velocity;
		if(life > 0)
        {
			if (ground)
			{
				if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.RightShift) && Input.GetButtonDown("Jump")))
				{
					velocity = SmallJump(velocity);
				}
				if (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.W))
				{
					velocity = BigJump(velocity);
				}
			}
			if (downCk)
			{
				if (Input.GetButtonDown("Fire2"))
				{
					animator.SetBool("AgiFlg", true);
					BColl2D.enabled = false;
					SendMessage("DownCheck", SendMessageOptions.DontRequireReceiver);
				}
				else if (Input.GetButtonUp("Fire2"))
				{
					animator.SetBool("AgiFlg", false);
					BColl2D.enabled = true;
				}
				if (BColl2D.enabled == false)
				{
					SendMessage("DownCheck", SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				animator.SetBool("AgiFlg", false);
				BColl2D.enabled = true;
			}
		}

		/*if (axis != 0){
			spriteRenderer.flipX = axis < 0;
			velocity.x = axis * 2;
		}*/
		rig2d.velocity = velocity;


		var distanceFromGround = Physics2D.Raycast(transform.position, Vector3.down, 1, groundMask);

		// update animator parameters
		//animator.SetBool (hashIsCrouch, isDown);
		animator.SetFloat(hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
		animator.SetFloat(hashFallSpeed, rig2d.velocity.y);
		//animator.SetFloat (hashSpeed, Mathf.Abs (axis));

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			i++;
			animator.SetBool("DamageFlg", true);
			StartCoroutine(DamageFlgCheck(i));
			//Debug.Log(animator.GetBool("DamageFlg"));
			float startPosition = 30.5f;
			Transform x = collision.gameObject.GetComponent<Transform>();
			// 通り過ぎた分を加味してポジションを再設定
			float diff = collision.transform.position.x - x.transform.position.x;
			Vector3 restartPosition = collision.transform.position;
			restartPosition.x = startPosition + diff;
			//restartPosition.y = Random.Range(0f, 2.5f);
			//Debug.Log(restartPosition.y);
			//restartPosition.y = 1.0f;
			collision.transform.position = restartPosition;
			//x.transform.position = new Vector3(restartPosition.x, beemy, 0);
			//animator.SetBool("DamageFlg", true);
			//StartCoroutine(DamgeFlgCheck());
			//animator.SetTrigger(hashDamageTrigger);
			life--;
			// 同じゲームオブジェクトにアタッチされているコンポーネントにメッセージを送る
			SendMessage("LifeCheck", SendMessageOptions.DontRequireReceiver);
		}
	}


	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			ground = false;
		}
	}

	private Vector2 BigJump(Vector2 velocity)
	{
		velocity.y = 30;
		return velocity;
	}

	private Vector2 SmallJump(Vector2 velocity)
	{
		velocity.y = 15;
		return velocity;
	}

	public void Click_BigJump()
	{
		Vector2 velocity = rig2d.velocity;
		if (ground)
		{
			velocity = BigJump(velocity);
		}
		rig2d.velocity = velocity;

	}

	public void Click_SmallJump()
	{
		Vector2 velocity = rig2d.velocity;
		if (ground)
		{
			velocity = SmallJump(velocity);
		}
		rig2d.velocity = velocity;
	}

	IEnumerator DamageFlgCheck(int j)
	{
		yield return new WaitForSeconds(1.0f);
		if (j == i)
		{
			animator.SetBool("DamageFlg", false);
		}
	}
}

