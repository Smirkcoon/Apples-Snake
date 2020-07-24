using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeMove : MonoBehaviour
{
    public List<Transform> SnakeParts;
    public GameObject PartPref;
    [HideInInspector]
    public Transform Aplle;
    private float _nextMoveRate = 0.7f;
    private GameObject[] Blocks;
    private Transform _transform;
    private Vector3 _direction;   
    private float _nextMoveTime;
    private int ApplesCount;

    public AudioClip[] EatSound;
    private AudioSource AudioS;
    private void Start()
    {
        _transform = GetComponent<Transform>();        
        Blocks = GameObject.FindGameObjectsWithTag("Block");
        AudioS = GetComponent<AudioSource>();
    }      
    private void FixedUpdate()
    {        
        if (SnakeParts.Count >= 12)
        {
            FindObjectOfType<MapManager>().WinActivated(ApplesCount, _nextMoveRate);
        }
        else
        {
            if (Time.time > _nextMoveTime) //выполняется по таймеру
            {
                if (IsValidGridPosition() && !IfBlock() && !IfSnakePart())
                {
                    if (Aplle != null)
                    {
                        IfApple();
                    }
                    RotateAllParts();
                    MoveAllParts();
                }               
                _nextMoveTime = _nextMoveRate + Time.time; //сброс таймера
            }           
        }
    }
    public void RotatingLeft()
    {
        _transform.Rotate(0, 0, 90.0f, Space.World); //поворот влево по кнопке
    }
    public void RotatingRight()
    {
        _transform.Rotate(0, 0, -90.0f, Space.World); //поворот вправо по кнопке       
    }
    private void AddPart() //сохраняю координаты хвоста, убираю хвост из списка, добавляю часть, добавляю хвост
    {
        Transform tail = SnakeParts[SnakeParts.Count - 1];
        SnakeParts.RemoveAt(SnakeParts.Count-1);
        GameObject newPart = Instantiate(PartPref, tail.position, Quaternion.identity);
        SnakeParts.Add(newPart.transform);
        SnakeParts.Add(tail);
    }
    private void MoveAllParts()//движение всех частей змеи
    {
        for (int i = (SnakeParts.Count - 1); i > 0; i--)
        {
            SnakeParts[i].position = SnakeParts[i - 1].position;
        }
        switch (_transform.rotation.eulerAngles.z)
        {
            case 0:
                _direction = new Vector3(0, -1, 0);
                break;
            case 90:
                _direction = new Vector3(1, 0, 0);
                break;
            case 270:
                _direction = new Vector3(-1, 0, 0);
                break;
            case 180:
                _direction = new Vector3(0, 1, 0);
                break;
        }
        _transform.position += _direction;         
    }
    private void RotateAllParts()//поворот всех частей змеи
    {
        for (int i = (SnakeParts.Count - 1); i > 0; i--)
        {                      
            if (Mathf.Round(SnakeParts[i].rotation.eulerAngles.z) != Mathf.Round(SnakeParts[i - 1].rotation.eulerAngles.z)) 
            {
                SnakeParts[i].rotation = Quaternion.Euler(SnakeParts[i - 1].rotation.eulerAngles);
            }
        }
    }
    private bool IsValidGridPosition()//проверка в зоне игры ли змея
    {
        if (_transform.position.x >= 0 && _transform.position.x <= 20 && _transform.position.y >= 0 && _transform.position.y <= 20)
        {
            return true;
        }
        else
        {
            FindObjectOfType<MapManager>().LoseActivated(false);
            return false;
        }
    }
    private bool IfSnakePart()//укусила ли себя змея
    {
        for (int i = 1; i < SnakeParts.Count; i++)
        {
            if (SnakeParts[i].position == _transform.position)
            {
                FindObjectOfType<MapManager>().LoseActivated(false);
                return true;
            }                     
        }
        return false;
    }
    private bool IfApple()//если сьела яблоко
    {       
        if (Aplle.position == _transform.position)
        {
            AddPart();
            _nextMoveRate -= 0.05f;
            Destroy(Aplle.gameObject);
            FindObjectOfType<MapManager>().AppleGenerator();
            ApplesCount += 1;
            int i = Random.Range(0, EatSound.Length);
            AudioS.clip = EatSound[i];
            AudioS.Play();
            return false;
        }
        return true;
    }
    private bool IfBlock()//если преграда
    {
        for (int i = 0; i < Blocks.Length; i++)
        {           
            if (_transform.position == Blocks[i].transform.position)
            {
                FindObjectOfType<MapManager>().LoseActivated(true);
                return true;
            }
        }
        return false;
    }    
}
