using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speedRate = 8f;

    [SerializeField]
    private GunController _gun;

    //Make sure to attach these Buttons in the Inspector
    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Camera _camera;

    private int _score;

    private Vector3 _initPos;

    private bool _canShoot;

    void Start()
    {
        _canShoot = true;
        _score = 0;
        _initPos = transform.localPosition;

        EventManager.onShooted += AddScore;
        EventManager.onRestart += ResetObject;

        _restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Jump");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, y, z);
        transform.Translate(movement * _speedRate * Time.deltaTime);

        //ËÊÌ
        if (Input.GetMouseButtonDown(0) && _canShoot)
        {
            Vector3 mousePos = Input.mousePosition;
            var ballPos = _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
            _gun.Shoot(ballPos);
        }
    }

    private void AddScore()
    {
        Debug.Log("+1");
        _score++;
        Debug.Log(_score);

        if (_score >= 3)
        {
            EventManager.RaiseGameOver();
            _canShoot = false;
        }
    }

    private void ResetObject()
    {
        gameObject.transform.localPosition = _initPos;
        gameObject.transform.rotation = Quaternion.identity;
        _score = 0;
        _canShoot = true;
    }

    void OnRestartButtonClick()
    {
        Debug.Log("Restart");
        EventManager.RaiseRestart();
    }
}
