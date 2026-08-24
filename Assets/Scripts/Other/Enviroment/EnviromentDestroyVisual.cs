using NUnit.Framework;
using UnityEngine;

public class EnviromentDestroyVisual : MonoBehaviour
{
    [SerializeField] private EnviromentDestroy _eviromentDestroy;
    [SerializeField] private GameObject _bushDeathVFXPrefab;
    private GameObject _currentVFX;


      private void Start()
    {
        _eviromentDestroy.OnEnviromentGotHit += EnviromentDestroy_OnEnviromentGotHit;
    }

    private void EnviromentDestroy_OnEnviromentGotHit(object sender, System.EventArgs e)
    {
        _currentVFX = Instantiate(_bushDeathVFXPrefab, transform.position, Quaternion.identity);
        _currentVFX.GetComponent<ParticleSystem>().Play();
        Destroy(_currentVFX,_currentVFX.GetComponent<ParticleSystem>().main.duration);
    }

    private void OnDestroy()
    {
        _eviromentDestroy.OnEnviromentGotHit -= EnviromentDestroy_OnEnviromentGotHit;
    }
}
