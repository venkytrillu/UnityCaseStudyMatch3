using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentDots = new List<GameObject>();
    private void Start()
    {
        board = GameObject.FindGameObjectWithTag(Tags.Board).GetComponent<Board>();
    }

    public void FindAllMatch()
    {
        StartCoroutine(FindAllMatches());
    }

    IEnumerator FindAllMatches()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i <board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if(currentDot!=null)
                {
                    if(i>0 && i< board.width-1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot!=null && rightDot !=null)
                        {
                            if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<Dots>().isRowBoom
                                    || leftDot.GetComponent<Dots>().isRowBoom
                                   || rightDot.GetComponent<Dots>().isRowBoom)
                                {
                                    currentDots.Union(GetRowPieces(j));
                                }

                                if (currentDot.GetComponent<Dots>().isColumnBoom)
                                {
                                    currentDots.Union(GetColumnPieces(i));
                                }
                                if (leftDot.GetComponent<Dots>().isColumnBoom)
                                {
                                    currentDots.Union(GetColumnPieces(i-1));
                                }
                                if (rightDot.GetComponent<Dots>().isColumnBoom)
                                {
                                    currentDots.Union(GetColumnPieces(i+1));
                                }

                                if (!currentDots.Contains(leftDot))
                                {
                                    currentDots.Add(leftDot);
                                }
                                leftDot.GetComponent<Dots>().isMatched = true;
                                if (!currentDots.Contains(rightDot))
                                {
                                    currentDots.Add(rightDot);
                                }
                                rightDot.GetComponent<Dots>().isMatched = true;
                                if (!currentDots.Contains(currentDot))
                                {
                                    currentDots.Add(currentDot);
                                }
                                currentDot.GetComponent<Dots>().isMatched = true;
                            }
                        }

                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i,j+1];
                        GameObject downDot = board.allDots[i,j-1];
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<Dots>().isColumnBoom
                                   || upDot.GetComponent<Dots>().isColumnBoom
                                  || downDot.GetComponent<Dots>().isColumnBoom)
                                {
                                    currentDots.Union(GetColumnPieces(i));
                                }

                                if (currentDot.GetComponent<Dots>().isRowBoom)
                                {
                                    currentDots.Union(GetRowPieces(j));
                                }
                                if (upDot.GetComponent<Dots>().isRowBoom)
                                {
                                    currentDots.Union(GetRowPieces(j + 1));
                                }
                                if (downDot.GetComponent<Dots>().isRowBoom)
                                {
                                    currentDots.Union(GetRowPieces(j- 1));
                                }

                                if (!currentDots.Contains(upDot))
                                {
                                    currentDots.Add(upDot);
                                }
                                upDot.GetComponent<Dots>().isMatched = true;
                                if (!currentDots.Contains(downDot))
                                {
                                    currentDots.Add(downDot);
                                }
                                downDot.GetComponent<Dots>().isMatched = true;
                                if (!currentDots.Contains(currentDot))
                                {
                                    currentDots.Add(currentDot);
                                }
                                currentDot.GetComponent<Dots>().isMatched = true;
                            }
                        }

                    }
                }
            }
        }
    }//FindAllMatches


    List<GameObject> GetColumnPieces(int col)
    {
        List<GameObject> dots = new List<GameObject>();
        for(int i = 0;i < board.height;i++)
        {
            if(board.allDots[col,i]!=null)
            {
                dots.Add(board.allDots[col, i]);
                board.allDots[col, i].GetComponent<Dots>().isMatched = true;
            }
        }
        return dots;
    }//GetColumnPieces

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dots>().isMatched = true;
            }
        }
        return dots;
    }//GetRowPieces

}//class













































































































































