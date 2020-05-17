using System.Collections;
using UnityEngine;

namespace AreaOfAres.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Credits Canvas")]
        [SerializeField] private Canvas _creditsCanvas;
        [SerializeField] private Vector3 _creditsStartPosition;
        [SerializeField] private Vector3 _creditsTargetPosition;
        [SerializeField] private bool _creditsCanvasShown;

        [Header("Character Canvas")]
        [SerializeField] private Canvas _charactersCanvas;
        [SerializeField] private Vector3 _charactersStartPosition;
        [SerializeField] private Vector3 _charactersTargetPosition;
        [SerializeField] private bool _charactersCanvasShown;

        [Header("Play Canvas")]
        [SerializeField] private Canvas _playCanvas;
        [SerializeField] private Vector3 _playStartPosition;
        [SerializeField] private Vector3 _playTargetPosition;
        [SerializeField] private bool _roomCanvasShown;

        [Header("Room Canvas")]
        [SerializeField] private Canvas _roomCanvas;
        [SerializeField] private Vector3 _roomStartPosition;
        [SerializeField] private Vector3 _roomTargetPosition;
        [SerializeField] private bool _playCanvasShown;

        private Coroutine creditCoroutine;
        private Coroutine charactersCoroutine;
        private Coroutine playCoroutine;
        private Coroutine roomCoroutine;

        private float screenWidth;
        private float screenHeight;

        private void Start()
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            _creditsCanvasShown = false;
            _charactersCanvasShown = false;
            _roomCanvasShown = false;
            _playCanvasShown = false;

            SetPanelPositions();
        }

        private void Update()
        {
            if (screenWidth != Screen.width ||
                screenHeight != Screen.height)
            {
                screenWidth = Screen.width;
                screenHeight = Screen.height;
                SetPanelPositions();
                ShowActivePanel();
            }
        }

        private void SetPanelPositions()
        {
            _creditsStartPosition = new Vector3(-Screen.width * 0.5f, Screen.height * 0.5f, 0);
            _creditsTargetPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            _creditsCanvas.transform.position = _creditsStartPosition;

            _charactersStartPosition = new Vector3(Screen.width * 0.5f, Screen.height * 1.5f, 0);
            _charactersTargetPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            _charactersCanvas.transform.position = _charactersStartPosition;

            _playStartPosition = new Vector3(Screen.width * 1.5f, Screen.height * 0.5f, 0);
            _playTargetPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            _playCanvas.transform.position = _playStartPosition;

            _roomStartPosition = new Vector3(Screen.width * 0.5f, Screen.height * -0.5f, 0);
            _roomTargetPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            _roomCanvas.transform.position = _roomStartPosition;
        }

        private void ShowActivePanel()
        {
            if (_creditsCanvasShown)
            { ShowCreditsCanvas(); }
            if (_charactersCanvasShown)
            { ShowCharactersCanvas(); }
            if (_playCanvasShown)
            { ShowPlayCanvas(); }
            if (_roomCanvasShown)
            { ShowRoomCanvas(); }
        }

        #region Buttons Commands
        public void ExitGame()
        {
            Application.Quit();
        }
        #endregion

        #region Canvas Commands
        public void ShowCreditsCanvas()
        {
            if (creditCoroutine != null)
                StopCoroutine(creditCoroutine);
            creditCoroutine = StartCoroutine(ZoomCanvas(_creditsCanvas, _creditsTargetPosition));
            _creditsCanvasShown = true;
        }
        public void HideCreditsCanvas()
        {
            if (creditCoroutine != null)
                StopCoroutine(creditCoroutine);
            creditCoroutine = StartCoroutine(ZoomCanvas(_creditsCanvas, _creditsStartPosition));
            _creditsCanvasShown = false;
        }

        public void ShowCharactersCanvas()
        {
            if (charactersCoroutine != null)
                StopCoroutine(charactersCoroutine);
            charactersCoroutine = StartCoroutine(ZoomCanvas(_charactersCanvas, _charactersTargetPosition));
            _charactersCanvasShown = true;
        }
        public void HideCharactersCanvas()
        {
            if (charactersCoroutine != null)
                StopCoroutine(charactersCoroutine);
            charactersCoroutine = StartCoroutine(ZoomCanvas(_charactersCanvas, _charactersStartPosition));
            _charactersCanvasShown = false;
        }

        public void ShowPlayCanvas()
        {
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);
            playCoroutine = StartCoroutine(ZoomCanvas(_playCanvas, _playTargetPosition));
            _playCanvasShown = true;
        }
        public void HidePlayCanvas()
        {
            if (playCoroutine != null)
                StopCoroutine(playCoroutine);
            playCoroutine = StartCoroutine(ZoomCanvas(_playCanvas, _playStartPosition));
            _playCanvasShown = false;
        }

        public void ShowRoomCanvas()
        {
            if (roomCoroutine != null)
                StopCoroutine(roomCoroutine);
            roomCoroutine = StartCoroutine(ZoomCanvas(_roomCanvas, _roomTargetPosition));
            _roomCanvasShown = true;
        }
        public void HideRoomCanvas()
        {
            if (roomCoroutine != null)
                StopCoroutine(roomCoroutine);
            roomCoroutine = StartCoroutine(ZoomCanvas(_roomCanvas, _roomStartPosition));
            _roomCanvasShown = false;
        }

        public void HideAllCanvases()
        {
            HideRoomCanvas();
            HidePlayCanvas();
            HideCharactersCanvas();
            HideCreditsCanvas();
        }

        private IEnumerator ZoomCanvas(Canvas canvas, Vector3 target)
        {
            float zoomTime = 3f;
            float startTime = Time.time;
            float moveSpeed = 500.0f;
            float journeyLength;

            journeyLength = Vector3.Distance(canvas.transform.position, target);
            zoomTime = journeyLength / moveSpeed;

            while (Vector3.Distance(canvas.transform.position, target) > 0.1)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;
                canvas.transform.position = Vector3.Lerp(canvas.transform.position, target, fractionOfJourney);
                yield return null;
            }

            canvas.transform.position = target;
        }
        #endregion
    }
}