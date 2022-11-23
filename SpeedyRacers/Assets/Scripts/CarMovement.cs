using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField]
    public float driftForce;
    [SerializeField]
    public float speedForce;
    [SerializeField]
    public float turningForce;
    [SerializeField]
    public float maxSpeed;

    float accelerationForce = 0;
    float steeringForce = 0;

    float rotationAngle = 0;

    public float velocityVsUp = 0;

    Rigidbody2D carRB;

    //het script om automatisch de Rigidbody naar de auto te doen
    void Awake()
    {
        carRB = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthagonalVelocity();

        ApplySteering();
    }

    void ApplyEngineForce()
    {
        // hoe snel je gaat
        velocityVsUp = Vector2.Dot(transform.up, carRB.velocity);

        //het limiet van de speedyness
        if (velocityVsUp > maxSpeed && accelerationForce > 0)
        {
            return;
        }

        //50% eraf als de auto naar achter gaat
        if (velocityVsUp < 0 && velocityVsUp > -maxSpeed * 0.5f && accelerationForce > 0)

            // minder snelhijd als je draaid
            if (carRB.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationForce > 0)
            {
                return;
            }
        // de auto vertragen
        if (accelerationForce == 0)
        {
            carRB.drag = Mathf.Lerp(carRB.drag, 2.0f, Time.fixedDeltaTime * 2);
        }
        else
        {
            carRB.drag = 0;
        }

        // NJEEWWWWW vrowuem
        Vector2 engineForceVector = transform.up * accelerationForce * speedForce;

        // vroem vroem naar voren
        carRB.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float minSpeedBeforeAllowedTurning = (carRB.velocity.magnitude / 8);
        minSpeedBeforeAllowedTurning = Mathf.Clamp01(minSpeedBeforeAllowedTurning);

        // roteren
        rotationAngle -= steeringForce * turningForce * minSpeedBeforeAllowedTurning;

        // sturen bij roteren
        carRB.MoveRotation(rotationAngle);
    }
    // minder snel als je roteerd
    void KillOrthagonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRB.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRB.velocity, transform.right);

        carRB.velocity = forwardVelocity + rightVelocity * driftForce;
    }
    public void SetInput(Vector2 inputVector)
    {
        steeringForce = inputVector.x;
        accelerationForce = inputVector.y;
    }
}