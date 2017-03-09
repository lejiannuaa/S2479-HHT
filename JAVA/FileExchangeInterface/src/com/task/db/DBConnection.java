package com.task.db;

import java.sql.Connection;
import java.sql.DriverManager;
import java.util.Hashtable;

public class DBConnection {

	public static Connection getConnect(Hashtable connectInfo) {
		Connection conn = null;
	    try {
	    	System.out.println("========getConnect=========");
	    	String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			Class.forName(driver);
			conn = DriverManager.getConnection(url, userName, pwd);
	    }catch (Exception e) {
	      System.out.println("connect Error");
	      e.printStackTrace();

	    }
	    return conn;
	}
}
