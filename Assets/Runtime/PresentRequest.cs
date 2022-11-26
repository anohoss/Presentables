using anoho.Presentables;
using System;
using System.Collections;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class PresentRequest: IEnumerator {
    private Presenter _result = null;

    public Presenter Result => _result;



    private bool _isDone = false;

    public bool IsDone => _isDone;



    internal PresentRequest(PresentableCanvas canvas, Type type) {
        CoroutineRunner.Instance.StartCoroutine(Execute(canvas, type, res => {
            _result = res;
            _isDone = true;
        }));
    }



    private IEnumerator Execute(PresentableCanvas canvas, Type type, Action<Presenter> onCompleted) {
        Presenter presenter = null;
        yield return CreatePresenter(type, res => {
            presenter = res;
        });

        if(presenter == null) {
            onCompleted(null);
            yield break;
        }

        SetPresenterOnCanvas(canvas, presenter);
        onCompleted(presenter);
        yield break;
    }



    private IEnumerator CreatePresenter(Type type, Action<Presenter> onCompleted) {
        if (type == null) {
            onCompleted(null);
            yield break;
        }

        Presenter creator = new GameObject().AddComponent(type) as Presenter;

        if (creator == null) {
            Debug.LogError($"{type.Name} is not inherited from {nameof(MonoBehaviour)}");
            onCompleted(null);
            yield break;
        }

        Presenter instance = null;
        yield return creator.Instantiate(res => {
            instance = res;
        });

        UnityObject.Destroy(creator.gameObject);

        onCompleted(instance);
        yield break;
    }



    private void SetPresenterOnCanvas(PresentableCanvas canvas, Presenter presenter) {
        var presenterCanvas = CreateCanvasForPresenter(canvas);
        presenterCanvas.name = $"{presenter.name} Canvas";
        presenter.transform.SetParent(presenterCanvas.transform, false);
        presenter._canvas = presenterCanvas;

        RectTransform rectTransform = presenter.GetComponent<RectTransform>();

        // set anchor to streatch
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }



    private Canvas CreateCanvasForPresenter(PresentableCanvas parent) {
        GameObject go = new GameObject($"Canvas");
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.gameObject.layer = parent.gameObject.layer;
        canvas.transform.SetParent(parent.transform, false);

        RectTransform rectTransform = canvas.transform as RectTransform;

        // set anchor to streatch
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        return canvas;
    }



    object IEnumerator.Current => Result;



    bool IEnumerator.MoveNext() {
        return !IsDone;
    }



    void IEnumerator.Reset() { }
}


