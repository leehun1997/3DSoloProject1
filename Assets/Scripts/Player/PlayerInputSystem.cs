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
    private float camCurXRot; //위아래의 회전은 플레이어를 돌릴 수 없기 때문에
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
        Cursor.lockState = CursorLockMode.Locked; //게임 중 마우스 커서가 안보이게 해주는 것
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
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; //입력정보를 Vector2로 받고 정면이 y, 오른쪽이 x 이기때문에
        dir *= movementSpeed;

        dir.y = rb.velocity.y; //이동은 위로 향하는 값고 상관X Vector3에서 정면이 z, 우측이 x, 상단이 y라는 점은 항상 명심
        rb.velocity = dir;//velocity란 변화량이다. 가속이나 다른 값에 상관없이 입력받은 값으로 순간 변화시켜주는 것이다.
    }

    void Look()
    {
        camCurXRot += mouseDelta.y * cameraRotateSpeed; //위 아래 회전 담당
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);//정해준 값 내에서만 회전
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); //회전시키는 것이기 때문에 

        transform.eulerAngles += new Vector3(0, mouseDelta.x * cameraRotateSpeed, 0); //좌우 회전 담당 +인 이유는 delta 값으로 순간적인 변화를 주기 때문에 값을 할당하는게 아닌 더해주는 것이다.
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
            rb.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);//왜 ForceMode를 안넣으면 작동을 안하는가?
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
