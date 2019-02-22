#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_CheckConnection : GSTypedRequest<LogEventRequest_CheckConnection, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_CheckConnection() : base("LogEventRequest"){
			request.AddString("eventKey", "CheckConnection");
		}
	}
	
	public class LogChallengeEventRequest_CheckConnection : GSTypedRequest<LogChallengeEventRequest_CheckConnection, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_CheckConnection() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "CheckConnection");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_CheckConnection SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_Move : GSTypedRequest<LogEventRequest_Move, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_Move() : base("LogEventRequest"){
			request.AddString("eventKey", "Move");
		}
		
		public LogEventRequest_Move Set_Action( string value )
		{
			request.AddString("Action", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_Move : GSTypedRequest<LogChallengeEventRequest_Move, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_Move() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "Move");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_Move SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_Move Set_Action( string value )
		{
			request.AddString("Action", value);
			return this;
		}
	}
	
}
	

namespace GameSparks.Api.Messages {

		public class ScriptMessage_ChallengeStartingPlayerMessage : ScriptMessage {
		
			public new static Action<ScriptMessage_ChallengeStartingPlayerMessage> Listener;
	
			public ScriptMessage_ChallengeStartingPlayerMessage(GSData data) : base(data){}
	
			private static ScriptMessage_ChallengeStartingPlayerMessage Create(GSData data)
			{
				ScriptMessage_ChallengeStartingPlayerMessage message = new ScriptMessage_ChallengeStartingPlayerMessage (data);
				return message;
			}
	
			static ScriptMessage_ChallengeStartingPlayerMessage()
			{
				handlers.Add (".ScriptMessage_ChallengeStartingPlayerMessage", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}

}
