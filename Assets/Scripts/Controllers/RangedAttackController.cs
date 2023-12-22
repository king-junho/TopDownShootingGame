using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangedAttackData _attackData;
    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;
    private ProjectileManager _projectileManager;

    public bool fxOnDestroy = true;
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();

    }
    private void Update()//이동처리
    {
        if(!_isReady)
        {
            return;
        }
        _currentDuration += Time.deltaTime;
        if(_currentDuration > _attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }
        _rigidbody.velocity = _direction * _attackData.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((levelCollisionLayer.value==(levelCollisionLayer.value|(1<<collision.gameObject.layer))))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position)-_direction*.2f,fxOnDestroy);
        }
        else if (_attackData.target.value == (_attackData.target.value | 1 << collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if(healthSystem != null)
            {
                healthSystem.ChangeHealth(-_attackData.power);
                if (_attackData.isOnKnockback)
                {
                    TopDownMovement movement = collision.GetComponent<TopDownMovement>();
                    if(movement!=null)
                    {
                        movement.ApplyKnockBack(transform, _attackData.knockbackPower, _attackData.knockbackTime);
                    }
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }
    public void InitalizeAttack(Vector2 direction, RangedAttackData attackData, ProjectileManager projectileManager)//초기화
    {
        _projectileManager = projectileManager;
        _attackData = attackData;
        _direction = direction;

        UpdateProjectileSprite();
        _trailRenderer.Clear();
        _currentDuration = 0;
        _spriteRenderer.color = attackData.projectileColor;

        transform.right = _direction;

        _isReady = true;
    }
    private void UpdateProjectileSprite()//초기화
    {
        transform.localScale = Vector2.one * _attackData.size;

    }
    private void DestroyProjectile(Vector3 position, bool createFx)//삭제
    {
        if(createFx)
        {
            _projectileManager.CreateImpactParticlelesAtPosition(position, _attackData);

        }
        gameObject.SetActive(false);
    }
}
