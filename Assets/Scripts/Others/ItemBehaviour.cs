using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    private enum ItemType { Key, Fuel }

    [SerializeField]
    private ItemType _type;
    [SerializeField]
    private float _fuelAmount = 0.25f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (_type)
            {
                case ItemType.Key:
                    FindObjectOfType<PlayerController>().HasKey = true;
                    break;
                case ItemType.Fuel:
                    FindObjectOfType<PlayerController>().AddFuel(_fuelAmount);
                    break;
            }
            if (_type == ItemType.Key)
            {
                FindObjectOfType<PlayerController>().HasKey = true;
            }
            Destroy(gameObject);
        }
    }
}