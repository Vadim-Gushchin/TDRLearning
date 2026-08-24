using System;
using System.Xml;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class EnviromentDestroy : MonoBehaviour
{
    [SerializeField] private EnviromentSO enviromentSO;
    [SerializeField] private  SelfDestroyVFX _selfDestroyVFX;

    private int _curentHealth;
    public event EventHandler OnEnviromentGotHit;

    private void Start()
    {
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
                NavMeshSurfaceManagment.Instance.RebakeNavMeshSurface();
                
            }
        }
    }
}
