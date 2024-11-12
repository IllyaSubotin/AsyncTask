using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button openPopupButton;
    [SerializeField] private Button closePopupButton;
    [SerializeField] private NumberPopup numberPopup;

    private CancellationTokenSource _cts;

    private void Start()
    {
        openPopupButton.onClick.AddListener(OpenPopup);
        closePopupButton.onClick.AddListener(CancelPopup);
    }

    private async void OpenPopup()
    {
        _cts = new CancellationTokenSource();

        try
        {
            int[] sortedNumbers = await numberPopup.ShowAsync(_cts.Token);
            Debug.Log("Отримано відсортований масив: " + string.Join(", ", sortedNumbers));
        }
        catch (TaskCanceledException)
        {
            Debug.Log("Скасовано");
        }
        finally
        {
            _cts = null;
        }
    }

    private void CancelPopup()
    {
        _cts?.Cancel(); 
        if(_cts != null)
        {
            if(_cts.IsCancellationRequested)
            {
                numberPopup.CancelTask();
            }
        }
    }
}