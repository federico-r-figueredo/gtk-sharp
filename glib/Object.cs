// Object.cs - GObject class wrapper implementation
//
// Authors: Bob Smith <bob@thestuff.net>
//	    Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001 Bob Smith and Mike Kestner

namespace GLib {

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Runtime.InteropServices;

	/// <summary>
	///	Object Class
	/// </summary>
	///
	/// <remarks>
	///	Wrapper class for GObject.
	/// </remarks>

	public class Object  {

		// Private class and instance members
		IntPtr _obj;
		EventHandlerList _events;
		Hashtable Data;
		static Hashtable Objects = new Hashtable();

		/// <summary>
		///	GetObject Shared Method 
		/// </summary>
		///
		/// <remarks>
		///	Used to obtain a CLI typed object associated with a 
		///	given raw object pointer. This method is primarily
		///	used to wrap object references that are returned 
		///	by either the signal system or raw class methods that
		///	return GObject references.
		/// </remarks>
		///
		/// <returns>
		///	The wrapper instance.
		/// </returns>

		public static Object GetObject(IntPtr o)
		{
			Object obj = (Object)Objects[(int)o];
			if (obj != null) return obj;
			return null; //FIXME: Call TypeParser here eventually.
		}

		/// <summary>
		///	Object Constructor
		/// </summary>
		///
		/// <remarks>
		///	Dummy constructor needed for derived classes.
		/// </remarks>

		public Object () {}

		/// <summary>
		///	Object Constructor
		/// </summary>
		///
		/// <remarks>
		///	Creates an object from a raw object reference.
		/// </remarks>

		public Object (IntPtr raw)
		{
			Raw = raw;
		}

		/// <summary>
		///	Raw Property
		/// </summary>
		///
		/// <remarks>
		///	The raw GObject reference associated with this wrapper.
		///	Only subclasses of Object can access this read/write
		///	property.  For public read-only access, use the
		///	Handle property.
		/// </remarks>

		protected IntPtr Raw {
			get {
				return _obj;
			}
			set {
				Objects [value] = this;
				_obj = value;
			}
		}       

		/// <summary>
		///	Handle Property
		/// </summary>
		///
		/// <remarks>
		///	The raw GObject reference associated with this object.
		///	Subclasses can use Raw property for read/write
		///	access.
		/// </remarks>

		public IntPtr Handle {
			get {
				return _obj;
			}
		}

		/// <summary>
		///	EventList Property
		/// </summary>
		///
		/// <remarks>
		///	A list object containing all the events for this 
		///	object indexed by the Gtk+ signal name.
		/// </remarks>

		protected EventHandlerList EventList {
			get {
				if (_events == null)
					_events = new EventHandlerList ();
				return _events;
			}
		}

		/// <summary>
		///	Equals Method
		/// </summary>
		///
		/// <remarks>
		///	Checks equivalence of two Objects.
		/// </remarks>

		public override bool Equals (object o)
		{
			if (!(o is Object))
				return false;

			return (Handle == ((Object) o).Handle);
		}

		/// <summary>
		///	GetHashCode Method
		/// </summary>
		///
		/// <remarks>
		///	Calculates a hashing value.
		/// </remarks>

		public override int GetHashCode ()
		{
			return Handle.GetHashCode ();
		}

		/// <summary>
		///	GetData Method
		/// </summary>
		///
		/// <remarks>
		///	Accesses arbitrary data storage on the Object.
		/// </remarks>

		public object GetData (String key)
		{
			if (Data == null)
				return null;

			return Data [key];
		}

		/// <summary>
		///	SetData Method
		/// </summary>
		///
		/// <remarks>
		///	Stores arbitrary data on the Object.
		/// </remarks>

		public void SetData (String key, object val)
		{
			if (Data == null)
				Data = new Hashtable ();

			Data [key] = val;
		}

		/// <summary>
		///	GetProperty Method
		/// </summary>
		///
		/// <remarks>
		///	Accesses a Value Property.
		/// </remarks>

		[DllImport("gobject-2.0")]
		static extern void g_object_get_property (
				IntPtr obj, string name, out IntPtr val);

		public void GetProperty (String name, out GLib.Value val)
		{
			IntPtr v;
			g_object_get_property (Raw, name, out v);
			val =  new GLib.Value (v, v);
		}

		/// <summary>
		///	SetProperty Method
		/// </summary>
		///
		/// <remarks>
		///	Accesses a Value Property.
		/// </remarks>

		[DllImport("gobject-2.0")]
		static extern void g_object_set_property (
				IntPtr obj, string name, IntPtr val);

		public void SetProperty (String name, GLib.Value val)
		{
			g_object_set_property (Raw, name, val.Handle);
		}

	}
}
