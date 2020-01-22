﻿using System;
using CodeBase;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ProcedureCodeT {

		///<summary>Returns a procedure code object that utilizes the procCode passed in.
		///Either returns the pre-existing code from the cache or creates a new one.  Throws an exception if procCode is longer than 15 chars.</summary>
		public static ProcedureCode CreateProcCode(string procCode,bool isCanadianLab=false) {
			//The ProcCode column on the procedurecode table is a VARCHAR(15).  MySQL will not throw an exception but will instead truncate the ProcCode.
			//Engineers might not be expecting this and might write an invalid unit test assuming that this method did what they told it to do.
			if(procCode.Length > 15) {
				throw new ODException("Invalid procCode passed into ProcedureCodeT.CreateProcCode(); Must be less than 15 characters.");
			}
			AddIfNotPresent(procCode,isCanadianLab);
			return ProcedureCodes.GetOne(procCode);
		}
		
		public static void Update(ProcedureCode procCode) {
			ProcedureCodes.Update(procCode);
			ProcedureCodes.RefreshCache();
		}

		/// <summary>Returns true if a procedureCode was added.</summary>
		public static bool AddIfNotPresent(string procCode,bool isCanadianLab=false) {
			if(!ProcedureCodes.GetContainsKey(procCode)) {
				ProcedureCodes.Insert(new ProcedureCode {
					ProcCode=procCode,
					IsCanadianLab=isCanadianLab,
				});
				ProcedureCodes.RefreshCache();
				return true;
			}
			return false;
		}

		///<summary>Clears the procedurecode table.  Does not truncate as to not let the PKs repeat.</summary>
		public static void ClearProcedureCodeTable() {
			string command="DELETE FROM procedurecode";
			DataCore.NonQ(command);
			ProcedureCodes.RefreshCache();
		}

	}
}
