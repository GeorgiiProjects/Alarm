using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _changeVolume;
    [SerializeField] private int _stepVolume;

    private AudioSource _audioSource;
    private Coroutine _alarmWork;
    private Coroutine _alarmReduce;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            StopCoroutine(DecreaseSound());
            _alarmWork = StartCoroutine(ActivatingAlarm());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopCoroutine(_alarmWork);
        _alarmReduce = StartCoroutine(DecreaseSound());
    }

    private IEnumerator ActivatingAlarm()
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            _audioSource.Play();

            yield return new WaitForSeconds(0.5f);

            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _maxVolume, _changeVolume * Time.deltaTime);
        }
    }

    private IEnumerator DecreaseSound()
    {
        for (int j = 0; j < _stepVolume; j++)
        {
            yield return new WaitForSeconds(0.5f);

            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _maxVolume, -_changeVolume * Time.deltaTime);
        }
    }
}
