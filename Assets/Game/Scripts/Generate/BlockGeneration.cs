using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGeneration : MonoBehaviour
{
    [Header("Status")]
    public TypeEndGame typeEndGame;
    public Level CurrentLevel;
    public float CurrentZ;
    public float CurrentY;
    public float FinishZ;

    [Header("References")]
    public Block startGround_Prefab;
    public Block[] obtacle_prefab;

    public Block_Default block_Nuoc_Prefab;
    public Block_Default block_Ho_Prefab;
    public Block_Default block_Tuong_Prefab;
    public Block_Default block_Cloud_Prefab;
    public Block_Default block_Up_Prefab;
    public Block_Default block_Nhan_Prefab;
    public Block_Default block_Trap_Prefab;

    public Block_Default endGame_1_Prefab;
    public Block_Default endGame_1;

    [Header("Level Editor")]
    public List<Level> listLevel = new List<Level>();

    public enum TypeEndGame
    {
       endGame_1,
       endGame_2
    }

    [System.Serializable]
    public class Level
    {
        public List<TaskBlock> listTaskBlock = new List<TaskBlock>();
    }

    [System.Serializable]
    public class TaskBlock
    {
        public List<TypeBlock> listTypeBlock = new List<TypeBlock>();

        public enum TypeBlock
        {
            Block_Nuoc,
            Block_Ho,
            Block_Tuong,
            Block_Cloud,
            Block_Up,
            Block_Nhan,
            Block_Trap
               
        }
    }

    public void GenerateLevel()
    {
        StartCoroutine(C_GenerateLevel());
    }

    private IEnumerator C_GenerateLevel()
    {
        yield return null;
        int lvl = GameManager.Instance.levelGame;
        if (lvl >= listLevel.Count) lvl = Random.Range(0, listLevel.Count);

        // get current level
        CurrentLevel = listLevel[lvl];
        bool isStairLevel = (GameManager.Instance.levelGame % 2 == 0) ? true : false;

        // generate taskblock from level
        for (int i = 0; i < CurrentLevel.listTaskBlock.Count; i++)
        {
            TaskBlock _taskBlock = CurrentLevel.listTaskBlock[i];
            GameObject _parent = new GameObject();
            _parent.transform.SetParent(transform);
            _parent.name = "TaskBlock " + i.ToString();

            // generate startGround of taskBlock
            Block _startGround = Instantiate(startGround_Prefab, _parent.transform);
            SetTransform(_startGround.transform, _startGround.distance);

            int x = 0;
            if (lvl % 2 == 0)
            {
                x = 1;
            }
            else
            {
                x = 0;
            }

            for (int k = 0; k < _taskBlock.listTypeBlock.Count; k++)
            {
                // check obtacle or item
                string _name = _taskBlock.listTypeBlock[k].ToString();

                if(_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Nuoc)
                {
                    Block_Default _block = Instantiate(block_Nuoc_Prefab, _parent.transform);
                    float distanceExtra = (x + 1) * 10;
                    SetTransform(_block.transform, GetDistance(_block.transform) + distanceExtra);
                }
                else if(_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Ho)
                {
                    Block_Default _block = Instantiate(block_Ho_Prefab, _parent.transform);
                    float distanceExtra = (x + 1) * 10;
                    SetTransform(_block.transform, GetDistance(_block.transform) + distanceExtra);
                }
                else if (_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Tuong)
                {
                    Block_Default _block = Instantiate(block_Tuong_Prefab, _parent.transform);
                    SetTransform(_block.transform, GetDistance(_block.transform));
                }
                else if (_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Cloud)
                {
                    Block_Default _block = Instantiate(block_Cloud_Prefab, _parent.transform);
                    SetTransform(_block.transform, GetDistance(_block.transform));
                }
                else if (_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Up)
                {
                    Block_Default _block = Instantiate(block_Up_Prefab, _parent.transform);
                    SetTransform(_block.transform, GetDistance(_block.transform));
                    CurrentY += _block.transform.GetChild(1).transform.localPosition.y;
                }
                else if (_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Nhan)
                {
                    Block_Default _block = Instantiate(block_Nhan_Prefab, _parent.transform);
                    SetTransform(_block.transform, GetDistance(_block.transform));
                    CurrentY += _block.transform.GetChild(1).transform.localPosition.y;
                }
                else if (_taskBlock.listTypeBlock[k] == TaskBlock.TypeBlock.Block_Trap)
                {
                    Block_Default _block = Instantiate(block_Trap_Prefab, _parent.transform);
                    SetTransform(_block.transform, GetDistance(_block.transform));
                }
                yield return null;
            }

            FinishZ = CurrentZ;
            if (false) _parent.transform.position = Vector3.down * i * 2.0f;
            yield return null;
        }


        typeEndGame = TypeEndGame.endGame_1;
        // end game 1
        endGame_1 = Instantiate(endGame_1_Prefab, transform);
        SetTransform(endGame_1.transform, GetDistance(endGame_1.transform));
    }

    public void GenerateEndGameFromSumMember(int sum)
    {
        if(typeEndGame == TypeEndGame.endGame_1)
        {
     //       endGame_1.GenerateWall(sum);
        }
        else
        {
    //        endGame_2.GenerateWall(sum);
        }
    }

    private void SetTransform(Transform _T,float _distance)
    {
        Vector3 _pos = Vector3.zero;
        _pos.z = CurrentZ;
        _pos.y = CurrentY;
        _T.position = _pos;
        CurrentZ += _distance;
    }

    private void SetTransform(Transform _T, float _distance,float _y)
    {
        Vector3 _pos = Vector3.zero;
        _pos.z = CurrentZ;
        _pos.y = _y;
        _T.position = _pos;
        CurrentZ += _distance;
    }

    private float GetDistance(Transform _parent)
    {
        float _distance = 0.0f;

        for(int i = 0; i < _parent.childCount; i++)
        {
            if(_parent.GetChild(i).gameObject.name == "Ground")              
            {
                _distance += _parent.GetChild(i).localScale.z;
            }else if(_parent.GetChild(i).gameObject.name == "Tuong")
            {
                _distance += 10.0f;
            }
        }

        return _distance;
    }
}
