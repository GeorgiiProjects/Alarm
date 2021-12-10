using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float _alarmVolume;  
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _changeVolume;
    [SerializeField] private int _stepVolume;

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
            _audioSource.Stop();
        }
    }

    private IEnumerator ActivatingAlarm()
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            StartSignal();

            yield return new WaitForSeconds(0.5f);

            _alarmVolume = Mathf.MoveTowards(_alarmVolume, _maxVolume, _changeVolume * Time.deltaTime);

            if (_alarmVolume >= _maxVolume)
            {
                for (int j = 0; j < _stepVolume; j++)
                {
                    ReduceSignal();

                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }

    private void StartSignal()
    {       
        _audioSource.volume = _alarmVolume;
        _audioSource.Play();     
    }

    private void ReduceSignal()
    {
        _alarmVolume = Mathf.MoveTowards(_alarmVolume, _maxVolume, -_changeVolume * Time.deltaTime);
        _audioSource.volume = _alarmVolume;
        _audioSource.Play();
    }
}
