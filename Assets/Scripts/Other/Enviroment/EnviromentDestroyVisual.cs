using NUnit.Framework;
using UnityEngine;

public class EnviromentDestroyVisual : MonoBehaviour
{
    [SerializeField] private EnviromentDestroy eviromentDestroy;
    [SerializeField] private GameObject bushDeathVFXPrefab;
    private GameObject _currentVFX;


      private void Start()
    {
        eviromentDestroy.OnEnviromentGotHit += EnviromentDestroy_OnEnviromentGotHit;
    }

    private void EnviromentDestroy_OnEnviromentGotHit(object sender, System.EventArgs e)
    {
        _currentVFX = Instantiate(bushDeathVFXPrefab, transform.position, Quaternion.identity);
        _currentVFX.GetComponent<ParticleSystem>().Play();
        Destroy(_currentVFX,_currentVFX.GetComponent<ParticleSystem>().main.duration);
    }

    private void OnDestroy()
    {
        eviromentDestroy.OnEnviromentGotHit -= EnviromentDestroy_OnEnviromentGotHit;
    }
}
