using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SudokuSpace : MonoBehaviour {

    public bool[] numPossible = new bool[9];
    public int correctAnswer, spaceNum, boxNum;

    public Text answerText, hintText;

    public GameController controller;

    public void SelectSpace()
    {
        controller.StartEdit(spaceNum);
    }

    public void SetAnswer(int iIn)
    {
        correctAnswer = iIn;

        NullifynumPossible();

        answerText.text = iIn.ToString();
        hintText.text = "";
    }

    private void NullifynumPossible()
    {
        for( int i = 0; i < numPossible.Length; i++)
        {
            numPossible[i] = false;
        }
    }

    public void CreateHintText()
    {
        string[] tempString = new string[9];

        if(correctAnswer > 0)
        {
            hintText.text = "";
            return;
        }

        for( int i = 0; i < tempString.Length; i++)
        {
            tempString[i] = numPossible[i] ? (i +1).ToString() : "X";
        }

        hintText.text = tempString[0] + " " + tempString[1] + " " + tempString[2] + "\n"
                      + tempString[3] + " " + tempString[4] + " " + tempString[5] + "\n"
                      + tempString[6] + " " + tempString[7] + " " + tempString[8];
    }

    public void ResetSpace()
    {
        hintText.text = "";
        correctAnswer = 0;

        answerText.text = "";

        for (int i = 0; i < numPossible.Length; i++)
        {
            numPossible[i] = true;
        }

        GetComponentInParent<Button>().interactable = true;

    }

    public void SoftResetSpace()
    {
        answerText.text = "";
        hintText.text = "";


        for (int i = 0; i < numPossible.Length; i++)
        {
            numPossible[i] = true;
        }

    }

}
