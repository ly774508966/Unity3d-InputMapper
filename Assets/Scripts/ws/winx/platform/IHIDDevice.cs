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

namespace ws.winx.platform
{
		public interface IHIDDevice
		{
           

			IHIDInterface hidInterface {get;}
            int index { get; }
			int VID{get;}
			int PID{get;}
			string ID{ get; }
			
			IntPtr deviceHandle {get;}
			string DevicePath {get;}
            object Extension { set; get; }
            string Name { get; }
		    
          
		}
}

