using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using UnityEngine;
using MonoMod.RuntimeDetour;
using Object = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;
using GungeonAPI;
using BotsMod;
using ItemAPI;
using SaveAPI;

namespace CustomCharacters
{
    public static class Hooks
    {
		public static void Init()
		{
			try
			{

				Hook getNicknamehook = new Hook(
				    typeof(StringTableManager).GetMethod("GetTalkingPlayerNick", BindingFlags.NonPublic | BindingFlags.Static),
				    typeof(Hooks).GetMethod("GetTalkingPlayerNickHook")
				);

				Hook getNamehook = new Hook(
				    typeof(StringTableManager).GetMethod("GetTalkingPlayerName", BindingFlags.NonPublic | BindingFlags.Static),
				   typeof(Hooks).GetMethod("GetTalkingPlayerNameHook")
				);

				Hook getValueHook = new Hook(
					typeof(dfLanguageManager).GetMethod("GetValue", BindingFlags.Public | BindingFlags.Instance),
					typeof(Hooks).GetMethod("GetValueHook")
				);

				Hook lateStartHook = new Hook(
					typeof(Foyer).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(Hooks).GetMethod("LateStart")
				);

				Hook punchoutUIHook = new Hook(
					typeof(PunchoutPlayerController).GetMethod("UpdateUI", BindingFlags.Public | BindingFlags.Instance),
					typeof(Hooks).GetMethod("PunchoutUpdateUI")
				);

				Hook foyerCallbacksHook = new Hook(
					typeof(Foyer).GetMethod("SetUpCharacterCallbacks", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(Hooks).GetMethod("FoyerCallbacks2")

				);
				//ETGModConsole.Log("charapi hooks: 1");
				Hook languageManagerHook = new Hook(
					typeof(dfControl).GetMethod("getLocalizedValue", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(Hooks).GetMethod("DFGetLocalizedValue")
				);

				var braveSETypes = new Type[]
				{
					typeof(string),
					typeof(string),
				};
				Hook braveLoad = new Hook(
					typeof(BraveResources).GetMethod("Load", BindingFlags.Public | BindingFlags.Static, null, braveSETypes, null),
					typeof(Hooks).GetMethod("BraveLoadObject")
				);

				Hook playerSwitchHook = new Hook(
					typeof(Foyer).GetMethod("PlayerCharacterChanged", BindingFlags.Public | BindingFlags.Instance),
					typeof(Hooks).GetMethod("OnPlayerChanged")
				);
				//ETGModConsole.Log("charapi hooks: 2");
				Hook clearP1Hook = new Hook(
					typeof(ETGModConsole).GetMethod("SwitchCharacter", BindingFlags.NonPublic | BindingFlags.Instance),
					typeof(Hooks).GetMethod("PrimaryPlayerSwitched")
				);

				Hook clearP2Hook = new Hook(
					typeof(GameManager).GetMethod("ClearSecondaryPlayer", BindingFlags.Public | BindingFlags.Instance),
					typeof(Hooks).GetMethod("OnP2Cleared")
				);

				Hook setWinPicHook = new Hook(
					typeof(AmmonomiconDeathPageController).GetMethod("SetWinPic", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("SetWinPicHook", BindingFlags.Static | BindingFlags.NonPublic)
				);

				Hook interactHook = new Hook(
					typeof(ArkController).GetMethod("Interact", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("InteractHook", BindingFlags.Static | BindingFlags.Public)
				);

				//Hook getNumMetasToQuickRestartHook = new Hook(
				//	typeof(AmmonomiconDeathPageController).GetMethod("GetNumMetasToQuickRestart", BindingFlags.Static | BindingFlags.Public),
				//	typeof(Hooks).GetMethod("GetNumMetasToQuickRestartHook", BindingFlags.Static | BindingFlags.Public)
				//);

				Hook updateHook = new Hook(
					typeof(CharacterSelectIdleDoer).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("UpdateHook", BindingFlags.Static | BindingFlags.NonPublic)
				);

				Hook onEnableHook = new Hook(
					typeof(CharacterSelectIdleDoer).GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("OnEnableHook", BindingFlags.Static | BindingFlags.NonPublic)
				);

				Hook canBeSelectedHook = new Hook(
					typeof(FoyerCharacterSelectFlag).GetMethod("CanBeSelected", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("CanBeSelectedHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook onSelectedCharacterCallbackHook = new Hook(
					typeof(FoyerCharacterSelectFlag).GetMethod("OnSelectedCharacterCallback", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("OnSelectedCharacterCallbackHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook DoGhostBlankHook = new Hook(
					typeof(PlayerController).GetMethod("DoGhostBlank", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("DoGhostBlankHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook GetBaseAnimationNameHook = new Hook(
					typeof(PlayerController).GetMethod("GetBaseAnimationName", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("GetBaseAnimationNameHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook GetPlayerStatValueHook = new Hook(
					typeof(GameStatsManager).GetMethod("GetPlayerStatValue", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("GetPlayerStatValueHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook LoadHook = new Hook(
					typeof(GameStatsManager).GetMethod("Load", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("LoadHook", BindingFlags.Static | BindingFlags.Public)
				);
				Hook SaveHook = new Hook(
					typeof(GameStatsManager).GetMethod("Save", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("SaveHook", BindingFlags.Static | BindingFlags.Public)
				);

				/*Hook LoadOptionHook = new Hook(
					typeof(GameOptions).GetMethod("Load", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("LoadOptionHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook SaveOptionsHook = new Hook(
					typeof(GameOptions).GetMethod("Save", BindingFlags.Static | BindingFlags.Public),
					typeof(Hooks).GetMethod("SaveOptionsHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook ClearOverheadElementHook = new Hook(
					typeof(FoyerCharacterSelectFlag).GetMethod("ClearOverheadElement", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("ClearOverheadElementHook", BindingFlags.Static | BindingFlags.Public)
				);

				

				Hook RegisterOverrideColorHook = new Hook(
					typeof(GameActor).GetMethod("RegisterOverrideColor", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("RegisterOverrideColorHook", BindingFlags.Static | BindingFlags.Public)
				);
				Hook DeregisterOverrideColorHook = new Hook(
					typeof(GameActor).GetMethod("DeregisterOverrideColorHook", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("DeregisterOverrideColor", BindingFlags.Static | BindingFlags.Public)
				);*/

				Hook ChangeSpecialShaderFlagHook = new Hook(
					typeof(PlayerController).GetMethod("ChangeSpecialShaderFlag", BindingFlags.Instance | BindingFlags.Public),
					typeof(Hooks).GetMethod("ChangeSpecialShaderFlagHook", BindingFlags.Static | BindingFlags.Public)
				);

				Hook GetDeathPortraitNameHook = new Hook(
					typeof(AmmonomiconDeathPageController).GetMethod("GetDeathPortraitName", BindingFlags.Instance | BindingFlags.NonPublic),
					typeof(Hooks).GetMethod("GetDeathPortraitNameHook", BindingFlags.Static | BindingFlags.NonPublic)
				);

				Hook hook3 = new Hook(typeof(PlayerController).GetProperty("LocalShaderName", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), typeof(Hooks).GetMethod("LocalShaderNameGetHook"));

				//BotsModule.Log("hooks done");
			}
			catch (Exception e)
			{
				ToolsCharApi.PrintException(e);
			}
		}

		private static string GetDeathPortraitNameHook(Func<AmmonomiconDeathPageController, string> orig, AmmonomiconDeathPageController self)
		{
			if ((int)GameManager.Instance.PrimaryPlayer.characterIdentity > 10 && GameManager.Instance.PrimaryPlayer.gameObject.GetComponent<CustomCharacter>() != null)
            {
				ETGModConsole.Log($"coop_page_death_{GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.nameShort}_001");
				return $"coop_page_death_{GameManager.Instance.PrimaryPlayer.gameObject.GetComponent<CustomCharacter>().data.nameShort}_001";

			}
			else
            {
				return orig(self);
			}
			
		}

		public static void LateStart(Action<Foyer> orig, Foyer self)
		{
			orig(self);

			try
			{


				//ToolsCharApi.Print("Late start called");
				//if (hasInitialized) return;
				//ToolsCharApi.StartTimer("Initializing mod");

				//Loader.Init();

				//ToolsCharApi.StopTimerAndReport("Initializing mod");
				//hasInitialized = true;



				//BotsModule.Log("List of Custom Characters From Enter the Beyond:", LOST_COLOR);

				//foreach (var character in CharacterBuilder.storedCharacters)
				//{
				//	ETGModConsole.Log("    " + (FoyerCharacterHandler.CheckUnlocked(character.Value.First) == false ? "[Locked] " : "") + character.Value.First.nameShort, (FoyerCharacterHandler.CheckUnlocked(character.Value.First) == false ? LOCKED_CHARACTOR_COLOR : "#00ff44"));
				//}

			}
			catch (Exception e)
			{
				BotsModule.Log("(late start) Characters Broke (crying is a valid response)", "#eb1313");
				BotsModule.Log(string.Format(e + ""), "#eb1313");
			}

		}

		public static string LocalShaderNameGetHook(Func<PlayerController, string> orig, PlayerController self)
		{

			bool flag = !GameOptions.SupportsStencil;
			string result;
			if (flag)
			{
				result = "Brave/PlayerShaderNoStencil";
			}
			else
			{
				if (self.characterIdentity > (PlayableCharacters)10)
				{
					if (self.gameObject.GetComponent<CustomCharacter>().data == null)
					{
						//ETGModConsole.Log($"[Charapi]: custom character data nulled... thats really bad");
						if (!self.gameObject.GetComponent<CustomCharacter>().GetData())
						{
							ETGModConsole.Log($"[Charapi]: custom character data nulled... seems it dosent exists... fuck!");
						}						
					}

					if (self.IsUsingAlternateCostume && self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial != null)
					{

						if (self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial.GetTexture("_MainTex") != self.sprite.renderer.material.GetTexture("_MainTex"))
						{
							self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
						}
						self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial;
						result = self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial.shader.name;


					}
					else if (self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial != null)
					{
						
						if (self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial.GetTexture("_MainTex") != self.sprite.renderer.material.GetTexture("_MainTex"))
						{
							self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
						}
						self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial;
						result = self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial.shader.name;

					}
					else
					{
						result = orig(self);
					}

				}
				else
				{
					result = orig(self);
				}
			}
			return result;
		}


		/*public static bool SaveOptionsHook(Func<GameOptions, bool> orig)
		{
			ETGModConsole.Log($"Quick start character set to: \"{GameManager.Options.LastPlayedCharacter}\"");

			if (!SaveFileBullShit.HasInstance)
			{
				SaveFileBullShit.Load();
			}

			if ((int)SaveFileBullShit.Instance.customLastPlayedCharacter > 10)
			{
				GameManager.Options.LastPlayedCharacter = (PlayableCharacters)SaveFileBullShit.Instance.customLastPlayedCharacter;
			} 
			

			SaveFileBullShit.Save();

			return orig(GameManager.Options);
		}

		public static void LoadOptionHook(Action<GameOptions> orig)
		{
			orig(GameManager.Options);

			if (!SaveFileBullShit.HasInstance)
			{
				SaveFileBullShit.Load();
			}

			if ((int)SaveFileBullShit.Instance.customLastPlayedCharacter > 10)
			{
				GameManager.Options.LastPlayedCharacter = (PlayableCharacters)SaveFileBullShit.Instance.customLastPlayedCharacter;
			}

			//if ((int)SaveFileBullShit.Instance.customLastPlayedCharacter > 10)
            //{
				GameManager.Options.LastPlayedCharacter = (PlayableCharacters)SaveFileBullShit.Instance.customLastPlayedCharacter;
			//}
			
		}*/

		//stupid janky hooks that only exist so we can have things glow >:|
		public static void ChangeSpecialShaderFlagHook(Action<PlayerController, int, float> orig, PlayerController self, int flagIndex, float val)
		{
			if ((int)self.characterIdentity > 10 && self.gameObject.GetComponent<CustomCharacter>())
            {
				if (val == 0)
                {
					if (self.gameObject.GetComponent<CustomCharacter>().data.useGlow)
                    {
						if (self.IsUsingAlternateCostume && self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial != null)
						{
							if (self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial.GetTexture("_MainTex") != self.sprite.renderer.material.GetTexture("_MainTex"))
							{
								self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
							}
							self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial;
						}
						else if (self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial != null)
						{
							if (self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial.GetTexture("_MainTex") != self.sprite.renderer.material.GetTexture("_MainTex"))
                            {
								self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
							}
							self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial;
						} 
						else
                        {
							ETGModConsole.Log($"[Charapi]: a glow material nulled");
                        }
					}					
                }
				else
                {
					if (self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial.GetTexture("_MainTex") != self.sprite.renderer.material.GetTexture("_MainTex"))
					{
						self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
					}
					self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial;				
				}
            }
			orig(self, flagIndex, val);
		}

		public static void RegisterOverrideColorHook(Action<GameActor, Color, string> orig, GameActor self, Color overrideColor, string source)
		{
			//
			if (self is PlayerController && (int)(self as PlayerController).characterIdentity > 10 && self.gameObject.GetComponent<CustomCharacter>() != null)
			{
				ETGModConsole.Log($"[Charapi]: (RegisterOverrideColorHook) 1");
				//self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
				if (self.gameObject.GetComponent<CustomCharacter>().data == null)
				{
					ETGModConsole.Log($"[Charapi]: custom character data nulled... thats really bad");
					if (!self.gameObject.GetComponent<CustomCharacter>().GetData())
					{
						ETGModConsole.Log($"[Charapi]: custom character data nulled... seems it dosent exists... fuck!");
					}
					ETGModConsole.Log($"[Charapi]: (RegisterOverrideColorHook) 2");
				}
				ETGModConsole.Log($"[Charapi]: (RegisterOverrideColorHook) 3");

				for (int i = 0; i < self.healthHaver.bodySprites.Count; i++)
				{
					if (self.healthHaver.bodySprites[i])
					{
						self.healthHaver.bodySprites[i].usesOverrideMaterial = true;
						self.healthHaver.bodySprites[i].renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial;
					}
					else
                    {
						ETGModConsole.Log($"[Charapi]: i hate you");
					}
				}

				if (self.renderer == null)
                {
					ETGModConsole.Log($"[Charapi]: i hate you");
				}

				//self.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial;
				ETGModConsole.Log($"[Charapi]: (RegisterOverrideColorHook) 4");
			}
			orig(self, overrideColor, source);
		}

		public static void DeregisterOverrideColorHook(Action<GameActor, string> orig, GameActor self, string source)
		{
			orig(self, source);
			if (self is PlayerController && (int)(self as PlayerController).characterIdentity > 10 && self.gameObject.GetComponent<CustomCharacter>())
			{
				if (self.gameObject.GetComponent<CustomCharacter>().data.useGlow)
				{
					if ((self as PlayerController).IsUsingAlternateCostume && self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial != null)
					{
						self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
						self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.altGlowMaterial;
					}
					else if (self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial != null)
					{
						self.gameObject.GetComponent<CustomCharacter>().data.normalMaterial.SetTexture("_MainTexture", self.sprite.renderer.material.GetTexture("_MainTex"));
						self.sprite.renderer.material = self.gameObject.GetComponent<CustomCharacter>().data.glowMaterial;
					}
					else
					{
						ETGModConsole.Log($"[Charapi]: a glow material nulled");
					}
				}
			}			
		}
		

		public static void LoadHook(Action<GameStatsManager> orig)
		{
			ETGModConsole.Log($"loading save file...");
			orig(GameStatsManager.Instance);
			if (!SaveFileBullShit.HasInstance)
            {
				SaveFileBullShit.Load();
			}

			foreach (var characterStats in SaveFileBullShit.Instance.m_customCharacterStats)
			{
				var character = characterStats.Key;
				var stats = characterStats.Value;
				ETGModConsole.Log($"Found custom character \"{character}\" adding them to m_characterStats");
				if (!GameStatsManager.Instance.m_characterStats.ContainsKey((PlayableCharacters)character) && stats != null)
				{
					GameStatsManager.Instance.m_characterStats.Add((PlayableCharacters)character, stats);
				}
				else if (!GameStatsManager.Instance.m_characterStats.ContainsKey((PlayableCharacters)character) && stats == null)
				{
					GameStatsManager.Instance.m_characterStats.Add((PlayableCharacters)character, new GameStats());
				}
				else if (GameStatsManager.Instance.m_characterStats.ContainsKey((PlayableCharacters)character) && stats != null)
				{
					GameStatsManager.Instance.m_characterStats[(PlayableCharacters)character] = stats;
				}
				else if (GameStatsManager.Instance.m_characterStats.ContainsKey((PlayableCharacters)character) && stats == null)
				{
					GameStatsManager.Instance.m_characterStats[(PlayableCharacters)character] = new GameStats();
				}

			}
		}

		public static bool SaveHook(Func<GameStatsManager, bool> orig)
		{

			if (!SaveFileBullShit.HasInstance)
            {
				SaveFileBullShit.Load();
			}

			foreach (var characterStats in GameStatsManager.Instance.m_characterStats)
			{
				
				var character = characterStats.Key;
				var stats = characterStats.Value;
				if ((int)character > 10)
                {
					ETGModConsole.Log($"Found character \"{character}\" moving them to the custom save file");
					if (!SaveFileBullShit.Instance.m_customCharacterStats.ContainsKey((CustomPlayableCharacters)character))
					{
						SaveFileBullShit.Instance.m_customCharacterStats.Add((CustomPlayableCharacters)character, new GameStats());
					}
					else if (SaveFileBullShit.Instance.m_customCharacterStats.ContainsKey((CustomPlayableCharacters)character) && stats != null)
					{
						SaveFileBullShit.Instance.m_customCharacterStats[(CustomPlayableCharacters)character] = stats;				
					}
				}
				
			}
			SaveFileBullShit.Save();
			//foreach(var character in thingsToRemove)
			//{
			//	if (GameStatsManager.Instance.m_characterStats.ContainsKey(character))
			//    {
			//		GameStatsManager.Instance.m_characterStats.Remove(character);
			//	}
			//	
			//}
			return orig(GameStatsManager.Instance);
		}


		public static float GetPlayerStatValueHook(Func<GameStatsManager, TrackedStats, float> orig, GameStatsManager self, TrackedStats stat)
		{			
			float statValue = orig(self, stat);

			foreach(var whydodgerollmustyouhurtmelikethis in self.m_characterStats)
            {				
				if(whydodgerollmustyouhurtmelikethis.Key > (PlayableCharacters)10)
                {
					statValue += whydodgerollmustyouhurtmelikethis.Value.GetStatValue(stat);
				}				
			}

			return statValue;
		}


		public static void ClearOverheadElementHook(Action<FoyerCharacterSelectFlag> orig, FoyerCharacterSelectFlag self)
		{
			FieldInfo _extantOverheadUIElement = typeof(FoyerCharacterSelectFlag).GetField("m_extantOverheadUIElement", BindingFlags.NonPublic | BindingFlags.Instance);

			if ((_extantOverheadUIElement.GetValue(self) as dfControl) != null && FakePrefab.IsFakePrefab((_extantOverheadUIElement.GetValue(self) as dfControl).gameObject))
			{
				(_extantOverheadUIElement.GetValue(self) as dfControl).gameObject.SetActive(false);
				//UnityEngine.Object.Destroy((_extantOverheadUIElement.GetValue(self) as dfControl).gameObject);
				_extantOverheadUIElement.SetValue(self, null);
			} 
			else
            {
				orig(self);
            }
		}

		public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
		public static string GetBaseAnimationNameHook(Func<PlayerController, Vector2, float, bool, bool, string> orig, PlayerController self, Vector2 v, float gunAngle, bool invertThresholds = false, bool forceTwoHands = false)
        {
			if (!string.IsNullOrEmpty(self.gameObject?.GetComponent<CustomCharacter>()?.overrideAnimation))
			{
				return self.gameObject?.GetComponent<CustomCharacter>()?.overrideAnimation;
			} 
			else
            {
				return orig(self, v, gunAngle, invertThresholds, forceTwoHands);
			}
			
        }

		public static void DoGhostBlankHook(Action<PlayerController> orig, PlayerController self)
		{
			if(self.gameObject.GetComponent<CustomCharacter>()?.data.coopBlankReplacement != null)
            {
				FieldInfo _blankCooldownTimer = typeof(PlayerController).GetField("m_blankCooldownTimer", BindingFlags.NonPublic | BindingFlags.Instance);
				self.QueueSpecificAnimation("ghost_sneeze_right");
				ReflectionHelper.SetValue(_blankCooldownTimer, self, self.gameObject.GetComponent<CustomCharacter>().data.coopBlankReplacement(self));
			} 
			else
            {
				orig(self);
            }

			//if (action != null)
			//{
			//
			//}
			// else 
			//{
			//	call orig
			/*}

			if (ghostProj == null)
            {
				ghostProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
				ghostProj.gameObject.SetActive(false);

				ghostProj.baseData.speed = 50;
				ghostProj.baseData.UsesCustomAccelerationCurve = true;
				ghostProj.baseData.AccelerationCurve = new AnimationCurve
				{
					postWrapMode = WrapMode.Clamp,
					preWrapMode = WrapMode.Clamp,
					keys = new Keyframe[]
					{
					new Keyframe
					{
						time = 0f,
						value = 0f,
						inTangent = 0f,
						outTangent = 0f
					},
					new Keyframe
					{
						time = 1f,
						value = 1f,
						inTangent = 2f,
						outTangent = 2f
					},
					}
				};
				ghostProj.baseData.CustomAccelerationCurveDuration = 0.3f;
				ghostProj.baseData.IgnoreAccelCurveTime = 0f;
				ghostProj.shouldRotate = true;
				ghostProj.SetProjectileSpriteRight("lost_ghost_blank_proj", 9, 7, true, tk2dBaseSprite.Anchor.LowerLeft);
			}
			self.QueueSpecificAnimation("ghost_sneeze_right");		
			for (int i = 0; i < 8; i++)
			{
				//BotsModule.Log(i + ": proj hopefully spawned");
				GameObject gameObject = SpawnManager.SpawnProjectile(ghostProj.gameObject, self.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, i * 45), true);
				gameObject.SetActive(true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = self;
				component.Shooter = self.specRigidbody;
			}
			self.m_blankCooldownTimer = 5f;*/
		}
		private static void UpdateHook(Action<CharacterSelectIdleDoer> orig, CharacterSelectIdleDoer self)
		{
			if (self.GetComponent<CustomCharacterFoyerController>() != null && self.GetComponent<CustomCharacterFoyerController>().useGlow && self.sprite.renderer.material != self.GetComponent<CustomCharacterFoyerController>().data.glowMaterial)
			{
				var character = self.GetComponent<CustomCharacterFoyerController>();
				self.sprite.usesOverrideMaterial = true;
				self.sprite.renderer.material = character.data.glowMaterial;

			}
			orig(self);
		}

		public static void OnSelectedCharacterCallbackHook(Action<FoyerCharacterSelectFlag, PlayerController> orig, FoyerCharacterSelectFlag self, PlayerController newCharacter)
		{
			orig(self, newCharacter);
			//ETGModConsole.Log($"{newCharacter.gameObject.name} - {self.CharacterPrefabPath}");
			if (newCharacter.gameObject.name.ToLower().Contains(self.CharacterPrefabPath.ToLower(), false))
			{
				if (self.GetComponent<CustomCharacterFoyerController>() != null && self.GetComponent<CustomCharacterFoyerController>().metaCost > 0)
				{

					self.gameObject.SetActive(false);
					self.GetComponent<SpeculativeRigidbody>().enabled = false;

					GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY, -(self.GetComponent<CustomCharacterFoyerController>().metaCost));
				}
			}
		}

		private static void OnEnableHook(Action<CharacterSelectIdleDoer> orig, CharacterSelectIdleDoer self)
		{
			if (self.GetComponent<CustomCharacterFoyerController>() != null && self.GetComponent<CustomCharacterFoyerController>().useGlow && self.sprite.renderer.material != self.GetComponent<CustomCharacterFoyerController>().data.glowMaterial)
			{
				var character = self.GetComponent<CustomCharacterFoyerController>();
				self.sprite.usesOverrideMaterial = true;
				self.sprite.renderer.material = character.data.glowMaterial;
			}

			orig(self);
		}

		public static bool CanBeSelectedHook(Func<FoyerCharacterSelectFlag, bool> orig, FoyerCharacterSelectFlag self)
		{
			if (self.GetComponent<CustomCharacterFoyerController>() != null && self.GetComponent<CustomCharacterFoyerController>().metaCost > 0)
			{
				return (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= self.GetComponent<CustomCharacterFoyerController>().metaCost);
			} 
			else
			{
				return orig(self);
			}
			
		}

		public static QuickRestartOptions GetNumMetasToQuickRestartHook(Func<QuickRestartOptions> orig)
		{


			
			QuickRestartOptions result = orig();

			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].characterIdentity > (PlayableCharacters)10)
				{
					result.NumMetas += GameManager.Instance.AllPlayers[i].GetComponent<CustomCharacter>().data.metaCost;
				}
			}
			return result;
		}
        #region Dumb Bad Hooks
        

		public static void InteractHook(Action<ArkController, PlayerController> orig, ArkController self, PlayerController interactor)
		{

			if ((int)interactor.characterIdentity > 10 && ETGModMainBehaviour.Instance.gameObject.GetComponent("CharApiHiveMind") != null)
            {
				FieldInfo _hasBeenInteracted = typeof(ArkController).GetField("m_hasBeenInteracted", BindingFlags.NonPublic | BindingFlags.Instance);

				SpriteOutlineManager.RemoveOutlineFromSprite(self.sprite, false);
				SpriteOutlineManager.RemoveOutlineFromSprite(self.LidAnimator.sprite, false);
				if (!(bool)_hasBeenInteracted.GetValue(self))
				{
					_hasBeenInteracted.SetValue(self, true);
				}
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					GameManager.Instance.AllPlayers[i].RemoveBrokenInteractable(self);
				}
				BraveInput.DoVibrationForAllPlayers(Vibration.Time.Normal, Vibration.Strength.Medium);
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(interactor);
					float num = Vector2.Distance(otherPlayer.CenterPosition, interactor.CenterPosition);
					if (num > 8f || num < 0.75f)
					{
						Vector2 a = Vector2.right;
						if (interactor.CenterPosition.x < self.ChestAnimator.sprite.WorldCenter.x)
						{
							a = Vector2.left;
						}
						otherPlayer.WarpToPoint(otherPlayer.transform.position.XY() + a * 2f, true, false);
					}
				}
				var comp = ETGModMainBehaviour.Instance.gameObject.GetComponent("CharApiHiveMind");
				
				self.StartCoroutine(comp.GetType().GetMethod("Open", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { self, interactor }) as IEnumerator);
			}
			else
            {
				orig(self, interactor);
			}
		}

		


		
		
        #endregion
        //Hook for Punchout UI being updated (called when UI updates)
        public static void PunchoutUpdateUI(Action<PunchoutPlayerController> orig, PunchoutPlayerController self)
        {
            orig(self);
            var customChar = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>();
            if (customChar != null)
            {
                char index = self.PlayerUiSprite.SpriteName.Last();
                SpriteHandler.HandlePunchoutSprites(self, customChar.data);
                if (customChar.data.punchoutFaceCards != null)
                {
                    self.PlayerUiSprite.SpriteName = customChar.data.nameInternal + "_punchout_facecard" + index;
                    ToolsCharApi.Print(self.PlayerUiSprite.SpriteName);
                }
            }
        }
		
        public static string GetTalkingPlayerNickHook(Func<string> orig)
        {
            PlayerController talkingPlayer = Hooks.GetTalkingPlayer();
            if (talkingPlayer.IsThief)
            {
                return "#THIEF_NAME";
            }
            if(talkingPlayer.GetComponent<CustomCharacter>() != null)
            {
                if (talkingPlayer.GetComponent<CustomCharacter>().data != null)
                {
                    return "#PLAYER_NICK_" + talkingPlayer.GetComponent<CustomCharacter>().data.nickname.ToUpper();
                }
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
            {
                return "#PLAYER_NICK_RANDOM";
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
            {
                return "#PLAYER_NICK_GUNSLINGER";
            }
            return "#PLAYER_NICK_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
        }

        public static string GetValueHook(Func<dfLanguageManager, string, string> orig, dfLanguageManager self, string key)
        {
            if (characterDeathNames.Contains(key))
            {
                if(GameManager.Instance.PrimaryPlayer != null && GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>() != null && GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data != null)
                {
                    return GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.name;
                }
            }
            return orig(self, key);
        }

        public static string GetTalkingPlayerNameHook(Func<string> orig)
        {
            PlayerController talkingPlayer = Hooks.GetTalkingPlayer();
            if (talkingPlayer.IsThief)
            {
                return "#THIEF_NAME";
            }
            if (talkingPlayer.GetComponent<CustomCharacter>() != null)
            {
                if (talkingPlayer.GetComponent<CustomCharacter>().data != null)
                {
                    return "#PLAYER_NAME_" + talkingPlayer.GetComponent<CustomCharacter>().data.nameShort.ToUpper();
                }
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Eevee)
            {
                return "#PLAYER_NAME_RANDOM";
            }
            if (talkingPlayer.characterIdentity == PlayableCharacters.Gunslinger)
            {
                return "#PLAYER_NAME_GUNSLINGER";
            }
            return "#PLAYER_NAME_" + talkingPlayer.characterIdentity.ToString().ToUpperInvariant();
        }

        private static PlayerController GetTalkingPlayer()
        {
            List<TalkDoerLite> allNpcs = StaticReferenceManager.AllNpcs;
            for (int i = 0; i < allNpcs.Count; i++)
            {
                if (allNpcs[i])
                {
                    if (!allNpcs[i].IsTalking || !allNpcs[i].TalkingPlayer || GameManager.Instance.HasPlayer(allNpcs[i].TalkingPlayer))
                    {
                        if (allNpcs[i].IsTalking && allNpcs[i].TalkingPlayer)
                        {
                            return allNpcs[i].TalkingPlayer;
                        }
                    }
                }
            }
            return GameManager.Instance.PrimaryPlayer;
        }
		

		//Triggers FoyerCharacterHandler (called from Foyer.SetUpCharacterCallbacks)
		
		public static List<FoyerCharacterSelectFlag> FoyerCallbacks2(Func<Foyer, List<FoyerCharacterSelectFlag>> orig, Foyer self)
		{
			var sortedByX = orig(self);

			var sortedByXCustom = FoyerCharacterHandler.AddCustomCharactersToFoyer(sortedByX);

			foreach(var character in sortedByXCustom)
			{
				sortedByX.Add(character);
				//self.OnPlayerCharacterChanged = character.OnSelectedCharacterCallback;
			}

			return sortedByX;
		}

        //Used to add in strings 
        public static string DFGetLocalizedValue(Func<dfControl, string, string> orig, dfControl self, string key)
        {
            foreach (var pair in StringHandler.customStringDictionary)
            {
                if (pair.Key.ToLower() == key.ToLower())
                {
                    return pair.Value;
                }
            }
            return orig(self, key);
        }

        //Used to set fake player prefabs to active on instantiation (hook doesn't work on this call)
        public static Object BraveLoadObject(Func<string, string, Object> orig, string path, string extension = ".prefab")
        {
            var value = orig(path, extension);
            if (value == null)
            {
                path = path.ToLower();
                if (CharacterBuilder.storedCharacters.ContainsKey(path))
                {
                    var character = CharacterBuilder.storedCharacters[path].Second;
                    return character;
                }
            }
            return value;
        }

        public static void OnPlayerChanged(Action<Foyer, PlayerController> orig, Foyer self, PlayerController player)
        {
            ResetInfiniteGuns();
            orig(self, player);
        }

        public static void OnP2Cleared(Action<GameManager> orig, GameManager self)
        {
            orig(self);
            ResetInfiniteGuns();
        }

        public static void PrimaryPlayerSwitched(Action<ETGModConsole, string[]> orig, ETGModConsole self, string[] args)
        {
            try
            {
                orig(self, args);
            }
            catch { }
            ResetInfiniteGuns();
        }

        //Resets all the character-specific infinite guns 
        public static Dictionary<int, GunBackupData> gunBackups = new Dictionary<int, GunBackupData>();
        public static void ResetInfiniteGuns()
        {
            var player1 = GameManager.Instance?.PrimaryPlayer?.GetComponent<CustomCharacter>();
            var player2 = GameManager.Instance?.SecondaryPlayer?.GetComponent<CustomCharacter>();
            List<int> removables = new List<int>();
            foreach (var entry in gunBackups)
            {
                if ((player1 && player1.GetInfiniteGunIDs().Contains(entry.Key)) || (player2 && player2.GetInfiniteGunIDs().Contains(entry.Key))) continue;
                var gun = PickupObjectDatabase.GetById(entry.Key) as Gun;
                gun.InfiniteAmmo = entry.Value.InfiniteAmmo;
                gun.CanBeDropped = entry.Value.CanBeDropped;
                gun.PersistsOnDeath = entry.Value.PersistsOnDeath;
                gun.PreventStartingOwnerFromDropping = entry.Value.PreventStartingOwnerFromDropping;
                removables.Add(entry.Key);
                ToolsCharApi.Print($"Reset {gun.EncounterNameOrDisplayName} to infinite = {gun.InfiniteAmmo}");
            }
            foreach (var id in removables)
                gunBackups.Remove(id);

        }
		public static Hook getOrLoadByName_Hook;
		public static Hook setWinPicHook;

		
		private static void SetWinPicHook(Action<AmmonomiconDeathPageController> orig, AmmonomiconDeathPageController self)
		{
			orig(self);
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			if (GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON && GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>()
				&& (bool)typeof(AmmonomiconDeathPageController).GetMethod("ShouldUseJunkPic", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null))
            {
				if (GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data == null || GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.junkanWinPic == null)
                {
					self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
				}
				else
                {
					self.photoSprite.Texture = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.junkanWinPic;
				}
				
			}
			else if (tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
			{
				self.photoSprite.Texture = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.pastWinPic;
			}
			else
			{
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
			}
			/*
			if ((bool)typeof(AmmonomiconDeathPageController).GetMethod("ShouldUseJunkPic", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, null) && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
			{
				switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
				{
					case PlayableCharacters.Pilot:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_pilot_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Convict:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_convict_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Robot:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_robot_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Soldier:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_marine_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Guide:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_hunter_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Bullet:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_bullet_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Eevee:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_eevee_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Gunslinger:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_slinger_001", ".png") as Texture);
						goto IL_1B4;
				}
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
				IL_1B4:;
			}
			else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_BossRush_001", ".png") as Texture);
			}
			else
			{
				
				if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
						{
							self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
						}
						else
						{
							switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
							{
								case PlayableCharacters.Pilot:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Pilot_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Convict:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Convict_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Robot:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Robot_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Soldier:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Marine_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Guide:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Hunter_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Bullet:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Bullet_001", ".png") as Texture);
									goto IL_384;
							}
							self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
							IL_384:;
						}
					}
					else if (GameManager.IsGunslingerPast)
					{
						self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Slinger_001", ".png") as Texture);
					}
					else
					{
						self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Lich_Kill_001", ".png") as Texture);
					}
				}
				else
				{
					self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
				}
			}*/
		}
		/*
		private static void SetWinPicHook(Action<AmmonomiconDeathPageController> orig, AmmonomiconDeathPageController self)
		{

			//BotsModule.Log("setWinPicHook: 1", BotsModule.LOCKED_CHARACTOR_COLOR);

			if (ShouldUseJunkPic() && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
			{
				switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
				{
					case PlayableCharacters.Pilot:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_pilot_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Convict:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_convict_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Robot:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_robot_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Soldier:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_marine_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Guide:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_hunter_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Bullet:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_bullet_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Eevee:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_eevee_001", ".png") as Texture);
						goto IL_1B4;
					case PlayableCharacters.Gunslinger:
						self.photoSprite.Texture = (BraveResources.Load("win_pic_junkan_slinger_001", ".png") as Texture);
						goto IL_1B4;

					default:
                        if (GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>() != null)
                        {
                            self.photoSprite.Texture = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.pastWinPic;
                            goto IL_1B4;
                        }
                        break;

				}
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
			IL_1B4:;
			}
			else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
			{
				self.photoSprite.Texture = (BraveResources.Load("Win_Pic_BossRush_001", ".png") as Texture);
			}
			else
			{

				//BotsModule.Log("setWinPicHook: 2", BotsModule.LOCKED_CHARACTOR_COLOR);

				GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
				if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
						{
							self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
						}
						else
						{
							switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
							{
								case PlayableCharacters.Pilot:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Pilot_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Convict:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Convict_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Robot:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Robot_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Soldier:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Marine_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Guide:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Hunter_001", ".png") as Texture);
									goto IL_384;
								case PlayableCharacters.Bullet:
									self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Bullet_001", ".png") as Texture);
									goto IL_384;
								default:
                                    if (GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>() != null)
                                    {
                                        self.photoSprite.Texture = GameManager.Instance.PrimaryPlayer.GetComponent<CustomCharacter>().data.junkanWinPic;
                                        goto IL_384;
                                    }
                                    break;
							}
							self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
						IL_384:;
						}
					}
					else if (GameManager.IsGunslingerPast)
					{
						self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Slinger_001", ".png") as Texture);
					}
					else
					{
						self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Lich_Kill_001", ".png") as Texture);
					}
				}
				else
				{
					self.photoSprite.Texture = (BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture);
				}
			}
		}


		private static bool ShouldUseJunkPic()
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (playerController)
				{
					for (int j = 0; j < playerController.passiveItems.Count; j++)
					{
						if (playerController.passiveItems[j] is CompanionItem)
						{
							CompanionItem companionItem = playerController.passiveItems[j] as CompanionItem;
							if (companionItem.ExtantCompanion && companionItem.ExtantCompanion.GetComponent<SackKnightController>() && companionItem.ExtantCompanion.GetComponent<SackKnightController>().CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}*/



		public static List<string> characterDeathNames = new List<string>
        {
            "#CHAR_ROGUE_SHORT",
            "#CHAR_CONVICT_SHORT",
            "#CHAR_ROBOT_SHORT",
            "#CHAR_MARINE_SHORT",
            "#CHAR_GUIDE_SHORT",
            "#CHAR_CULTIST_SHORT",
            "#CHAR_BULLET_SHORT",
            "#CHAR_PARADOX_SHORT",
            "#CHAR_GUNSLINGER_SHORT"
        };

        public struct GunBackupData
        {
            public bool InfiniteAmmo,
                CanBeDropped,
                PersistsOnDeath,
                PreventStartingOwnerFromDropping;
        }
    }
}
