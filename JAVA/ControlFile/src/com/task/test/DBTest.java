package com.task.test;

import java.sql.*;
import org.logicalcobwebs.proxool.configuration.PropertyConfigurator;  
import org.logicalcobwebs.proxool.ProxoolException;

public class DBTest {

	public String getPath(){
	
		return this.getClass().getResource("DBTest.class").toString().split("file:/")[1].split("test/DBTest.class")[0]+"cfg/proxool.properties";
	}
	public static void main(String[] args) {
		Connection conn=null;        
		try {           
			DBTest t = new DBTest();
			PropertyConfigurator.configure(t.getPath());            
			conn = DriverManager.getConnection("proxool.HOLATEST:oracle.jdbc.OracleDriver:jdbc:oracle:thin:@10.130.1.9:1521:holaweb","msg","holamsg");    
			if(conn!=null){
				System.out.println("数据连接测试成功！");            
				Statement Stmt=conn.createStatement();             
				ResultSet Rst=null;             
				Rst=Stmt.executeQuery("select * from CHGCTL");            
				while(Rst.next()){
					System.out.println(Rst.getString("CHGCODE"));
				}
			}  
		}catch(SQLException e) {           
				System.out.println("error"+e);        
		}catch(ProxoolException e1){           
				System.out.println(e1);        
		} finally{            
				try{                 
					if (conn != null)                       
						conn.close();            
				}catch(SQLException e2){                  
					System.out.println(e2);            
				}                         
		}    
	}
}
