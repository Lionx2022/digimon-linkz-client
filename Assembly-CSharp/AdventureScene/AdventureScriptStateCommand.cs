﻿using System;

namespace AdventureScene
{
	public sealed class AdventureScriptStateCommand : AdventureBaseCommand
	{
		public const string COMMAND_NAME = "#adv_scr_state";

		private string state;

		public AdventureScriptStateCommand()
		{
			this.state = string.Empty;
			this.continueAnalyze = true;
		}

		public override string GetCommandName()
		{
			return "#adv_scr_state";
		}

		public override bool GetParameter(string[] commandParams)
		{
			bool result = false;
			try
			{
				this.state = commandParams[1];
				base.SetWaitScriptEngine(true);
				result = true;
			}
			catch
			{
				base.OnErrorGetParameter();
			}
			return result;
		}

		public override bool RunScriptCommand()
		{
			bool result = true;
			string text = this.state;
			switch (text)
			{
			case "INIT":
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.SetScriptState(AdventureScriptEngine.ScriptState.INIT);
				goto IL_B9;
			case "RUN":
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.SetScriptState(AdventureScriptEngine.ScriptState.RUN);
				goto IL_B9;
			case "END":
				ClassSingleton<AdventureSceneData>.Instance.adventureScriptEngine.SetScriptState(AdventureScriptEngine.ScriptState.END);
				goto IL_B9;
			}
			result = false;
			IL_B9:
			base.ResumeScriptEngine();
			return result;
		}
	}
}
