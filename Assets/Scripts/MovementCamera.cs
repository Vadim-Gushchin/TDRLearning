using UnityEngine;

public class MovementCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    public Vector3 _offset;

    private void Update()
    {
        transform.position = _playerTransform.position + _offset;
    }
}
