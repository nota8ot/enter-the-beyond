using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using BotsMod;
using System.Reflection;

namespace BotsMod
{
    public class TestPassive : PassiveItem
    {
        //Call this method from the Start() method of your ETGModule extension
        public static void Init()
        {

            string itemName = "Test Passive";
            string resourceName = "BotsMod/sprites/wip";
            GameObject obj = new GameObject();
            //var item = BotsModule.WarCrime2;//obj.AddComponent<PirmalShotgrub>().GetComponent<PickupObject>();
            var item = obj.AddComponent<TestPassive>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            //ItemBuilder.AddAnimatedSpriteToObject(itemName, new List<string> { "BotsMod/sprites/Spells/ShittySpellTempSprite_001", "BotsMod/sprites/Spells/ShittySpellTempSprite_002", "BotsMod/sprites/Spells/ShittySpellTempSprite_003", "BotsMod/sprites/Spells/ShittySpellTempSprite_004", "BotsMod/sprites/Spells/ShittySpellTempSprite_005", "BotsMod/sprites/Spells/ShittySpellTempSprite_006", "BotsMod/sprites/Spells/ShittySpellTempSprite_007", "BotsMod/sprites/Spells/ShittySpellTempSprite_008", "BotsMod/sprites/Spells/ShittySpellTempSprite_009", "BotsMod/sprites/Spells/ShittySpellTempSprite_010", "BotsMod/sprites/Spells/ShittySpellTempSprite_011", "BotsMod/sprites/Spells/ShittySpellTempSprite_012", }, obj);



            string shortDesc = "testing item";
            string longDesc = "this item is purly for testing";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "bot");
            item.quality = ItemQuality.EXCLUDED;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.MovementSpeed, 5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            //ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 1f, StatModifier.ModifyMethod.ADDITIVE);

            //var model = Tools.BotsAssetBundle.LoadAsset<GameObject>("TestModel");
            //var model = UnityEngine.Object.Instantiate(Tools.BotsAssetBundle.LoadAsset<GameObject>("TestModel"), GameManager.Instance.PrimaryPlayer.sprite.WorldCenter, new Quaternion());
            //model.transform.parent = rotator.transform;

            //model.SetActive(true);

            //GameObject.DontDestroyOnLoad(model);

        }


        private ImprovedAfterImage zoomy;
        public override void Pickup(PlayerController player)
        {
            if (player.CurrentGun)
            {
                this.ProcessGunShader(player.CurrentGun);
            }



            PickupObject byId = PickupObjectDatabase.GetById(60);
            LootEngine.TryGivePrefabToPlayer(byId.gameObject, player, true);
            //player.PostProcessBeam += this.PostProcessProjectileHelixBeam;
            player.PostProcessProjectile += this.PostProcessProjectileHelix;
            //player.OnIsRolling += Player_OnIsRolling;
            //player.OnDodgedProjectile += Player_OnDodgedProjectile;
            

            base.Pickup(player);
            return;
            WindowRect = new Rect(500f, 0f, 450f, 900f);

           



            zoomy = player.gameObject.AddComponent<ImprovedAfterImage>();
            zoomy.dashColor = new Color(180, 32, 42);
            zoomy.spawnShadows = false;
            zoomy.shadowTimeDelay = 0.01f;
            zoomy.shadowLifetime = 0.3f;
            zoomy.minTranslation = 0.05f;
            zoomy.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
            player.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
            zoomy.spawnShadows = true;


            player.healthHaver.ApplyDamage(10000, Vector2.zero, "happiness");

           
        }

        private void ProcessGunShader(Gun g)
        {
            MeshRenderer component = g.GetComponent<MeshRenderer>();
            if (!component)
            {
                return;
            }
            Material[] sharedMaterials = component.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i].shader == ShaderCache.Acquire("Brave/PlayerShaderEevee"))
                {
                    return;
                }
            }
            Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
            Material material = new Material(ShaderCache.Acquire("Brave/PlayerShaderEevee"));
            material.SetTexture("_EeveeTex", ResourceManager.LoadAssetBundle("shared_auto_001").LoadAsset<Texture2D>("nebula_reducednoise"));
            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;
        }

        private void Player_OnDodgedProjectile(Projectile obj)
        {
            if (obj.Owner is PlayerController)
            {
                obj.baseData.speed *= 3;
                obj.OnDestruction += Obj_OnDestruction;
            }
        }

        private void Obj_OnDestruction(Projectile obj)
        {
            Exploder.DoDefaultExplosion(obj.sprite.WorldCenter, Vector2.zero);
        }

        private void Player_OnIsRolling(PlayerController obj)
        {
            if (obj.CurrentRollState == PlayerController.DodgeRollState.PreRollDelay)
            {
                this.DisableEffect(obj);
                this.EnableVFX(obj, new Color32(255, 0, 0, 255));

            }

            if (obj.CurrentRollState == PlayerController.DodgeRollState.InAir)
            {
                this.DisableEffect(obj);
                this.EnableVFX(obj, new Color32(0, 255, 0, 255));
            }

            if (obj.CurrentRollState == PlayerController.DodgeRollState.OnGround)
            {
                this.DisableEffect(obj);
                this.EnableVFX(obj, new Color32(255, 170, 0, 255));
            }

            if (obj.CurrentRollState == PlayerController.DodgeRollState.AdditionalDelay)
            {
                this.DisableEffect(obj);
                this.EnableVFX(obj, new Color32(0, 255, 229, 255));
            }

            if (obj.CurrentRollState == PlayerController.DodgeRollState.None)
            {
                this.DisableEffect(obj);
                //this.EnableVFX(obj, new Color32(221, 255, 0, 255));
            }

            if (obj.CurrentRollState == PlayerController.DodgeRollState.Blink)
            {
                this.DisableEffect(obj);
                this.EnableVFX(obj, new Color32(149, 0, 255, 255));
            }
            
        }

        public void EnableVFX(PlayerController target, Color color)
        {

            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
            if (outlineMaterial != null)
            {
                outlineMaterial.SetColor("_OverrideColor", color);
            }

            
        }

        // Token: 0x060072CA RID: 29386 RVA: 0x002E3174 File Offset: 0x002E1374
        public void DisableVFX(PlayerController target)
        {

            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(target.sprite);
            if (outlineMaterial != null)
            {
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
            }

            
        }

        private static Rect WindowRect;

        private static Vector2 ScrollPos;

        private Shader m_glintShader;

        private bool UpOrDown;

        public void OnHit(Projectile projectile, SpeculativeRigidbody target, bool fatal)
        {
            if (target.aiActor != null)
            {
                target.aiActor.SetOverrideOutlineColor(new Color(255, 0, 140));
            } 
            else if(target.gameObject != null && target.gameObject.GetComponentInChildren<AIActor>() != null) 
            {
                target.gameObject.GetComponentInChildren<AIActor>().SetOverrideOutlineColor(new Color(255, 0, 140));
            }
           
        }

        private void PostProcessProjectileHelix(Projectile obj, float effectChanceScalar)
        {

            obj.gameObject.AddComponent<ProjBoostModifer>();

            /*obj.OnHitEnemy += OnHit;
            if (obj is InstantDamageOneEnemyProjectile)
            {
                return;
            }
            if (obj is InstantlyDamageAllProjectile)
            {
                return;
            }
            else if (this.UpOrDown)
            {
                obj.OverrideMotionModule = new BotsProjectileMotionModule();
            }
            else
            {
                obj.OverrideMotionModule = new BotsProjectileMotionModule
                {
                    ForceInvert = true
                };
            }
            this.UpOrDown = !this.UpOrDown;*/
            
        }

        private void PostProcessProjectileHelixBeam(BeamController beam)
        {
            if (beam.Owner is AIActor)
            {
                return;
            }

            else if (this.UpOrDown)
            {
                beam.projectile.OverrideMotionModule = new BotsProjectileMotionModule();
            }
            else
            {
                BotsProjectileMotionModule helixProjectileMotionModule = new BotsProjectileMotionModule();
                helixProjectileMotionModule.ForceInvert = true;
                beam.projectile.OverrideMotionModule = helixProjectileMotionModule;
            }
            this.UpOrDown = !this.UpOrDown;
        }



        public static void DrawProperty(string text)
        {
            GUILayout.Label(text, new GUILayoutOption[0]);
            GUILayout.Space(8f);
            
        }

        public void WindowFunction(int windowID)
        {
            ScrollPos = GUILayout.BeginScrollView(ScrollPos, new GUILayoutOption[0]);
            GUILayout.EndScrollView();
            UnityEngine.GUI.DragWindow();
            UnityEngine.GUI.color = Color.green;
        }


    }

        
}

/*
protected override void Update()
{
    base.Update();
    if (!this.m_pickedUp && base.gameObject.layer != LayerMask.NameToLayer("Unpixelated"))
    {
        base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
    }
}

// Token: 0x06007082 RID: 28802 RVA: 0x002CA328 File Offset: 0x002C8528
public override void Pickup(PlayerController player)
{
    if (this.m_pickedUp)
    {
        return;
    }
    SimpleSpriteRotator[] componentsInChildren = base.GetComponentsInChildren<SimpleSpriteRotator>();
    if (componentsInChildren != null)
    {
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
        }
    }
    GameManager.Instance.PrimaryPlayer.PastAccessible = true;
    Shader.SetGlobalFloat("_MapActive", 1f);

    CreateCompanion(player);
    CreateCompanion2(player);

    base.Pickup(player);
}
private GameObject m_extantCompanion;

string CompanionGuid = "ebf2314289ff4a4ead7ea7ef363a0a2e";
string CompanionGuid2 = "ab4a779d6e8f429baafa4bf9e5dca3a9";

private void CreateCompanion(PlayerController owner)
{
    CompanionSynergyProcessor companionSynergy = new CompanionSynergyProcessor();

    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid);
    Vector3 position = owner.transform.position;
    GameObject extantCompanion = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, position, Quaternion.identity);
    m_extantCompanion = extantCompanion;
    CompanionController orAddComponent = m_extantCompanion.GetOrAddComponent<CompanionController>();
    orAddComponent.Initialize(owner);
    if (orAddComponent.specRigidbody)
    {
        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
    }
}

private void CreateCompanion2(PlayerController owner)
{
    CompanionSynergyProcessor companionSynergy = new CompanionSynergyProcessor();

    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.CompanionGuid2);
    Vector3 position = owner.transform.position;
    GameObject extantCompanion = UnityEngine.Object.Instantiate<GameObject>(orLoadByGuid.gameObject, position, Quaternion.identity);
    m_extantCompanion = extantCompanion;
    CompanionController orAddComponent = m_extantCompanion.GetOrAddComponent<CompanionController>();
    orAddComponent.Initialize(owner);
    if (orAddComponent.specRigidbody)
    {
        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(orAddComponent.specRigidbody, null, false);
    }
}

// Token: 0x06007083 RID: 28803 RVA: 0x002CA398 File Offset: 0x002C8598
public override DebrisObject Drop(PlayerController player)
{
    DebrisObject debrisObject = base.Drop(player);
    debrisObject.GetComponent<TestPassive>().m_pickedUpThisRun = true;
    debrisObject.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
    return debrisObject;
}
/*
public override void Pickup(PlayerController player)
{
    PlayerItem currentItem = player.CurrentItem;
    float num = -1;
    currentItem.Use(player, out num);
    player.PostProcessProjectile += this.PostProcessProjectile;
    // this.proj = base.GetComponent<Projectile>();
    //Projectile projectile = this.proj;
    // projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHitEnemy));
    /*int i = 0;

     foreach (PlayerItem item in player.activeItems)
     {
         PlayerItem playerItem = player.activeItems[i];
         //PlayerItem playerItem = new PlayerItem();
         float number = 0;
         playerItem.Use(player, out number);
         //playerItem.DoEffect(player) as PlayerItem;
         i++;
         ETGModConsole.Log(i + "", false);
     }

    base.Pickup(player);


}
private void PostProcessProjectile(Projectile obj, float effectChanceScalar)
{
    obj.OnHitEnemy = this.HandleHitEnemy;

}
private void HandleHitEnemy(Projectile sourceProjectile, SpeculativeRigidbody hitRigidbody, bool fatal)
{
    //if (hitRigidbody = null) return;
    StartCoroutine(BulletReturn(sourceProjectile));
}

private IEnumerator BulletReturn(Projectile sourceProjectile)
{
    yield return new WaitForSeconds(0.2f);
    sourceProjectile.Direction = sourceProjectile.Direction * -1;

    yield break;
}

private Projectile proj;*/
//}
//}
