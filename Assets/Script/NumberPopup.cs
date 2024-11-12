using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberPopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField numbersInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button closeButton;

    private void Start() 
    {   
        confirmButton.onClick.AddListener(OnConfirm);
        closeButton.onClick.AddListener(CancelTask);
    }

    private TaskCompletionSource<int[]> _taskCompletionSource;

    public Task<int[]> ShowAsync(CancellationToken cancellationToken)
    {
        _taskCompletionSource = new TaskCompletionSource<int[]>();

        gameObject.SetActive(true);


        return _taskCompletionSource.Task;
    }

    private async void OnConfirm()
    {
      
        string[] inputStrings = numbersInput.text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int[] numbers = inputStrings.Select(int.Parse).ToArray();

        int[] sortedNumbers = await Task.Run(() => numbers.OrderBy(n => n).ToArray());

        _taskCompletionSource.SetResult(sortedNumbers);
    
        Cleanup();
        
    }

    public void CancelTask()
    {
        if (!_taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource.SetCanceled(); 
        }
        Cleanup();
    }

    private void Cleanup()
    {
        gameObject.SetActive(false);
        numbersInput.text = "";
    }
}