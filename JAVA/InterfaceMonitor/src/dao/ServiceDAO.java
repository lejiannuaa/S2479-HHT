package dao;

import java.util.List;

import model.IntfInfo;

public interface ServiceDAO {
	
	public List<IntfInfo> getAllIntfSystemDB() throws Exception;
	

	public String getMaxIntfTime(String sche, String table) throws Exception;

}
