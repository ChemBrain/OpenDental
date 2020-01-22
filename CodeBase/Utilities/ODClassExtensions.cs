﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBase {
	///<summary>For extensions that are not for primitives and not for UI components.</summary>
	public static class ODClassExtensions {

		public static IEnumerable<T> AsEnumerable<T>(this IEnumerable enumerable) {
			foreach(object obj in enumerable) {
				yield return (T)obj;
			}
		}
	}
}
