using System;

using ScrollsModLoader.Interfaces;
using System.Text.RegularExpressions;
using UnityEngine;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace ChatColors
{
	public class ChatColors : BaseMod
	{
        List<string> sellWords = new List<string> {"WTS", "Selling", "wts", "selling"};
        List<string> buyWords = new List<string> {"WTB", "Buying", "wtb", "buying" };
		
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
            returnValue = 0; 

            if (info.targetMethod.Equals("ChatMessage")) // ChatMessage (received) in ChatRooms
            {
                RoomChatMessageMessage rcmm = (RoomChatMessageMessage)info.arguments[0];

                string strRegex = @"\d+(gold|g|\sgold|\sg|)\b";
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
