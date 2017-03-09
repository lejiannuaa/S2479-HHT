package dao;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class DatabaseConnection {
    
	private static Connection conn = null;
    
    public DatabaseConnection(String Driver, String URL, String USER, String PASSWORD) throws Exception
    {
    	try{
            //1.������������
            Class.forName(Driver);
            //2. ������ݿ�����
            this.conn = DriverManager.getConnection(URL, USER, PASSWORD);
    	}catch (Exception e){
    		throw e;
    	}
    	
    }
    
    public Connection getConnection(){
        return this.conn;
    }
    
    public void close() throws Exception
    {
    	if(this.conn!=null)
    	{
    		try{
    			this.conn.close();
    		}catch (Exception e)
    		{
    			throw e;
    		}
    	}
    }


}
