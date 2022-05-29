using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed = 60f;
    [SerializeField] private Vector3 direction = new Vector3(0, 1, 0);


    private void Update()
    {
        transform.RotateAround(transform.position, direction, speed * Time.deltaTime);
    }




    #region Initializer

    public void Init(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
    }

    #endregion

}
