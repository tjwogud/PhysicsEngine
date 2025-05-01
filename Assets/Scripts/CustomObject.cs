using Microsoft.Win32.SafeHandles;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform rect;

    public float mass = 100;

    public Vector2 size = Vector2.zero;
    public Vector2 position = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public Vector2 acceleration = Vector2.zero;

    public float angle = 0;
    public float angularVelocity = 0;

    public Vector2 offset = Vector2.zero;
    public bool dragging = false;
    public Vector2 shoot = Vector2.zero;
    public bool shooting = false;

    public Vector2 TopRight => Utils.Polar(size.magnitude / 2, size.GetAngle() + angle) + position;
    public Vector2 TopLeft => Utils.Polar(size.magnitude / 2, Mathf.PI - size.GetAngle() + angle) + position;
    public Vector2 BottomLeft => Utils.Polar(size.magnitude / 2, Mathf.PI + size.GetAngle() + angle) + position;
    public Vector2 BottomRight => Utils.Polar(size.magnitude / 2, -size.GetAngle() + angle) + position;

    private void Update()
    {
        if (dragging)
        {
            if (!shooting && Input.GetKeyDown(KeyCode.LeftShift))
            {
                shoot = MousePosition();
                shooting = true;
            }
            if (!shooting)
                position = MousePosition() + offset;
            else
                position = shoot + offset;
            if (Input.mouseScrollDelta != Vector2.zero)
            {
                angle -= Input.mouseScrollDelta.y / 10;
                FixAngle();
            }
        }
        rect.anchoredPosition = position;
        rect.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        rect.sizeDelta = size;
    }

    private void FixedUpdate()
    {
        //if (!Input.GetKeyDown(KeyCode.Z) && !Input.GetKey(KeyCode.X))
        //    return;
        float delta = Time.fixedDeltaTime;

        if (!dragging)
        {
            CheckCollisions();

            acceleration = EnvSettings.Instance.GravityAcceleration;
            velocity += acceleration * delta;
            position += velocity * delta;

            angle += angularVelocity * delta;
            FixAngle();
        }
    }

    private void FixAngle()
    {
        while (angle < 0) angle += 2 * Mathf.PI;
        angle %= 2 * Mathf.PI;
    }

    private void CheckCollisions()
    {
        for (int i = 0; i < 5; i++)
        {
            CheckCollisionAt(TopRight);
            CheckCollisionAt(TopLeft);
            CheckCollisionAt(BottomLeft);
            CheckCollisionAt(BottomRight);
        }
    }

    private void CheckCollisionAt(Vector2 point)
    {
        Rect border = EnvSettings.Instance.Border;
        if (point.y < border.min.y)
        {
            ResolveCollision(point, Vector2.up, border.min.y - point.y);
            velocity = new(velocity.x * 0.99f, velocity.y);
        }
        if (point.y > border.max.y)
        {
            ResolveCollision(point, Vector2.down, point.y - border.max.y);
        }
        if (point.x < border.min.x)
        {
            ResolveCollision(point, Vector2.right, border.min.x - point.x);
        }
        if (point.x > border.max.x)
        {
            ResolveCollision(point, Vector2.left, point.x - border.max.x);
        }
    }

    private void ResolveCollision(Vector2 point, Vector2 normal, float depth)
    {
        const float percent = 0.8f;
        Vector2 correction = depth * percent * normal;
        position += correction;

        Vector2 r = point - position;
        Vector2 rで = new(-r.y, r.x);
        Vector2 pointVelocity = velocity + angularVelocity * rで;
        float velocityAlongNormal = Vector2.Dot(pointVelocity, normal);
        if (velocityAlongNormal > 0)
            return;
        float j = -(1 + EnvSettings.Instance.E) * velocityAlongNormal;
        j /= 1 / mass + Mathf.Pow(Vector2.Dot(rで, normal), 2) / CalculateMomentOfInertia();

        velocity += j / mass * normal;
        angularVelocity += j * Vector2.Dot(rで, normal) / CalculateMomentOfInertia();
    }

    private float CalculateMomentOfInertia()
    {
        float result = 1f / 12 * mass * size.sqrMagnitude;
        return result;
    }

    private Vector2 MousePosition()
    {
        return (Vector2)transform.parent.InverseTransformPoint(Input.mousePosition);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (dragging)
            return;
        velocity = Vector2.zero;
        angularVelocity = 0;
        offset = position - MousePosition();
        dragging = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!dragging)
            return;
        dragging = false;
        if (shooting)
        {
            velocity = shoot - MousePosition();
            velocity *= 5;
            shooting = false;
        }
    }
}