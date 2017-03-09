package dao.proxy;

import java.util.List;

import model.IntfInfo;
import dao.DatabaseConnection;
import dao.ServiceDAO;
import dao.impl.ServiceDAOImpl;

public class ServiceDAOProxy implements ServiceDAO{
	
	private DatabaseConnection dbc=null;
	private ServiceDAO dao=null;
	
	public ServiceDAOProxy(String Driver, String URL, String USER, String PASSWORD) throws Exception
	{
		this.dbc=new DatabaseConnection(Driver, URL, USER, PASSWORD);
		this.dao=new ServiceDAOImpl(this.dbc.getConnection());
	}

	@Override
	public List<IntfInfo> getAllIntfSystemDB() throws Exception {
		// TODO Auto-generated method stub
		List<IntfInfo> list=null;
		
		try{
			list = this.dao.getAllIntfSystemDB();
		}catch (Exception e){
			throw e;
		}finally{
			this.dbc.close();
		}
		return list;
	}


	@Override
	public String getMaxIntfTime(String sche, String table) throws Exception {
		// TODO Auto-generated method stub
		String maxIntfTime = null;
		
		try{
			maxIntfTime = this.dao.getMaxIntfTime(sche, table);
		}catch (Exception e){
			throw e;
		}finally{
			this.dbc.close();
		}
		return maxIntfTime;
	}

}
