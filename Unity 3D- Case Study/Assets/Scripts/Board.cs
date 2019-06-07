using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public PlayState playState;
    public int width,height,offset;
    private MatchFinder matchFinder;
    public GameObject[] dots;
    public GameObject[,] allDots;
    public GameObject DestoryEffect;
    

    // Start is called before the first frame update
    void Start()
    {
        playState = PlayState.Move;
        matchFinder = GameObject.FindGameObjectWithTag(Tags.MatchFinder).GetComponent<MatchFinder>();
        allDots = new GameObject[width, height];
        BoardSetup();
    }
 

    void BoardSetup()
    {
        for(int i =0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                DotsInsert(i,j,gameObject);
            }
        }
    }
    GameObject newdot;
    void DotsInsert(int i,int j,GameObject parent)
    {
        //int dotTouse = Random.Range(0, dots.Length);
        
        
        int dotnum = Random.Range(0, 100);


        if (dotnum <= 50)
        {
            newdot = Instantiate(dots[0], parent.transform.position, Quaternion.identity);
        }
        if (dotnum > 50 && dotnum <= 70)
        {
            newdot = Instantiate(dots[1], parent.transform.position, Quaternion.identity);
        }
        if (dotnum > 70 && dotnum <= 80)
        {
            newdot = Instantiate(dots[2], parent.transform.position, Quaternion.identity);
        }
        if (dotnum > 80 && dotnum <= 90)
        {
            newdot = Instantiate(dots[3], parent.transform.position, Quaternion.identity);
        }
        if (dotnum > 90 && dotnum <= 100)
        {
            newdot = Instantiate(dots[Random.Range(4, dots.Length)], parent.transform.position, Quaternion.identity);
        }

        newdot.GetComponent<Dots>().row = j;
        newdot.GetComponent<Dots>().column = i;
        newdot.transform.parent = transform;
        newdot.name = "(" + i + "," + j + ")";
        allDots[i, j] = newdot;
       
    }

    public void NoMatchsFound()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ReArrangeMatches(i, j);
            }
        }
    }

    void ReArrangeMatches(int i,int j)
    {
       // GameObject newdot;
       // int dotTouse;// = Random.Range(0, dots.Length);
        int maxLoop = 0;
        while (!MatchAt(i, j, allDots[i,j]) && maxLoop <300)
        {
            Destroy(allDots[i, j]);
            maxLoop++;
            
        }
        maxLoop = 0;

        DestoryMatchAt(i, j);
        DestoryMatch();
        
    }


    
    bool MatchAt(int col,int row,GameObject currentDot)
    {
        if(col>1&& row>1)
        {
            if(allDots[col-1,row].tag==currentDot.tag && allDots[col-2,row].tag==currentDot.tag)
            {
                return true;
            }
            
            if (allDots[col, row-1].tag == currentDot.tag && allDots[col, row - 2].tag == currentDot.tag)
            {
                return true;
            }
        }
        else if(col<=1||row<=1)
        {
            if(row>1)
            {
                if (allDots[col, row - 1].tag == currentDot.tag && allDots[col , row - 2].tag == currentDot.tag)
                {
                    return true;
                }
            }
            if (col > 1)
            {
                if (allDots[col - 1, row ].tag == currentDot.tag && allDots[col - 2, row ].tag == currentDot.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }


    private void DestoryMatchAt(int col,int row)
    {
        if(allDots[col,row].GetComponent<Dots>().isMatched)
        {
            matchFinder.currentDots.Remove(allDots[col, row]);
            GameObject destoryeffect = Instantiate(DestoryEffect, allDots[col, row].transform.position, Quaternion.identity);
            ParticleSystem pr0 =
            destoryeffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            pr0.startColor = allDots[col, row].GetComponent<SpriteRenderer>().color;
            ParticleSystem pr1 =
            destoryeffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
            pr1.startColor = allDots[col, row].GetComponent<SpriteRenderer>().color;
            Destroy(destoryeffect, 0.2f);
            Destroy(allDots[col,row]);
            allDots[col, row] = null;
        } 
    }

    public void DestoryMatch()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if(allDots[i,j]!=null)
                {
                    DestoryMatchAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCol());
    }

    private IEnumerator DecreaseRowCol()
    {
        int numCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    numCount++;
                }
                else if(numCount>0)
                {
                    allDots[i, j].GetComponent<Dots>().row -= numCount;
                    allDots[i, j] = null;
                }
            }
            numCount = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FillDotsOnBoard());
    }

    private IEnumerator FillDotsOnBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);
        while (MatchesDotsOnBoard())
        {
            yield return new WaitForSeconds(.4f);
            DestoryMatch();
        }
        yield return new WaitForSeconds(.5f);
        playState = PlayState.Move;
        Debug.Log(playState);
    }

    void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 pos = new Vector2(i, j+offset);
                    int num = Random.Range(0, dots.Length);
                    DotsInsert(i, j, gameObject);
                }
            }
        }
    }//RefillBoard

    private bool MatchesDotsOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j]!= null)
                {
                   if(allDots[i, j].GetComponent<Dots>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }//MatcheDotsOnBoard


    

}//class




































































































































































