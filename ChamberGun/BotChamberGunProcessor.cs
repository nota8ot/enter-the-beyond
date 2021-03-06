using System;
using System.Collections;
using System.Reflection;
using BotsMod;
using Dungeonator;
using UnityEngine;

namespace BotsMod
{
    public class BotChamberGunProcessor : MonoBehaviour, ILevelLoadedListener
    {
        public BotChamberGunProcessor()
        {

            this.CastleGunID = 647;
            this.GungeonGunID = 660;
            this.MinesGunID = 807;
            this.HollowGunID = 659;
            this.ForgeGunID = 658;
            this.HellGunID = 763;
            this.OublietteGunID = 657;
            this.JungleGunID = 368;
            this.AbbeyGunID = 806;
            this.RatgeonGunID = 808;
            this.OfficeGunID = 823;
            this.BeyondGunID = 599;

            this.RefillsOnFloorChange = true;
        }

        private void Awake()
        {
            this.m_currentTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON;
            this.m_gun = base.GetComponent<Gun>();
            Gun gun = this.m_gun;
            gun.OnReloadPressed = (Action<PlayerController, Gun, bool>)Delegate.Combine(gun.OnReloadPressed, new Action<PlayerController, Gun, bool>(this.HandleReloadPressed));
        }

        private GlobalDungeonData.ValidTilesets GetFloorTileset()
        {
            if (GameManager.Instance.IsLoadingLevel || !GameManager.Instance.Dungeon)
            {
                return GlobalDungeonData.ValidTilesets.CASTLEGEON;
            }           
            return GameManager.Instance.Dungeon.tileIndices.tilesetId;
        }

        public bool IsCurrentTileset(int curTilesetId, out int gunFormId)
        {
            gunFormId = 0;
            foreach (var comp in base.GetComponents<Component>())
            {
                if (comp.ToString().ToLower().Contains("customchambergun"))
                {

                    int[] fields = new int[3];
                    fields[0] = (int)ReflectionHelper.GetValue(comp.GetType().GetField("TilesetId"), comp);
                    fields[1] = (int)ReflectionHelper.GetValue(comp.GetType().GetField("MasterRoundId"), comp);
                    fields[2] = (int)ReflectionHelper.GetValue(comp.GetType().GetField("GunId"), comp);

                    if (fields[0] == curTilesetId)
                    {
                        gunFormId = fields[2];
                        return true;
                    }
                }
            }
            
            return false;
        }

        private bool IsValidTileset(GlobalDungeonData.ValidTilesets t)
        {
            if (t == this.GetFloorTileset())
            {
                return true;
            }
            PlayerController playerController = this.m_gun.CurrentOwner as PlayerController;
            if (playerController)
            {
                if (t == GlobalDungeonData.ValidTilesets.CASTLEGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Castle))
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.GUNGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Gungeon))
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.MINEGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Mines))
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.CATACOMBGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Catacombs))
                {
                    return true;
                }
                if (t == GlobalDungeonData.ValidTilesets.FORGEGEON && playerController.HasPassiveItem(GlobalItemIds.MasteryToken_Forge))
                {
                    return true;
                }
                foreach (var comp in base.GetComponents<Component>())
                {
                    if (comp.ToString().ToLower().Contains("customchambergun"))
                    {
                        int[] fields = new int[3];
                        fields[0] = (int)ReflectionHelper.GetValue(comp.GetType().GetField("TilesetId"), comp);
                        fields[1] = (int)ReflectionHelper.GetValue(comp.GetType().GetField("MasterRoundId"), comp);
                        fields[2] = (int)ReflectionHelper.GetValue(comp.GetType().GetField("GunId"), comp);

                        if (fields[0] == (int)t && playerController.HasPassiveItem(fields[1]))
                        {
                            return true;
                        }                    
                    }
                }
            }
            return false;
        }

        private void ChangeToTileset(GlobalDungeonData.ValidTilesets t)
        {
            if (t == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                this.ChangeForme(this.CastleGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.SEWERGEON)
            {
                this.ChangeForme(this.OublietteGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.SEWERGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.GUNGEON)
            {
                this.ChangeForme(this.GungeonGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.GUNGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
            {
                this.ChangeForme(this.AbbeyGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.CATHEDRALGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                this.ChangeForme(this.MinesGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.MINEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.RATGEON)
            {
                this.ChangeForme(this.RatgeonGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.RATGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
            {
                this.ChangeForme(this.HollowGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.CATACOMBGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.OFFICEGEON)
            {
                this.ChangeForme(this.OfficeGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.OFFICEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.FORGEGEON)
            {
                this.ChangeForme(this.ForgeGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.FORGEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.HELLGEON)
            {
                this.ChangeForme(this.HellGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.HELLGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
            {
                this.ChangeForme(this.JungleGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.JUNGLEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.BELLYGEON)
            {
                this.ChangeForme(this.BellyGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.BELLYGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.PHOBOSGEON)
            {
                this.ChangeForme(this.PhobosGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.PHOBOSGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.WESTGEON)
            {
                this.ChangeForme(this.OldWestGunID);
                this.m_currentTileset = GlobalDungeonData.ValidTilesets.WESTGEON;
            }
            else if (IsCurrentTileset((int)t, out int i))
            {
                this.ChangeForme(i);
                this.m_currentTileset = t;
            }
            else
            {
                this.ChangeForme(this.CastleGunID);
                this.m_currentTileset = this.GetFloorTileset();
            }
        }

        private void ChangeForme(int targetID)
        {
            Gun targetGun = PickupObjectDatabase.GetById(targetID) as Gun;
            this.m_gun.TransformToTargetGun(targetGun);
        }

        private void Update()
        {
            if (Dungeon.IsGenerating || GameManager.Instance.IsLoadingLevel)
            {
                return;
            }
            if (this.m_gun && (!this.m_gun.CurrentOwner || !this.IsValidTileset(this.m_currentTileset)))
            {
                GlobalDungeonData.ValidTilesets validTilesets = this.GetFloorTileset();
                if (!this.m_gun.CurrentOwner)
                {
                    validTilesets = GlobalDungeonData.ValidTilesets.CASTLEGEON;
                }
                if (this.m_currentTileset != validTilesets)
                {
                    this.ChangeToTileset(validTilesets);
                }
            }
            this.JustActiveReloaded = false;
        }

        private GlobalDungeonData.ValidTilesets GetNextTileset(GlobalDungeonData.ValidTilesets t)
        {
            if (t == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                return GlobalDungeonData.ValidTilesets.SEWERGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.SEWERGEON)
            {
                return GlobalDungeonData.ValidTilesets.JUNGLEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
            {
                return GlobalDungeonData.ValidTilesets.GUNGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.GUNGEON)
            {
                return GlobalDungeonData.ValidTilesets.CATHEDRALGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
            {
                return GlobalDungeonData.ValidTilesets.BELLYGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.BELLYGEON)
            {
                return GlobalDungeonData.ValidTilesets.MINEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.MINEGEON)
            {
                return GlobalDungeonData.ValidTilesets.RATGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.RATGEON)
            {
                return GlobalDungeonData.ValidTilesets.CATACOMBGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
            {
                return GlobalDungeonData.ValidTilesets.WESTGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.WESTGEON)
            {
                return GlobalDungeonData.ValidTilesets.OFFICEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.OFFICEGEON)
            {
                return GlobalDungeonData.ValidTilesets.PHOBOSGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.PHOBOSGEON)
            {
                return GlobalDungeonData.ValidTilesets.FORGEGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.FORGEGEON)
            {
                return GlobalDungeonData.ValidTilesets.HELLGEON;
            }
            else if (t == GlobalDungeonData.ValidTilesets.HELLGEON)
            {
                return (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND;
            }
            else if (t == (GlobalDungeonData.ValidTilesets)CustomValidTilesets.BEYOND)
            {
                return GlobalDungeonData.ValidTilesets.CASTLEGEON;
            }
            else
            {
                return GlobalDungeonData.ValidTilesets.CASTLEGEON;
            }
        }

        private GlobalDungeonData.ValidTilesets GetNextValidTileset()
        {
            GlobalDungeonData.ValidTilesets nextTileset = this.GetNextTileset(this.m_currentTileset);
            while (!this.IsValidTileset(nextTileset))
            {
                nextTileset = this.GetNextTileset(nextTileset);
            }
            return nextTileset;
        }

        private void HandleReloadPressed(PlayerController ownerPlayer, Gun sourceGun, bool manual)
        {
            if (this.JustActiveReloaded)
            {
                return;
            }
            if (manual && !sourceGun.IsReloading)
            {
                GlobalDungeonData.ValidTilesets nextValidTileset = this.GetNextValidTileset();
                if (this.m_currentTileset != nextValidTileset)
                {
                    this.ChangeToTileset(nextValidTileset);
                }
            }
        }

        public void BraveOnLevelWasLoaded()
        {
            if (this.RefillsOnFloorChange && this.m_gun && this.m_gun.CurrentOwner)
            {
                this.m_gun.StartCoroutine(this.DelayedRegainAmmo());
            }
        }

        private IEnumerator DelayedRegainAmmo()
        {
            yield return null;
            while (Dungeon.IsGenerating)
            {
                yield return null;
            }
            if (this.RefillsOnFloorChange && this.m_gun && this.m_gun.CurrentOwner)
            {
                this.m_gun.GainAmmo(this.m_gun.AdjustedMaxAmmo);
            }
            yield break;
        }

        public int CastleGunID;
        public int GungeonGunID;
        public int MinesGunID;
        public int HollowGunID;
        public int ForgeGunID;
        public int HellGunID;
        public int OublietteGunID;
        public int AbbeyGunID;
        public int RatgeonGunID;
        public int OfficeGunID;
        public int JungleGunID;
        public int BellyGunID;
        public int PhobosGunID;
        public int OldWestGunID;
        public int BeyondGunID;
        public bool RefillsOnFloorChange;
        private GlobalDungeonData.ValidTilesets m_currentTileset;
        private Gun m_gun;
        public bool JustActiveReloaded;
    }
}
