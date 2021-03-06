﻿#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ws.winx.devices;
using ws.winx.unity;
using ws.winx.drivers;
using System.IO;

namespace ws.winx.platform.android
{
    public class AndroidHIDInterface : IHIDInterface,IDisposable
    {
        #region Fields

        public const string TAG = "AndroidHIDInterface";

        private List<IDriver> __drivers;

        private IDriver __defaultJoystickDriver = new UnityDriver();
      
        GameObject _container;

		bool hidCallbacksRegistered;

        //link towards Browser
        internal readonly AndroidHIDBehaviour droidHIDBehaviour;
        private Dictionary<string, HIDDevice> __Generics;

		public event EventHandler<DeviceEventArgs<string>> DeviceDisconnectEvent;
		public event EventHandler<DeviceEventArgs<IDevice>> DeviceConnectEvent;

		private DeviceProfiles __profiles;


        #endregion


        public AndroidHIDInterface()
        {
            UnityEngine.Debug.Log("AndroidHIDInterface");
            __drivers = new List<IDriver>();
           
            __Generics = new Dictionary<string, HIDDevice>();

            _container = new GameObject("AndroidHIDBehaviourGO");



            droidHIDBehaviour = _container.AddComponent<AndroidHIDBehaviour>();
          
              
         
        }

	
        public void DeviceConnectedEventHandler(object sender, AndroidMessageArgs<AndroidJavaObject> args)
        {
            AndroidJavaObject device = args.data;
			int pid = device.Get<int> ("PID");

			if (!__Generics.ContainsKey(pid.ToString()))
            {
                // UnityEngine.Debug.Log(args.Message);
                GenericHIDDevice info = new GenericHIDDevice(__Generics.Count, device, this);

                info.hidInterface = this;

                ResolveDevice(info);
            }
        }

        public void DeviceDisconnectedEventHandler(object sender, AndroidMessageArgs<int> args)
        {

            string pid = args.data.ToString();
           
            if (__Generics.ContainsKey(pid))
            {
				HIDDevice device=__Generics[pid];
                this.droidHIDBehaviour.Log(TAG, "Device " + device.Name + " index:" + device.index+ " Removed");
                this.__Generics.Remove(pid);

				this.DeviceDisconnectEvent(this,new DeviceEventArgs<string>(pid));
               
            }



        }


        public IDriver defaultDriver
        {
            get
            {
                return __defaultJoystickDriver;
            }
             set
            {
                 __defaultJoystickDriver=value;
            }

        }



        /// <summary>
        /// Try to attach compatible driver based on device info
        /// </summary>
        /// <param name="deviceInfo"></param>
        protected void ResolveDevice(HIDDevice deviceInfo)
        {

            IDevice joyDevice = null;

            //loop thru drivers and attach the driver to device if compatible
            if (__drivers != null)
                foreach (var driver in __drivers)
                {
                    joyDevice = driver.ResolveDevice(deviceInfo);
                    if (joyDevice != null)
                    {
                       

                        Debug.Log("Device index:" + deviceInfo.index + " PID:" + deviceInfo.PID + " VID:" + deviceInfo.VID 
							          + " attached to " + driver.GetType().ToString());
                      //  this.droidHIDBehaviour.Log("AndroidHIDInterface", "Device index:"+joyDevice.ID+" PID:" + deviceInfo.PID + " VID:" + deviceInfo.VID + " attached to " + driver.GetType().ToString());
                        this.__Generics[deviceInfo.ID] = deviceInfo;

							this.DeviceConnectEvent(this,new DeviceEventArgs<IDevice>(joyDevice));

                        break;
                    }
                }

            if (joyDevice == null)
            {//set default driver as resolver if no custom driver match device
                joyDevice = defaultDriver.ResolveDevice(deviceInfo);


                if (joyDevice != null)
                {

                      // Debug.Log(__joysticks[deviceInfo.index]);
                    Debug.Log("Device index:" + deviceInfo.index + " PID:" + deviceInfo.PID + " VID:" + deviceInfo.VID + " attached to " + __defaultJoystickDriver.GetType().ToString());
                     
                   // this.droidHIDBehaviour.Log("AndroidHIDInterface", "Device index:" + joyDevice.ID + " PID:" + joyDevice.PID + " VID:" + joyDevice.VID + " attached to " + __defaultJoystickDriver.GetType().ToString() + " Path:" + deviceInfo.DevicePath + " Name:" + joyDevice.Name);
                    this.__Generics[joyDevice.ID] = deviceInfo;

							this.DeviceConnectEvent(this,new DeviceEventArgs<IDevice>(joyDevice));
                }
                else
                {
                    Debug.LogWarning("Device PID:" + deviceInfo.PID + " VID:" + deviceInfo.VID + " not found compatible driver thru WinHIDInterface!");

                }

            }


        }

       

        public Dictionary<string, HIDDevice> Generics
        {
            get { return __Generics; }
        }

		#region IHIDInterface implementation



		public HIDReport ReadDefault (string id)
		{
			throw new NotImplementedException ();
		}

		public HIDReport ReadBuffered (string id)
		{
			throw new NotImplementedException ();
		}

		public void Write (object data, string id, HIDDevice.WriteCallback callback)
		{
			throw new NotImplementedException ();
		}


	

		public void Read(string id, HIDDevice.ReadCallback callback,int timeout=0xffff)
		{
			this.__Generics[id].Read(callback,timeout);
		}
		
		public void Read(string id, HIDDevice.ReadCallback callback)
		{
			this.__Generics[id].Read(callback,0);
		}
		
		public void Write(object data, string id, HIDDevice.WriteCallback callback,int timeout=0xffff)
		{
			this.__Generics[id].Write(data,callback,timeout);
		}
		
		public void Write(object data, string id)
		{
			this.__Generics[id].Write(data);
		}

		public bool Contains (string id)
		{
			return __Generics != null && __Generics.ContainsKey (id);
		}

		Dictionary<string, HIDDevice> IHIDInterface.Generics {
			get {
				throw new NotImplementedException ();
			}
		}

		public Dictionary<string, string> Profiles {
			get {
				throw new NotImplementedException ();
			}
		}

	

		public void SetProfiles(DeviceProfiles profiles){
			__profiles = profiles;
			
			
		}
		
		
		public void LoadProfiles(string fileName){
			
			__profiles=Resources.Load<DeviceProfiles> ("DeviceProfiles");
			
		}
		
		
		public DeviceProfile LoadProfile(string key){
			
			DeviceProfile profile=null;
			
			if (__profiles.vidpidProfileNameDict.ContainsKey (key)) {
				
				string profileName=__profiles.vidpidProfileNameDict[key];

				if(__profiles.runtimePlatformDeviceProfileDict[profileName].ContainsKey(Application.platform)){
					
					profile=__profiles.runtimePlatformDeviceProfileDict[profileName][Application.platform];
				}
				
			}
			
			
			return profile;
		}


		public void AddDriver (IDriver driver)
		{
			__drivers.Add (driver);
		}





		public void Enumerate(){
			
			if(!hidCallbacksRegistered){
				droidHIDBehaviour.DeviceDisconnectedEvent += new EventHandler<AndroidMessageArgs<int>>(DeviceDisconnectedEventHandler);
				droidHIDBehaviour.DeviceConnectedEvent += new EventHandler<AndroidMessageArgs<AndroidJavaObject>>(DeviceConnectedEventHandler);
				hidCallbacksRegistered=true;
			}
			
			droidHIDBehaviour.Enumerate();
		}


		public void Write (object data, int device, HIDDevice.WriteCallback callback)
		{
			throw new NotImplementedException ();
		}


      

      



        public void Update()
        {
            throw new NotImplementedException();
        }




        #endregion     

        

    

        public void Dispose()
        {
			if(__Generics!=null){
	            foreach (KeyValuePair<string, HIDDevice> entry in __Generics)
	            {
	                entry.Value.Dispose();
	            }

	            __Generics.Clear();
			}


			Debug.Log ("Try to remove Drivers");
			if(__drivers!=null) __drivers.Clear();


        }
    }
}
#endif