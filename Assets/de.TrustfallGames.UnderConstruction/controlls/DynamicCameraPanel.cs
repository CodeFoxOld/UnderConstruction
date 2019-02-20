using System.Collections;
using System.Collections.Generic;
using de.TrustfallGames.UnderConstruction.character;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.ui.components;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DynamicCameraPanel : InteractableButton
{
    [Header("Duration of camera rotation in frames")] [Range(1, 50)] [SerializeField] 
    private int _rotationSpeed = 25;

    [Header("Distance in world space, before swipe is counted")] [Range(0, 500)] [SerializeField]
    private int _swipeDistance = 150;

    [Header("The centered dynamic camera object, that will be rotated")] [SerializeField]
    private GameObject _dynamicCam;
    
    [Header("The control X")] [SerializeField]
    private GameObject _controlX;

    [Header("The game UI")] [SerializeField]
    private GameUIBehaviour GUI;


    private float _targetRotation;
    private float _rotationSteps;
    private Vector2 _touchOrigin;
    private SwipedDirection _swipeDir;
    private bool _rotating;

    void Start()
    {
        _dynamicCam = Camera.main.transform.parent.gameObject;
        _rotationSteps = _rotationSpeed;
    }

    void FixedUpdate()
    {      
        if (_rotating)
        {
            RotateMap();
        }
    }

    protected override void OnButtonPressed(PointerEventData eventData)
    {
        _touchOrigin = eventData.position;
    }

    protected override void OnButtonReleased(PointerEventData eventData)
    {     
        if (!_rotating)
        {
            _swipeDir = GetSwipeDirection(eventData.position - _touchOrigin);

            if (_swipeDir != SwipedDirection.None)
            {
                _targetRotation = 90 / _rotationSteps;
                _rotating = true;
            }
        }
    }
    
    private void RotateMap()
    {
        if (_swipeDir == SwipedDirection.Left)
        {
            _dynamicCam.transform.Rotate(new Vector3(0, -_targetRotation, 0));
            _controlX.transform.Rotate(new Vector3(0,0, -_targetRotation));
        }
        else if (_swipeDir == SwipedDirection.Right)
        {
            _dynamicCam.transform.Rotate(new Vector3(0, _targetRotation, 0));
            _controlX.transform.Rotate(new Vector3(0,0, _targetRotation));
        }
        _rotationSteps--;

        if (_rotationSteps == 0)
        {
            _rotating = false;
            _rotationSteps = _rotationSpeed;
            SwapBaggerButtons();        
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

    private void SwapBaggerButtons()
    {
        GUI.SwapDestructorButtonListeners();
    }

    private enum SwipedDirection
    {
        None,
        Right,
        Left
    }

    
}
