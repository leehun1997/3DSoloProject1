using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    [Header("ForMove")]
    public float movementSpeed;
    private Vector2 curMovementInput;

    [Header("ForLook")]
    public Transform cameraContainer;
    public float cameraRotateSpeed;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot; //���Ʒ��� ȸ���� �÷��̾ ���� �� ���� ������
    private Vector2 mouseDelta;

    [Header("ForJump")]
    public float jumpPower;

    private Rigidbody rb;
    private Camera camera;
    public LayerMask groundLayerMask;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //���� �� ���콺 Ŀ���� �Ⱥ��̰� ���ִ� ��
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; //�Է������� Vector2�� �ް� ������ y, �������� x �̱⶧����
        dir *= movementSpeed;

        dir.y = rb.velocity.y; //�̵��� ���� ���ϴ� ���� ���X Vector3���� ������ z, ������ x, ����� y��� ���� �׻� ���
        rb.velocity = dir;//velocity�� ��ȭ���̴�. �����̳� �ٸ� ���� ������� �Է¹��� ������ ���� ��ȭ�����ִ� ���̴�.
    }

    void Look()
    {
        camCurXRot += mouseDelta.y * cameraRotateSpeed; //�� �Ʒ� ȸ�� ���
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);//������ �� �������� ȸ��
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); //ȸ����Ű�� ���̱� ������ 

        transform.eulerAngles += new Vector3(0, mouseDelta.x * cameraRotateSpeed, 0); //�¿� ȸ�� ��� +�� ������ delta ������ �������� ��ȭ�� �ֱ� ������ ���� �Ҵ��ϴ°� �ƴ� �����ִ� ���̴�.
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && isGround())
        {
            GameManger.Instance.Player.PlayerGage.controller.StaminaGage.ChangeGage(-10);
            rb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);//�� ForceMode�� �ȳ����� �۵��� ���ϴ°�?
        }
    }
    public void OnInteraction(InputAction.CallbackContext context)
    {

    }
    public void OnInventory(InputAction.CallbackContext context)
    {

    }

    bool isGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.forward * -0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f),Vector3.down),
            new Ray(transform.position + (transform.right * -0.2f) + (transform.up * 0.01f),Vector3.down)
        };

        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray, 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
