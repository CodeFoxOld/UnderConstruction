using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DynamicCameraBehaviour : MonoBehaviour
{
    [Header("Camera rotation Speed, while swiping")]
    [Range(1, 50)]
    [SerializeField] private int _rotationSpeed = 25;
    
    private float _targetRotation;
    private Vector2 _touchOrigin = -Vector2.one;
    private float _rotationSteps;
    private SwipedDirection _swipeDir;
    private bool _rotating;
    
    
    void Update()
    {
        //Get The Swipe and rotate the Map if not already rotating
        if (Input.touchCount > 0 && !_rotating)
        {           
            Touch firstTouch = Input.touches[0];

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
        
        SwipedDirection swipedDir;
        
        if (positiveX > positiveY)
        {
            swipedDir = (swipeVector.x > 0) ? SwipedDirection.Right : SwipedDirection.Left;
        }
        else
        {
            swipedDir = SwipedDirection.None;
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
