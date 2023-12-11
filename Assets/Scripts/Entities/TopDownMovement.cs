using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private CharacterStatsHandler _stats;

    private Vector2 _movementDirection = Vector2.zero;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>(); //GetComponent�� inspector �ȿ��� �� ������Ʈ���� �����ϴ� ���
        _rigidbody = GetComponent<Rigidbody2D>();
        _stats = GetComponent<CharacterStatsHandler>();
    }

    private void FixedUpdate()
    {
        ApplyMovement(_movementDirection);
    }
    private void Start()
    {
        _controller.OnMoveEvent += Move;
    }
    private void Move(Vector2 direction)
    {
        _movementDirection = direction;
    }
    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * _stats.CurrentStats.speed;
        _rigidbody.velocity = direction;
    }
}
