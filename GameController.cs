using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[System.Serializable]
public class SudokuBox
{
    public GameObject[] box;
}
public class GameController : MonoBehaviour {

    public  GameObject[] sudokuSpace;
    public SudokuBox[] SudokuBoxArray;
    public GameObject sudokuInput, sResetButton;

    private int gameMode = 0;
    private int[] givenAnswers = new int[100];
    private int gaCounter = 0, gaLockedIn = 0;

    public int chosenSpace;
	// Use this for initialization
	void Start () {
	   
        for(int i = 0; i < sudokuSpace.Length; i++)
        {
            sudokuSpace[i].GetComponentInParent<SudokuSpace>().controller = this;
            sudokuSpace[i].GetComponentInParent<SudokuSpace>().spaceNum = i;
            //Debug.Log("the owner for space " + i + " is " + sudokuSpace[i].GetComponentInParent<SudokuSpace>().controller);
        }

        sudokuInput.SetActive(false);// no soft reset untill we lock in for the first time
        sResetButton.GetComponentInParent<Button>().interactable = false;

    }

    public void StartEdit(int iIn)// sets the location of input field over the space we want to edit
    {
        Debug.Log("attempting to start edit from space #" + iIn);

        sudokuInput.transform.position = sudokuSpace[iIn].transform.position;
        sudokuInput.SetActive(true);
        
        chosenSpace = iIn;
    }

    public void RecieveNewInt( Text textIn)// sends the desired number from inut field to the space as the answer.
    {
        int i = int.Parse(textIn.text);
        //int iR = chosenSpace, iC = chosenSpace;

        sudokuSpace[chosenSpace].GetComponentInParent<SudokuSpace>().SetAnswer(i);


        if(gameMode == 0 )//assigns current space to the tracker. if we have already locked in answers will calculate hints
        {
            givenAnswers[gaCounter] = chosenSpace;
            gaCounter++;
        }
        else
        {
            givenAnswers[gaCounter] = chosenSpace;
            gaCounter++;

            CreateHints(chosenSpace, i);
        }
               
        
        
        //sudokuSpace[chosenSpace].GetComponentInParent<SudokuSpace>().CreateHintText();
        //sudokuInput.GetComponentInParent<Text>().text = "";
        sudokuInput.SetActive(false);// hide input field untill we click on next space
    }

    private void CreateHints(int inSpace,int i)// this calculates and shows all hints in the row, column, and box/square thingy for the answer for the chosen space
    {
        int iC = inSpace, iR = inSpace;

        for (int l = 0; l < 9; l++)// will check all spaces in a coloumn
        {
            iC = iC + 9;

            if (iC > 80)
            {
                iC -= 81;
            }
            sudokuSpace[iC].GetComponentInParent<SudokuSpace>().numPossible[i - 1] = false;
            sudokuSpace[iC].GetComponentInParent<SudokuSpace>().CreateHintText();
        }

        iR = iR / 9;

        iR *= 9;


        for (int p = 0; p < 9; p++)//will check all spaces in a row
        {

            sudokuSpace[iR + p].GetComponentInParent<SudokuSpace>().numPossible[i - 1] = false;
            sudokuSpace[iR + p].GetComponentInParent<SudokuSpace>().CreateHintText();
        }
        //following checks all spaces in a given box
        int tempBox = sudokuSpace[inSpace].GetComponentInParent<SudokuSpace>().boxNum;
        for (int bi = 0; bi < 9; bi++)
        {
            SudokuBoxArray[tempBox].box[bi].GetComponentInParent<SudokuSpace>().numPossible[i - 1] = false;
            SudokuBoxArray[tempBox].box[bi].GetComponentInParent<SudokuSpace>().CreateHintText();
        }
    }

    public void ResetGame()// fully resets the board, emptys the tracker and resets counters to zero
    {
        for (int i = 0; i < sudokuSpace.Length; i++)
        {
            sudokuSpace[i].GetComponentInParent<SudokuSpace>().ResetSpace();
        }

        gaCounter = 0;
        gameMode = 0;

        sResetButton.GetComponentInParent<Button>().interactable = false;
    }

    public void LockInGivenSpaces()// makes the answers we have given so far uninteractable and "saved" for any soft resets
    {
        sResetButton.GetComponentInParent<Button>().interactable = true;

        for ( int i = 0; i < gaCounter; i++)
        {
            if (gameMode < 1)
            {
                CreateHints(givenAnswers[i], sudokuSpace[givenAnswers[i]].GetComponentInParent<SudokuSpace>().correctAnswer);
            }
            sudokuSpace[givenAnswers[i]].GetComponentInParent<Button>().interactable = false;
        }
        gameMode++;// will now show hints as we give moer answers on the board

        gaLockedIn = gaCounter;// saves the locked in spaces in the tracker so that they won't get reset
    }

    public void SoftReset()// will set the board to how it was at the last lock in
    {
        Debug.Log(" gacounter = " + gaCounter + ". and gaLockedIn = " + gaLockedIn);
        for (int i = gaLockedIn; i < gaCounter; i++)
        {
            Debug.Log("attempting to preform soft reset");
            sudokuSpace[givenAnswers[i]].GetComponentInParent<SudokuSpace>().ResetSpace();// reset any spaces that we input answers for

            givenAnswers[i] = 0;//empy array of unneeded answers/ answers that no longer exist

        }

        gaCounter = gaLockedIn;//set our tracker and locked in spaces to be the same.

        for (int i = 0; i < sudokuSpace.Length; i++)
        {
            sudokuSpace[i].GetComponentInParent<SudokuSpace>().SoftResetSpace();// empty all texts on the board
        }

        for (int i = 0; i < gaCounter; i++)// re set answer texts and recalulate hints
        {
            sudokuSpace[givenAnswers[i]].GetComponentInParent<SudokuSpace>().SetAnswer(sudokuSpace[givenAnswers[i]].GetComponentInParent<SudokuSpace>().correctAnswer);
            CreateHints(givenAnswers[i], sudokuSpace[givenAnswers[i]].GetComponentInParent<SudokuSpace>().correctAnswer);
        }
    }

    public void ExitGame()
    {
        Debug.Log("exiting application");

        Application.Quit();

    }
}
