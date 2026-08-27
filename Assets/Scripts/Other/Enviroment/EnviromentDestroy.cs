using System;
using System.Xml;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class EnviromentDestroy : MonoBehaviour
{
    [SerializeField] private EnviromentSO enviromentSO;


    private int _curentHealth;
    public event EventHandler OnEnviromentGotHit;

    private void Start()
    {
        if (enviromentSO == null)
        {
            Debug.LogError("EnviromentSO is not assigned in the Inspector!", gameObject);
            return;
        }
        _curentHealth = enviromentSO.enviromentHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Sword sword))
        {
            OnEnviromentGotHit?.Invoke(this, EventArgs.Empty);
            _curentHealth--;
            if (_curentHealth < 0)
            {
                Destroy(gameObject);
                if (NavMeshSurfaceManagment.Instance != null)
                {
                    NavMeshSurfaceManagment.Instance.RebakeNavMeshSurface();
                }
                else
                {
                    Debug.LogWarning("NavMeshSurfaceManagment.Instance is null!", gameObject);
                }
            }
        }
    }
}
