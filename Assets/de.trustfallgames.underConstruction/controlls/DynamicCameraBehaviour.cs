using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DynamicCameraBehaviour : MonoBehaviour
{
    [Header("Duration of camera rotation in frames")] [Range(1, 50)] [SerializeField] 
    private int _rotationSpeed = 25;

    [Header("Distance in world space, before swipe is counted")] [Range(0, 500)] [SerializeField]
    private int _swipeDistance = 150;


    private float _targetRotation;
    private Vector2 _touchOrigin = -Vector2.one;
    private float _rotationSteps;
    private SwipedDirection _swipeDir;
    private bool _rotating;
    private Touch firstTouch;

    void Update()
    {
        //Get The Swipe and rotate the Map if not already rotating
        if (Input.touchCount == 1 && !_rotating)
        {                      
            if (Input.GetTouch(0).fingerId == 0)
            {
                firstTouch = Input.GetTouch(0);
            }

            if (firstTouch.phase == TouchPhase.Began)
            {
                _touchOrigin = firstTouch.position;
            }
            else if (firstTouch.phase == TouchPhase.Ended)
            {             
                Vector2 touchVector = firstTouch.position - _touchOrigin;              
                _swipeDir = GetSwipeDirection(touchVector);
                 
                _rotationSteps = _rotationSpeed;
                _targetRotation = 90 / _rotationSteps;
                _rotating = true;             
            }         
        }
        
        if (_rotating)
        {
            RotateMap();
        }
    }

    private void RotateMap()
    {
        if (_swipeDir == SwipedDirection.Left)
        {
            transform.Rotate(new Vector3(0, -_targetRotation, 0));
        }
        else if (_swipeDir == SwipedDirection.Right)
        {
            transform.Rotate(new Vector3(0, _targetRotation, 0));
        }
        _rotationSteps--;

        if (_rotationSteps == 0)
        {
            _rotating = false;
        }
    }
    
    private SwipedDirection GetSwipeDirection(Vector2 swipeVector)
    {
        float positiveX = Mathf.Abs(swipeVector.x);
        float positiveY = Mathf.Abs(swipeVector.y);
        
        SwipedDirection swipedDir = SwipedDirection.None;
        
        if (positiveX > positiveY)
        {
            if (swipeVector.x > _swipeDistance)
            {
                swipedDir = SwipedDirection.Right;
            }
            else if (swipeVector.x < -_swipeDistance)
            {
                swipedDir = SwipedDirection.Left;
            }
        }

        return swipedDir;
    }
    
    private enum SwipedDirection
    {
        None,
        Right,
        Left
    }
}
