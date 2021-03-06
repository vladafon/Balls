using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GunController : MonoBehaviour
{
    [SerializeField]
    private float _shootingForce = 1500;


    [SerializeField]
    private int _bulletPoolSize = 10;

    [SerializeField]
    private int _bulletLifeTime = 10;


    [SerializeField]
    private PhysicMaterial _bulletMaterial;

    [SerializeField]
    private float _bulletDeadY = -10;


    [SerializeField]
    private List<GameObject> _targets;


    private List<GameObject> _bullets;


    public void Shoot(Vector3 bulletPos)
    {
        var bullet = _bullets.FirstOrDefault(c => c.activeInHierarchy == false);

        if (bullet != null)
        {
            bulletPos += new Vector3(0, 0, 0);

            bullet.transform.position = bulletPos;

            bullet.SetActive(true);

            var rb = bullet.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            rb.AddForce(new Vector3(0, 0, _shootingForce * rb.mass));

            var bc = bullet.GetComponent<BulletController>();

            bc.StartLifeTimeCoroutine(_bulletLifeTime);

        }
    }

    void Start()
    {
        BulletPoolInit();

        Debug.Log($"Pool size {_bulletPoolSize}");

        EventManager.onRestart += ResetObject;
    }

    private void BulletPoolInit()
    {
        if (_bullets != null && _bullets.Count > 0)
        {
            foreach(var bullet in _bullets)
            {
                Destroy(bullet);
            }
        }

        _bullets = new List<GameObject>();

        for (int i = 0; i < _bulletPoolSize; i++)
        {
            var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            BulletInit(bullet);
            _bullets.Add(bullet);
        }
    }

    private void BulletInit(GameObject bullet)
    {
        if (!bullet.TryGetComponent<BulletController>(out BulletController bc))
        {
            bc = bullet.AddComponent<BulletController>();
        }

        bc.BulletControllerInit(_targets, _bulletDeadY, _bulletMaterial);
    }

    private void ResetObject()
    {
        BulletPoolInit();
    }
}
