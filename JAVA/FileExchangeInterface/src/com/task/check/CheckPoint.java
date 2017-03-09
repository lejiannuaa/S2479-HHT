package com.task.check;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.task.common.PropertysUtil;
import com.task.db.DBConnection;

/**
 * 提供一个检查点，用于判断是否可以继续执行保存动作
 * 
 * @author S1608
 * 
 */
public class CheckPoint {

	/**
	 * 标示抛转表中的chgcode
	 */
	private  String chgcode = "";
	
	private static CheckPoint cp = null;
	
	/**
	 * 有异常被触发
	 * 该变量每次在被异常触发修改为true，当用户返回后，该值又返回false。
	 */
	private boolean fireException = false;
	
	/**
	 * 返回实例
	 * @return
	 */
	public synchronized static CheckPoint getInstance(){
		if(cp == null)
			cp = new CheckPoint();
		return cp;
	}
	
	private CheckPoint(){
		
	}
	
	
	/**
	 * 根据code来更新状态
	 * 
	 * @param state:只接受0和1，非零即为真
	 */
	public void updateState(int state) {
		//格式化所有传入的值
//		if(state>=1)
//			state = 1;
//		else
//			state = 0;
//		Connection con = DBConnection.getConnect(PropertysUtil.getConnectInfo());
//		String sql = "update INTERFACE.HOLA_APP_CHG_INST_STS set STATUS=? where CHGCODE=? and ISACTIVE='Y'";
//		PreparedStatement ps = null;
//		try {
//			ps = con.prepareStatement(sql);
//			ps.setObject(1, state);
//			ps.setObject(2, chgcode==null?"":chgcode);
//			ps.execute();
//			//有异常发生了，值会被设为true
//			if(state==0)
//				fireException = true;
//		} catch (SQLException e) {
//			e.printStackTrace();
//		} finally {
//			try {
//				if (ps != null)
//					ps.close();
//				if (con != null && con.isClosed() == false)
//					con.close();
//			} catch (SQLException e) {
//				e.printStackTrace();
//			}
//		}
	}

	/**
	 * 获得某个code的状态。 如果没有查询到某个code的状态，返回true。
	 * 
	 * @param code
	 */
	public boolean getState() {
		//查询状态为Y的且code符合条件的内容
//		Connection con = DBConnection.getConnect(PropertysUtil.getConnectInfo());
//		String sql = "select STATUS from INTERFACE.HOLA_APP_CHG_INST_STS where CHGCODE=? and ISACTIVE='Y'";
//		PreparedStatement ps = null;
//		ResultSet rs = null;
//		try {
//			ps = con.prepareStatement(sql);
//			ps.setObject(1, chgcode);
//			rs = ps.executeQuery();
//			
//			//如果查询出的值为0，说明有异常存在，返回false。
//			while(rs.next()){
//				String status = rs.getString("STATUS");
//				if(status.equals("0"))
//					return false;
//			}
//			
//		} catch (SQLException e) {
//			e.printStackTrace();
//		} finally {
//			try {
//				if (rs != null)
//					rs.close();
//				if (ps != null)
//					ps.close();
//				if (con != null && con.isClosed() == false)
//					con.close();
//			} catch (SQLException e) {
//				e.printStackTrace();
//			}
//		}
		return true;
	}

	public static void main(String[] args) {
//		PropertysUtil pu = new PropertysUtil();
//		pu.initPropertysUtil();
		
//		CheckPoint cp = new CheckPoint();
//		cp.updateState("NT", 1);
//		System.out.println(cp.getState("NT"));
		boolean fireException = false;
		boolean fe = fireException;
		fireException = true;
		System.out.println(fe==fireException);
	}

	public String getCode() {
		return chgcode;
	}

	public void setCode(String code) {
		this.chgcode = code;
	}

	/**
	 * 返回是异常是否被触发的标示。<br>
	 * 该标示每次在被返回后，将被重新设为false。
	 * @return
	 */
	public boolean isFireException() {
		boolean fe = fireException;
		fireException = false;
		return fe;
	}


}
