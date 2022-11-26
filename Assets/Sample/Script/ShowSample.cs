using anoho.Presentables;
using System.Collections;
using UnityEngine;

public class ShowSample : MonoBehaviour
{
    private PresentableCanvas _canvas;

    [SerializeField]
    private CanvasSettingsObject _canvasSettings;

    [SerializeField]
    private CameraSettingsObject _cameraSettings;

    private void Awake() {
        _canvas = Presentables.GetCanvas(_canvasSettings, _cameraSettings);
        StartCoroutine(ShowSamplePresenter());
    }

    public IEnumerator ShowSamplePresenter() {
        var request = _canvas.PresentAsync<SamplePresenter>();
        yield return request;

        var presenter = request.Result as SamplePresenter;
        presenter.Show();
    }
}
