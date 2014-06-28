//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using ws.winx.platform;
using System.ComponentModel;

namespace ws.winx.input
{

    public static class InputManager
    {
       

       
       
		private static InputCombination[] __inputCombinations;
		private static InputSettings __settings;//=new InputSettings();
		private static IHIDInterface __hidInterface;//=new ws.winx.platform.windows.WinHIDInterface();
        private static List<IJoystickDriver> __drivers;

		internal static IHIDInterface hidInterface{
			get{ 

				if(__hidInterface==null){
					if((Application.platform & (RuntimePlatform.WindowsPlayer | RuntimePlatform.WindowsEditor))!=0){
                        __hidInterface = new ws.winx.platform.windows.WinHIDInterface(__drivers);
                        

					}else				
					if((Application.platform & (RuntimePlatform.OSXPlayer | RuntimePlatform.OSXEditor))!=0){
						__hidInterface=new ws.winx.platform.osx.OSXHIDInterface(__drivers);
					}

					Debug.Log("init hidInterface");
				}



				return __hidInterface; }
		}

			

		public static InputSettings Settings{
			get{  if(__settings==null) __settings=new InputSettings(); return __settings;}
		}



		public static void AddDriver(IJoystickDriver driver){
            if(__drivers==null) __drivers= new List<IJoystickDriver>();
            __drivers.Add(driver);
		}



		/// <summary>
		/// Adds the state input.
		/// </summary>
		/// <param name="stateName">State name.</param>
		/// <param name="at">At.</param>
		/// <param name="combos">Combos.</param>
		public static void AddStateInput(String stateName,int at=-1,params KeyCode[] combos){
			
			AddStateInput(stateName,new InputCombination(combos),at);
			
		}


        /// <summary>
        /// Adds the state input.
        /// </summary>
        /// <param name="stateName">State name hash.</param>
        /// <param name="at">At.</param>
        /// <param name="combos">Combos.</param>
        public static void AddStateInput(int stateNameHash, int at = -1, params KeyCode[] combos)
        {

            AddStateInput(stateNameHash, new InputCombination(combos), at);

        }


		/// <summary>
		/// Adds the state input.
		/// </summary>
		/// <param name="stateName">State name.</param>
		/// <param name="at">At.</param>
		/// <param name="combos">Combos.</param>
		public static void AddStateInput(String stateName,int at=-1,params int[] combos){

            AddStateInput(stateName, new InputCombination(combos), at);
			
		}


        /// <summary>
        /// Adds the state input.
        /// </summary>
        /// <param name="stateName">State name Hash.</param>
        /// <param name="at">At.</param>
        /// <param name="combos">Combos.</param>
        public static void AddStateInput(int stateNameHash, int at = -1, params int[] combos)
        {

            AddStateInput(stateNameHash, new InputCombination(combos), at);

        }


		/// <summary>
		/// Adds the state input.
		/// </summary>
		/// <param name="stateName">State name </param>
		/// <param name="codeCombination">Code combination.
		/// just "I" for KeyCode.I
		/// or "I"+InputAction.DOUBLE_DESIGNATOR 
		///	 or "I"+InputAction.DOUBLE_DESIGNATOR+InputAction.SPACE_DESIGNATOR+(other code);
		///   or just "I(x2)" depending of InputAction.DOUBLE_DESIGNATOR value
		/// </param>
		/// <param name="at">At.Valid:-1(next) or 0(primary) and 1(secondary)</param>
		public static void AddStateInput(String stateName,String codeCombination,int at=-1){

			AddStateInput(stateName,new InputCombination(codeCombination),at);

		}

        /// <summary>
        /// Adds the state input.
        /// </summary>
        /// <param name="stateNameHash">State name hash(int) </param>
        /// <param name="codeCombination">Code combination.
        /// just "I" for KeyCode.I
        /// or "I"+InputAction.DOUBLE_DESIGNATOR 
        ///	 or "I"+InputAction.DOUBLE_DESIGNATOR+InputAction.SPACE_DESIGNATOR+(other code);
        ///   or just "I(x2)" depending of InputAction.DOUBLE_DESIGNATOR value
        /// </param>
        /// <param name="at">At.Valid:-1(next) or 0(primary) and 1(secondary)</param>
        public static void AddStateInput(int stateNameHash, String codeCombination, int at = -1)
        {

            AddStateInput(stateNameHash, new InputCombination(codeCombination), at);

        }


        /// <summary>
        /// Adds the state input.
        /// </summary>
        /// <param name="stateName">State name.</param>
        /// <param name="combos">Combos (ex. (int)KeyCode.P,(int)KeyCode.Joystick2Button12,JoystickDevice.toCode(...))</param>
        public static void AddStateInput(String stateName, params int[] combos)
        {
            AddStateInput(stateName, new InputCombination(combos));
        }


        /// <summary>
        /// Adds the state input.
        /// </summary>
        /// <param name="stateName">State name hash.</param>
        /// <param name="combos">Combos (ex. (int)KeyCode.P,(int)KeyCode.Joystick2Button12,JoystickDevice.toCode(...))</param>
        public static void AddStateInput(int stateNameHash, params int[] combos)
        {
            AddStateInput(stateNameHash, new InputCombination(combos));
        }


        /// <summary>
        /// Adds the state input.
        /// </summary>
        /// <param name="stateName">State name.</param>
        /// <param name="combination">Combination.</param>
        /// <param name="at">At.Valid:-1(next) or 0(primary) and 1(secondary)</param>
        public static void AddStateInput(string stateName, InputCombination combination, int at = -1)
        {

            if (at > 2) throw new Exception("Only indexes 0(Primary) and 1(Secondary) input are allowed");

            int stateNameHash=Animator.StringToHash(stateName);
            InputState state;

            if (!Settings.stateInputs.ContainsKey(stateNameHash))
            {
                //create InputState and add to Dictionary of state inputs
                state = __settings.stateInputs[stateNameHash] = new InputState(stateName, stateNameHash);
            }
            else
            {
                state = __settings.stateInputs[stateNameHash];
            }

            state.Add(combination, at);

        }


		/// <summary>
		/// Adds the state input.
		/// </summary>
		/// <param name="stateName">State name hash.</param>
		/// <param name="combination">Combination.</param>
		/// <param name="at">At.Valid:-1(next) or 0(primary) and 1(secondary)</param>
		public static void AddStateInput(int stateNameHash,InputCombination combination,int at=-1){

			if(at>2) throw new Exception("Only indexes 0(Primary) and 1(Secondary) input are allowed");
			
			
			InputState state;
			
			if(!Settings.stateInputs.ContainsKey(stateNameHash)){
				//create InputState and add to Dictionary of state inputs
				state=__settings.stateInputs[stateNameHash]=new InputState("GenState_"+stateNameHash,stateNameHash);
			}else{
				state=__settings.stateInputs[stateNameHash];
			}

			state.Add(combination,at);
	
		}


	

		//[Not tested] idea for expansion
		public static void PlayStateOnInputUp(Animator animator,int stateNameHash,int layer=0,float normalizedTime=0f){
					if(InputManager.GetInputUp(stateNameHash,false)) 
						animator.Play(stateNameHash,layer,normalizedTime); 
		}

		public static void PlayStateOnInputDown(Animator animator,int stateNameHash,int layer=0,float normalizedTime=0f){
			if(InputManager.GetInputDown(stateNameHash,false)) 
				animator.Play(stateNameHash,layer,normalizedTime); 
		}


		public static void CrossFadeStateOnInputUp(Animator animator,int stateNameHash,float transitionDuration=0.5f,int layer=0,float normailizeTime=0f){
				if(InputManager.GetInputUp(stateNameHash,false)) 
				animator.CrossFade(stateNameHash,transitionDuration,layer,normailizeTime);

		}

		public static void CrossFadeStateOnInputDown(Animator animator,int stateNameHash,float transitionDuration=0.5f,int layer=0,float normailizeTime=0f){
			if(InputManager.GetInputDown(stateNameHash,false)) 
				animator.CrossFade(stateNameHash,transitionDuration,layer,normailizeTime);
    	}
  
		
		
		
		/// <summary>
		/// Loads the Input settings from InputSettings.xml and deserialize into OO structure.
		/// Create your .xml settings with InputMapper Editor
		/// </summary>
		public static InputSettings loadSettings(String path="InputSettings.xml"){
			//DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int,InputCombination[]>),"Inputs","");
			DataContractSerializer serializer = new DataContractSerializer(typeof(InputSettings),"Inputs","");
			XmlReaderSettings xmlSettings=new XmlReaderSettings();
			xmlSettings.CloseInput=true;
			xmlSettings.IgnoreWhitespace=true;
			
			using(XmlReader reader=XmlReader.Create(path,xmlSettings))
			{

				__settings=(InputSettings)serializer.ReadObject(reader);

			}

			return __settings;
		}

		/// <summary>
		/// Saves the settings to InputSettings.xml.
		/// </summary>
		public static void saveSettings(String path){

			//DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int,InputCombination[]>),"Inputs","");
			DataContractSerializer serializer = new DataContractSerializer(typeof(InputSettings),"Inputs","");
			

			XmlWriterSettings xmlSettings=new XmlWriterSettings();
			xmlSettings.Indent=true;
			xmlSettings.CloseOutput=true;//this would close stream after write 
		//	xmlSettings.IndentChars="\t";
		//	xmlSettings.NewLineOnAttributes = false;
		//	xmlSettings.OmitXmlDeclaration = true;





			using(XmlWriter writer=XmlWriter.Create(path,xmlSettings))
			{
				//serializer.WriteObject(writer, __settings.stateInputs);
				serializer.WriteObject(writer,__settings);

				//Write the XML to file and close the writer.
				writer.Flush();
				writer.Close(); 


			}



		}

       
      



      

        //public void resetMap(){
        //}

		/// <summary>
		/// Gets the input of real or virutal axis(2keys used as axis) mapped to State.
		/// </summary>
		/// <returns>The input.</returns>
		/// <param name="stateNameHash">State name hash.</param>
		/// <param name="fromRange">From range.</param>
		/// <param name="toRange">To range.</param>
		/// <param name="sensitivity">Sensitivity.</param>
		/// <param name="dreadzone">Dreadzone.</param>
		/// <param name="gravity">Gravity.</param>
		public static float GetInput(int stateNameHash,float sensitivity=0.3f,float dreadzone=0.1f,float gravity=0.3f){
			__inputCombinations=__settings.stateInputs[stateNameHash].combinations;


            return __inputCombinations[0].GetAxis(sensitivity, dreadzone, gravity) + (__inputCombinations.Length == 2 && __inputCombinations[1] != null ? __inputCombinations[1].GetAxis(sensitivity, dreadzone, gravity) : 0);

		}

		/// <summary>
		/// Gets the input.
		/// </summary>
		/// <returns><c>true</c>, if input happen, <c>false</c> otherwise.</returns>
		/// <param name="stateNameHash">State name hash.</param>
		/// <param name="atOnce">If set to <c>true</c> at once.</param>
		public static bool GetInput(int stateNameHash,bool atOnce=false){
			__inputCombinations=__settings.stateInputs[stateNameHash].combinations;
            return __inputCombinations[0].GetInput(atOnce) || (__inputCombinations.Length == 2 && __inputCombinations[1] != null && __inputCombinations[1].GetInput(atOnce));
        }

		/// <summary>
		/// Gets the input up.
		/// </summary>
		/// <returns><c>true</c>, if input binded to state happen, <c>false</c> otherwise.</returns>
		/// <param name="stateNameHash">State name hash.</param>
		/// <param name="atOnce">If set to <c>true</c> at once.</param>
		public static bool GetInputUp(int stateNameHash,bool atOnce=false){
			__inputCombinations=__settings.stateInputs[stateNameHash].combinations;
            return __inputCombinations[0].GetInputUp(atOnce) || (__inputCombinations.Length == 2 && __inputCombinations[1] != null && __inputCombinations[1].GetInputUp(atOnce));
		}

		/// <summary>
		/// Gets the input down.
		/// </summary>
		/// <returns><c>true</c>, if input binded to state down happen, <c>false</c> otherwise.</returns>
		/// <param name="stateNameHash">State name hash.</param>
		/// <param name="atOnce">If set to <c>true</c> at once.</param>
		public static bool GetInputDown(int stateNameHash,bool atOnce=false){
			__inputCombinations=__settings.stateInputs[stateNameHash].combinations;
            return __inputCombinations[0].GetInputDown(atOnce) || (__inputCombinations.Length == 2 && __inputCombinations[1] != null && __inputCombinations[1].GetInputDown(atOnce));
		}



		/// <summary>
		/// Log states - inputs values to console
		/// </summary>
		public static string Log(){
			StringBuilder content=new StringBuilder();
			int i;
			//primary,secondary...
			InputCombination[] combinations;

			foreach (var keyValuPair in __settings.stateInputs)
			{
				content.AppendLine("Name:"+keyValuPair.Value.name+" Key:"+keyValuPair.Key);
				combinations=keyValuPair.Value.combinations;

				for(i=0;i<combinations.Length;i++){
					if(combinations[i]!=null)
					content.AppendLine(i+": " +combinations[i].ToString());
				}

				content.AppendLine();
				 

			}


						return content.ToString();

		}


		#region Settings
		[DataContract]
		public class InputSettings{



			

			[DataMember(Order=4)]
			public float singleClickSensitivity{
				get{ return InputAction.SINGLE_CLICK_SENSITIVITY; }
				set{ InputAction.SINGLE_CLICK_SENSITIVITY=value; }

			}

			[DataMember(Order=5)]
			public float doubleClickSensitivity{
				get{ return InputAction.DOUBLE_CLICK_SENSITIVITY; }
				set{ InputAction.DOUBLE_CLICK_SENSITIVITY=value; }
				
			}

			[DataMember(Order=6)]
			public float longClickSensitivity{
				get{ return InputAction.LONG_CLICK_SENSITIVITY; }
				set{ InputAction.LONG_CLICK_SENSITIVITY=value; }
				
			}

			[DataMember(Order=7)]
			public float combinationsClickSensitivity{
				get{ return InputAction.COMBINATION_CLICK_SENSITIVITY; }
				set{ InputAction.COMBINATION_CLICK_SENSITIVITY=value; }
				
			}

			[DataMember(Order=1)]
			public string doubleDesignator{
				get{ return InputAction.DOUBLE_DESIGNATOR; }
				set{ InputAction.DOUBLE_DESIGNATOR=value; }
				
			}


			[DataMember(Order=2)]
			public string longDesignator{
				get{ return InputAction.LONG_DESIGNATOR; }
				set{ InputAction.LONG_DESIGNATOR=value; }
				
			}


			[DataMember(Order=3)]
			public string spaceDesignator{
				get{ return InputAction.SPACE_DESIGNATOR.ToString(); }
				set{ InputAction.SPACE_DESIGNATOR=value[0]; }
				
			}

			[DataMember(Name="StateInputs",Order=8)]
			protected Dictionary<int,InputState> _stateInputs;
			
			public Dictionary<int,InputState> stateInputs{
				get {return _stateInputs;}
			}


		   public InputSettings(){
					_stateInputs=new Dictionary<int,InputState>();
		   }
		}
		#endregion
    

    }
}


