package service;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import model.IntfInfo;
import common.ConfigHelper;
import common.MailUtils;
import factory.DAOFactory;

public class MonitorService {

	private static Log log = LogFactory.getLog(MonitorService.class);

	public static List<IntfInfo> receiveDBForAllSystem() {
		// TODO Auto-generated method stub
		
		String Driver = ConfigHelper.getInstance().getValue("Server.jdbc.driverClassName");
	    String URL = ConfigHelper.getInstance().getValue("Server.jdbc.url");
	    String USER = ConfigHelper.getInstance().getValue("Server.jdbc.username");
	    String PASSWORD = ConfigHelper.getInstance().getValue("Server.jdbc.password");
	    
	    List<IntfInfo> list = null;
	    
	    try {
	    	
	    	list = DAOFactory.getServiceDAOInstance(Driver, URL, USER, PASSWORD).getAllIntfSystemDB();
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	    
		return list;
	}
	
	public static void monitortime(IntfInfo intfInfo){
		

		String Driver = intfInfo.getSysDBDriver();
	    String URL = intfInfo.getSysDBUrl();
	    String USER = intfInfo.getUserName();
	    String PASSWORD = intfInfo.getPassword();

	    String intfName = intfInfo.getIntfName();
	    String sche = intfInfo.getDBSche();
	    String table = intfInfo.geTable();
	    
	    String maxIntfTime = null;
	    
	    try {
	    	
	    	maxIntfTime = DAOFactory.getServiceDAOInstance(Driver, URL, USER, PASSWORD).getMaxIntfTime(sche, table);
			
	    	if(maxIntfTime != null)
	    	{
	    		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
	    		Date d1 = sdf.parse(maxIntfTime);
	    		Date d2 = new Date();
	    		
	    		long diff = d2.getTime() - d1.getTime();
	    		long count = diff/(1000*60);
	    		
	    		String str_timeout = ConfigHelper.getInstance().getValue("schedule.timeout");

	            int timeout = Integer.parseInt(str_timeout);

	    		if(count > timeout)
	    		{
	    			MailUtils.sendMail(intfName + " 长时间未运行，请查看", intfName + "异常");
	    			log.info(intfName + "故障，邮件已发送");
	    		}
	    		
	    	}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	    
	    
	}
	
}
