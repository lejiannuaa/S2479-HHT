package com.task.db;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

import org.apache.log4j.Logger;

import com.task.common.DateUtil;
import com.task.common.Uuid32;

public class DBOperator {

	private static Logger logger = Logger.getLogger( DBOperator.class );
	
	public List getData(String sql,Object[] obj,Hashtable connectInfo){
		List list = new ArrayList();
		Connection conn = null;
		PreparedStatement stmt = null ;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			
			conn = DBManager.getConnection(url, driver, userName, pwd);
			
			stmt = conn.prepareStatement(sql);
			list = DBManager.getdata(sql,obj,conn,stmt);
		}catch(Exception e){
			e.printStackTrace();
			logger.info("getDataError url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			System.out.println("getDataError url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
		}finally{
			try {
				if(stmt!=null){
					stmt.close();
				}
				if(conn!=null){
					DBManager.disconnect(conn);
				}
			} catch (Exception e) {
//				e.printStackTrace();
			}
		}
		return list;
	}
	
	public void execDataByTrancation(Hashtable connectInfo,String sql,List objAryList){
		Connection conn = null;
		PreparedStatement stmt = null;
		int index = 0;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			conn = DBManager.getConnection(url, driver, userName, pwd);
//			conn = DBConnection.getConnect(connectInfo);
			conn.setAutoCommit(false);
			stmt = conn.prepareStatement(sql);
			System.out.println("objAryList="+objAryList.size());
			for(int i=0;i<objAryList.size();i++){
//				conn = DBManager.getConnection(url, driver, userName, pwd);
				Object[] obj = (Object[]) objAryList.get(i);
//				DBManager.executsqlAddBatch(sql, obj, conn, stmt);
				DBManager.stmtaddBatch(obj, stmt,i);
				if(i!=0&&i%10000==0){
					index = i;
					int[] counts = stmt.executeBatch();
					stmt = conn.prepareStatement(sql);
					System.out.println("i="+i+"  counts="+counts.length);
				}
			}
			System.out.println("sartExec Batch = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			int[] counts = stmt.executeBatch();
			System.out.println("counts="+counts.length);
			System.out.println("endExec Batch = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			conn.commit();
		}catch(Exception e){
//			e.printStackTrace();
			logger.info("execTranDataError url : "+connectInfo.get("Url")+"  index="+index+"  msg : "+e.getMessage());
			System.out.println("execTranDataError url : "+connectInfo.get("Url")+"  index="+index+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
		}finally{
			try {
				if(conn!=null){
					conn.setAutoCommit(true);
				}
				if(stmt!=null){
					stmt.close();
				}
				if(conn!=null){
					DBManager.disconnect(conn);
				}
				if(objAryList!=null){
					objAryList = null;
				}
			} catch (Exception e) {
//				e.printStackTrace();
			}
		}
	}
	
	public void execDataByTrancation(Hashtable connectInfo,List sqlList,List objAryList){
		Connection conn = null;
		PreparedStatement stmt = null;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			conn = DBManager.getConnection(url, driver, userName, pwd);
			conn.setAutoCommit(false);
			for(int i=0;i<objAryList.size();i++){
				Object[] obj = (Object[]) objAryList.get(i);
				String sql = (String) sqlList.get(i);
				stmt = conn.prepareStatement(sql);
				DBManager.executsql(sql, obj, conn, stmt);
			}
			conn.commit();
		}catch(Exception e){
			e.printStackTrace();
			logger.info("execTranDataError url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			System.out.println("execTranDataError url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
		}finally{
			try {
				if(conn!=null){
					conn.setAutoCommit(true);
				}
				if(stmt!=null){
					stmt.close();
				}
				if(conn!=null){
					DBManager.disconnect(conn);
				}
			} catch (Exception e) {
//				e.printStackTrace();
			}
		}
	}
	
	public void execData(Hashtable connectInfo,String sql,Object[] obj){
		Connection conn = null;
		PreparedStatement stmt = null;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			conn = DBManager.getConnection(url, driver, userName, pwd);
			conn.setAutoCommit(false);
			stmt = conn.prepareStatement(sql);
			DBManager.executsql(sql, obj, conn, stmt);
			conn.commit();
		}catch(Exception e){
//			e.printStackTrace();
			logger.info("execDataError url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			System.out.println("execDataError url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
		}finally{
			try {
				if(conn!=null){
					conn.setAutoCommit(true);
				}
				if(stmt!=null){
					stmt.close();
				}
				if(conn!=null){
					DBManager.disconnect(conn);
				}
			} catch (Exception e) {
//				e.printStackTrace();
			}
		}
	}
	
	public List getColumnsList(){
		List list = null;
		try{
			list = DBManager.getColumns();
		}catch(Exception e){
			e.printStackTrace();
		}
		return list;
	}
	
	public static void main(String[] args){
		DBOperator dbo = new DBOperator();
		
		Hashtable connectInfo = new Hashtable();
		connectInfo.put("Url", "jdbc:sqlserver://127.0.0.1:1433;DataBaseName=WorkRoomDB");
		connectInfo.put("Driver", "com.microsoft.sqlserver.jdbc.SQLServerDriver");
		connectInfo.put("UserName", "sa");
		connectInfo.put("Pwd", "123456");
		
		String sql = "insert into tbl_team (Name,WorkRoomId,Status) values (?,?,?)";
		List objAryList = new ArrayList();
		for(int i=0;i<200000;i++){
			Object[] obj = new Object[3];
			obj[0] = "name"+i;
			obj[1] = "id"+i;
			obj[2] = "1";
			objAryList.add(obj);
		}
		dbo.execDataByTrancation(connectInfo, sql, objAryList);
	}
}
