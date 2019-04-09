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
		public class LogEventRequest_AcceptFriendRequest : GSTypedRequest<LogEventRequest_AcceptFriendRequest, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_AcceptFriendRequest() : base("LogEventRequest"){
			request.AddString("eventKey", "AcceptFriendRequest");
		}
		
		public LogEventRequest_AcceptFriendRequest Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_AcceptFriendRequest : GSTypedRequest<LogChallengeEventRequest_AcceptFriendRequest, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_AcceptFriendRequest() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "AcceptFriendRequest");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_AcceptFriendRequest SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_AcceptFriendRequest Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogEventRequest_ChatOpponent : GSTypedRequest<LogEventRequest_ChatOpponent, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_ChatOpponent() : base("LogEventRequest"){
			request.AddString("eventKey", "ChatOpponent");
		}
		public LogEventRequest_ChatOpponent Set_PlayerChat( GSData value )
		{
			request.AddObject("PlayerChat", value);
			return this;
		}			
		
		public LogEventRequest_ChatOpponent Set_Message( string value )
		{
			request.AddString("Message", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_ChatOpponent : GSTypedRequest<LogChallengeEventRequest_ChatOpponent, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_ChatOpponent() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "ChatOpponent");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_ChatOpponent SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_ChatOpponent Set_PlayerChat( GSData value )
		{
			request.AddObject("PlayerChat", value);
			return this;
		}
		
		public LogChallengeEventRequest_ChatOpponent Set_Message( string value )
		{
			request.AddString("Message", value);
			return this;
		}
	}
	
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
	
	public class LogEventRequest_DeclineFriendRequest : GSTypedRequest<LogEventRequest_DeclineFriendRequest, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_DeclineFriendRequest() : base("LogEventRequest"){
			request.AddString("eventKey", "DeclineFriendRequest");
		}
		
		public LogEventRequest_DeclineFriendRequest Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_DeclineFriendRequest : GSTypedRequest<LogChallengeEventRequest_DeclineFriendRequest, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_DeclineFriendRequest() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "DeclineFriendRequest");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_DeclineFriendRequest SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_DeclineFriendRequest Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogEventRequest_FindPlayers : GSTypedRequest<LogEventRequest_FindPlayers, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_FindPlayers() : base("LogEventRequest"){
			request.AddString("eventKey", "FindPlayers");
		}
		
		public LogEventRequest_FindPlayers Set_displayName( string value )
		{
			request.AddString("displayName", value);
			return this;
		}
		
		public LogEventRequest_FindPlayers Set_username( string value )
		{
			request.AddString("username", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_FindPlayers : GSTypedRequest<LogChallengeEventRequest_FindPlayers, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_FindPlayers() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "FindPlayers");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_FindPlayers SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_FindPlayers Set_displayName( string value )
		{
			request.AddString("displayName", value);
			return this;
		}
		public LogChallengeEventRequest_FindPlayers Set_username( string value )
		{
			request.AddString("username", value);
			return this;
		}
	}
	
	public class LogEventRequest_FriendRequest : GSTypedRequest<LogEventRequest_FriendRequest, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_FriendRequest() : base("LogEventRequest"){
			request.AddString("eventKey", "FriendRequest");
		}
		
		public LogEventRequest_FriendRequest Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_FriendRequest : GSTypedRequest<LogChallengeEventRequest_FriendRequest, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_FriendRequest() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "FriendRequest");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_FriendRequest SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_FriendRequest Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogEventRequest_GameLost : GSTypedRequest<LogEventRequest_GameLost, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_GameLost() : base("LogEventRequest"){
			request.AddString("eventKey", "GameLost");
		}
		
		public LogEventRequest_GameLost Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_GameLost : GSTypedRequest<LogChallengeEventRequest_GameLost, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_GameLost() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "GameLost");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_GameLost SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_GameLost Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogEventRequest_GameWon : GSTypedRequest<LogEventRequest_GameWon, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_GameWon() : base("LogEventRequest"){
			request.AddString("eventKey", "GameWon");
		}
		
		public LogEventRequest_GameWon Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_GameWon : GSTypedRequest<LogChallengeEventRequest_GameWon, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_GameWon() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "GameWon");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_GameWon SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_GameWon Set_playerID( string value )
		{
			request.AddString("playerID", value);
			return this;
		}
	}
	
	public class LogEventRequest_GetFriendRequests : GSTypedRequest<LogEventRequest_GetFriendRequests, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_GetFriendRequests() : base("LogEventRequest"){
			request.AddString("eventKey", "GetFriendRequests");
		}
	}
	
	public class LogChallengeEventRequest_GetFriendRequests : GSTypedRequest<LogChallengeEventRequest_GetFriendRequests, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_GetFriendRequests() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "GetFriendRequests");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_GetFriendRequests SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_GetFriendsList : GSTypedRequest<LogEventRequest_GetFriendsList, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_GetFriendsList() : base("LogEventRequest"){
			request.AddString("eventKey", "GetFriendsList");
		}
		
		public LogEventRequest_GetFriendsList Set_group( string value )
		{
			request.AddString("group", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_GetFriendsList : GSTypedRequest<LogChallengeEventRequest_GetFriendsList, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_GetFriendsList() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "GetFriendsList");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_GetFriendsList SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_GetFriendsList Set_group( string value )
		{
			request.AddString("group", value);
			return this;
		}
	}
	
	public class LogEventRequest_GetNewGuestUser : GSTypedRequest<LogEventRequest_GetNewGuestUser, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_GetNewGuestUser() : base("LogEventRequest"){
			request.AddString("eventKey", "GetNewGuestUser");
		}
	}
	
	public class LogChallengeEventRequest_GetNewGuestUser : GSTypedRequest<LogChallengeEventRequest_GetNewGuestUser, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_GetNewGuestUser() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "GetNewGuestUser");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_GetNewGuestUser SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_LeavingOpponent : GSTypedRequest<LogEventRequest_LeavingOpponent, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_LeavingOpponent() : base("LogEventRequest"){
			request.AddString("eventKey", "LeavingOpponent");
		}
	}
	
	public class LogChallengeEventRequest_LeavingOpponent : GSTypedRequest<LogChallengeEventRequest_LeavingOpponent, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_LeavingOpponent() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "LeavingOpponent");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_LeavingOpponent SetChallengeInstanceId( String challengeInstanceId )
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
	
	public class LogEventRequest_PlayAgain : GSTypedRequest<LogEventRequest_PlayAgain, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_PlayAgain() : base("LogEventRequest"){
			request.AddString("eventKey", "PlayAgain");
		}
		
		public LogEventRequest_PlayAgain Set_challengeID( string value )
		{
			request.AddString("challengeID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_PlayAgain : GSTypedRequest<LogChallengeEventRequest_PlayAgain, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_PlayAgain() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "PlayAgain");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_PlayAgain SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_PlayAgain Set_challengeID( string value )
		{
			request.AddString("challengeID", value);
			return this;
		}
	}
	
	public class LogEventRequest_PlayerScore : GSTypedRequest<LogEventRequest_PlayerScore, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_PlayerScore() : base("LogEventRequest"){
			request.AddString("eventKey", "PlayerScore");
		}
		public LogEventRequest_PlayerScore Set_playerAttr( long value )
		{
			request.AddNumber("playerAttr", value);
			return this;
		}			
		
		public LogEventRequest_PlayerScore Set_playerName( string value )
		{
			request.AddString("playerName", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_PlayerScore : GSTypedRequest<LogChallengeEventRequest_PlayerScore, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_PlayerScore() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "PlayerScore");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_PlayerScore SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_PlayerScore Set_playerAttr( long value )
		{
			request.AddNumber("playerAttr", value);
			return this;
		}			
		public LogChallengeEventRequest_PlayerScore Set_playerName( string value )
		{
			request.AddString("playerName", value);
			return this;
		}
	}
	
	public class LogEventRequest_SetStartingPlayer : GSTypedRequest<LogEventRequest_SetStartingPlayer, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_SetStartingPlayer() : base("LogEventRequest"){
			request.AddString("eventKey", "SetStartingPlayer");
		}
		
		public LogEventRequest_SetStartingPlayer Set_StartingPlayer( string value )
		{
			request.AddString("StartingPlayer", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_SetStartingPlayer : GSTypedRequest<LogChallengeEventRequest_SetStartingPlayer, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_SetStartingPlayer() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "SetStartingPlayer");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_SetStartingPlayer SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_SetStartingPlayer Set_StartingPlayer( string value )
		{
			request.AddString("StartingPlayer", value);
			return this;
		}
	}
	
	public class LogEventRequest_UpdateWinner : GSTypedRequest<LogEventRequest_UpdateWinner, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_UpdateWinner() : base("LogEventRequest"){
			request.AddString("eventKey", "UpdateWinner");
		}
		
		public LogEventRequest_UpdateWinner Set_WinningPlayer( string value )
		{
			request.AddString("WinningPlayer", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_UpdateWinner : GSTypedRequest<LogChallengeEventRequest_UpdateWinner, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_UpdateWinner() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "UpdateWinner");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_UpdateWinner SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_UpdateWinner Set_WinningPlayer( string value )
		{
			request.AddString("WinningPlayer", value);
			return this;
		}
	}
	
}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_HighScoreLB : GSTypedRequest<LeaderboardDataRequest_HighScoreLB,LeaderboardDataResponse_HighScoreLB>
	{
		public LeaderboardDataRequest_HighScoreLB() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "HighScoreLB");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_HighScoreLB (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_HighScoreLB SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_HighScoreLB : GSTypedRequest<AroundMeLeaderboardRequest_HighScoreLB,AroundMeLeaderboardResponse_HighScoreLB>
	{
		public AroundMeLeaderboardRequest_HighScoreLB() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "HighScoreLB");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_HighScoreLB (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HighScoreLB SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_HighScoreLB : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_HighScoreLB(GSData data) : base(data){}
		public long? MAX_playerAttr{
			get{return response.GetNumber("MAX-playerAttr");}
		}
		public string SUPPLEMENTAL_playerName{
			get{return response.GetString("SUPPLEMENTAL-playerName");}
		}
	}
	
	public class LeaderboardDataResponse_HighScoreLB : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_HighScoreLB(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLB> Data_HighScoreLB{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLB>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HighScoreLB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLB> First_HighScoreLB{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLB>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HighScoreLB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLB> Last_HighScoreLB{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLB>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HighScoreLB(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_HighScoreLB : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_HighScoreLB(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLB> Data_HighScoreLB{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLB>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HighScoreLB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLB> First_HighScoreLB{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLB>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HighScoreLB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HighScoreLB> Last_HighScoreLB{
			get{return new GSEnumerable<_LeaderboardEntry_HighScoreLB>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HighScoreLB(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {

		public class ScriptMessage_AcceptedFriendRequest : ScriptMessage {
		
			public new static Action<ScriptMessage_AcceptedFriendRequest> Listener;
	
			public ScriptMessage_AcceptedFriendRequest(GSData data) : base(data){}
	
			private static ScriptMessage_AcceptedFriendRequest Create(GSData data)
			{
				ScriptMessage_AcceptedFriendRequest message = new ScriptMessage_AcceptedFriendRequest (data);
				return message;
			}
	
			static ScriptMessage_AcceptedFriendRequest()
			{
				handlers.Add (".ScriptMessage_AcceptedFriendRequest", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_ChallengeMove : ScriptMessage {
		
			public new static Action<ScriptMessage_ChallengeMove> Listener;
	
			public ScriptMessage_ChallengeMove(GSData data) : base(data){}
	
			private static ScriptMessage_ChallengeMove Create(GSData data)
			{
				ScriptMessage_ChallengeMove message = new ScriptMessage_ChallengeMove (data);
				return message;
			}
	
			static ScriptMessage_ChallengeMove()
			{
				handlers.Add (".ScriptMessage_ChallengeMove", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
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
		public class ScriptMessage_ConnectionLost : ScriptMessage {
		
			public new static Action<ScriptMessage_ConnectionLost> Listener;
	
			public ScriptMessage_ConnectionLost(GSData data) : base(data){}
	
			private static ScriptMessage_ConnectionLost Create(GSData data)
			{
				ScriptMessage_ConnectionLost message = new ScriptMessage_ConnectionLost (data);
				return message;
			}
	
			static ScriptMessage_ConnectionLost()
			{
				handlers.Add (".ScriptMessage_ConnectionLost", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_DeclinedFriendRequest : ScriptMessage {
		
			public new static Action<ScriptMessage_DeclinedFriendRequest> Listener;
	
			public ScriptMessage_DeclinedFriendRequest(GSData data) : base(data){}
	
			private static ScriptMessage_DeclinedFriendRequest Create(GSData data)
			{
				ScriptMessage_DeclinedFriendRequest message = new ScriptMessage_DeclinedFriendRequest (data);
				return message;
			}
	
			static ScriptMessage_DeclinedFriendRequest()
			{
				handlers.Add (".ScriptMessage_DeclinedFriendRequest", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_FriendOnline : ScriptMessage {
		
			public new static Action<ScriptMessage_FriendOnline> Listener;
	
			public ScriptMessage_FriendOnline(GSData data) : base(data){}
	
			private static ScriptMessage_FriendOnline Create(GSData data)
			{
				ScriptMessage_FriendOnline message = new ScriptMessage_FriendOnline (data);
				return message;
			}
	
			static ScriptMessage_FriendOnline()
			{
				handlers.Add (".ScriptMessage_FriendOnline", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_FriendRequest : ScriptMessage {
		
			public new static Action<ScriptMessage_FriendRequest> Listener;
	
			public ScriptMessage_FriendRequest(GSData data) : base(data){}
	
			private static ScriptMessage_FriendRequest Create(GSData data)
			{
				ScriptMessage_FriendRequest message = new ScriptMessage_FriendRequest (data);
				return message;
			}
	
			static ScriptMessage_FriendRequest()
			{
				handlers.Add (".ScriptMessage_FriendRequest", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_GameDisconnected : ScriptMessage {
		
			public new static Action<ScriptMessage_GameDisconnected> Listener;
	
			public ScriptMessage_GameDisconnected(GSData data) : base(data){}
	
			private static ScriptMessage_GameDisconnected Create(GSData data)
			{
				ScriptMessage_GameDisconnected message = new ScriptMessage_GameDisconnected (data);
				return message;
			}
	
			static ScriptMessage_GameDisconnected()
			{
				handlers.Add (".ScriptMessage_GameDisconnected", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_GuestAccountDetails : ScriptMessage {
		
			public new static Action<ScriptMessage_GuestAccountDetails> Listener;
	
			public ScriptMessage_GuestAccountDetails(GSData data) : base(data){}
	
			private static ScriptMessage_GuestAccountDetails Create(GSData data)
			{
				ScriptMessage_GuestAccountDetails message = new ScriptMessage_GuestAccountDetails (data);
				return message;
			}
	
			static ScriptMessage_GuestAccountDetails()
			{
				handlers.Add (".ScriptMessage_GuestAccountDetails", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_JoinFriendTeam : ScriptMessage {
		
			public new static Action<ScriptMessage_JoinFriendTeam> Listener;
	
			public ScriptMessage_JoinFriendTeam(GSData data) : base(data){}
	
			private static ScriptMessage_JoinFriendTeam Create(GSData data)
			{
				ScriptMessage_JoinFriendTeam message = new ScriptMessage_JoinFriendTeam (data);
				return message;
			}
	
			static ScriptMessage_JoinFriendTeam()
			{
				handlers.Add (".ScriptMessage_JoinFriendTeam", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_MatchmakingGroupNumber : ScriptMessage {
		
			public new static Action<ScriptMessage_MatchmakingGroupNumber> Listener;
	
			public ScriptMessage_MatchmakingGroupNumber(GSData data) : base(data){}
	
			private static ScriptMessage_MatchmakingGroupNumber Create(GSData data)
			{
				ScriptMessage_MatchmakingGroupNumber message = new ScriptMessage_MatchmakingGroupNumber (data);
				return message;
			}
	
			static ScriptMessage_MatchmakingGroupNumber()
			{
				handlers.Add (".ScriptMessage_MatchmakingGroupNumber", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_RefreshLobbyGames : ScriptMessage {
		
			public new static Action<ScriptMessage_RefreshLobbyGames> Listener;
	
			public ScriptMessage_RefreshLobbyGames(GSData data) : base(data){}
	
			private static ScriptMessage_RefreshLobbyGames Create(GSData data)
			{
				ScriptMessage_RefreshLobbyGames message = new ScriptMessage_RefreshLobbyGames (data);
				return message;
			}
	
			static ScriptMessage_RefreshLobbyGames()
			{
				handlers.Add (".ScriptMessage_RefreshLobbyGames", Create);
	
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
