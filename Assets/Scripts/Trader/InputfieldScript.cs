using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class InputfieldScript : MonoBehaviour
{
    private TMP_InputField inputField;
    private int MinimumNumber;
    public ItemScript itemScript;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(ValidateInput); //add a listener
        if (MinimumNumber < 1)
            MinimumNumber = 0;
    }

    private void ValidateInput(string value) //as the value changes it will check this
    {
        if (value == "") //if the user removes all fields
        {
            inputField.text = MinimumNumber.ToString();
            return;
        }

        int parsedValue = int.Parse(value); //change value to an int

        //Debug.Log(parsedValue + " " + MinimumNumber);

        if (parsedValue >= MinimumNumber && parsedValue <= 5) //if its within the fields
        {
            itemScript.ChangeItemTotalCost(parsedValue - MinimumNumber); //change the item total cost (display it to user)
            return;
        }
        else if (parsedValue < MinimumNumber)
        {
            inputField.text = MinimumNumber.ToString();
        }
        else if (parsedValue > 5)
        {
            inputField.text = "5";
        }
    }

    public void SetMinimumNumber(int Number)
    {
        MinimumNumber = Number;
    }
}