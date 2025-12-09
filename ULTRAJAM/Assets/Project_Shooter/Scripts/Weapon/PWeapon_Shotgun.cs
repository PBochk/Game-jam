using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shooter.Gameplay
{
    public class PWeapon_Shotgun : Weapon_Base
    {
        [Header("Shotgun Custom")]
        public float m_ReloadTime = 1.5f; // Установите желаемое время перезарядки (например, 1.5 сек)

        public float CostPerShot = 250f;
        private bool m_IsShooting = false;

        // Удаляем Start и Update из оригинального кода, т.к. они используют старую логику задержки.
        // Вместо них используем новую логику.

        void Update()
        {
            // Логика уменьшения отдачи (оставляем ее)
            RecoilTimer -= 10 * Time.deltaTime;
            if (RecoilTimer <= 0)
                RecoilTimer = 0;

            // Проверяем ввод и статус стрельбы/перезарядки
            if (Input_FireHold && !m_IsShooting)
            {
                StartCoroutine(ShootAndReloadCo());
            }
        }

        IEnumerator ShootAndReloadCo()
        {
            m_IsShooting = true;

            // 1. Выстрел
            FireWeapon();
            RecoilTimer = 1f; // Добавляем отдачу

            // 2. Долгая перезарядка (задержка)
            // Здесь можно добавить анимацию или звук перезарядки
            yield return new WaitForSeconds(m_ReloadTime);

            // 3. Сброс
            m_IsShooting = false;
        }

        public override void FireWeapon()
        {
            DamageControl playerDC = m_Owner.GetComponent<DamageControl>();

            if (playerDC != null)
            {
                var damageToSelf = MathF.Min(CostPerShot, playerDC.Damage - 2);
                // Наносим урон себе:
                playerDC.ApplyDamage(
                    damageToSelf, // Стоимость
                    m_Owner.transform.forward, // Направление (любое, так как урон себе)
                    damageToSelf); // Damage Factor
            }
            
            GameObject obj;
            int pelletCount = 0;
            float maxSpreadAngle = 0;
            float bulletLifeTime = 0.12f;
            
            // --- Настройки для разных уровней мощности ---
            if (m_PowerLevel == 0)
            {
                pelletCount = 20; // 15 дробинок
                maxSpreadAngle = 45; // Максимальный угол разброса 10 градусов
            }
            else if (m_PowerLevel == 1)
            {
                pelletCount = 10; // 20 дробинок
                maxSpreadAngle = 20f; // Увеличенный разброс и количество для Power Level 1
            }
            // --- Конец настроек ---

            for (int i = 0; i < pelletCount; i++)
            {
                obj = Instantiate(BulletPrefab);
                obj.transform.position = m_FirePoint.position;
                
                // 1. Создаем случайный хаотичный разброс
                float randomX = Random.Range(-maxSpreadAngle, maxSpreadAngle);
                float randomY = Random.Range(-maxSpreadAngle, maxSpreadAngle);
                
                // Применяем случайный поворот к направлению выстрела
                Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0);
                obj.transform.forward = spreadRotation * m_FirePoint.forward;
                
                Projectile_Base proj = obj.GetComponent<Projectile_Base>();
                
                // 2. Создаем случайную скорость (небольшая вариация)
                float speedMultiplier = Random.Range(0.6f, 0.8f); // Скорость будет от 85% до 115% от базовой
                
                proj.Creator = m_Owner;
                proj.Speed = ProjectileSpeed * speedMultiplier; // Применяем случайную скорость
                proj.Damage = Damage;
                proj.m_Range = Range;

                Destroy(obj, bulletLifeTime);
            }

            // Логика создания эффекта выстрела (остается без изменений)
            obj = Instantiate(EffectPrefab);
            //obj.transform.SetParent(m_ParticlePoint);
            obj.transform.localPosition = transform.position + m_FirePoint.localPosition;
            obj.transform.forward = m_ParticlePoint.forward;
            Destroy(obj, 0.5f);
            
            if (CameraControl.m_Current != null)
            {
                // Параметры: (Длительность = 0.2 сек, Интенсивность = 0.4)
                CameraControl.m_Current.StartShake(0.25f, 0.8f); 
            }
        }
        
    }
}