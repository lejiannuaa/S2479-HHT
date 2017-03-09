package factory;

import common.ConfigHelper;

import dao.ServiceDAO;
import dao.proxy.ServiceDAOProxy;

public class DAOFactory {
	public static ServiceDAO getServiceDAOInstance(String Driver, String URL, String USER, String PASSWORD) throws Exception
	{
		return new ServiceDAOProxy(Driver, URL, USER, PASSWORD);
	}
}
