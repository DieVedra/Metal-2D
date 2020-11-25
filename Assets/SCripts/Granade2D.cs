using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade2D : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sR;
    [SerializeField] private float _radius;
    [SerializeField] private float _force;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeExplode;
    [SerializeField] private float _damageGranade;
    [SerializeField] private float _timeDestroy;
    [SerializeField] private float _checkRadiusCollision;
    private bool _isGround;
    private bool _isEnemy;
    private bool _playCount = true;
    [SerializeField] private LayerMask _layerMaskEnemy;
    [SerializeField] private LayerMask _layerMaskCollision;
    [SerializeField] private AudioClip[] _audioClip; // _audioClip[0] - createsound, _audioClip[1] - collision sound,  _audioClip[2] - expload sound
    private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _pS;

    [ContextMenu("ExplosionGranate2D()")]

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _sR = GetComponent<SpriteRenderer>();

        if (PlayerController.instance.BulletShotUp)
        {
            _speed *= 2f;
        }
        _rb.AddForce(transform.right * _speed);

        StartCoroutine(ExplodeTimer());
    }

    private void Update()
    {
        _isGround = Physics2D.OverlapCircle(transform.position, _checkRadiusCollision, _layerMaskCollision);
        _isEnemy = Physics2D.OverlapCircle(transform.position, _checkRadiusCollision, _layerMaskEnemy);

        if (_playCount && _isGround)
        {
            SoundCollisions();
        }

        if (!_isGround)
        {
            _playCount = true;
        }

        if (_isEnemy)
        {
            ExplosionGranate2D();
        }

    }
    private void SoundCollisions()
    {
        if (_sR.enabled == true)
        {
            _audioSource.clip = _audioClip[1];
            _audioSource.Play();

            _playCount = false;
        }
    }
    private IEnumerator ExplodeTimer()
    {
        _audioSource.clip = _audioClip[0];
        _audioSource.Play();

        yield return new WaitForSeconds(_timeExplode);

        ExplosionGranate2D();
    }
    private void ExplosionGranate2D()
    {
        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, _radius, _layerMaskEnemy);

        foreach (Collider2D hit in overlappedColliders)
        {
            if (hit.attachedRigidbody != null)
            {
                Vector3 direction = hit.transform.position - transform.position;

                direction.z = 0;

                hit.attachedRigidbody.AddForce(direction.normalized * _force);

                hit.GetComponent<EnemyControl>().TakeDamage(_damageGranade);
            }
        }
        Instantiate(_pS, transform.position, Quaternion.Euler(0, 0, 0));
        _audioSource.clip = _audioClip[2];
        _audioSource.Play();

        _sR.enabled = false;

        StartCoroutine(DestroyTimer());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _checkRadiusCollision);
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(_timeDestroy);
        Destroy(gameObject);
    }
}
