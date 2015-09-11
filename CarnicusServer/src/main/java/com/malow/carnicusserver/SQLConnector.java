package com.malow.carnicusserver;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;

import com.malow.carnicusserver.models.responses.CharacterInfoResponse;

public class SQLConnector 
{	
	public static class WrongPasswordException extends Exception
	{
		private static final long serialVersionUID = 1L;
	}
	
	public static class NoCharacterFoundException extends Exception
	{
		private static final long serialVersionUID = 2L;
	}
	  
	public static int authenticateAccount(String username, String password) throws Exception
	{
		int accountID = -1;
		Class.forName("com.mysql.jdbc.Driver");
		Connection connect = DriverManager.getConnection("jdbc:mysql://localhost/Carnicus?" + "user=CarnicusServer&password=qqiuIUr348EW");
		  
		PreparedStatement accountStatement = connect.prepareStatement("SELECT * FROM Accounts WHERE username = ? ; ");
		accountStatement.setString(1, username);
		ResultSet accountResult = accountStatement.executeQuery();
		
		if (accountResult.next()) 
		{
			String dbpw = accountResult.getString("password");
			int accid = accountResult.getInt("id");
			if(dbpw.equals(password))
			{
				accountID = accid;
			}
			else
			{
				throw new WrongPasswordException();
			}
		}
		else
		{
			System.out.println("No such account: " + username);
			throw new WrongPasswordException();
		}
		 
		accountStatement.close();
		connect.close();
		accountResult.close();
		return accountID;
	}
	
	public static CharacterInfoResponse getCharacterInfo(int accountID, long clientID) throws Exception
	{
		Class.forName("com.mysql.jdbc.Driver");
		Connection connect = DriverManager.getConnection("jdbc:mysql://localhost/Carnicus?" + "user=CarnicusServer&password=qqiuIUr348EW");
		CharacterInfoResponse response = null;
				
		PreparedStatement characterStatement = connect.prepareStatement("SELECT * FROM Characters WHERE account_id = ? ; ");
		characterStatement.setInt(1, accountID);
		ResultSet characterResult = characterStatement.executeQuery();
		
		if(characterResult.next())
		{
			String name = characterResult.getString("name");
			float posx = characterResult.getFloat("posx");
			float posy = characterResult.getFloat("posy");
			float posz = characterResult.getFloat("posz");
			
			
			response = new CharacterInfoResponse(name, posx, posy, posz, clientID);
		}
		else
		{
			throw new NoCharacterFoundException();
		}
		
		characterStatement.close();
		characterResult.close();
		connect.close();
		
		return response;
	}
	
	public static boolean updateCharacterPosition(int accountID, float x, float y, float z) throws Exception
	{
		Class.forName("com.mysql.jdbc.Driver");
		Connection connect = DriverManager.getConnection("jdbc:mysql://localhost/Carnicus?" + "user=CarnicusServer&password=qqiuIUr348EW");
		
		PreparedStatement characterStatement = connect.prepareStatement("UPDATE characters SET posx = ?, posy = ?, posz = ? WHERE account_id = ?;");
		characterStatement.setFloat(1, x);
		characterStatement.setFloat(2, y);
		characterStatement.setFloat(3, z);
		characterStatement.setInt(4, accountID);
		int rowCount = characterStatement.executeUpdate();
		characterStatement.close();
		
		if(rowCount != 1)
		{
			throw new Exception();
		}
		
		connect.close();
		
		return true;
	}
} 