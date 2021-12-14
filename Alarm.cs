using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{   
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _changeVolume;
    [SerializeField] private int _stepVolume;

    private float _volume;

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
            _alarmWork = StartCoroutine(ActivatingAlarm());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            StopCoroutine(_alarmWork);
            StartCoroutine(DecreaseSound());
        }
    }

    private IEnumerator ActivatingAlarm()
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            StartSignal();

            yield return new WaitForSeconds(0.5f);

            _volume = Mathf.MoveTowards(_volume, _maxVolume, _changeVolume * Time.deltaTime);
        }
    }

    private IEnumerator DecreaseSound()
    {
        for (int j = 0; j < _stepVolume; j++)
        {
            ReduceSignal();

            yield return new WaitForSeconds(0.5f);

            _volume = Mathf.MoveTowards(_volume, _maxVolume, -_changeVolume * Time.deltaTime);
        }
    }

    private void StartSignal()
    {
        _audioSource.volume = _volume;
        _audioSource.Play();
    }

    private void ReduceSignal()
    {
        _audioSource.volume = _volume;
    }
}
