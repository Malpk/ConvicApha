using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
  [SerializeField] private Weapon weapon;
  [SerializeField] private Transform weaponSpawnPos;

  public void SetWeapon(GameObject weaponPref)
  {
    var weaponobj = Instantiate(weaponPref, weaponSpawnPos.position, Quaternion.identity);
    weapon = weaponobj.GetComponent<Weapon>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.H))
    {
      weapon.TryShoot();
    }
  }
}
