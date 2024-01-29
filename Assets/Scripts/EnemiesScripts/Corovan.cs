using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corovan : MovableEnemy
{
    public override void InitializeEnemyVariant()
    {
        if (!isInitialized) InitializeEnemy();

        if (isWayEnemy)
        {
            ChangeEnemyParent(true);
            Flip();

            if (gameService.IsBossStage() && gameService.currentBoss == BossType.VirusPhantom)
            {
                foreach (var view in gameObject.GetComponentsInChildren<CustomSpriteRenderer>())
                    view.SetMaterial(MaterialManager.instance.GetMaterial("Phantom"));
            }

            if (enemyView.spriteRenderer.materials.Length < 2)
            {
                enemyView.spriteRenderer.materials = new Material[2] { enemyView.spriteRenderer.material, MaterialManager.instance.GetMaterial("SpriteOutline").material };
                if (enemyView.GetMaterial(MaterialType.OutlineMaterial).GetInt("_OutlineEnabled") == 0)
                    enemyView.SetMaterialInt(MaterialType.OutlineMaterial, "_OutlineEnabled", 1);
            }
            ActivateVulnerable();
        }
        ChangeMovingSettings();
    }

    public override void SetEnemyType()
    {
        type = EnemyType.Corovan;
    }

    protected override void OnDisable()
    {

    }
}
