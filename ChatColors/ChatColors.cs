using System;

using ScrollsModLoader.Interfaces;
using System.Text.RegularExpressions;
using UnityEngine;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace ChatColors
{
 /*  ___  _           _      ___       _               
    / __|| |_   __ _ | |_   / __| ___ | | ___  _ _  ___
   | (__ | ' \ / _` ||  _| | (__ / _ \| |/ _ \| '_|(_-<
    \___||_||_|\__,_| \__|  \___|\___/|_|\___/|_|  /__/                                                    

                    mod by levela.
              Inspired by the idea of vimes.
                 www.scrollsguide.com
   */

    public class ChatColors : BaseMod
	{
        //initialize everything here, Game is loaded at this point
        private List<String> watchedRooms = new List<string>();

		public ChatColors ()
		{
            watchedRooms.Add("trading");
            watchedRooms.Add("wtb");
		}

		public static string GetName ()
		{
            return "ChatColors";
		}

		public static int GetVersion ()
		{
            return 3;
		}

		public static MethodDefinition[] GetHooks (TypeDefinitionCollection scrollsTypes, int version)
		{
            try
            {
                return new MethodDefinition[] {
                    scrollsTypes["ChatRooms"].Methods.GetMethod("ChatMessage", new Type[]{typeof(RoomChatMessageMessage)}),
            	};
            }
            catch
            {
                return new MethodDefinition[] { };
            }
		}

		
		public override bool BeforeInvoke (InvocationInfo info, out object returnValue)
		{
            returnValue = null;

            if (info.targetMethod.Equals("ChatMessage")) // ChatMessage (received) in ChatRooms
            {                
                RoomChatMessageMessage rcmm = (RoomChatMessageMessage)info.arguments[0];
                String roomName = rcmm.roomName.ToLower();

                foreach (String roomToCheck in watchedRooms)
                {
                    if (roomName.Contains(roomToCheck)) //Restrict to trading rooms only
                    { 
                        string goldRegexString = @"\b\d+(gold|g|\sgold|\sg|)\b";
                        string sellRegexString = @"\b(WTS)(ell)?(ing)?|(sell)(ing)?\b";
                        string buyRegexString = @"\b(WTB)(uy)?(ing)?|(buy)(ing)?\b";

                        RegexOptions ignoreCase = RegexOptions.IgnoreCase;

                        Regex goldRegex = new Regex(goldRegexString, ignoreCase);
                        Regex sellRegex = new Regex(sellRegexString,ignoreCase);
                        Regex buyRegex = new Regex(buyRegexString, ignoreCase);
              
                        rcmm.text = sellRegex.Replace(rcmm.text, "<color=#ff4343>$&</color>");
                        rcmm.text = buyRegex.Replace(rcmm.text, "<color=#4eff43>$&</color>");
                        rcmm.text = goldRegex.Replace(rcmm.text, "<color=#f1f425>$&</color>");

                    }
                }
                 
            }

            return false;
		}

		public override void AfterInvoke (InvocationInfo info, ref object returnValue)
		{
			
		}
	}
}

