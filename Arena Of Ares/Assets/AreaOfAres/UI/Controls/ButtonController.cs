using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace AreaOfAres.UI
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private Vector3 _scaleUp;
        [SerializeField] private Vector3 _scaleDown;
        [SerializeField] private float _scaleTime;
        [SerializeField] private Transform _image;
        [SerializeField] private AudioSource _audio;

        private Coroutine _coroutine;
        private IEnumerator _scaleDownCoroutine;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
            _scaleUp = Vector3.Scale(transform.localScale, new Vector3(1.1f, 1.1f, 1.1f));
            _scaleDown = transform.localScale;
        }

        public void ScaleUp()
        {
            _audio.Play();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ScaleUpDown(_scaleDown, _scaleUp, _scaleTime));
        }

        private IEnumerator ScaleUpDown(Vector3 orginalScale, Vector3 targetScale, float seconds)
        {
            StartCoroutine(Scale(targetScale, seconds));

            yield return new WaitForSeconds(seconds);

            StartCoroutine(Scale(orginalScale, seconds));
        }

        private IEnumerator Scale(Vector3 targetScale, float seconds)
        {
            float scaleTime = seconds;
            float scaleSpeed = Vector3.Distance(transform.localScale, targetScale) * scaleTime;
            while (scaleTime > 0)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / scaleSpeed);
                scaleTime -= Time.deltaTime;

                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}