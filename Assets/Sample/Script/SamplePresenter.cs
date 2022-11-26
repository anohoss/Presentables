using UnityEngine;
using anoho.Presentables;
using UnityEngine.UI;
using System.Collections;
using System;

public class SamplePresenter : Presenter {

    [SerializeField]
    private Text _header;

    [SerializeField]
    private Text _recommendation;

    public void Show() {
        _header.enabled = true;
        _recommendation.enabled = true;
    }

    public void Hide() {
        _header.enabled = false;
        _recommendation.enabled = false;
    }

    internal override IEnumerator Instantiate(Action<Presenter> onCompleted) {
        var request = Resources.LoadAsync<SamplePresenter>("Sample");
        yield return request;

        var asset = request.asset as SamplePresenter;
        if(asset == null) {
            onCompleted(null);
            yield break;
        }

        var instance = Instantiate(asset);
        onCompleted(instance);
        yield break;
    }
}
