package com.task.db;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;
import java.util.Hashtable;
import java.util.List;

import org.apache.log4j.Logger;

import com.sin.db.ConnectionPool;

public class DBManager {

	private static Logger logger = Logger.getLogger(DBManager.class);

	private static Hashtable<String, ConnectionPool> ht = new Hashtable();

	private static long TIMEOUT = 6000000;//5秒没有空闲的链接则抛出null
	
	private static List columnlist = new ArrayList();

	public static ConnectionPool createPool(String url, String driver,String userName, String pwd){
		ConnectionPool pool = null;
		if (!ht.containsKey(url)) {
//			System.out.println("create pool = "+url+";"+userName+";"+pwd);
			pool = new ConnectionPool(url, 5, 10, 18000000, // milliseconds
					url, userName, pwd, driver);
			ht.put(url + ";" + userName + ";" + pwd, pool);
		}
		return pool;
	}

	public synchronized static Connection getConnection(String url,String driver, String userName, String pwd) throws SQLException {
		if (ht.containsKey(url + ";" + userName + ";" + pwd))
			return DBManager.ht.get(url + ";" + userName + ";" + pwd).getConnection(TIMEOUT);
		else
			return DBManager.createPool(url, driver, userName, pwd).getConnection(TIMEOUT);
	}

	public static ConnectionPool getPool(String url) {
		if (ht.containsKey(url))
			return DBManager.ht.get(url);
		else
			return null;
	}

	/**
	 * 
	 * @param sql 需要执行的sql语句
	 * @param o   sql语句中的参数数组
	 * @return
	 * @throws SQLException
	 */
	public static boolean executsqlAddBatch(String sql, Object[] o, Connection conn,
			PreparedStatement stmt) throws SQLException {
		boolean re = false;
		if (o != null) {
			for (int i = 0; i < o.length; i++) {
				if(o[i]==null){
					stmt.setObject(i + 1, "");
				}else{
					stmt.setObject(i + 1, o[i]);
				}
			}
			stmt.addBatch();
		}
//		stmt.executeUpdate();
		re = true;
		return re;
	}
	
	public static boolean stmtaddBatch(Object[] o,PreparedStatement stmt,int index) throws SQLException {
		boolean re = false;
		if (o != null) {
			for (int i = 0; i < o.length; i++) {
				if(o[i]==null){
					stmt.setObject(i + 1, "");
				}else{
					stmt.setObject(i + 1, o[i]);
				}
			}
			stmt.addBatch();
//			System.out.println("addBatch="+index);
		}
//		stmt.executeUpdate();
		re = true;
		return re;
	}
	
	/**
	 * 
	 * @param sql 需要执行的sql语句
	 * @param o   sql语句中的参数数组
	 * @return
	 * @throws SQLException
	 */
	public static boolean executsql(String sql, Object[] o, Connection conn,
			PreparedStatement stmt) throws SQLException {
		boolean re = false;
		if (o != null) {
			for (int i = 0; i < o.length; i++) {
				if(o[i]==null){
					stmt.setObject(i + 1, "");
				}else{
//					if(o[i].toString().trim().equals("")){
						stmt.setObject(i + 1, o[i]);
//					}
				}
			}
		}
		stmt.executeUpdate();
		re = true;
		return re;
	}

	public static List getdata(String sql, Object[] obj, Connection conn,
			PreparedStatement stmt) throws SQLException {
		ResultSet result = null;
		ArrayList l = new ArrayList();
		for (int i = 0; i < obj.length; i++) {
			stmt.setObject(i + 1, obj[i]);
		}
		System.out.println("*** get result start ***");
		result = stmt.executeQuery();
		System.out.println("*** get result end ***");
//		int cnt=0;
		Object[] o = null;
		
		while (result.next()) {
//			System.out.println("===="+cnt);
			o = new Object[result.getMetaData().getColumnCount()];
			for (int i = 0; i < o.length; i++) {
				o[i] = result.getObject(i + 1);
			}
			l.add(o);
			o = null;
//			cnt++;
		}
		System.out.println("*** put to list end ***");
		setColumns(result);//获得表字段名
		return l;
	}
	
	public static void setColumns(ResultSet result) throws SQLException{
		ResultSetMetaData rsmd = result.getMetaData();
		columnlist = new ArrayList();
		if(rsmd != null){
	        int count  = rsmd.getColumnCount();
	        for(int i=1;i<=count;i++){
	          columnlist.add(rsmd.getColumnName(i));
	        }
	    }
	}
	
	public static List getColumns(){
		return columnlist;
	}

	/**
	 * 断开数据库连接
	 * @throws SQLException 
	 *
	 */
	public static void disconnect(Connection conn) throws SQLException {
//		try {
//			System.out.println("start sleep");
//			Thread.sleep(20000);
//			System.out.println("end sleep");
//		} catch (InterruptedException e) {
//			e.printStackTrace();
//		}
		conn.close();
	}
}
