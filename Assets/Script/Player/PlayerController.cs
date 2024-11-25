using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float _walkSpeed = 10.0f;

    [SerializeField]
    private float _rotateSpeed = 2f;

    private Rigidbody _rigidbody;

    private float _pitchAngle;

    private GameObject _mainCamera;
    public GameObject MainCamera
    {
        get => _mainCamera;
    }

    private IInteractable _interactableOnPointer = null;
    public IInteractable InteractableOnPointer
    {
        get => _interactableOnPointer;
        set => _interactableOnPointer = value;
    }

    [SerializeField]
    private IInteractable _interactableInHand = null;
    public IInteractable InteractableInHand
    {
        get => _interactableInHand;
        set => _interactableInHand = value;
    }

    public override void OnNetworkSpawn()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //transform.position = SpawnManager.Instance.GetPlayerSpawnPosition(NetworkManager.Singleton.IsHost);
        if (IsOwner)
        {
            _mainCamera = new GameObject("Main Camera");
            _mainCamera.transform.parent = transform;
            _mainCamera.transform.localPosition = new Vector3(0, 20, 0);
            _mainCamera.AddComponent<Camera>();
            _mainCamera.AddComponent<AudioListener>();
            _mainCamera.tag = "MainCamera";

            Cursor.lockState = CursorLockMode.Locked;
            ClientManager.Instance.SetPlayer(this, true);
        }
        else
        {
            ClientManager.Instance.SetPlayer(this, false);  
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        CameraHandler();
        IInteractable it = null;

        if (InteractableInHand == null)
        {
            MoveHandler();
            it = FindInteractableObject();
            HighlightControl(it);
        }        

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InteractableInHand == null)
            {
                if (it != null)
                {
                    InteractableInHand = it;
                    it.StartInteraction(this);
                    RopeController ropeController = InteractableInHand as RopeController;
                    ropeController.ShowHighlight();
                }
            }
                
            else
            {
                RopeController ropeController = InteractableInHand as RopeController;
                ropeController.HideHighlight();
                InteractableInHand.StopInteraction(this);
                InteractableInHand = null;

            }

        }
        
    }

    public void HandClear()
    {
        InteractableInHand = null;
        _interactableOnPointer = null;
    }

    private void MoveHandler()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = (v * transform.forward + h * transform.right).normalized * _walkSpeed;
        _rigidbody.velocity = new Vector3(moveDir.x, _rigidbody.velocity.y, moveDir.z);
    }

    private void CameraHandler()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        _pitchAngle = Mathf.Clamp(_pitchAngle + mouseY * _rotateSpeed, -90, 90);

        transform.Rotate(new Vector3(0, mouseX * _rotateSpeed, 0));
        Vector3 cameraRot = _mainCamera.transform.rotation.eulerAngles;
        _mainCamera.transform.rotation = Quaternion.Euler(_pitchAngle, cameraRot.y, cameraRot.z);
    }

    private IInteractable FindInteractableObject()
    {
        if (InteractableInHand != null)
        {
            return null;
        }

        Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit hit);

        if (hit.collider == null)
        {
            return null;
        }

        IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
        
        if (interactable == null)
        {
            return null;
        }

        if (Vector3.Distance(transform.position, hit.collider.transform.position) > 5.0f)
        {
            return null;
        }

        return interactable;
    } 

    private void HighlightControl(IInteractable interactable)
    {
        if (interactable == null)
        {
            
            // Interactable Object가 커서 바깥으로 나갔을 때
            if (_interactableOnPointer != null)
            {
                RopeController ropeController = _interactableOnPointer as RopeController;
                ropeController.HideHighlight();
                _interactableOnPointer = null;
            }
        }
        else
        {
            
            // 새로운 Interactable Object가 커서 안으로 들어 왔을 때
            if (_interactableOnPointer == null)
            {
                _interactableOnPointer = interactable;
                RopeController ropeController = _interactableOnPointer as RopeController;
                ropeController.ShowHighlight();
            }
            else
            {
                if (_interactableOnPointer != interactable)
                {
                    RopeController ropeController = _interactableOnPointer as RopeController;
                    ropeController.HideHighlight();

                    _interactableOnPointer = interactable;
                    ropeController = _interactableOnPointer as RopeController;
                    ropeController.ShowHighlight();
                }
            }
        }
    }
}
