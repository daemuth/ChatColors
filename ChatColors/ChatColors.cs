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
        List<string> sellWords = new List<string> {"WTS", "Selling", "wts", "selling", "SELLING"};
        List<string> buyWords = new List<string> {"WTB", "Buying", "wtb", "buying", "BUYING"};
		
        //initialize everything here, Game is loaded at this point
		public ChatColors ()
		{
		}

		public static string GetName ()
		{
            return "ChatColors";
		}

		public static int GetVersion ()
		{
            return 1;
		}

		//only return MethodDefinitions you obtained through the scrollsTypes object
		//safety first! surround with try/catch and return an empty array in case it fails
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
                Console.WriteLine("Fail van!");
                return new MethodDefinition[] { };
            }
		}

		
		public override bool BeforeInvoke (InvocationInfo info, out object returnValue)
		{
            returnValue = null;

            if (info.targetMethod.Equals("ChatMessage")) // ChatMessage (received) in ChatRooms
            {
                RoomChatMessageMessage rcmm = (RoomChatMessageMessage)info.arguments[0];

                string strRegex = @"\b\d+(gold|g|\sgold|\sg|)\b";
                RegexOptions myRegexOptions = RegexOptions.IgnoreCase;
                Regex firstRegex = new Regex(strRegex, myRegexOptions);

                foreach (string x in sellWords)
                {
                    if (rcmm.text.Contains(x))
                    {
                        rcmm.text = firstRegex.Replace(rcmm.text, "<color=#f1f425>$&</color>");
                        rcmm.text = rcmm.text.ToString().Replace(x, "<color=#ff4343>"+x.ToString()+"</color>");
                    }
                }

                foreach (string x in buyWords)
                {
                    if (rcmm.text.Contains(x))
                    {
                        rcmm.text = firstRegex.Replace(rcmm.text, "<color=#f1f425>$&</color>");
                        rcmm.text = rcmm.text.ToString().Replace(x, "<color=#4eff43>" + x.ToString() + "</color>");     
                    }
                }                
            }

            else
            {            
        
            }

            return false;
		}

		public override void AfterInvoke (InvocationInfo info, ref object returnValue)
		{
			
		}
	}
}

