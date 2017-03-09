package com.task.db;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

import org.apache.log4j.Logger;

import com.task.check.CheckPoint;
import com.task.common.DateUtil;
import com.task.common.Uuid32;

public class DBOperator {

	private static Logger logger = Logger.getLogger( DBOperator.class );
	
	public void disConnect(List <Connection> connList,List<PreparedStatement> stmtList){
		Connection conn = null;
		PreparedStatement stmt = null ;
		try{
			for(int i=0;i<stmtList.size();i++){
				stmt = stmtList.get(i);
				if(stmt!=null){
					stmt.close();
				}
			}
			for(int i=0;i<connList.size();i++){
				conn = connList.get(i);
				if(conn!=null){
					DBManager.disconnect(conn);
				}
			}
		}catch(Exception e){
			e.printStackTrace();
		}finally{
			connList = null;
			stmtList = null;
			conn = null;
			stmt = null ;
		}
	}
	
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
			logger.info("getDataError url : "+connectInfo.get("Url")+" sql="+sql+"  msg : "+e.getMessage());
			System.out.println("getDataError url : "+connectInfo.get("Url")+" sql="+sql+"  msg : "+e.getMessage());
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
	
	public void execDataByTrancationForAS400(Hashtable connectInfo,String sql,List objAryList){
		Connection conn = null;
		PreparedStatement stmt = null;
		int index = 0;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			conn = DBManager.getConnection(url, driver, userName, pwd);
//			conn.setAutoCommit(false);
			stmt = conn.prepareStatement(sql);
			System.out.println("AS400 objAryList="+objAryList.size());
			for(int i=0;i<objAryList.size();i++){
				Object[] obj = (Object[]) objAryList.get(i);
				DBManager.stmtaddBatch(obj, stmt,i);
				if(i!=0&&i%10000==0){
					index = i;
					int[] counts = stmt.executeBatch();
					stmt = conn.prepareStatement(sql);
					System.out.println("i="+i+"  counts="+counts.length);
				}
//				if(sql.indexOf("HOLA_EC_ODRITEM")!=-1||sql.indexOf("HOLA_EC_ODRITEM_PROMO")!=-1||sql.indexOf("HOLA_EC_ODRMST")!=-1){
//					for(int j=0;j<obj.length;j++){
//						System.out.println("obj "+j+" = "+obj[j]);
//					}
//				}
			}
			System.out.println("sartExecAS400 Batch = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			int[] counts = stmt.executeBatch();
			System.out.println("counts="+counts.length);
			System.out.println("endExecAS400 Batch = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
//			conn.commit();
		}catch(Exception e){
			e.printStackTrace();
			logger.info("execDataByTrancationForAS400Error sql url : "+connectInfo.get("Url")+" sql="+sql+"  index="+index+"  msg : "+e.getMessage());
			System.out.println("execDataByTrancationForAS400Error sql url : "+connectInfo.get("Url")+" sql="+sql+"  index="+index+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
			//如果发生异常，就把状态设定为错误 by mark
			CheckPoint cp = CheckPoint.getInstance();
			cp.updateState(0);
		}finally{
			try {
//				if(conn!=null){
//					conn.setAutoCommit(true);
//				}
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
	
	public void execBatchBySQLTrancation(Hashtable connectInfo,List sqlList){
		Connection conn = null;
		Statement  stmt = null;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			conn = DBManager.getConnection(url, driver, userName, pwd);
			conn.setAutoCommit(false);
			stmt = conn.createStatement();
			for(int i=0;i<sqlList.size();i++){
				String sql = sqlList.get(i).toString();
				System.out.println(sql);
				stmt.addBatch(sql);
			}
			System.out.println("start exec = "+DateUtil.formatDate3(DateUtil.getCurrentDate()));
			int[] count = stmt.executeBatch();
			conn.commit();
			System.out.println("end exec = "+DateUtil.formatDate3(DateUtil.getCurrentDate())+"  count="+count.length);
		}catch(Exception e){
			e.printStackTrace();
			logger.info("execDataByTrancationError sql url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			System.out.println("execDataByTrancationError sql url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
			}
			//如果发生异常，就把状态设定为错误 by mark
			CheckPoint cp = CheckPoint.getInstance();
			cp.updateState(0);
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
				if(sqlList!=null){
					sqlList = null;
				}
			} catch (Exception e) {
//				e.printStackTrace();
			}
		}
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
				
//				if(sql.indexOf("HOLA_APP_RANGE_DOCIPLW")!=-1||sql.indexOf("HOLA_APP_RANGE_DOCDELW")!=-1){
//					for(int j=0;j<obj.length;j++){
//						System.out.println("obj "+j+" = "+obj[j]);
//					}
//				}
				
				
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
			logger.info("execDataByTrancationError sql url : "+connectInfo.get("Url")+" sql="+sql+"  index="+index+"  msg : "+e.getMessage());
			//System.out.println("execDataByTrancationError sql url : "+connectInfo.get("Url")+" sql="+sql+"  index="+index+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
			//如果发生异常，就把状态设定为错误 by mark
			CheckPoint cp = CheckPoint.getInstance();
			cp.updateState(0);
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
	
	public void execDataByTrancationForAS400(Hashtable connectInfo,List sqlList,List objAryList){
		Connection conn = null;
		PreparedStatement stmt = null;
		try{
			String url = connectInfo.get("Url").toString();
			String driver = connectInfo.get("Driver").toString();
			String userName = connectInfo.get("UserName").toString();
			String pwd = connectInfo.get("Pwd").toString();
			conn = DBManager.getConnection(url, driver, userName, pwd);
//			conn.setAutoCommit(false);
			for(int i=0;i<objAryList.size();i++){
				Object[] obj = (Object[]) objAryList.get(i);
				String sql = (String) sqlList.get(i);
//				if(sql.indexOf("HOLA_EC_ODRITEM")!=-1||sql.indexOf("HOLA_EC_ODRITEM_PROMO")!=-1||sql.indexOf("HOLA_EC_ODRMST")!=-1){
//					for(int j=0;j<obj.length;j++){
//						System.out.println("obj "+j+" = "+obj[j]);
//					}
//				}
				stmt = conn.prepareStatement(sql);
				DBManager.executsql(sql, obj, conn, stmt);
			}
//			conn.commit();
		}catch(Exception e){
			e.printStackTrace();
			logger.info("execDataByTrancationForAS400Error sqllist url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			System.out.println("execDataByTrancationForAS400Error sqllist url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
			//如果发生异常，就把状态设定为错误 by mark
			CheckPoint cp = CheckPoint.getInstance();
			cp.updateState(0);
		}finally{
			try {
//				if(conn!=null){
//					conn.setAutoCommit(true);
//				}
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
			logger.info("execDataByTrancationError sqllist url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			System.out.println("execDataByTrancationError sqllist url : "+connectInfo.get("Url")+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
			//如果发生异常，就把状态设定为错误 by mark
			CheckPoint cp = CheckPoint.getInstance();
			cp.updateState(0);
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
//			System.out.println("sql="+sql);
//			for(int i=0;i<obj.length;i++){
//				System.out.println(obj[i]+" = "+obj[i].toString().length());
//			}
			DBManager.executsql(sql, obj, conn, stmt);
			conn.commit();
		}catch(Exception e){
//			e.printStackTrace();
			logger.info("execDataError url : "+connectInfo.get("Url")+" sql="+sql+"  msg : "+e.getMessage());
			System.out.println("execDataError url : "+connectInfo.get("Url")+" sql="+sql+"  msg : "+e.getMessage());
			try {
				conn.rollback();
			} catch (Exception e1) {
//				e1.printStackTrace();
			}
			//如果发生异常，就把状态设定为错误 by mark
			CheckPoint cp = CheckPoint.getInstance();
			cp.updateState(0);
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
