using Terraria.ModLoader;
using Terraria.Chat;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


namespace WhoIsMvp
{
	class DamageStorage
	{
		public static Dictionary<string, ulong> damageMap = new Dictionary<string,ulong>();
	}
	
	public class WhoIsMvp : GlobalNPC
	{
		public override void OnKill(NPC npc) 
		{
			ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("杀死了野生的" + npc.GetFullNetName() + "BOSS属性" + npc.boss), Color.Red);
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("参与击杀" + DamageStorage.damageMap.Count), Color.Red);
            if (npc.boss)
			{
				Mod.Logger.InfoFormat("damageMap size {0}", DamageStorage.damageMap.Count);
                ulong sum = 0;
				foreach(var item in DamageStorage.damageMap) 
				{
					sum += item.Value;
				}
				//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("杀死了野生的" + npc.GetFullNetName()), Color.Red);
				foreach(var item in DamageStorage.damageMap) 
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(item.Key + "造成了" + item.Value + "伤害;占比" + item.Value/sum*100 + "%"), Color.Red);
				}
                DamageStorage.damageMap.Clear();
            }
		}

		public override void OnSpawn (NPC npc, IEntitySource source)
		{
			//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("出现了，野生的" + npc.GetFullNetName() + "BOSS属性" + npc.boss), Color.Red);
			if(npc.boss)
			{
				DamageStorage.damageMap.Clear();
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("清理了 map" + DamageStorage.damageMap.Count), Color.Blue);
                //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("出现了，野生的" + npc.GetFullNetName()), Color.Red);
            }
        }

    }

	public class MyPlayer : ModPlayer
	{

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if(target.boss)
			{
                ulong d = Convert.ToUInt64(damage);
                string username = this.Player.name;
                if (DamageStorage.damageMap.ContainsKey(username))
                {
                    DamageStorage.damageMap[username] = DamageStorage.damageMap[username] + d;
                }
                else
                {
                    DamageStorage.damageMap.Add(username, d);
                }
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("参与击杀" + DamageStorage.damageMap.Count), Color.Red);
            }
			
			//ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(this.Player.name + "远程攻击了" + target.GetFullNetName() + "，伤害" +  d), Color.Yellow);
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if(target.boss)
			{
                ulong d = Convert.ToUInt64(damage);
                string username = this.Player.name;
                if (DamageStorage.damageMap.ContainsKey(username))
                {
                    DamageStorage.damageMap[username] = DamageStorage.damageMap[username] + d;
                }
                else
                {
                    DamageStorage.damageMap.Add(username, d);
                }
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("参与击杀" + DamageStorage.damageMap.Count), Color.Red);

            }
            //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(this.Player.name + "打击了" + target.GetFullNetName()+ "，伤害" +  d), Color.Yellow);
        }
    }
}