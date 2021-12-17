using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float _currentVolume;
    [SerializeField] private float _changeVolume;
    [SerializeField] private int _stepVolume;

    private bool _inApartments;

    private AudioSource _audioSource;
    private Coroutine _alarmWork;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (_alarmWork != null)
            {
                StopCoroutine(_alarmWork);
            }
            _inApartments = true;
            _alarmWork = StartCoroutine(RunSignal());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(_alarmWork);
        _inApartments = false;
        _alarmWork = StartCoroutine(RunSignal());
    }

    private IEnumerator RunSignal()
    {
        if (_inApartments)
        {
            _audioSource.Play();
            _currentVolume = 1;
        }
        else
        {
            _currentVolume = 0;
        }

        var waitForSeconds = new WaitForSeconds(0.5f);      

        for (int i = 0; i < _stepVolume; i++)
        {
            yield return waitForSeconds;

            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _currentVolume, _changeVolume * Time.deltaTime);
        }
    }
}
