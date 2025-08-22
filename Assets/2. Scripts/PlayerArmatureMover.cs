using StarterAssets;
using TMPro;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArmatureMover : NetworkBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private StarterAssetsInputs starterAsset;
    [SerializeField] private ThirdPersonController controller;
    [SerializeField] private Transform playerRoot;

    [SerializeField] private GameObject bombPrefab;

    private NetworkVariable<int> score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private TextMeshProUGUI scoreTextUI;

    private void Awake()
    {
        cc.enabled = false;
        playerInput.enabled = false;
        starterAsset.enabled = false;
        controller.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            cc.enabled = true;
            playerInput.enabled = true;
            starterAsset.enabled = true;
            controller.enabled = true;

            var cinemachine = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineCamera>();
            cinemachine.Target.TrackingTarget = playerRoot;
        }
    }


    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddScoreServerRpc();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            ThrowBombServerRpc();
        }
    }

    [ServerRpc]
    private void ThrowBombServerRpc()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }



    [ServerRpc] // Netcode Server 형식으로 값을 올려주고 싶을때 사용
    private void AddScoreServerRpc()
    {
        ScoreManager.Instance.AddScore();
    }

}
