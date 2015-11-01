using UnityEngine;
using System.Collections;

public class Hearing {
	public delegate void EventCallback(SbiEvent e);
	public Object target;
	public Object listener;
	public string type;
	public EventCallback method;
	public static Hearing[] push(ref Hearing[] array, Hearing item) {
		int index = array.Length;
		System.Array.Resize(ref array, array.Length + 1);
		array[index] = item;
		return array;
	}
}
