package dao.impl;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import model.IntfInfo;
import dao.ServiceDAO;

public class ServiceDAOImpl implements ServiceDAO{

	private Connection conn=null;
	private PreparedStatement pstmt=null;
	
	public ServiceDAOImpl(Connection conn)
	{
		this.conn=conn;
	}
	

	@Override
	public List<IntfInfo> getAllIntfSystemDB() throws Exception {
		// TODO Auto-generated method stub

		List<IntfInfo> list=new ArrayList<IntfInfo>();
		String sql="select * from intf_monitor where STATUS=?";
		
		this.pstmt=this.conn.prepareStatement(sql);
		this.pstmt.setString(1, "E");
		ResultSet rs=this.pstmt.executeQuery();
		
		IntfInfo IntfInfo=null;
		while(rs.next())
		{
			IntfInfo=new IntfInfo();
			IntfInfo.setIntfName(rs.getString("INTFNAME"));
			IntfInfo.setStatus(rs.getString("STATUS"));
			IntfInfo.setSysDBUrl(rs.getString("SYSDBURL"));
			IntfInfo.setSysDBDriver(rs.getString("SYSDBDRIVER"));
			IntfInfo.setDBSche(rs.getString("DBSCHE"));
			IntfInfo.setUserName(rs.getString("USERNAME"));
			IntfInfo.setPassword(rs.getString("PASSWORD"));
			IntfInfo.setTable(rs.getString("TABLE"));
			list.add(IntfInfo);
		}
		
		this.pstmt.close();
		return list;
	}


	@Override
	public String getMaxIntfTime(String sche, String table) throws Exception {
		// TODO Auto-generated method stub
		String maxIntfTime = null;
		String sql="select max(RUNNINGTIME) as RUNNINGTIME from " + table;
		//String sql="select max(RUNNINGTIME) as RUNNINGTIME from " + sche + "." + table;
		
		this.pstmt=this.conn.prepareStatement(sql);
		ResultSet rs=this.pstmt.executeQuery();
		
		while(rs.next())
		{
			maxIntfTime = rs.getString("RUNNINGTIME");
		}
		
		this.pstmt.close();
		return maxIntfTime;
	}

}
